using TraditionalRx.Models.Domain;
using TraditionalRx.Models.DTO;
using TraditionalRx.Repositories.Abstract;

namespace TraditionalRx.Repositories.Implementation
{
    public class MedicineService : IMedicineService
    {
        private readonly DatabaseContext ctx;
        public MedicineService(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public bool Add(Medicine model) 
        {
            try
            {
                ctx.Medicine.Add(model);
                ctx.SaveChanges();
                foreach (int categoryId in model.Categories)
                {
                    var medicineCategory = new MedicineCategory
                    {
                        MedicineId = model.Id,
                        CategoryId = categoryId
                    };
                    ctx.MedicineCategory.Add(medicineCategory);
                }
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            } 
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null)
                    return false;
                var medicineCategories= ctx.MedicineCategory.Where(a => a.MedicineId == data.Id);
                foreach(var medicineCategory in medicineCategories)
                {
                    ctx.MedicineCategory.Remove(medicineCategory);
                }
                ctx.Medicine.Remove(data);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Medicine GetById(int id)
        {
            var categories = (from category in ctx.Category
                              join mg in ctx.MedicineCategory
                              on category.Id equals mg.CategoryId
                              where mg.MedicineId == id
                              select category.CategoryName
                              ).ToList();
            var categoryNames = string.Join(',', categories);
            var medicine = ctx.Medicine.Find(id);
            medicine.CategoryNames = categoryNames;
            return medicine;
        }

        public MedicineListVm List(string term="",bool paging=false, int currentPage=0)
        {
            var data = new MedicineListVm();
           
            var list = ctx.Medicine.ToList();
 
           
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(a => a.CommonName.ToLower().StartsWith(term)).ToList();
            }

            if (paging)
            {
                // here we will apply paging
                int pageSize = 5;
                int count = list.Count;
                int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                list = list.Skip((currentPage - 1)*pageSize).Take(pageSize).ToList();
                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = TotalPages;
            }

            foreach (var medicine in list)
            {
                var categories = (from category in ctx.Category
                              join mg in ctx.MedicineCategory
                              on category.Id equals mg.CategoryId
                              where mg.MedicineId == medicine.Id
                              select category.CategoryName
                              ).ToList();
                var categoryNames = string.Join(',', categories);
                medicine.CategoryNames = categoryNames;
            }
            data.MedicineList = list.AsQueryable();
            return data;
        }

        public bool Update(Medicine model)
        {
            try
            {
                // these categoryIds are not selected by users and still present is medicineCategory table corresponding to
                // this medicineId. So these ids should be removed.
                var categoriesToDeleted = ctx.MedicineCategory.Where(a => a.MedicineId == model.Id && !model.Categories.Contains(a.CategoryId)).ToList();
                foreach(var mCategory in categoriesToDeleted)
                {
                    ctx.MedicineCategory.Remove(mCategory);
                }
                foreach (int genId in model.Categories)
                {
                    var medicineCategory = ctx.MedicineCategory.FirstOrDefault(a => a.MedicineId == model.Id && a.CategoryId == genId);
                    if (medicineCategory == null)
                    {
                        medicineCategory = new MedicineCategory { CategoryId = genId, MedicineId = model.Id };
                        ctx.MedicineCategory.Add(medicineCategory);
                    }
                }

                ctx.Medicine.Update(model);
                // we have to add these category ids in medicineCategory table
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<int> GetCategoryByMedicineId(int medicineId)
        {
            var categoryIds = ctx.MedicineCategory.Where(a => a.MedicineId == medicineId).Select(a => a.CategoryId).ToList();
            return categoryIds;
        }
       
    }
}
