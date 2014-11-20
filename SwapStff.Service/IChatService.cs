using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;

namespace SwapStff.Service
{
    public interface IChatService
    {
        List<Chat> GetAll();
        Chat GetById(string id);
        void Insert(Chat model);
        void Update(Chat model);
        void Delete(Chat model);
    }
}
