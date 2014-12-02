using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Models
{
    public class AbuseItemsModel
    {
        [Display(Name = "Item ID")]
        public int ItemID { get; set; }

        [Display(Name = "Profile ID")]
        public int ProfileID { get; set; }

        [Display(Name = "Item Title")]
        public string ItemTitle { get; set; }

        [Display(Name = "Item Description")]
        public string ItemDescription { get; set; }

        [Display(Name = "Item Image")]
        public string ItemImage { get; set; }

        [Display(Name = "ProfileId By")]
        public Nullable<int> ProfileIdBy { get; set; }

        [Display(Name = "IsLikeDislikeAbuseBy")]
        public Nullable<byte> IsLikeDislikeAbuseBy { get; set; }

        [Display(Name = "Abuse Message")]
        public string AbuseMessage { get; set; }
    }
}