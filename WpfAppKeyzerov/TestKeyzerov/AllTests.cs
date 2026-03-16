using Xunit;
using Keyzerov.Core.Services;
using Keyzerov.Core.Models;
using System;

namespace Keyzerov.Tests
{
    public class UnitTest1
    {
        #region

        [Fact]
        public void Discount_LessThan10000_Returns0()
        {
            int result = DiscountCalculator.GetDiscountPercent(5000);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Discount_Exactly10000_Returns5()
        {
            int result = DiscountCalculator.GetDiscountPercent(10000);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Discount_Between10000And50000_Returns5()
        {
            int result = DiscountCalculator.GetDiscountPercent(25000);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Discount_Exactly50000_Returns10()
        {
            int result = DiscountCalculator.GetDiscountPercent(50000);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Discount_Between50000And300000_Returns10()
        {
            int result = DiscountCalculator.GetDiscountPercent(150000);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Discount_MoreThan300000_Returns15()
        {
            int result = DiscountCalculator.GetDiscountPercent(500000);
            Assert.Equal(15, result);
        }

        #endregion

        #region Тесты модели Partner (4 теста)

        [Fact]
        public void Partner_Create_ValidData()
        {
            var partner = new Partner
            {
                Name = "Test Partner",
                TypeId = 1,
                Rating = 10,
                INN = "1234567890",
                Address = "Moscow",
                DirectorName = "Ivanov",
                Phone = "+7 123 456 78 90",
                Email = "test@test.ru",
                SalesPlaces = "Moscow, SPB"
            };

            Assert.Equal("Test Partner", partner.Name);
            Assert.Equal(1, partner.TypeId);
            Assert.Equal(10, partner.Rating);
        }

        [Fact]
        public void Partner_Rating_CanBeZero()
        {
            var partner = new Partner();
            partner.Rating = 0;
            Assert.Equal(0, partner.Rating);
        }

        [Fact]
        public void Partner_CurrentDiscount_DefaultZero()
        {
            var partner = new Partner();
            Assert.Equal(0, partner.CurrentDiscount);
        }

        [Fact]
        public void Partner_INN_CanBeSet()
        {
            var partner = new Partner();
            partner.INN = "1234567890";
            Assert.Equal("1234567890", partner.INN);
        }

        #endregion

        #region 

        [Fact]
        public void SalesHistory_Create_ValidData()
        {
            var sale = new SalesHistory
            {
                PartnerId = 1,
                ProductName = "Product Test",
                Quantity = 1000,
                SaleDate = new DateTime(2025, 1, 15)
            };

            Assert.Equal(1, sale.PartnerId);
            Assert.Equal("Product Test", sale.ProductName);
            Assert.Equal(1000, sale.Quantity);
        }

        [Fact]
        public void SalesHistory_Quantity_CanBeSet()
        {
            var sale = new SalesHistory();
            sale.Quantity = 100;
            Assert.Equal(100, sale.Quantity);
        }

        [Fact]
        public void SalesHistory_SaleDate_HasValue()
        {
            var sale = new SalesHistory();
            Assert.True(sale.SaleDate.Year >= 2025);
        }

        #endregion

        #region 

        [Fact]
        public void PartnerType_Create_ValidData()
        {
            var type = new PartnerType
            {
                Id = 1,
                TypeName = "Retail Store"
            };

            Assert.Equal(1, type.Id);
            Assert.Equal("Retail Store", type.TypeName);
        }

        [Fact]
        public void PartnerType_TypeName_CanBeSet()
        {
            var type = new PartnerType();
            type.TypeName = "Wholesale";
            Assert.Equal("Wholesale", type.TypeName);
        }

        #endregion
    }
}