using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Models
{
    public class UserChatsModel
    {
        [Display(Name = "Chat Id")]
        public int ChatId{get;set; }
 
        [Display(Name = "Item Id (Sender)")]
        public int ItemIDBy{get;set;}

        [Display(Name = "Item Title (Sender)")]
        public string ItemTitleBy{get;set;}

        [Display(Name = "Item Image (Sender)")]
        public string ItemImageBy{get;set;}

        [Display(Name = "ProfileId (Sender)")]
        public Nullable<int> ProfileIdBy { get; set; }

        [Display(Name = "Item Id (Recipient)")]
        public int ItemIDTo { get; set; }

        [Display(Name = "Item Title (Recipient)")]
        public string ItemTitleTo { get; set; }

        [Display(Name = "Item Image (Recipient)")]
        public string ItemImageTo { get; set; } 

        [Display(Name = "ProfileId (Recipient)")]
        public Nullable<int> ProfileIdTo { get; set; }

        [Display(Name = "Chat Content")]
        public string ChatContent { get; set; }

        [Display(Name = "Date Time Created")]
        public Nullable<System.DateTime> DateTimeCreated { get; set; }

        [Display(Name = "Is Read")]
        public Nullable<bool> IsRead { get; set; }
        

    }
}
