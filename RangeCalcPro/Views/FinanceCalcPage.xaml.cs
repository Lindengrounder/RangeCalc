using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Text;
using System.IO;

namespace RangeCalcPro
{
    public partial class FinanceCalcPage : ContentPage
    {
        public ObservableCollection<FinancialItem> Items { get; set; }
        public ICommand DeleteCommand { get; }

        public FinanceCalcPage()
        {
            InitializeComponent();
            //AddButton.Clicked += OnAddItemClicked;
            Items = new ObservableCollection<FinancialItem>();
            DeleteCommand = new Command<FinancialItem>(OnDeleteItem);
            BindingContext = this;
            LoadData();
            UpdateTotals();
            Items.CollectionChanged += (s, e) => UpdateTotals();
        }
        private void OnCalculateClicked(object sender, EventArgs e)
        {
            UpdateTotals();
        }

        private void OnAddItemClicked(object sender, EventArgs e)
        {
            //DisplayAlert("Тест", "Кнопка сработала", "OK");
            Items.Add(new FinancialItem { Id = Guid.NewGuid().ToString(), Name = "", Amount = 0 });
            SaveData();
        }

        private void OnDeleteItem(FinancialItem item)
        {
            Items.Remove(item);
            SaveData();
        }

        private void UpdateTotals()
        {
            double total = Items.Sum(i => i.Amount);
            TotalAmountLabel.Text = $"Сумма: {total:F2} ₽";
            double rank = CalculateRank(total);
            TotalRankLabel.Text = $"Ранг: {rank:F2}";
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(Items);
            Preferences.Set("finance_items", json);
        }

        private void LoadData()
        {
            var json = Preferences.Get("finance_items", "");
            if (!string.IsNullOrEmpty(json))
            {
                var items = JsonSerializer.Deserialize<ObservableCollection<FinancialItem>>(json);
                if (items != null)
                {
                    foreach (var item in items)
                        Items.Add(item);
                }
            }
        }
        private async void OnSaveToFileClicked(object sender, EventArgs e)
        {
            // 1. Получаем актуальные данные
            var total = Items.Sum(i => i.Amount);
            var rank = CalculateRank(total);

            // 2. Формируем CSV-содержимое
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Название;Сумма");
            foreach (var item in Items)
            {
                csvContent.AppendLine($"{item.Name};{item.Amount}");
            }
            csvContent.AppendLine($"ИТОГО;{total}");
            csvContent.AppendLine($"РАНГ;{rank}");

            // 3. Сохраняем в файл во внутреннюю папку приложения
            var directoryPath = FileSystem.AppDataDirectory;
            var fileName = $"finance_report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
            var fullPath = Path.Combine(directoryPath, fileName);

            try
            {
                await File.WriteAllTextAsync(fullPath, csvContent.ToString(), Encoding.UTF8);
                await DisplayAlert("Успех", $"Файл сохранён в папку приложения:\n{fullPath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось сохранить файл: {ex.Message}", "OK");
            }
        }
        private double CalculateRank(double capital)
        {
            const double B = 32000.0;
            const double A = 6.25;
            double absC = Math.Abs(capital);
            if (absC <= B) return capital >= 0 ? absC / B : -absC / B;
            double rank = 1.0 + Math.Log(absC / B) / Math.Log(A);
            return capital >= 0 ? rank : -rank;
        }
    }

    public class FinancialItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
    }
}