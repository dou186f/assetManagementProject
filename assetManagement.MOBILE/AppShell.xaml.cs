using assetManagement.MOBILE.Pages;

namespace assetManagement.MOBILE
{
    public partial class AppShell : Shell
    {
        public const string EmployeeDetailRoute = "employeeDetail";
        public const string CategoryDetailRoute = "categoryDetail";
        public const string AssetDetailRoute = "assetDetail";
        public const string DepartmentDetailRoute = "departmentDetail";
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(EmployeeDetailRoute, typeof(EmployeeDetailPage));
            Routing.RegisterRoute(CategoryDetailRoute, typeof(CategoryDetailPage));
            Routing.RegisterRoute(AssetDetailRoute, typeof(AssetDetailPage));
            Routing.RegisterRoute(DepartmentDetailRoute, typeof(DepartmentDetailPage));
        }
    }
}
