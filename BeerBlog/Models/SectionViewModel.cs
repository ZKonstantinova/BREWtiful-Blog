using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeerBlog.Models
{
    public class SectionViewModel
    {
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(30)]
        public string Name { get; set; }

        public int SectionnName { get; set; }
        public List<Section> Sectionns { get; set; }

        public ICollection<Section> Sections { get; set; }
    }
}