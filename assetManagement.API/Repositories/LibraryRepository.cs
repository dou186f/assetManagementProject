using Microsoft.EntityFrameworkCore;
using assetManagement.API.Data;
using assetManagement.API.Dtos;
using assetManagement.API.Interfaces;

namespace assetManagement.API.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly DataContext _context;
        public LibraryRepository(DataContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<LibraryItemDto>> SearchAsync(string? q, string field = "All", string type = "All", int take = 200)
        {
            const string CS = "Turkish_100_CI_AI";
            const string CS_LATIN = "Latin1_General_100_CI_AI";

            var employees = _context.employees.AsNoTracking()
                .Select(c => new
                {
                    Type = "Employee",
                    Id = c.id,
                    Name = c.name,
                    Serial = (string?)null,
                    Regi = (int?)c.regiNumber
                });

            var departments = _context.departments.AsNoTracking()
                .Select(d => new
                {
                    Type = "Department",
                    Id = d.id,
                    Name = d.name,
                    Serial = (string?)null,
                    Regi = (int?)null
                });

            var assets = _context.assets.AsNoTracking()
                .Select(e => new
                {
                    Type = "Asset",
                    Id = e.id,
                    Name = e.name,
                    Serial = (string?)e.serialNumber,
                    Regi = (int?)null
                });

            var categories = _context.categories.AsNoTracking()
                .Select(k => new
                {
                    Type = "Category",
                    Id = k.id,
                    Name = k.name,
                    Serial = (string?)null,
                    Regi = (int?)null
                });

            var typeLower = (type ?? "All").ToLowerInvariant();
            var qset = typeLower switch
            {
                "employee" => employees,
                "asset" => assets,
                "category" => categories,
                "department" => departments,
                _ => employees.Concat(assets)
            };

            if (!string.IsNullOrWhiteSpace(q))
            {
                var t = q.Trim();
                var fieldLower = (field ?? "All").ToLowerInvariant();

                switch (fieldLower)
                {
                    case "employee":
                        qset = qset.Where(x => x.Type == "Employee" && EF.Functions.Like(EF.Functions.Collate(x.Name, CS), $"%{t}%") ||
                                                                       EF.Functions.Like(EF.Functions.Collate(x.Name, CS_LATIN), $"%{t}%"));
                        break;

                    case "department":
                        qset = qset.Where(x => x.Type == "Department" && EF.Functions.Like(EF.Functions.Collate(x.Name, CS), $"%{t}%") ||
                                                                         EF.Functions.Like(EF.Functions.Collate(x.Name, CS_LATIN), $"%{t}%"));
                        break;

                    case "asset":
                        qset = qset.Where(x => x.Type == "Asset" && EF.Functions.Like(EF.Functions.Collate(x.Name, CS), $"%{t}%") ||
                                                                    EF.Functions.Like(EF.Functions.Collate(x.Name, CS_LATIN), $"%{t}%"));
                        break;

                    case "category":
                        qset = qset.Where(x => x.Type == "Category" && EF.Functions.Like(EF.Functions.Collate(x.Name, CS), $"%{t}%") ||
                                                                       EF.Functions.Like(EF.Functions.Collate(x.Name, CS_LATIN), $"%{t}%"));
                        break;

                    case "serialnumber":
                        qset = qset.Where(x => x.Type == "Asset" && x.Serial != null &&
                                               EF.Functions.Like(EF.Functions.Collate(x.Serial, CS), $"%{t}%") ||
                                                                 EF.Functions.Like(EF.Functions.Collate(x.Serial, CS_LATIN), $"%{t}%"));
                        break;

                    case "reginumber":
                        {
                            int? parsed = int.TryParse(t, out var rn) ? rn : (int?)null;
                            if (parsed.HasValue)
                                qset = qset.Where(x => x.Type == "Employee" && x.Regi == parsed.Value);
                            else
                                qset = qset.Where(x => x.Type == "Employee" && EF.Functions.Like(EF.Functions.Collate(x.Name, CS), $"%{t}%") ||
                                                                               EF.Functions.Like(EF.Functions.Collate(x.Name, CS_LATIN), $"%{t}%"));
                            break;
                        }

                    case "all":
                    default:
                        {
                            int? parsed = int.TryParse(t, out var rn) ? rn : (int?)null;
                            qset = qset.Where(x =>
                                EF.Functions.Like(EF.Functions.Collate(x.Name, CS), $"%{t}%") ||
                                                    EF.Functions.Like(EF.Functions.Collate(x.Name, CS_LATIN), $"%{t}%") ||
                                (x.Serial != null && EF.Functions.Like(EF.Functions.Collate(x.Serial, CS), $"%{t}%")) ||
                                                    EF.Functions.Like(EF.Functions.Collate(x.Serial, CS_LATIN), $"%{t}%") ||
                                (parsed.HasValue && x.Regi == parsed.Value)
                            );
                            break;
                        }
                }
            }

            var limited = qset
                .OrderBy(x => x.Type)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.Id)
                .Take(Math.Clamp(take, 1, 500));

            var list = await limited
                .Select(x => new LibraryItemDto(
                    x.Type,
                    x.Id,
                    x.Type == "Employee" ? x.Name : null, 
                    x.Type == "Department" ? x.Name : null,
                    x.Type == "Asset" ? x.Name : null,
                    x.Type == "Category" ? x.Name : null,
                    x.Serial,
                    x.Regi
                ))
                .ToListAsync();

            return list;
        }
    }
}