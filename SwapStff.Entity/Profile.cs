using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SwapStff.Entity
{
    public class Profile //: BaseEntity<int>
    {
        public int ProfileId { get; set; }
        public string Username { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<System.DateTime> DateTimeCreated { get; set; }
        public Nullable<decimal> Distance { get; set; }
        public string GCM_RegistrationID { get; set; }
        public int ItemMatchNotification { get; set; }
        public int ChatNotification { get; set; }
        
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<Chat> Chats1 { get; set; }
        public virtual ICollection<ItemMatch> ItemMatches { get; set; }
        public virtual ICollection<Item> Items { get; set; }

    }
}
