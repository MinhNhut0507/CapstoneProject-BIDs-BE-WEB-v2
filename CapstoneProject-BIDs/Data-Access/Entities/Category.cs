using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class Category
    {
        public Category()
        {
            Descriptions = new HashSet<Description>();
            Items = new HashSet<Item>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Description> Descriptions { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
