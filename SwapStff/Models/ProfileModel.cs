using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Models
{
    public class ProfileModel
    {
        [Display(Name = "Profile ID")]
        public int ProfileId { get; set; }
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Display(Name = "Latitude")]
        public Nullable<decimal> Latitude { get; set; }

        [Display(Name = "Longitude")]
        public Nullable<decimal> Longitude { get; set; }

        [Display(Name = "Created On")]
        public Nullable<System.DateTime> DateTimeCreated { get; set; }

        [Display(Name = "Distance")]
        public Nullable<decimal> Distance { get; set; }

        [Display(Name = "Device ID")]
        public string GCM_RegistrationID { get; set; }

        [Display(Name = "Match Notification")]
        public int ItemMatchNotification { get; set; }

        [Display(Name = "Chat Notification")]
        public int ChatNotification { get; set; }
    }
}