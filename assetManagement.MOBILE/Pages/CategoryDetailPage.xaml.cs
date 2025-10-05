using Microsoft.Extensions.DependencyInjection;
using assetManagement.MOBILE.ViewModels;

namespace assetManagement.MOBILE.Pages;

public partial class CategoryDetailPage : ContentPage
{
    public CategoryDetailPage()
        : this(MauiProgram.Services.GetRequiredService<CategoryDetailViewModel>()) { }

    public CategoryDetailPage(CategoryDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
