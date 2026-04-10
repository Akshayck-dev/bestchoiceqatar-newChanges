using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BestChoiceQatar.Models
{
    public class SettingMetaData
    {
        [Display(Name = "Popup Text")]
        public string IntroText { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        [Display(Name = "Primary Mobile Number")]
        public string Mobile1 { get; set; }
        [Display(Name = "secondary mobile number")]
        public string Mobile2 { get; set; }
        [Display(Name = "WhatsApp Link")]
        public string Whatsup { get; set; }
        [Display(Name = "Instagram Link")]
        public string Instagram { get; set; }
        [Display(Name = "Facebook Link")]
        public string Facebook { get; set; }
        [Display(Name = "Twitter Link")]
        public string Twitter { get; set; }
        [Display(Name = "LinkedIn Link")]
        public string LinkedIn { get; set; }
        [Display(Name = "About Page Text")]
        public string AboutText { get; set; }
        [Display(Name = "Home page Intro Text")]
        public string HompageIntroText { get; set; }
        public System.DateTime Lastupdatedon { get; set; }
    }
}