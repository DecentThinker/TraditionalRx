using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TraditionalRx.Models.Domain;
using TraditionalRx.Repositories.Abstract;

namespace TraditionalRx.Controllers
{
    [Authorize]
    public class MedicineController : Controller
    {
        private readonly IMedicineService _medicineService;
        private readonly IFileService _fileService;
        private readonly ICategoryService _categoryService;
        public MedicineController(ICategoryService categoryService,IMedicineService MedicineService, IFileService fileService)
        {
            _medicineService = MedicineService;
            _fileService = fileService;
            _categoryService = categoryService;
        }
        public IActionResult Add()
        {
            var model = new Medicine();
            model.CategoryList = _categoryService.List().Select(a => new SelectListItem { Text = a.CategoryName, Value = a.Id.ToString() });
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Medicine model)
        {
            model.CategoryList = _categoryService.List().Select(a => new SelectListItem { Text = a.CategoryName, Value = a.Id.ToString() });
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.MedicineImage = imageName;
            }
            var result = _medicineService.Add(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var model = _medicineService.GetById(id);
            var selectedCategories = _medicineService.GetCategoryByMedicineId(model.Id);
            MultiSelectList multiCategoryList = new MultiSelectList(_categoryService.List(), "Id", "CategoryName", selectedCategories);
            model.MultiCategoryList = multiCategoryList;
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Medicine model)
        {
            var selectedCategories = _medicineService.GetCategoryByMedicineId(model.Id);
            MultiSelectList multiCategoryList = new MultiSelectList(_categoryService.List(), "Id", "CategoryName", selectedCategories);
            model.MultiCategoryList = multiCategoryList;
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.MedicineImage = imageName;
            }
            var result = _medicineService.Update(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(MedicineList));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        public IActionResult MedicineList()
        {
            var data = this._medicineService.List();
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            var result = _medicineService.Delete(id);
            return RedirectToAction(nameof(MedicineList));
        }
    }
}
