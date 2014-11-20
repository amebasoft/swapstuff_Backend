using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwapStff.Models
{
    public class ChatModel
    {
        public int ChatId { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<int> ProfileIdBy { get; set; }
        public Nullable<int> ProfileIdTo { get; set; }
        public string ChatContent { get; set; }
        public Nullable<System.DateTime> DateTimeCreated { get; set; }
        public Nullable<bool> IsRead { get; set; }

        public Nullable<int> CountMessage { get; set; }
    }
}