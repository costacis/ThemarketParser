using Google.DataTable.Net.Wrapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThemarketParser.Data;
using ThemarketParser.Models;

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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string GetStatisticsForChart()
        {
            var query = _db.Items.GroupBy(g => g.concreteCategory.name, p => p.price).Select(g => new { Category = g.Key, Price = g.Average(), Min = g.Min(), Max = g.Max() });
            var dt = new DataTable();
            dt.AddColumn(new Column(ColumnType.String, "Category", "Категория"));
            dt.AddColumn(new Column(ColumnType.Number, "Price", "Цена"));
            dt.AddColumn(new Column(ColumnType.Number, "Min", "Минимум"));
            dt.AddColumn(new Column(ColumnType.Number, "Max", "Максимум"));
            foreach (var item in query)
            {
                Row r = dt.NewRow();
                r.AddCellRange(new Cell[] { new Cell(item.Category), new Cell(item.Price), new Cell(item.Min), new Cell(item.Max) });
                dt.AddRow(r);
            }
            return dt.GetJson();
        }
    }
}