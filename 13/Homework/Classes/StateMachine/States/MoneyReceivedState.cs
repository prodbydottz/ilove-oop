namespace DesignPatternsModuleHomework13.Classes.StateMachine.States;

public class MoneyReceivedState : ITicketMachineState
{
    private readonly TicketMachine _context;

    public MoneyReceivedState(TicketMachine context) => _context = context;

    public void SelectTicket(Classes.Models.TicketType ticket) => throw new InvalidOperationException("Билет уже оплачен. Отмените операцию для смены билета.");

    public void InsertMoney(decimal amount)
    {
        Console.WriteLine("Средств на вашем балансе достаточно.");
    }

    public void DispenseTicket()
    {
        if (_context.SelectedTicket is null)
        {
            throw new InvalidOperationException("Выберите билет.");
        }

        _context.Payments.DeductCost(_context.SelectedTicket);
        _context.Inventory.Reserve(_context.SelectedTicket);
        var change = _context.Payments.ReturnChange();
        Console.WriteLine(change > 0 ? $"Билет выдан, сдача: {change:C}." : "Билет выдан.");
        _context.SetState(new TicketDispensedState(_context));
    }

    public void Cancel()
    {
        var change = _context.Payments.ReturnChange();
        Console.WriteLine($"Транзакция отменена. Вам возвращено {change:C}");
        _context.SetState(new TransactionCanceledState(_context));
    }
}
