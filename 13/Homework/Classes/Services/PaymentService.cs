using DesignPatternsModuleHomework13.Classes.Models;

namespace DesignPatternsModuleHomework13.Classes.Services;

public class PaymentService
{
    private decimal _currentBalance;

    public decimal CurrentBalance => _currentBalance;

    public void InsertMoney(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        _currentBalance += amount;
    }

    public bool IsEnoughMoney(TicketType ticket) => _currentBalance >= ticket.Price;

    public decimal DeductCost(TicketType ticket)
    {
        if (!IsEnoughMoney(ticket))
        {
            throw new InvalidOperationException("Недостаточно средств.");
        }

        _currentBalance -= ticket.Price;
        return ticket.Price;
    }

    public decimal ReturnChange()
    {
        var change = _currentBalance;
        _currentBalance = 0;
        return change;
    }

    public void Reset() => _currentBalance = 0;
}
