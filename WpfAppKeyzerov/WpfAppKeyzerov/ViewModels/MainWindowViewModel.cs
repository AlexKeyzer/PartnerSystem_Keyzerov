using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Keyzerov.Core.Models;
using Keyzerov.Core.Services;
using Keyzerov.WPF.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Keyzerov.WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IPartnerService _partnerService;
        private Partner? _selectedPartner;
        private ObservableCollection<Partner> _partners = new();
        private ObservableCollection<SalesHistory> _salesHistory = new();
        private int _currentDiscount = 0;
        private string _statusMessage = "Готов к работе";

        public ObservableCollection<Partner> Partners
        {
            get => _partners;
            set => SetProperty(ref _partners, value);
        }

        public ObservableCollection<SalesHistory> SalesHistory
        {
            get => _salesHistory;
            set => SetProperty(ref _salesHistory, value);
        }

        public Partner? SelectedPartner
        {
            get => _selectedPartner;
            set
            {
                if (SetProperty(ref _selectedPartner, value))
                {
                    _ = LoadPartnerDetailsAsync();
                }
            }
        }

        public int CurrentDiscount
        {
            get => _currentDiscount;
            set => SetProperty(ref _currentDiscount, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ICommand LoadDataCommand { get; }
        public ICommand AddPartnerCommand { get; }
        public ICommand EditPartnerCommand { get; }
        public ICommand DeletePartnerCommand { get; }
        public ICommand AddSaleCommand { get; } 
        public ICommand ExitCommand { get; }

        public MainWindowViewModel(IPartnerService partnerService)
        {
            _partnerService = partnerService;
            LoadDataCommand = new RelayCommand(async _ => await LoadDataAsync());
            AddPartnerCommand = new RelayCommand(async _ => await AddPartnerAsync());
            EditPartnerCommand = new RelayCommand(async _ => await EditPartnerAsync(), _ => SelectedPartner != null);
            DeletePartnerCommand = new RelayCommand(async _ => await DeletePartnerAsync(), _ => SelectedPartner != null);
            AddSaleCommand = new RelayCommand(async _ => await AddSaleAsync(), _ => SelectedPartner != null);  // ← НОВОЕ
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var list = await _partnerService.GetAllPartnersAsync();
                Partners.Clear();
                foreach (var p in list)
                {
                    var total = await _partnerService.GetTotalSalesQuantityAsync(p.Id);
                    p.CurrentDiscount = DiscountCalculator.GetDiscountPercent(total);
                    Partners.Add(p);
                }
                StatusMessage = $"Загружено партнеров: {Partners.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadPartnerDetailsAsync()
        {
            if (SelectedPartner == null)
            {
                SalesHistory.Clear();
                CurrentDiscount = 0;
                return;
            }

            try
            {
                var history = await _partnerService.GetSalesHistoryAsync(SelectedPartner.Id);
                SalesHistory.Clear();
                foreach (var s in history) SalesHistory.Add(s);

                CurrentDiscount = await _partnerService.GetDiscountPercentAsync(SelectedPartner.Id);
                StatusMessage = $"Партнер: {SelectedPartner.Name}, Скидка: {CurrentDiscount}%";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddPartnerAsync()
        {
            var vm = App.ServiceProvider.GetRequiredService<PartnerEditViewModel>();
            await vm.InitializeForNewAsync();
            var window = new PartnerEditWindow(vm);
            if (window.ShowDialog() == true)
            {
                await LoadDataAsync();
            }
        }

        private async Task EditPartnerAsync()
        {
            if (SelectedPartner == null) return;
            var vm = App.ServiceProvider.GetRequiredService<PartnerEditViewModel>();
            await vm.InitializeForEditAsync(SelectedPartner.Id);
            var window = new PartnerEditWindow(vm);
            if (window.ShowDialog() == true)
            {
                await LoadDataAsync();
                var updated = await _partnerService.GetPartnerByIdAsync(SelectedPartner.Id);
                if (updated != null) SelectedPartner = updated;
            }
        }

        private async Task DeletePartnerAsync()
        {
            if (SelectedPartner == null) return;
            if (MessageBox.Show($"Удалить партнера '{SelectedPartner.Name}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    await _partnerService.DeletePartnerAsync(SelectedPartner.Id);
                    await LoadDataAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task AddSaleAsync()
        {
            if (SelectedPartner == null) return;

            var vm = App.ServiceProvider.GetRequiredService<AddSaleViewModel>();
            vm.PartnerId = SelectedPartner.Id;
            var window = new AddSaleWindow(vm);
            if (window.ShowDialog() == true)
            {
                await LoadPartnerDetailsAsync();
                await LoadDataAsync();
            }
        }
    }
}