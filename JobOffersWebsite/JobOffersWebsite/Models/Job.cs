using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobOffersWebsite.Models;

namespace Job_Offers_Website.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [Display(Name ="اسم الوظيفة")]
        public string Title { get; set; }

        [Required]
        [MaxLength(255)]
        [AllowHtml]
        [Display(Name = "وصف الوظيفة")]
        public string JobContent { get; set; }

        [Display(Name = "مدة العمل اليومي")]
        public string JobTime { get; set; }

        [Display(Name = "نوع الوظيفة")]
        public int CategoryId { get; set; }

        public string UserID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}