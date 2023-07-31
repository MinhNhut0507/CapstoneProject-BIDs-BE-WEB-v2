using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string DetailImage { get; set; }
        public bool Status { get; set; }

        public virtual Item Item { get; set; } 
    }
}
