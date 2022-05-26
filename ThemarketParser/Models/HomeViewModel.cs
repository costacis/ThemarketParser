namespace ThemarketParser.Models
{
	public class HomeViewModel
	{
		/*public HomeViewModel()
		{
			conditions = new List<Condition>();
		}
		public List<Condition> conditions { get; set; }*/
		public int[] conditionIds { get; set; }
		public string[] categoryNames { get; set; }
		public int priceA { get; set; }
		public int priceB { get; set; }
		public string chart1 { get; set; }
		public string chart2 { get; set; }
	}
}
