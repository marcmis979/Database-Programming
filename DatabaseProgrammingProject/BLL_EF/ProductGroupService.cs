using BBL.DTOModels;
using BBL.ServiceInterfaces;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_EF
{
    public class ProductGroupService : IProductGroupService
    {
        private readonly AppDbContext _context;

        public ProductGroupService(AppDbContext context)
        {
            _context = context;
        }

        public void AddGroup(string groupName, int? parentID = null)
        {
            var group = new ProductGroup
            {

                Name = groupName,
                ParentID = parentID
            };
            _context.ProductGroups.Add(group);
            _context.SaveChanges();
        }

        public IEnumerable<ProductGroupDTO> GetGroups(int? parentId = null, string sortBy = "Name", bool ascending = true)
        {
            var query = _context.ProductGroups.AsQueryable();

            if (parentId.HasValue)
            {
                query = query.Where(pg => pg.ParentID == parentId);
            }

            query = sortBy.ToLower() switch
            {
                "name" => ascending ? query.OrderBy(pg => pg.Name) : query.OrderByDescending(pg => pg.Name),
                _ => query.OrderBy(pg => pg.ID)
            };

            return query.Select(pg => new ProductGroupDTO(
                pg.ID,
                GetFullGroupName(pg),
                pg.ChildGroups.Any()
            )).ToList();
        }
        private string GetFullGroupName(ProductGroup group)
        {
            List<string> names = new();
            while (group != null)
            {
                names.Insert(0, group.Name);
                group = _context.ProductGroups.FirstOrDefault(g => g.ID == group.ParentID);
            }
            return string.Join(" > ", names);
        }
    }
}
