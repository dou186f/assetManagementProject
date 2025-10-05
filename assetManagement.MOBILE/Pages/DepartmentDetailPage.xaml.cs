using Microsoft.Extensions.DependencyInjection;
using assetManagement.MOBILE.ViewModels;

namespace assetManagement.MOBILE.Pages;

public partial class DepartmentDetailPage : ContentPage
{
    public DepartmentDetailPage()
        : this(MauiProgram.Services.GetRequiredService<DepartmentDetailViewModel>()) { }

    public DepartmentDetailPage(DepartmentDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
