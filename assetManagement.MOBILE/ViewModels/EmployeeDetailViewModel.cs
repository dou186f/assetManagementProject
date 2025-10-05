using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using assetManagement.MOBILE.Models;
using assetManagement.MOBILE.Services;

namespace assetManagement.MOBILE.ViewModels
{
    [QueryProperty(nameof(EmployeeId), "id")]
    public class EmployeeDetailViewModel : INotifyPropertyChanged
    {
        private readonly EmployeeService _employeeservice;
        private readonly DepartmentService _departmentservice;

        public ObservableCollection<DepartmentModel> Departments { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private bool _isBusy;
        public bool IsBusy { 
            get => _isBusy;
            set 
            { 
                _isBusy = value; 
                OnPropertyChanged(); 
                ((Command)AddToDepartmentCommand).ChangeCanExecute(); 
                ((Command)RemoveFromDepartmentCommand).ChangeCanExecute(); 
            } 
        }

        private bool _isAdderVisible = false;
        public bool IsAdderVisible
        {
            get => _isAdderVisible;
            set { _isAdderVisible = value; OnPropertyChanged(); }
        }

        private EmployeeModel _employee = new();
        public EmployeeModel Employee 
        { 
            get => _employee; 
            set 
            { 
                _employee = value; 
                OnPropertyChanged(); 
            } 
        }

        private string? _departmentName;
        public string? DepartmentName 
        { 
            get => _departmentName; 
            set 
            { _departmentName = value; 
                OnPropertyChanged(); 
            } 
        }

        private DepartmentModel? selectedDepartment;
        public DepartmentModel? SelectedDepartment
        {
            get => selectedDepartment;
            set 
            { selectedDepartment = value;
                OnPropertyChanged();
                ((Command)AddToDepartmentCommand).ChangeCanExecute();
            }
        }

        private int _employeeId;
        public int EmployeeId 
        { 
            get => _employeeId; 
            set 
            {
                _employeeId = value;
                _ = InitAsync(); 
            } 
        }

        public ICommand AddToDepartmentCommand { get; }
        public ICommand RemoveFromDepartmentCommand { get; }
        public ICommand ToggleAdderCommand { get; }

        public EmployeeDetailViewModel(EmployeeService employeeservice, DepartmentService departmentservice)
        {
            AddToDepartmentCommand = new Command(async () => await AddAsync(), () => SelectedDepartment != null && !IsBusy);
            RemoveFromDepartmentCommand = new Command(async () => await RemoveAsync(), () => !IsBusy);
            ToggleAdderCommand = new Command(() => IsAdderVisible = !IsAdderVisible);
            _employeeservice = employeeservice;
            _departmentservice = departmentservice;
        }

        private async Task LoadAsync(int id)
        {
            if (id <= 0 || IsBusy) return;
            IsBusy = true;
            try 
            {
                var dto = await _employeeservice.GetEmployeeByIdAsync(id);
                Employee = dto ?? new EmployeeModel { id = id, name = "Not Found!" };

                DepartmentName = null;
                var depId = Employee?.departId;
                if (depId is int dId)
                {
                    var dep = await _employeeservice.GetDepartmentByIdAsync(dId);
                    DepartmentName = dep?.name ?? $"#{dId}";
                }
                else
                {
                    DepartmentName = "(No department)";
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
                Departments.Clear();
                var list = await _departmentservice.GetAllAsync();
                foreach (var d in list) Departments.Add(d);
            }
            finally { IsBusy = false; }
        }

        private async Task AddAsync()
        {
            if (SelectedDepartment is null) return;
            try
            {
                IsBusy = true;
                await _departmentservice.AddEmployeeAsync(SelectedDepartment.id, EmployeeId, reassign: false);
                await Shell.Current.DisplayAlert("Okey", "Added to department.", "OK");
            }
            catch (InvalidOperationException ex)
            {
                var move = await Shell.Current.DisplayAlert("Transport required", $"{ex.Message}\nMove to selected department?", "Yes", "No");
                if (move)
                {
                    await _departmentservice.AddEmployeeAsync(SelectedDepartment.id, EmployeeId, reassign: true);
                    await Shell.Current.DisplayAlert("Moved", "The employee was moved to the selected department.", "OK");
                }
            }
            finally { IsBusy = false; await LoadAsync(EmployeeId); IsAdderVisible = false; }
        }

        private async Task RemoveAsync()
        {
            try
            {
                IsBusy = true;
                await _departmentservice.RemoveEmployeeAsync(EmployeeId);
                await Shell.Current.DisplayAlert("Okey", "The employee was removed from the department.", "OK");
            }
            finally { IsBusy = false; await LoadAsync(EmployeeId); IsAdderVisible = false; }
        }

        private bool _initRunning, _initDone;
        public async Task InitAsync()
        {
            if (_initDone || _initRunning) return;
            _initRunning = true;
            try
            {
                await LoadAsync(_employeeId); 
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
