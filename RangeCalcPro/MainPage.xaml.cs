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
            const double B = 32000.0;   // базовая сумма
            const double A = 6.25;      // основание логарифма

            if (capital == 0.0)
                return 0.0;

            double absCapital = Math.Abs(capital);
            double ratio = absCapital / B;

            // Логарифм по основанию A: log_A(ratio) = ln(ratio) / ln(A)
            double log = Math.Log(ratio) / Math.Log(A);

            if (capital > 0)
            {
                // Положительный капитал: ранг = 1 + log_A(C/B)
                return 1.0 + log;
            }
            else
            {
                // Отрицательный капитал (долг): ранг = - (1 + log_A(|C|/B))
                return -(1.0 + log);
            }
        }
        /*public static double CalculateRank(double capital)
        {
            const double B = 32000.0;
            const double A = 6.25;
            const double alpha = A - 1.0; // = 5.25

            // Для любых C > -B/alpha (т.е. C > -6095.24) формула работает
            double ratio = capital / B;
            double value = 1.0 + ratio * alpha;
            if (value <= 0.0)
            {
                // Для очень глубоких долгов используем симметрию (редкий случай)
                return -CalculateRank(-capital);
            }
            return Math.Log(value) / Math.Log(A);
        }*/
        private void OnCounterClicked(object? sender, EventArgs e)
        {
            if (int.TryParse(InitialCountEntry.Text, out int enteredValue))
            {
                count = enteredValue;
                SemanticScreenReader.Announce($"Count установлен в {count}");
            }
            else
            {
                DisplayAlert("Ошибка", "Введите целое число", "OK");
            }
            count = CalculateRank(count);

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
