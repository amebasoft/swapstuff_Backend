using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;

namespace SwapStff.Service
{
    public interface IUserService
    {
        List<User> GetAll();
        User GetById(string id);
        List<User> GetUsers();
        void Insert(User model);
        void Update(User model);
        void Delete(User model);
    }
}
