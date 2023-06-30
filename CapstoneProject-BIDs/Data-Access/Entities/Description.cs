﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class Description
    {
        public Description()
        {
            ItemDescriptions = new HashSet<ItemDescription>();
        }

        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public bool Status { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<ItemDescription> ItemDescriptions { get; set; }

    }
}
