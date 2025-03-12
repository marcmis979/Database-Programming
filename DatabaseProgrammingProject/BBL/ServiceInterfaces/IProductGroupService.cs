using BBL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL.ServiceInterfaces
{
    public interface IProductGroupService
    {
        IEnumerable<ProductGroupDTO> GetGroups(int? parentId = null, string? sortBy = "Name", bool ascending = true);
        void AddGroup(string groupName, int? parentId = null);
    }
}
