using Google.DataTable.Net.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThemarketParser.Data;
using ThemarketParser.Models;
using System;
using TemplateEngine.Docx;
using System.Text.RegularExpressions;

namespace ThemarketParser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ThemarketDBContext _db;


        public HomeController(ILogger<HomeController> logger, ThemarketDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.conditions = _db.Condition.ToList();
            ViewBag.categories = _db.Categories.Where(i => i.parentCategoryId == 1).ToList();
            ViewBag.min = _db.Items.Min(i => i.price);
            ViewBag.max = _db.Items.Max(i => i.price);
            return View();
        }
		[HttpPost]
        [ValidateAntiForgeryToken]
        public string[] Form(HomeViewModel obj)
        {
            foreach (var c in obj.categoryNames) Console.WriteLine(c);
            var query = _db.Items.Include(i => i.category).Include(i => i.city)
                .Where(i => obj.conditionIds.Contains(i.conditionId))
                .Where(i => obj.categoryNames.Contains(i.category.name))
                .Where(i => i.price >= Math.Min(obj.priceA, obj.priceB) && i.price <= Math.Max(obj.priceA, obj.priceB));
            var ds1 = query.GroupBy(g => g.category.name, p => p.price).Select(g => new { Category = g.Key, Price = g.Average(), Min = g.Min(), Max = g.Max() });
            var dt1 = new DataTable();
            dt1.AddColumn(new Column(ColumnType.String, "Category", "Категория"));
            dt1.AddColumn(new Column(ColumnType.Number, "Min", "Минимум"));
            dt1.AddColumn(new Column(ColumnType.Number, "Price", "Средняя цена"));
            dt1.AddColumn(new Column(ColumnType.Number, "Max", "Максимум"));
            foreach (var item in ds1)
            {
                Row r = dt1.NewRow();
                r.AddCellRange(new Cell[] { new Cell(item.Category), new Cell(item.Min), new Cell(item.Price), new Cell(item.Max) });
                dt1.AddRow(r);
            }

            var ds2 = query.GroupBy(g => g.city.iso).Select(g => new { City = g.Key, Count = g.Count() });
            var dt2 = new DataTable();
            dt2.AddColumn(new Column(ColumnType.String, "Region", "Регион"));
            dt2.AddColumn(new Column(ColumnType.Number, "Count", "Количество"));
            foreach (var item in ds2)
            {
                Row r = dt2.NewRow();
                r.AddCellRange(new Cell[] { new Cell(item.City), new Cell(item.Count) });
                dt2.AddRow(r);
            }

            return new string[] { dt1.GetJson(), dt2.GetJson() };
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
        public IActionResult Index(HomeViewModel obj)
        {
            var query = _db.Items.Include(i => i.category).Include(i => i.city).Include(i => i.condition)
                .Where(i => obj.conditionIds.Contains(i.conditionId))
                .Where(i => obj.categoryNames.Contains(i.category.name))
                .Where(i => i.price >= Math.Min(obj.priceA, obj.priceB) && i.price <= Math.Max(obj.priceA, obj.priceB));
            System.IO.File.Delete("../../ThemarketParserData.docx");
            System.IO.File.Copy("../../inputReport.docx", "../../ThemarketParserData.docx");
            var categoryTable = new TableContent("categoryTable");
            foreach (var item in query.GroupBy(g => g.category.name, p => p.price).Select(g => new { category = g.Key, count = g.Count(), price = g.Min() + "-" + g.Max() }).OrderByDescending(g => g.count))
			{
                categoryTable.AddRow(
                        new FieldContent("categoryName", item.category),
                        new FieldContent("categoryCount", item.count.ToString()),
                        new FieldContent("categoryPrice", item.price));
            }
            var conditionTable = new TableContent("conditionTable");
            foreach (var item in query.GroupBy(g => g.condition.trnslation, p => p.price).Select(g => new { category = g.Key, count = g.Count(), price = g.Min() + "-" + g.Max() }).OrderByDescending(g => g.count))
            {
                conditionTable.AddRow(
                        new FieldContent("conditionName", item.category),
                        new FieldContent("conditionCount", item.count.ToString()),
                        new FieldContent("conditionPrice", item.price));
            }
            var regionTable = new TableContent("regionTable");
            foreach (var item in query.GroupBy(g => g.city.iso).Select(g => new { category = g.Key, count = g.Count()}).OrderByDescending(g => g.count))
            {
                regionTable.AddRow(
                        new FieldContent("regionName", item.category),
                        new FieldContent("regionCount", item.count.ToString()));
            }
            var valuesToFill = new Content(
                categoryTable,
                conditionTable,
                regionTable,
                new FieldContent("reportDate", DateTime.Now.ToString()),
                new ImageContent("chart1", Convert.FromBase64String(Regex.Match(obj.chart1, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value)),
                new ImageContent("chart2", Convert.FromBase64String(Regex.Match(obj.chart2, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value))
            );
            using (var outputDocument = new TemplateProcessor("../../ThemarketParserData.docx").SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}