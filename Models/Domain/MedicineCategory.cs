using System.ComponentModel.DataAnnotations;

namespace TraditionalRx.Models.Domain
{
    public class MedicineCategory
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public int CategoryId { get; set; }
    }
}
