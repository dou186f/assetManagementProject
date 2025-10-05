using assetManagement.MOBILE.ViewModels;

namespace assetManagement.MOBILE.Pages;
public partial class MainPage : ContentPage
{
    public MainPage(LibrarySearchViewModel vm) 
    { 
        InitializeComponent(); 
        BindingContext = vm; 
    }
}
