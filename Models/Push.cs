using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SendNotificationProject.Models
{
    public class Push
    {
        public String to { get; set; }

        public objectnotification notification { get; set; }
     
    }

    public class objectnotification
    {
        public string title { get; set; }
        public string body { get; set; }
        public string icon { get; set; }
        public string image { get; set; }
        public string click_action { get; set; }
    }
}