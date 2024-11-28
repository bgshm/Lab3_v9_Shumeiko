using System.Collections.ObjectModel;
namespace Lab3_v9_Shumeiko
{
    public partial class MainPage : ContentPage
    {

        #region Глобальні змінні

        private JSONProcessor jsonProcessor;
        private DataManager dataManager;

        #endregion

        public MainPage()
        {
            InitializeComponent();
            jsonProcessor = new JSONProcessor(); 
            dataManager = new DataManager(jsonProcessor);
        }

        #region Лістнери кнопок

        private async void OnLoadFileClicked(object sender, EventArgs e)
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "application/json" } },
                { DevicePlatform.iOS, new[] { "public.json" } },
                { DevicePlatform.MacCatalyst, new[] { "public.json" } },
                { DevicePlatform.WinUI, new[] { ".json" } },
            });
            var fileResult = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Виберіть JSON файл",
                FileTypes = customFileType
            });

            if (fileResult != null)
            {
                try
                {
                    dataManager.LoadData(fileResult.FullPath);
                    DataGrid.ItemsSource = dataManager.DataItems;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Помилка", $"не вдалося завантажити файл: {ex.Message}", "ОК");
                }
            }
        }

        private void OnAddClicked(object sender, EventArgs e)
        {
            DataItem newItem = new DataItem
            {
                Category = "New category",
                Name = "New name",
                Description = "New description",
                Type = "New type",
                Version = "1.0",
                Author = "Author",
                Terms = "Terms",
                Distribution = "Distribution"
            };

            dataManager.AddDataItem(newItem);
            DataGrid.ItemsSource = null; 
            DataGrid.ItemsSource = dataManager.DataItems;
        }

        private async void OnRemoveClicked(object sender, EventArgs e)
        {
            if (DataGrid.SelectedItem is DataItem selectedItem)
            {
                bool confirm = await DisplayAlert("Підтвердження", "Ви впевнені, що хочете видалити елемент?", "Так", "Ні");
                if (confirm)
                {
                    dataManager.RemoveDataItem(selectedItem);
                    DataGrid.ItemsSource = null;
                    DataGrid.ItemsSource = dataManager.DataItems;
                }
            }
            else
            {
                await DisplayAlert("Помилка", "Виберіть елемент для видалення", "ОК");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                dataManager.SaveData();
                await DisplayAlert("Виконано!", "Зміни збережені успішно", "ОК");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", $"Не вдалося зберегти файл: {ex.Message}", "ОК");
            }
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            string selectedCategory = CategoryPicker.SelectedItem?.ToString();
            string keyword = KeywordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(selectedCategory) || string.IsNullOrEmpty(keyword))
            {
                await DisplayAlert("Помилка", "Будь ласка, оберіть категорію і введіть ключове слово.", "ОК");
                return;
            }

            var filteredItems = jsonProcessor.SearchLINQ(dataManager.DataItems, selectedCategory, keyword);

            DataGrid.ItemsSource = filteredItems;
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            DataGrid.ItemsSource = dataManager.DataItems;
        }

        private async void OnAboutButtonClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Про програму", "Шумейко Богдан Сергійович\nII курс\nК-25\n2024 рік\nПрограма реалізує редактор JSON файлу, з можливістю десереалізувати файл, додавати елементи, редагувати та видаляти їх, а також сереалізувати таблицю в вихідний файл", "ОК");
        }

        private async void OnExitButtonClicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Вихід", "Ви впевнені, що хочете вийти?", "Так", "Ні"))
            {
                Application.Current?.Quit();
            }
        }

        #endregion
    }
}