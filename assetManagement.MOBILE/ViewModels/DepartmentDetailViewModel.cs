using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using assetManagement.MOBILE.Models;
using assetManagement.MOBILE.Services;
using static assetManagement.MOBILE.AppShell;

namespace assetManagement.MOBILE.ViewModels
{
    [QueryProperty(nameof(DepartmentId), "id")]
    public class DepartmentDetailViewModel : INotifyPropertyChanged
    {
        private readonly DepartmentService _departmentservice;
        private readonly EmployeeService _employeeservice;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<EmployeeModel> Employees { get; } = new();
        void OnPropertyChanged([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }

        private DepartmentModel _department = new();
        public DepartmentModel Department { get => _department; set { _department = value; OnPropertyChanged(); } }

        private int _departmentId;
        public int DepartmentId { get => _departmentId; set { _departmentId = value; _ = LoadAsync(value); } }
        public ICommand OpenItemDetailCommand { get; }

        public DepartmentDetailViewModel(DepartmentService departmentservice, EmployeeService employeeservice)
        {
            _departmentservice = departmentservice;
            _employeeservice = employeeservice;

            OpenItemDetailCommand = new Command<EmployeeModel>(async item => await OpenItemDetailAsync(item));
        }

        private async Task LoadAsync(int id)
        {
            if (id <= 0 || IsBusy) return;
            IsBusy = true;
            try
            {
                Employees.Clear();
                var dto = await _departmentservice.GetDepartmentByIdAsync(id);
                Department = dto ?? new DepartmentModel { id = id, name = "Not Found!" };

                var list = await _employeeservice.GetEmployeesAllAsync();
                foreach (var employee in list)
                {
                    if (employee.departId == DepartmentId)
                    {
                        Employees.Add(employee);
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task OpenItemDetailAsync(EmployeeModel item)
        {
            if (item == null) return;
            var nav = new Dictionary<string, object> { ["id"] = item.id };
            await Shell.Current.GoToAsync(EmployeeDetailRoute, nav);
        }

    }
}
