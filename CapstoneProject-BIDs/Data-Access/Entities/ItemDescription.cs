using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public partial class ItemDescription
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid DescriptionId { get; set; }
        public string Detail { get; set; }
        public bool Status { get; set; }

        public virtual Item Item { get; set; }
        public virtual Description Description { get; set; }
    }
}
