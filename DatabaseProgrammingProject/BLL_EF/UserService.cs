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
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public static User? LoggedUser { get; private set; }

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public bool Login(UserDTO user)
        {
            if (LoggedUser != null)
                return true;

            var existingUser = _context.Users
                .FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);

            if (existingUser != null)
            {
                LoggedUser = existingUser;
                return true;
            }

            return false;
        }

        public void Logout()
        {
            LoggedUser = null;
        }
    }
}
