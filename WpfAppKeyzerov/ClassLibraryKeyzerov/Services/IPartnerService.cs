using Keyzerov.Core.Models;

namespace Keyzerov.Core.Services
{
    public interface IPartnerService
    {
        Task<List<Partner>> GetAllPartnersAsync();
        Task<Partner?> GetPartnerByIdAsync(int id);
        Task SavePartnerAsync(Partner partner);
        Task DeletePartnerAsync(int id);
        Task<List<PartnerType>> GetPartnerTypesAsync();
        Task<List<SalesHistory>> GetSalesHistoryAsync(int partnerId);
        Task<int> GetTotalSalesQuantityAsync(int partnerId);
        Task<int> GetDiscountPercentAsync(int partnerId);
        Task AddSaleAsync(SalesHistory sale);
    }
}