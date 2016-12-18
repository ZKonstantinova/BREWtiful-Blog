using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeerBlog.Models
{
    public class Section
    {
        private ICollection<Category> categories;
        public Section ()
        {
            this.categories = new HashSet<Category>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(20)]
        public string Name { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}