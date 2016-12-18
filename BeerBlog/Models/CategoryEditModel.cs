using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeerBlog.Models
{
    public class CategoryEditModel
    {
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(30)]
        public string Name { get; set; }

        public int SectionId { get; set; }
        public List<Section> Sections { get; set; }

        public ICollection<Category> Categories { get; set; }

    }
}