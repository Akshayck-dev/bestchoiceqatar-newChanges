using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BestChoiceQatar.Models
{
    public class AppointmentMetaData
    {
        public int ID { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please enter Full Name")]
        public string FullName { get; set; }
        
        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "Please enter Contact Number")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Please enter Emailid")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Please select the Date")]
        public System.DateTime Appointment_Date { get; set; }

        [Display(Name = "Created On")]
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public bool IsDataActive { get; set; }

    }
    [MetadataType(typeof(AppointmentMetaData))]
    public partial class Appointment
    {
        public HttpPostedFileBase Upload { get; set; }
        public string Emails { get; set; }


    }
}