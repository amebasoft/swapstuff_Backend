using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;

namespace SwapStff.Service
{
    public interface IProfileService
    {
        List<Profile> GetAll();
        Profile GetById(string id);
        void Insert(Profile model);
        void Update(Profile model);
        void Delete(Profile model);
    }
}
