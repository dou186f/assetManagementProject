namespace assetManagement.API.Models
{
    public class AssetModel
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string serialNumber { get; set; } = string.Empty;
        public int categoryId { get; set; } = 0;
        public int? ownerId { get; set; } = 0;
        public DateTime? assignedAt { get; set; } = null;
    }
}
