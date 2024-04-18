using TraditionalRx.Models.Domain;

namespace TraditionalRx.Models.DTO
{
    public class MedicineListVm
    {
        public IQueryable<Medicine>? MedicineList { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Term { get; set; }
    }
}
