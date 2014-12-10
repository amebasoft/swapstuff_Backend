using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Models
{
    public class AppSettingModel
    {
        public int SettingID { get; set; }

        [Display(Name = "Max Distance")]
        public int MaxDistance { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}