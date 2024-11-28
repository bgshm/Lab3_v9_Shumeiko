
namespace Lab3_v9_Shumeiko
{
    public class DataItem
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Terms { get; set; }
        public string Distribution { get; set; }
    }

    public class FacultyNetworkRoot
    {
        public List<DataItem> FacultyNetwork { get; set; }
    }

}
