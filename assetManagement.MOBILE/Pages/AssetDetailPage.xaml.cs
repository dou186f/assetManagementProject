using Microsoft.Extensions.DependencyInjection;
using assetManagement.MOBILE.ViewModels;

namespace assetManagement.MOBILE.Pages;

public partial class AssetDetailPage : ContentPage
{
    public AssetDetailPage()
        : this(MauiProgram.Services.GetRequiredService<AssetDetailViewModel>()) { }

    public AssetDetailPage(AssetDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
