namespace Keyzerov.Core.Services
{
    public static class DiscountCalculator
    {
        public static int GetDiscountPercent(int totalQuantity)
        {
            if (totalQuantity > 300000) return 15;
            if (totalQuantity >= 50000) return 10;
            if (totalQuantity >= 10000) return 5;
            return 0;
        }
    }
}