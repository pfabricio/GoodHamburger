namespace GoodHamburger.Domain.ValueObjects
{
    public readonly record struct Money
    {
        public decimal Amount { get; }

        public Money(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));
            
            Amount = amount;
        }

        public static Money Zero => new(0);
        public static Money FromDecimal(decimal amount) => new(amount);

        public Money Add(Money other) => new(Amount + other.Amount);
        public Money Subtract(Money other) => new(Amount - other.Amount);
        public Money Multiply(decimal factor) => new(Amount * factor);
        public Money ApplyDiscount(decimal percentage) => new(Amount * (1 - percentage));

        public override string ToString() => $"R$ {Amount:N2}";

        public static implicit operator decimal(Money money) => money.Amount;
        public static implicit operator Money(decimal amount) => new(amount);
    }
}
