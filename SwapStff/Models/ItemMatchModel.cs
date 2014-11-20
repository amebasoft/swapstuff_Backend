using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwapStff.Models
{
    public class ItemMatchModel
    {
        public int ItemMatchID { get; set; }
        public int ItemID { get; set; }
        public Nullable<int> ProfileIdBy { get; set; }
        public Nullable<decimal> Distance { get; set; }
        public Nullable<byte> IsLikeDislikeAbuseBy { get; set; }
        public Nullable<System.DateTime> DateTimeCreated { get; set; }
        public string ChatMessage { get; set; }
        public Nullable<int> MessageCount { get; set; }
        public string AbuseMessage { get; set; }

        public string ItemTitle { get; set; }
        public string ItemImage { get; set; }
    }
}