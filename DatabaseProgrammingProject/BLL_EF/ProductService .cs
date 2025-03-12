using BBL.DTOModels;
using BBL.ServiceInterfaces;
using DAL;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_EF
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public IEnumerable<ProductResponseDTO> GetProducts(
             string sortBy = "Name",
             bool ascending = true,
             string? nameFilter = null,
             string? groupNameFilter = null,
             int? groupIdFilter = null,
             bool includeInactive = false)
        {
            var query = _context.Products
                .Include(p => p.Group)
                .ThenInclude(g => g.ParentGroup)
                .Where(p => includeInactive || p.IsActive);

            if (!string.IsNullOrEmpty(nameFilter))
                query = query.Where(p => p.Name.Contains(nameFilter));

            if (groupIdFilter.HasValue)
                query = query.Where(p => p.GroupID == groupIdFilter.Value);

            if (!string.IsNullOrEmpty(groupNameFilter))
                query = query.Where(p => p.Group.Name.Contains(groupNameFilter)); //czy produkt ma id grupy

            query = sortBy switch
            {
                "Price" => ascending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                "GroupName" => ascending ? query.OrderBy(p => p.Group.Name) : query.OrderByDescending(p => p.Group.Name),
                _ => ascending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
            };

            return query.Select(p => new ProductResponseDTO(
                p.ID,
                p.Name,
                p.Price,
                GetFormattedGroupName(p.Group)
            )).ToList();
        }

        private string GetFormattedGroupName(ProductGroup group)
        {
            var groupNames = new List<string>();
            var currentGroup = group;
            while (currentGroup != null)
            {
                groupNames.Insert(0, currentGroup.Name);
                currentGroup = currentGroup.ParentGroup; //z contextu 
            }

            return string.Join(" / ", groupNames);
        }


        public void AddProduct(ProductRequestDTO productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                GroupID = productDto.GroupId,
                IsActive = true
            };
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void DeactivateProduct(int productId)
        {
            var product = _context.Products
                .Include(p => p.OrderPositions)
                .Include(p => p.BasketPositions)
                .FirstOrDefault(p => p.ID == productId);

            if (product != null)
            {
                if (product.OrderPositions.Any() || product.BasketPositions.Any())
                {
                    product.IsActive = false;
                }
                else
                {
                    _context.Products.Remove(product);
                }
                _context.SaveChanges();
            }
        }


        public void ActiveProduct(int productID)
        {
            var product = _context.Products.Find(productID);
            if (product != null)
            {
                product.IsActive = true;
                _context.SaveChanges();
            }
        }
    }
}
