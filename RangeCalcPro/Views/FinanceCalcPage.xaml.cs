using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Microsoft.Maui.Controls;

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