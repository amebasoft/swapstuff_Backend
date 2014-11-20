using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Models
{
    public class ItemModel
    {
        [Display(Name = "Item ID")]
        public int ItemID { get; set; }

        [Display(Name = "Profile ID")]
        public int ProfileID { get; set; }

        [Display(Name = "Item Title")]
        public string ItemTitle { get; set; }

        [Display(Name = "Item Description")]
        public string ItemDescription { get; set; }

        [Display(Name = "Image")]
        public string ItemImage { get; set; }

        [Display(Name = "Created On")]
        public Nullable<System.DateTime> ItemDateTimeCreated { get; set; }

        [Display(Name = "Is Active")]
        public Nullable<bool> IsActive { get; set; }

        public double Distance { get; set; }
        public byte DeleteFirst { get; set; }
    }
}