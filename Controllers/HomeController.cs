using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Razor;
using TraditionalRx.Repositories.Abstract;
using TraditionalRx.Repositories.Implementation;

namespace TraditionalRx.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMedicineService _medicineService;
        public HomeController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }
        public IActionResult Index(string term="", int currentPage = 1)
        {
            var medicines = _medicineService.List(term,true,currentPage);
            return View(medicines);
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult MedicineDetail(int medicineId)
        {
            var medicine = _medicineService.GetById(medicineId);
            return View(medicine);
        }
    }
}
