using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class Item
    {
        public Item()
        {
            BookingItems = new BookingItem();
            Sessions = new Session();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string DescriptionDetail { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public double FristPrice { get; set; }
        public double StepPrice { get; set; }
        public bool Deposit { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; }

        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
        public virtual BookingItem BookingItems { get; set; }
        public virtual Session Sessions { get; set; }
    }
}
