using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeerBlog.Models
{
    public class Category
    {
        
        private ICollection<Article> articles;

        public Category ()
        {
            this.articles = new HashSet<Article>();
        }

        public Category(string name, int sectionId)
        {
            this.Name = name;
            this.SectionId = sectionId;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(30)]
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        [ForeignKey("Section")]
        public int SectionId { get; set; }

        public virtual Section Section { get; set; }

    
    }
}