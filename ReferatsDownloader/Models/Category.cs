using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReferatsDownloader.Models
{
    public class Category: DatabaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Referat> Referats { get; set; } = new List<Referat>();
    }
}
