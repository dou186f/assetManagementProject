using assetManagement.MOBILE.Services;
using assetManagement.MOBILE.ViewModels;
using assetManagement.MOBILE.Pages;

namespace assetManagement.MOBILE;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; } = default!;
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
             {
                 fonts.AddFont("Ubuntu-Bold.ttf", "Ubold");
                 fonts.AddFont("Ubuntu-BoldItalic.ttf", "Uboldi");
                 fonts.AddFont("Ubuntu-Italic.ttf", "Uital");
                 fonts.AddFont("Ubuntu-Light.ttf", "Ulight");
                 fonts.AddFont("Ubuntu-LightItalic.ttf", "Ulighti");
                 fonts.AddFont("Ubuntu-Medium.ttf", "Umed");
                 fonts.AddFont("Ubuntu-MediumItalic.ttf", "Umedi");
                 fonts.AddFont("Ubuntu-Regular.ttf", "Uregu");
             });

#if DEBUG
        var baseUri = new Uri("https://localhost:1234");
#else
        var baseUri = new Uri("https://localhost:1234");
#endif

        builder.Services.AddSingleton(new HttpClient { BaseAddress = baseUri });

        builder.Services.AddSingleton<LibraryServices>();
        builder.Services.AddSingleton<CategoryService>();
        builder.Services.AddSingleton<DepartmentService>();
        builder.Services.AddSingleton<EmployeeService>();
        builder.Services.AddSingleton<AssetService>();

        builder.Services.AddTransient<LibrarySearchViewModel>();
        builder.Services.AddTransient<AssetDetailViewModel>();
        builder.Services.AddTransient<CategoryDetailViewModel>();
        builder.Services.AddTransient<EmployeeDetailViewModel>();
        builder.Services.AddTransient<DepartmentDetailViewModel>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<CategoryPage>();
        builder.Services.AddTransient<DepartmentPage>();

        var app = builder.Build();
        Services = app.Services;

        return app;
    }
}
