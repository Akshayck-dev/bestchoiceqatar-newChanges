using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestChoiceQatar.Core.Entities.Common
{
    public class Select2Item
    {
        public int id { get; set; }

        public string text { get; set; }

        public static implicit operator List<object>(Select2Item v)
        {
            throw new NotImplementedException();
        }
    }
}