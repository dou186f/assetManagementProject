using assetManagement.MOBILE.ViewModels;

namespace assetManagement.MOBILE.Pages;

public partial class EmployeeDetailPage : ContentPage
{
    public EmployeeDetailPage()
        : this(MauiProgram.Services.GetRequiredService<EmployeeDetailViewModel>()) { }

    public EmployeeDetailPage(EmployeeDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is EmployeeDetailViewModel vm)
            await vm.InitAsync();
    }
}
