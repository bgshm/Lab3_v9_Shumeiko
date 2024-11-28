using System.Text.Json;

namespace Lab3_v9_Shumeiko
{
    public class JSONProcessor
    {
        public List<DataItem> Deserialize(string jsonString)
        {
            var root = JsonSerializer.Deserialize<FacultyNetworkRoot>(jsonString);
            return root?.FacultyNetwork ?? new List<DataItem>();
        }

        public string Serialize(List<DataItem> data)
        {
            var root = new FacultyNetworkRoot
            {
                FacultyNetwork = data
            };
            return JsonSerializer.Serialize(root, new JsonSerializerOptions { WriteIndented = true });
        }

        public List<DataItem> SearchLINQ(List<DataItem> dataItems, string searchCategory, string keyword)
        {
            if (string.IsNullOrEmpty(searchCategory) || string.IsNullOrEmpty(keyword))
                return new List<DataItem>();

            var propertyInfo = typeof(DataItem).GetProperty(searchCategory);
            if (propertyInfo == null)
                return new List<DataItem>();

            return dataItems.Where(item =>
                propertyInfo.GetValue(item, null) != null &&
                propertyInfo.GetValue(item, null).ToString()
                    .IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }
    }


}
