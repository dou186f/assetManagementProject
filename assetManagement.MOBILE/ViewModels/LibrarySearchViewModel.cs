using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using assetManagement.MOBILE.Services;
using assetManagement.MOBILE.Dtos;
using static assetManagement.MOBILE.AppShell;

namespace assetManagement.MOBILE.ViewModels;
public sealed class FilterOption
{
    public string Display { get; init; } = "";
}
public class LibrarySearchViewModel : INotifyPropertyChanged
{
    private readonly LibraryServices _service;
    public ObservableCollection<LibraryItemDto> Results { get; } = new();
    public ObservableCollection<FilterOption> TypeOptions { get; } = new(new[]
    {
        new FilterOption { Display = "All" },
        new FilterOption { Display = "Employee" },
        new FilterOption { Display = "Asset" },
    });

    private FilterOption _selectedFilter;
    public FilterOption SelectedFilter
    {
        get => _selectedFilter;
        set { _selectedFilter = value; OnPropertyChanged(); }
    }

    private string? _query;
    public string? Query
    {
        get => _query;
        set     
        {   
            _query = value; OnPropertyChanged(); 
        }
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    private bool _isFilterVisible = false;
    public bool IsFilterVisible
    {
        get => _isFilterVisible;
        set { _isFilterVisible = value; OnPropertyChanged(); }
    }

    private bool _isWelcomeVisible = true;
    public bool IsWelcomeVisible
    {
        get => _isWelcomeVisible;
        set { if (_isWelcomeVisible != value) { _isWelcomeVisible = value; OnPropertyChanged(); } }
    }

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); }
    }
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public ICommand SearchCommand { get; }
    public ICommand ToggleFilterCommand { get; }
    public ICommand ApplyFilterCommand { get; }
    public ICommand OpenItemDetailCommand { get; }

    public LibrarySearchViewModel(LibraryServices service)
    {
        _service = service;
        _selectedFilter = TypeOptions[0];
        SearchCommand = new Command(async () => await RunSearch());
        ToggleFilterCommand = new Command(() => IsFilterVisible = !IsFilterVisible);
        ApplyFilterCommand = new Command(() =>
        {
            IsFilterVisible = false;
        });
        OpenItemDetailCommand = new Command<LibraryItemDto>(async item => await OpenItemDetailAsync(item));
    }
    private async Task RunSearch()
    {
        if (IsBusy) return;
        IsBusy = true;
        ErrorMessage = null;
        try
        {
            if (IsWelcomeVisible)
                IsWelcomeVisible = false;

            Results.Clear();

            string typeForApi = SelectedFilter?.Display ?? "All";

            List<LibraryItemDto> list;
            try
            {
                var data = await _service.SearchAsync(Query, "all", typeForApi, 200);
                list = data?.ToList() ?? new List<LibraryItemDto>();
            }
            catch (HttpRequestException)
            {
                ErrorMessage = "The server could not be reached. Please check your internet connection or API address.";
                return;
            }
            catch (TaskCanceledException)
            {
                ErrorMessage = "The request timed out. Try again.";
                return;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return;
            }
            foreach (var item in list) Results.Add(item);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task OpenItemDetailAsync(LibraryItemDto item)
    {
        if (item == null) return;
        if (string.Equals(item.Type, "Employee", StringComparison.OrdinalIgnoreCase))
        {
            var nav = new Dictionary<string, object> { ["id"] = item.Id };
            await Shell.Current.GoToAsync(EmployeeDetailRoute, nav);
        }
        if (string.Equals(item.Type, "Asset", StringComparison.OrdinalIgnoreCase))
        {
            var nav = new Dictionary<string, object> { ["id"] = item.Id };
            await Shell.Current.GoToAsync(AssetDetailRoute, nav);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}