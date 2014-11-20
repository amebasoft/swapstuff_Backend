using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapStff.Entity
{
    public class ItemMatch 
    {
        public int ItemMatchID { get; set; }
        public int ItemID { get; set; }
        public Nullable<int> ProfileIdBy { get; set; }
        public Nullable<decimal> Distance { get; set; }
        public Nullable<byte> IsLikeDislikeAbuseBy { get; set; }
        public Nullable<System.DateTime> DateTimeCreated { get; set; }
        public string AbuseMessage { get; set; }

        public virtual Item Item { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
