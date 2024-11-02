using Microsoft.AspNetCore.Mvc;
using System.CodeDom;
using System.Net.Http;
using System.Security.Policy;
using WalksUI.ApiClients;
using WalksUI.Interface;
using WalksUI.Models.Domain;
using WalksUI.Models.Dto;


namespace WalksUI.Controllers
{
    public class RegionController : Controller
    {
        private readonly IApiClient apiClient;
        string url = "https://localhost:7144/api/Regions";

        public RegionController(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           
            List<Regions> responce=new List<Regions>();
            try
            {
                
                responce.AddRange(await apiClient.GetAsync<Regions>(url));
            }
            catch (Exception)
            {

                throw;
            }
            
            return View(responce);
        }
        [HttpGet]
        public IActionResult AddRegion() 
        {
            return View();       
        }
        [HttpPost]
        public async Task<IActionResult> AddRegion(AddRegionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var request = new RegionDto
                    {
                        Code = model.Code,
                        Name = model.Name,
                        RegionImageUrl = model.RegionImageUrl,
                    };
                    var responce = await apiClient.PostAsync<RegionDto>(url, request);
                    if (responce != null)
                    {
                        return RedirectToAction("Index", "Region");
                    }                 
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {        
            string url = $"https://localhost:7144/api/Regions/{id}";
            var responce = await apiClient.GetByIdAsync<RegionDto>(url); ;
            if (responce is not null)
            {
                return View(responce);
            }   
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto model)
        {
          if (ModelState.IsValid)
            {
                string url = $"https://localhost:7144/api/Regions/{model.Id}";
                var responce = await apiClient.PutAsync<RegionDto>(url, model);
                if (responce is not null)
                {
                    return RedirectToAction("Index", "Region");
                }
            }
            return View(null);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            string url = $"https://localhost:7144/api/Regions/{id}";
            var responce = await apiClient.GetByIdAsync<RegionDto>(url); ;
            if (responce is not null)
            {
                return View(responce);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto model)
        {
            string url = $"https://localhost:7144/api/Regions/{model.Id}";

            try
            {
                await apiClient.DeleteAsync(url);

                return RedirectToAction("Index", "Region");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred: " + ex.Message;
            }

            return View(model);
        }
    }
}
