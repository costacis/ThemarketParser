using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewBag.edited = _db.Items.FirstOrDefault(i => i.isModified == true) != null;
            int pageSize = 100;
            IQueryable<Item> source = _db.Items.Include(x => x.images).Include(x => x.size).Include(x => x.condition).OrderByDescending(x => x.addedAt);
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

        public async Task<IActionResult> Detail(string? id)
        {
            if (id == null || id == "") return NotFound();
            var item = await _db.Items
                .Include(x => x.images)
                .Include(x => x.brands)
                .Include(x => x.sexCategory)
                .Include(x => x.category)
                .Include(x => x.concreteCategory)
                .Include(x => x.condition)
                .Include(x => x.city)
                .Include(x => x.size)
                .FirstOrDefaultAsync(x => x.id == id);
            if (item == null) return NotFound();
            return View(item);
        }
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || id == "") return NotFound();
            var item = await _db.Items.FirstOrDefaultAsync(x => x.id == id);
            if (item == null) return NotFound();
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || id == "") return NotFound();
            var item = await _db.Items.FirstOrDefaultAsync(x => x.id == id);
            if (item == null) return NotFound();
            ViewBag.sizes = new SelectList(_db.Sizes.Where(c => c.categoryId == item.categoryId).ToList(), "id", "us");
            ViewBag.conditions = new SelectList(_db.Condition.ToList(), "id", "trnslation");
            ViewBag.cities = new SelectList(_db.Cities.ToList(), "id", "title");
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Item item)
        {
            if (!ModelState.IsValid)
            {
                item.isModified = true;
                _db.Items.Update(item);
                await _db.SaveChangesAsync();
                return RedirectToAction("Detail", new { id = item.id });
            }
            return View(item);
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
            var oldItems = _db.Items.ToList().Except(items).ToList();
            oldItems.ForEach(c => c.isModified = false);
            _db.AddOrUpdateRange(oldItems);
            _db.AddOrUpdateRange(images);
            await _db.SaveChangesAsync();
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
