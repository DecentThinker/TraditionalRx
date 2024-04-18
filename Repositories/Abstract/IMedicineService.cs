using TraditionalRx.Models.Domain;
using TraditionalRx.Models.DTO;

namespace TraditionalRx.Repositories.Abstract
{
    public interface IMedicineService
    {
       bool Add(Medicine model);
       bool Update(Medicine model);
       Medicine GetById(int id);
       bool Delete(int id);
       MedicineListVm List(string term = "", bool paging = false, int currentPage = 0);
        List<int> GetCategoryByMedicineId(int medicineId);

    }
}
