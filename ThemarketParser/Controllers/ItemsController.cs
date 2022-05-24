using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ThemarketParser.Data;
using ThemarketParser.Models;

namespace ThemarketParser.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ThemarketDBContext _db;
        private IHttpClientFactory _clientFactory;

        public ItemsController(ThemarketDBContext db, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _db = db;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 100;
            IQueryable<Item> source = _db.Items.Include(x => x.images);
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            ItemsViewModel viewModel = new ItemsViewModel
            {
                pageViewModel = pageViewModel,
                items = items
            };
            return View(viewModel);
        }

        public async Task<IActionResult> GetData()
        {
            var categories = _db.ConcreteCategories.ToList();
            var brands = new List<Brand>();
            var cities = new List<City>();
            var items = new List<Item>();
            var images = new List<Image>();
            foreach (var category in categories)
            {
                string url = $"https://gfdssdmsiwojishll.themarket.io/items?asdfsadf=1&sort=-addedAt&concreteCategoryIds[]={category.id}&skip=";
                string? data = "";
                int skip = 0;
                while (data != null && skip < 100)
                {
                    data = await SendRequest(url + skip.ToString());
                    skip += 18;
                    if (data != null)
                    {
                        var itemsGet = JArray.Parse(data);
                        foreach (var item in itemsGet)
                        {
                            var city = new City() { id = Convert.ToInt32(item["cityId"]), title = item["city"].ToString() };
                            if (!cities.Any(c => c.id == city.id))
                            {
                                cities.Add(city);
                                if (_db.Cities.Contains(city)) _db.Entry(city).State = EntityState.Modified;
                            }

                            var brandIds = item["brandIds"].ToObject<List<string>>();
                            var brandNames = item["brand"].ToString().Split(" x ");
                            for (int i = 0; i < brandIds.Count; i++)
                            {
                                var brand = new Brand() { id = brandIds[i], name = brandNames[i] };
                                if (!brands.Any(c => c.id == brand.id))
                                {
                                    brands.Add(brand);
                                    if (_db.Brands.Contains(brand)) _db.Entry(brand).State = EntityState.Modified;
                                }
                            }

                            var newItem = item.ToObject<Item>();
                            if (!items.Any(c => c.id == newItem.id))
                            {
                                items.Add(newItem);
                                if (_db.Items.Contains(newItem)) _db.Entry(newItem).State = EntityState.Modified;
                            }

                            var newImages = item["images"].ToObject<List<Image>>();
                            Console.WriteLine(item["images"].ToString());
                            foreach (var image in newImages)
                            {
                                if (!images.Any(c => c.id == image.id))
                                {
                                    image.itemId = newItem.id;
                                    if (image.url == null) image.url = "";
                                    images.Add(image);
                                    if (_db.Images.Contains(image)) _db.Entry(image).State = EntityState.Modified;
                                }
                            }
                        }
                    }
                }
            }
            _db.AddOrUpdateRange(cities);
            _db.AddOrUpdateRange(brands);
            _db.AddOrUpdateRange(items);
            _db.AddOrUpdateRange(images);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

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
