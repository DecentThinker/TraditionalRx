using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraditionalRx.Models.Domain
{
    public class Medicine
    {
        public int Id { get; set; }
        public string? ScientificName { get; set; }
        public string? CommonName { get; set; }
        public string? Family { get; set; }  
        public string? PartsOfPlant { get; set; }
        public string? Components { get; set; }
        public string? Compounds { get; set; }
        public string? Methodology { get; set; }
        public string? Reference { get; set; }
        public string? MedicineImage { get; set; }  // stores medicine image name with extension (eg, image0001.jpg)


        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        [Required]
        public List<int>? Categories { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? CategoryList { get; set; }

        [NotMapped]
        public string ? CategoryNames { get; set; }

        [NotMapped]
        public MultiSelectList ? MultiCategoryList { get; set; }

    }
}
