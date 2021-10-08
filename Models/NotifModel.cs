using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SendNotificationProject.Models
{
    public class NotifModel
    {
        public int ID { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string text { get; set; }

       
        public string icon { get; set; }
      
    
        //[Required]
        [DisplayName("Upload image")]

        public string image { get; set; }
        [Required]
        public string clickAction { get; set; }
        public string test { get; set; }
    }
}