
using BBL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL.ServiceInterfaces
{
    public interface IProductService
    {
        IEnumerable<ProductResponseDTO> GetProducts(
           string sortBy = "Name",
           bool ascending = true,
           string? nameFilter = null,
           string? groupNameFilter = null,
           int? groupIdFilter = null,
           bool includeInactive = false);

        void AddProduct(ProductRequestDTO product);
        void DeactivateProduct(int productId); //usuwamy jeśli relacja na to pozwala
        void ActiveProduct(int productId);
    }
}
