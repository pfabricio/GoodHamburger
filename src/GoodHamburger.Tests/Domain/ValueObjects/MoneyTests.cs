using GoodHamburger.Domain.ValueObjects;

namespace GoodHamburger.Tests.Domain.ValueObjects
{
    public class MoneyTests
    {
        [Fact]
        public void Money_ShouldNotAllowNegativeAmount()
        {
            Assert.Throws<ArgumentException>(() => new Money(-1));
        }

        [Fact]
        public void Money_Add_ShouldReturnCorrectSum()
        {
            var m1 = new Money(10.50m);
            var m2 = new Money(5.25m);

            var result = m1.Add(m2);

            Assert.Equal(15.75m, result.Amount);
        }

        [Fact]
        public void Money_Subtract_ShouldReturnCorrectDifference()
        {
            var m1 = new Money(10.00m);
            var m2 = new Money(3.50m);

            var result = m1.Subtract(m2);

            Assert.Equal(6.50m, result.Amount);
        }

        [Fact]
        public void Money_ApplyDiscount_ShouldCalculateCorrectly()
        {
            var money = new Money(10.00m);

            var result = money.ApplyDiscount(0.20m);

            Assert.Equal(8.00m, result.Amount);
        }

        [Fact]
        public void Money_ImplicitConversion_FromDecimal()
        {
            Money money = 15.50m;

            Assert.Equal(15.50m, money.Amount);
        }

        [Fact]
        public void Money_ImplicitConversion_ToDecimal()
        {
            var money = new Money(15.50m);
            decimal amount = money;

            Assert.Equal(15.50m, amount);
        }
    }
}
