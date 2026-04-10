using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestChoiceQatar.Models
{
    public class ServiceModel
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string DetailedContent { get; set; }
        public List<string> Features { get; set; }
        public List<KeyValuePair<string, string>> Specifications { get; set; }
        public string MainImage { get; set; }
        public List<string> GalleryImages { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
    }
}
