namespace assetManagement.API.Models
{
    public class EmployeeModel
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int regiNumber { get; set; }
        public int? departId { get; set; }
    }
}
