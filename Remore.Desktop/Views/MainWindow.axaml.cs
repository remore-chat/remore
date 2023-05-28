using Avalonia.Controls;

namespace Remore.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialized += OnInitialized;
        }

        private void OnInitialized(object? sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}