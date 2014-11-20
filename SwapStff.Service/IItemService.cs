using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;

namespace SwapStff.Service
{
    public interface IItemService
    {
        List<Item> GetAll();
        Item GetById(string id);
        List<Item> GetItemsWOImage();
        void Insert(Item model);
        void Update(Item model);
        void Delete(Item model);
    }
}
