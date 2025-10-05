using System.Collections.ObjectModel;
using System.Windows.Input;
using assetManagement.MOBILE.Models;
using assetManagement.MOBILE.Services;
using static assetManagement.MOBILE.AppShell;

namespace assetManagement.MOBILE.Pages;

public partial class CategoryPage : ContentPage
{
    public ObservableCollection<CategoryModel> Categories { get; } = new();

    private readonly CategoryService _categoryService;
    public ICommand OpenItemDetailCommand { get; }
    public CategoryPage(CategoryService categoryService)
    {
        InitializeComponent();
        _categoryService = categoryService;
        OpenItemDetailCommand = new Command<CategoryModel>(async item => await OpenItemDetailAsync(item));
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
            var list = await _categoryService.GetAllAsync();
            Categories.Clear();
            foreach (var b in list) Categories.Add(b);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata (Load)", ex.Message, "OK");
        }
    }

    private async Task OpenItemDetailAsync(CategoryModel item)
    {
        if (item == null) return;
        var nav = new Dictionary<string, object> { ["id"] = item.id };
        await Shell.Current.GoToAsync(CategoryDetailRoute, nav);
    }
}