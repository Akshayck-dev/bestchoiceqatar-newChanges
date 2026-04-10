using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BestChoiceQatar.Models
{
    public class UserMetaData
    {
        public int ID { get; set; }
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage ="Please enter the Email")]
        //[MaxLength(50, ErrorMessage = "Email should not have more than 50 characters")]
        [EmailAddress(ErrorMessage = "Please enter a valid Email Address")]
        public string Email { get; set; }
        [Display(Name = "Mobile Number")]
        [MaxLength(50, ErrorMessage = "Mobile Number should not have more than 50 characters")]
        [Required(ErrorMessage = "Please enter the Mobile Number")]
        public string MobileNumber { get; set; }
        public byte[] AuthData { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Is Super Admin")]
        public bool IsSuperAdmin { get; set; }
        [Display(Name = "Created On")]
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime LastUpdatedOn { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public bool IsDataActive { get; set; }
    }
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {        
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        //[Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        
       
    }
}