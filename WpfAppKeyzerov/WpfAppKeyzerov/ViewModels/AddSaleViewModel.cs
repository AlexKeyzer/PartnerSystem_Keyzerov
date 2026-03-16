using System.Windows;
using Keyzerov.Core.Models;
using Keyzerov.Core.Services;

namespace Keyzerov.WPF.ViewModels
{
    public class AddSaleViewModel : ViewModelBase
    {
        private readonly IPartnerService _partnerService;
        private string _productName = string.Empty;
        private int _quantity;
        private DateTime _saleDate = DateTime.Now;

        public int PartnerId { get; set; }

        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }

        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        public DateTime SaleDate
        {
            get => _saleDate;
            set => SetProperty(ref _saleDate, value);
        }

        public AddSaleViewModel(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        public async Task<bool> SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                MessageBox.Show("Введите наименование продукции", "Ошибка ввода",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Quantity <= 0)
            {
                MessageBox.Show("Количество должно быть больше 0", "Ошибка ввода",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (PartnerId <= 0)
            {
                MessageBox.Show("Не выбран партнер", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                var sale = new SalesHistory
                {
                    PartnerId = PartnerId,
                    ProductName = ProductName.Trim(),
                    Quantity = Quantity,
                    SaleDate = SaleDate.Kind == DateTimeKind.Utc ? SaleDate : SaleDate.ToUniversalTime()
                };

                System.Diagnostics.Debug.WriteLine($">>> [AddSale] Сохранение продажи: PartnerId={PartnerId}, Product={ProductName}, Qty={Quantity}, Date={SaleDate} (UTC)");

                await _partnerService.AddSaleAsync(sale);

                System.Diagnostics.Debug.WriteLine($">>> [AddSale] Продажа успешно сохранена!");

                MessageBox.Show($"Продажа успешно добавлена!\nПродукция: {ProductName}\nКоличество: {Quantity}",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($">>> [AddSale] ОШИБКА: {ex.Message}");
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine($">>> [AddSale] Inner: {ex.InnerException.Message}");

                MessageBox.Show(
                    $"Ошибка сохранения продажи:\n\n{ex.Message}\n\n" +
                    $"Проверьте:\n" +
                    $"- Подключение к базе данных\n" +
                    $"- Корректность введенных данных\n" +
                    $"- Наличие партнера в базе",
                    "Ошибка сохранения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }
        }
    }
}