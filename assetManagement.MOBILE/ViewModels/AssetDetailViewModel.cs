using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using assetManagement.MOBILE.Models;
using assetManagement.MOBILE.Services;

namespace assetManagement.MOBILE.ViewModels
{
    [QueryProperty(nameof(AssetId), "id")]
    public class AssetDetailViewModel : INotifyPropertyChanged
    {
        private readonly AssetService _assetservice;
        private readonly EmployeeService _employeeservice;

        public ObservableCollection<EmployeeModel> Employees { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                ((Command)AddToEmployeeCommand).ChangeCanExecute();
                ((Command)RemoveFromEmployeeCommand).ChangeCanExecute();
            }
        }

        private bool _isAdderVisible = false;
        public bool IsAdderVisible
        {
            get => _isAdderVisible;
            set { _isAdderVisible = value; OnPropertyChanged(); }
        }

        private AssetModel _asset = new();
        public AssetModel Asset 
        { 
            get => _asset; 
            set 
            { 
                _asset = value; 
                OnPropertyChanged(); 
            } 
        }

        private string? _employeeName;
        public string? EmployeeName 
        {
            get => _employeeName;
            set { _employeeName = value;
                OnPropertyChanged();
            }
        }

        private EmployeeModel? selectedEmployee;
        public EmployeeModel? SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;
                OnPropertyChanged();
                ((Command)AddToEmployeeCommand).ChangeCanExecute();
            }
        }

        private int _assetId;
        public int AssetId
        {
            get => _assetId;
            set
            {
                _assetId = value;
                _ = InitAsync();
            }
        }

        public ICommand AddToEmployeeCommand { get; }
        public ICommand RemoveFromEmployeeCommand { get; }
        public ICommand ToggleAdderCommand { get; }

        public AssetDetailViewModel(AssetService assetservice, EmployeeService Employeeservice)
        {
            AddToEmployeeCommand = new Command(async () => await AddAsync(), () => SelectedEmployee != null && !IsBusy);
            RemoveFromEmployeeCommand = new Command(async () => await RemoveAsync(), () => !IsBusy);
            ToggleAdderCommand = new Command(() => IsAdderVisible = !IsAdderVisible);
            _assetservice = assetservice;
            _employeeservice = Employeeservice;
        }

        private async Task LoadAsync(int id)
        {
            if (id <= 0 || IsBusy) return;
            IsBusy = true;
            try
            {
                var dto = await _assetservice.GetAssetByIdAsync(id);
                Asset = dto ?? new AssetModel { id = id, name = "Not Found!" };

                EmployeeName = null;
                var depId = Asset?.ownerId;
                if (depId is int dId)
                {
                    var dep = await _assetservice.GetCalisanByIdAsync(dId);
                    EmployeeName = dep?.name ?? $"#{dId}";
                }
                else
                {
                    EmployeeName = "(Asset in storage)";
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadDepartmentsAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                Employees.Clear();
                var list = await _employeeservice.GetEmployeesAllAsync();
                foreach (var d in list) Employees.Add(d);
            }
            finally { IsBusy = false; }
        }

        private async Task AddAsync()
        {
            if (SelectedEmployee is null) return;
            try
            {
                IsBusy = true;
                await _employeeservice.AddAssetAsync(SelectedEmployee.id, AssetId, reassign: false);
                await Shell.Current.DisplayAlert("Okey", "Given to Employee", "OK");
            }
            catch (InvalidOperationException ex)
            {
                var move = await Shell.Current.DisplayAlert("Transfer required", $"{ex.Message}\nShould it be given to the selected employee?", "Yes", "No");
                if (move)
                {
                    await _employeeservice.AddAssetAsync(SelectedEmployee.id, AssetId, reassign: true);
                    await Shell.Current.DisplayAlert("Moved", "The item was moved to the selected employee.", "OK");
                }
            }
            finally { IsBusy = false; await LoadAsync(AssetId); IsAdderVisible = false; }
        }

        private async Task RemoveAsync()
        {
            try
            {
                IsBusy = true;
                await _employeeservice.RemoveAssetAsync(AssetId);
                await Shell.Current.DisplayAlert("Okey", "The item was received from the employee.", "OK");
            }
            finally { IsBusy = false; await LoadAsync(AssetId); IsAdderVisible = false; }
        }

        private bool _initRunning, _initDone;
        public async Task InitAsync()
        {
            if (_initDone || _initRunning) return;
            _initRunning = true;
            try
            {
                await LoadAsync(_assetId);
                await LoadDepartmentsAsync();
            }
            finally
            {
                _initRunning = false;
                _initDone = true;
            }
        }

    }
}
