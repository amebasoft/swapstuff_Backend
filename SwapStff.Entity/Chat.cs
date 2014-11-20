using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapStff.Entity
{
    public class Chat
    {
        public int ChatId { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<int> ProfileIdBy { get; set; }
        public Nullable<int> ProfileIdTo { get; set; }
        public string ChatContent { get; set; }
        public Nullable<System.DateTime> DateTimeCreated { get; set; }
        public Nullable<bool> IsRead { get; set; }

        public virtual Item Item { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Profile Profile1 { get; set; }
    }
}
