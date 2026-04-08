namespace RangeCalcPro
{
    public partial class MainPage : ContentPage
    {
        double count = 1;

        public MainPage()
        {
            InitializeComponent();
        }

        public static double CalculateRank(double capital)
        {
            const double B = 32000.0;    // база
            const double A = 6.25;       // основание логарифма

            double absC = Math.Abs(capital);
            double rank;

            if (absC <= B)
            {
                // Линейный участок от 0 до B
                rank = absC / B;
            }
            else
            {
                // Логарифмический участок для больших сумм
                rank = 1.0 + Math.Log(absC / B) / Math.Log(A);
            }

            return capital >= 0 ? rank : -rank;
        }
        private void OnCounterClicked(object? sender, EventArgs e)
        {
            if (double.TryParse(InitialCountEntry.Text, out double enteredValue))
            {
                count = enteredValue;
                SemanticScreenReader.Announce($"Count установлен в {count}");
            }
            else
            {
                DisplayAlert("Ошибка", "Введите целое или дробное число", "OK");
            }
            count = CalculateRank(count);

            //if (count == 1)
            if (Math.Abs(count - 1.0) < 1e-6)
                CounterBtn.Text = $"Ранг: {count:F2}";
            else
                CounterBtn.Text = $"Значение: {count:F2}";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
