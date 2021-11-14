using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using JobOffersWebsite.Models;

namespace Job_Offers_Website.Models
{
    public class ApplyForJob
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Message { get; set; }
        public DateTime ApplyDate { get; set; }
        public int JobId { get; set; }
        public string UserId { get; set; }

        //Jobes Table 
        public virtual Job Job { get; set; }
         
        // User Table
        public virtual ApplicationUser user { get; set; }
    } 
}