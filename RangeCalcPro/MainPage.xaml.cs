namespace RangeCalcPro
{
    public partial class MainPage : ContentPage
    {
        int count = 1;

        public MainPage()
        {
            InitializeComponent();
        }


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
            count *=5;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
