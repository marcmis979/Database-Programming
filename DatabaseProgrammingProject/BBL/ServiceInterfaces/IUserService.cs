using BBL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL.ServiceInterfaces
{
    public interface IUserService
    {
        bool Login(UserDTO user);
        void Logout();
    }
}
