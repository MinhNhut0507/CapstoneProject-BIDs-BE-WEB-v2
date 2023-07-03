using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class Item
    {
        public Item()
        {
            BookingItems = new HashSet<BookingItem>();
            ItemDescriptions = new HashSet<ItemDescription>();
            Sessions = new HashSet<Session>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string DescriptionDetail { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public double FirstPrice { get; set; }
        public double StepPrice { get; set; }
        public bool Deposit { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<BookingItem> BookingItems { get; set; }
        public virtual ICollection<ItemDescription> ItemDescriptions { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
