using System.Collections.ObjectModel;
using System.Windows;
using Keyzerov.Core.Models;
using Keyzerov.Core.Services;

namespace Keyzerov.WPF.ViewModels
{
    public class PartnerEditViewModel : ViewModelBase
    {
        private readonly IPartnerService _partnerService;
        private Partner _partner = new();
        private ObservableCollection<PartnerType> _types = new();
        private int _selectedTypeId;

        public Partner Partner
        {
            get => _partner;
            set => SetProperty(ref _partner, value);
        }

        public ObservableCollection<PartnerType> Types
        {
            get => _types;
            set => SetProperty(ref _types, value);
        }

        public int SelectedTypeId
        {
            get => _selectedTypeId;
            set => SetProperty(ref _selectedTypeId, value);
        }

        public PartnerEditViewModel(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        public async Task InitializeForEditAsync(int id)
        {
            await LoadTypesAsync();
            var p = await _partnerService.GetPartnerByIdAsync(id);
            if (p != null)
            {
                Partner = p;
                SelectedTypeId = p.TypeId;
            }
        }

        public async Task InitializeForNewAsync()  
        {
            await LoadTypesAsync();
            Partner = new Partner();
            SelectedTypeId = Types.FirstOrDefault()?.Id ?? 0;
        }

        private async Task LoadTypesAsync()
        {
            var list = await _partnerService.GetPartnerTypesAsync();
            Types.Clear();
            foreach (var t in list) Types.Add(t);
        }

        public async Task<bool> SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Partner.Name))
            {
                MessageBox.Show("Наименование партнера обязательно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (Partner.Rating < 0)
            {
                MessageBox.Show("Рейтинг не может быть отрицательным", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            Partner.TypeId = SelectedTypeId;
            try
            {
                await _partnerService.SavePartnerAsync(Partner);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}