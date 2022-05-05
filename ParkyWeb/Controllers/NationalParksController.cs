using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        } 
        public IActionResult Index()
        {
            return View(new NationalPark() { });

        }
        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { date = await _npRepo.GetAllAsync(SD.NationalParkAPIPath) });
        }
    }
}
