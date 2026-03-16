using Microsoft.EntityFrameworkCore;
using Keyzerov.Core.Data;
using Keyzerov.Core.Models;

namespace Keyzerov.Core.Services
{
    public class PartnerService : IPartnerService
    {
        private readonly AppDbContext _context;

        public PartnerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Partner>> GetAllPartnersAsync()
        {
            return await _context.Partners
                .Include(p => p.Type)
                .ToListAsync();
        }

        public async Task<Partner?> GetPartnerByIdAsync(int id)
        {
            return await _context.Partners
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task SavePartnerAsync(Partner partner)
        {
            if (partner.Id == 0)
                _context.Partners.Add(partner);
            else
                _context.Partners.Update(partner);

            await _context.SaveChangesAsync();
        }

        public async Task DeletePartnerAsync(int id)
        {
            var partner = await _context.Partners.FindAsync(id);
            if (partner != null)
            {
                _context.Partners.Remove(partner);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<PartnerType>> GetPartnerTypesAsync()
        {
            return await _context.PartnerTypes.ToListAsync();
        }

        public async Task<List<SalesHistory>> GetSalesHistoryAsync(int partnerId)
        {
            return await _context.SalesHistory
                .Where(s => s.PartnerId == partnerId)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<int> GetTotalSalesQuantityAsync(int partnerId)
        {
            return await _context.SalesHistory
                .Where(s => s.PartnerId == partnerId)
                .SumAsync(s => s.Quantity);
        }

        public async Task<int> GetDiscountPercentAsync(int partnerId)
        {
            var total = await GetTotalSalesQuantityAsync(partnerId);
            return DiscountCalculator.GetDiscountPercent(total);
        }
        public async Task AddSaleAsync(SalesHistory sale)
        {
            try
            {
                if (sale.PartnerId <= 0)
                    throw new ArgumentException("PartnerId должен быть больше 0");

                if (string.IsNullOrWhiteSpace(sale.ProductName))
                    throw new ArgumentException("ProductName не может быть пустым");

                if (sale.Quantity <= 0)
                    throw new ArgumentException("Quantity должен быть больше 0");

                var partner = await _context.Partners.FindAsync(sale.PartnerId);
                if (partner == null)
                    throw new ArgumentException($"Партнер с Id={sale.PartnerId} не найден");

                if (sale.SaleDate == default)
                    sale.SaleDate = DateTime.Now;

                _context.SalesHistory.Add(sale);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                System.Diagnostics.Debug.WriteLine($"Database error: {dbEx.Message}");
                if (dbEx.InnerException != null)
                    System.Diagnostics.Debug.WriteLine($"Inner exception: {dbEx.InnerException.Message}");

                throw new Exception($"Ошибка сохранения в базу данных: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in AddSaleAsync: {ex.Message}");
                throw;
            }
        }
    }
}