using DesignPatternsModuleHomework13.Classes.Models;

namespace DesignPatternsModuleHomework13.Classes.StateMachine.States;

public class WaitingForMoneyState : ITicketMachineState
{
    private readonly TicketMachine _context;

    public WaitingForMoneyState(TicketMachine context) => _context = context;

    public void SelectTicket(TicketType ticket)
    {
        _context.UpdateSelectedTicket(ticket);
        Console.WriteLine($"Выбран другой билет: {ticket.Name}");
    }

    public void InsertMoney(decimal amount)
    {
        _context.Payments.InsertMoney(amount);
        Console.WriteLine($"Пополнено {amount:C}. Всего: {_context.Payments.CurrentBalance:C}");

        if (_context.SelectedTicket is { } ticket && _context.Payments.IsEnoughMoney(ticket))
        {
            _context.SetState(new MoneyReceivedState(_context));
        }
    }

    public void DispenseTicket() => throw new InvalidOperationException("Недостаточно средств или выбранный вами билет отсутствует.");

    public void Cancel()
    {
        var returned = _context.Payments.ReturnChange();
        Console.WriteLine($"Операция отменена. Возвращено {returned:C}");
        _context.SetState(new TransactionCanceledState(_context));
    }
}
