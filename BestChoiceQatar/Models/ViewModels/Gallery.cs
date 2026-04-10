using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
 

namespace BestChoiceQatar.Models
{
    public class GalleryMetaData
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }
        public string ChildImages { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public bool IsDataActive { get; set; }
    }

    [MetadataType(typeof(GalleryMetaData))]
    public partial class Gallery
    {
        public HttpPostedFileBase Photo { get; set; }
        public IEnumerable<HttpPostedFileBase> FileUpload { get; set; }


    }
}