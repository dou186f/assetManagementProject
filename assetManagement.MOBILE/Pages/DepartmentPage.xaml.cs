using System.Collections.ObjectModel;
using System.Windows.Input;
using assetManagement.MOBILE.Models;
using assetManagement.MOBILE.Services;
using static assetManagement.MOBILE.AppShell;

namespace assetManagement.MOBILE.Pages;

public partial class DepartmentPage : ContentPage
{
    public ObservableCollection<DepartmentModel> Departments { get; } = new();

    private readonly DepartmentService _departmanService;
    public ICommand OpenItemDetailCommand { get; }
    public DepartmentPage(DepartmentService departmentService)
    {
        InitializeComponent();
        _departmanService = departmentService;
        OpenItemDetailCommand = new Command<DepartmentModel>(async item => await OpenItemDetailAsync(item));
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            var list = await _departmanService.GetAllAsync();
            Departments.Clear();
            foreach (var b in list) Departments.Add(b);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata (Load)", ex.Message, "OK");
        }
    }

    private async Task OpenItemDetailAsync(DepartmentModel item)
    {
        if (item == null) return;
        var nav = new Dictionary<string, object> { ["id"] = item.id };
        await Shell.Current.GoToAsync(DepartmentDetailRoute, nav);
    }
}