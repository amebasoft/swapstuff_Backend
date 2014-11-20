using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;

namespace SwapStff.Service
{
    public interface IItemMatchService
    {
        List<ItemMatch> GetAll();
        ItemMatch GetById(string id);
        void Insert(ItemMatch model);
        void Update(ItemMatch model);
        void Delete(ItemMatch model);
    }
}
