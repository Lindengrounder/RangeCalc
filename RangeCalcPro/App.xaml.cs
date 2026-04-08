using Microsoft.Extensions.DependencyInjection;

namespace RangeCalcPro
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new Window(new AppShell());

            // Настройка размеров окна (работает только на Windows)
#if WINDOWS
            window.MinimumWidth = 300;
            window.MinimumHeight = 400;
            window.Width = 500;   // ширина
            window.Height = 800;  // высота
#endif

            return window;
        }
    }
}