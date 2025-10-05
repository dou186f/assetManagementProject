using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using assetManagement.MOBILE.Models;
using assetManagement.MOBILE.Services;
using static assetManagement.MOBILE.AppShell;

namespace assetManagement.MOBILE.ViewModels
{
    [QueryProperty(nameof(CategoryId), "id")]
    public class CategoryDetailViewModel : INotifyPropertyChanged
    {
        private readonly CategoryService _categoryservice;
        private readonly AssetService _assetservice;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<AssetModel> Assets { get; } = new();
        void OnPropertyChanged([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }

        private CategoryModel _category = new();
        public CategoryModel Category { get => _category; set { _category = value; OnPropertyChanged(); } }

        private int _categoryId;
        public int CategoryId { get => _categoryId; set { _categoryId = value; _ = LoadAsync(value); } }
        public ICommand OpenItemDetailCommand { get; }

        public CategoryDetailViewModel(CategoryService categoryservice, AssetService assetservice)
        {
            _categoryservice = categoryservice;
            _assetservice = assetservice;

            OpenItemDetailCommand = new Command<AssetModel>(async item => await OpenItemDetailAsync(item));
        }

        private async Task LoadAsync(int id)
        {
            if (id <= 0 || IsBusy) return;
            IsBusy = true;
            try
            {
                Assets.Clear();
                var dto = await _categoryservice.GetCategoryByIdAsync(id);
                Category = dto ?? new CategoryModel { id = id, name = "Not Found!" };

                var list = await _assetservice.GetAssetsAllAsync();
                foreach (var asset in list)
                {
                    if( asset.categoryId == CategoryId )
                    {
                        Assets.Add(asset);
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task OpenItemDetailAsync(AssetModel item)
        {
            if (item == null) return;
            var nav = new Dictionary<string, object> { ["id"] = item.id };
            await Shell.Current.GoToAsync(AssetDetailRoute, nav);
        }

    }
}
