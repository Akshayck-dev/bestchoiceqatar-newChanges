using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BestChoiceQatar.Models
{
    public class EnquiryMetaData
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public bool IsDataActive { get; set; }
    }
    [MetadataType(typeof(EnquiryMetaData))]
    public partial class Enquiry
    {
        

    }
}