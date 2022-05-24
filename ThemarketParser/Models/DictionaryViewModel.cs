namespace ThemarketParser.Models
{
    public class DictionaryViewModel
    {
        public IEnumerable<Condition> conditions { get; set; }
        public IEnumerable<SexCategory> sexCategories { get; set; }
        public IEnumerable<Size> sizes { get; set; }
        public IEnumerable<Brand> brands { get; set; }
        public IEnumerable<City> cities { get; set; }
    }
}
