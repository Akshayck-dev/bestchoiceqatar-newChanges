using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BestChoiceQatar.Models
{
    public class EventMetaData
    {
        public int ID { get; set; }
        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please enter the Subject")]
        public string Subject { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please enter the Description")]
        public string description { get; set; }
        [Required(ErrorMessage = "Please enter the Position")]
        public int Position { get; set; }
        //[Display(Name = "Is Active")]
        //public bool IsActive { get; set; }
        [Display(Name = "Created On")]
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime LastUpdatedOn { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        [Display(Name = "Is Active")]
        public bool IsDataActive { get; set; }
        public string Image { get; set; }
    }
    [MetadataType(typeof(EventMetaData))]
    public partial class Event
    {
        public HttpPostedFileBase Photo { get; set; }
    }
}