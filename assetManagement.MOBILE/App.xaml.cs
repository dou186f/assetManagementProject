using assetManagement.MOBILE.Pages;

namespace assetManagement.MOBILE;

public partial class App : Application
{
    
    public App() 
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}