using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using ThemarketParser.Data;
using ThemarketParser.Models;

namespace ThemarketParser.Controllers
{
    public class DictionariesController : Controller
    {
        private readonly ThemarketDBContext _db;
        private IHttpClientFactory _clientFactory;

        public DictionariesController(ThemarketDBContext db, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            string url = "https://gfdssdmsiwojishll.themarket.io/dictionaries/";
            string? data = await SendRequest(url + "conditions");
            if (data != null)
            {
                var conditions = JObject.Parse(data);
                _db.Condition.Clear();
                _db.Condition.AddRange(conditions["conditions"].ToObject<List<Condition>>());
            }
            data = await SendRequest(url + "sexCategories");
            if (data != null)
            {
                var categories = JObject.Parse(data);
                _db.SexCategories.Clear();
                _db.Categories.Clear();
                _db.ConcreteCategories.Clear();

                foreach (var sexcategory in categories["sexCategories"])
                {
                    var sexCat = sexcategory.ToObject<SexCategory>();
                    _db.SexCategories.Add(sexCat);
                    foreach (var category in sexcategory["categories"])
                    {
                        var cat = category.ToObject<Category>();
                        cat.parentCategoryId = sexCat.id;
                        _db.Categories.Add(cat);
                        var concreteCategories = category["concreteCategories"].ToObject<List<ConcreteCategory>>();
                        concreteCategories.ForEach(c => c.parentCategoryId = cat.id);
                        Console.WriteLine(category["concreteCategories"]);
                        foreach (var ccat in concreteCategories)
                        {
                            Console.WriteLine(ccat.id);
                            Console.WriteLine(ccat.sizesName);
                            Console.WriteLine(ccat.name);
                        }
                        
                        _db.ConcreteCategories.AddRange(concreteCategories);
                    }
                }
            }
            _db.SaveChanges();

            data = await SendRequest(url + "Sizes");
            if (data != null)
            {
                var categories = JObject.Parse(data);
                _db.Sizes.Clear();
                foreach (var category in categories["categories"])
                {
                    var sizes = category["sizes"].ToObject<List<Size>>();
                    var cat = _db.ConcreteCategories.FirstOrDefault(c => c.sizesName == category["name"].ToString());
                    if (cat != null)
                    {
                        sizes.ForEach(s => s.categoryId =cat.id);
                        _db.Sizes.AddRange(sizes);
                    }
                }
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {
            DictionaryViewModel data = new DictionaryViewModel();
            data.conditions = _db.Condition.ToList();
            data.sexCategories = _db.SexCategories.Include(c => c.childCategories).ThenInclude(c => c.childCategories).ToList();
            data.sizes = _db.Sizes.Include(c => c.category).ToList();
            data.brands = _db.Brands.ToList();
            data.cities = _db.Cities.ToList();
            return View(data);

        }
        /*
        public IActionResult CategoriesPartial()
        {
            IEnumerable<SexCategory> sexCategories = _db.SexCategories.ToList();
            return PartialView(sexCategories);
        }
        public IActionResult SizesPartial()
        {
            IEnumerable<Size> sizes = _db.Sizes.ToList();
            return PartialView(sizes);
        }*/


        public async Task<string?> SendRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return data;
            }
            return null;
        }
    }
}
