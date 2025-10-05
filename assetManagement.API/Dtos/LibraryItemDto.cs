namespace assetManagement.API.Dtos
{
    public record LibraryItemDto
    (
        string Type,
        int Id,
        string? EmployeeName,
        string? DepartmentName,
        string? AssetName,
        string? CategoryName,
        string? SerialNum,
        int? RegiNum
    );
}
