using System.Windows;
using Keyzerov.WPF.ViewModels;

namespace Keyzerov.WPF.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _vm;

        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();

            _vm = vm;
            DataContext = _vm;  

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(">>> [MainWindow] Окно загружено, вызываем LoadDataAsync");

            try
            {
                await _vm.LoadDataAsync();
                System.Diagnostics.Debug.WriteLine(">>> [MainWindow] LoadDataAsync завершён");
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($">>> [MainWindow] ОШИБКА: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Ошибка при загрузке данных:\n{ex.Message}",
                    "Критическая ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}