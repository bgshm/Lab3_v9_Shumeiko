
namespace Lab3_v9_Shumeiko
{
    public class DataManager
    {
        public List<DataItem> DataItems { get; private set; } = new List<DataItem>();

        private string filePath;
        private readonly JSONProcessor jsonProcessor;

        public DataManager(JSONProcessor processor)
        {
            jsonProcessor = processor;
        }

        public void LoadData(string path)
        {
            filePath = path;
            string jsonString = File.ReadAllText(path);
            DataItems = jsonProcessor.Deserialize(jsonString);
        }

        public void SaveData()
        {
            if (filePath != null)
            {
                string jsonString = jsonProcessor.Serialize(DataItems);
                File.WriteAllText(filePath, jsonString);
            }
        }

        public void AddDataItem(DataItem newItem)
        {
            DataItems.Add(newItem);
        }

        public void RemoveDataItem(DataItem item)
        {
            DataItems.Remove(item);
        }

        public void UpdateDataItem(DataItem originalItem, DataItem updatedItem)
        {
            int index = DataItems.IndexOf(originalItem);
            if (index != -1)
            {
                DataItems[index] = updatedItem;
            }
        }
    }


}
