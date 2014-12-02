using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SwapStff.Models
{
    public class ErrorExceptionLogsModel
    {
        [Display(Name = "Event Id")]
        public int EventId { get; set; }
        [Display(Name = "Log Date Time")]
        public Nullable<DateTime> LogDateTime { get; set; }
        [Display(Name = "Source")]
        public string Source { get; set; }
        [Display(Name = "Message")]
        public string Message { get; set; }
        [Display(Name = "Query String")]
        public string QueryString { get; set; }
        [Display(Name = "Target Site")]
        public string TargetSite { get; set; }
        [Display(Name = "Stack Trace")]
        public string StackTrace { get; set; }
        [Display(Name = "Server Name")]
        public string ServerName { get; set; }
        [Display(Name = "Request URL")]
        public string RequestURL { get; set; }
        [Display(Name = "User Agent")]
        public string UserAgent { get; set; }
        [Display(Name = "User IP")]
        public string UserIP { get; set; }
        [Display(Name = "User Authentication")]
        public string UserAuthentication { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }
}