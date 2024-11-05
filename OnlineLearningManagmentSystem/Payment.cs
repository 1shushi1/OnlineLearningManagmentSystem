using System;
using System.Collections.Generic;

public class Payment
{
    public int PaymentID { get; private set; }
    public double Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }

    private static List<Payment> _payments = new List<Payment>();

    public Payment(int paymentID, double amount, DateTime paymentDate)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");
        PaymentID = paymentID;
        Amount = amount;
        PaymentDate = paymentDate;
        AddToExtent(this);
    }

    public static void AddToExtent(Payment payment)
    {
        _payments.Add(payment ?? throw new ArgumentException("Payment cannot be null."));
    }

    public static IReadOnlyList<Payment> GetExtent() => _payments.AsReadOnly();

    public static void ClearExtent() => _payments.Clear();
}
