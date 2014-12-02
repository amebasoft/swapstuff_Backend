using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Models
{
    public class UserItemMatchModel
    {
        [Display(Name = "Profile Id1")]
        public Nullable<int> ProfileID1 { get; set; }

        [Display(Name = "Item Id1")]
        public int ItemID1 { get; set; }

        [Display(Name = "Item Name1")]
        public string ItemTitle1 { get; set; }

        [Display(Name = "Item Image1")]
        public string ItemImage1 { get; set; }

        [Display(Name = "Profile Id2")]
        public Nullable<int> ProfileID2 { get; set; }

        [Display(Name = "Item Id2")]
        public int ItemID2 { get; set; }

        [Display(Name = "Item Name2")]
        public string ItemTitle2 { get; set; }

        [Display(Name = "Item Image2")]
        public string ItemImage2 { get; set; }

        [Display(Name = "Distance")]
        public double Distance { get; set; }

    }
}