using System.Windows;
using Keyzerov.WPF.ViewModels;

namespace Keyzerov.WPF.Views
{
    public partial class AddSaleWindow : Window
    {
        private readonly AddSaleViewModel _vm;

        public AddSaleWindow(AddSaleViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (await _vm.SaveAsync())
            {
                DialogResult = true;
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}