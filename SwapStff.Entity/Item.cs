using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapStff.Entity
{
    public class Item 
    {
        public int ItemID { get; set; }
        public int ProfileID { get; set; }
        public string ItemTitle { get; set; }
        public string ItemDescription { get; set; }
        public string ItemImage { get; set; }
        public Nullable<System.DateTime> ItemDateTimeCreated { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<ItemMatch> ItemMatches { get; set; }
        public virtual Profile Profiles { get; set; }
    }
}
