using DesignPatternsModuleHomework13.Classes.Models;

namespace DesignPatternsModuleHomework13.Classes.StateMachine.States;

public class IdleState : ITicketMachineState
{
    private readonly TicketMachine _context;

    public IdleState(TicketMachine context)
    {
        _context = context;
        _context.Payments.Reset();
        _context.UpdateSelectedTicket(null);
    }

    public void SelectTicket(TicketType ticket)
    {
        if (!_context.Inventory.HasStock(ticket))
        {
            throw new InvalidOperationException("Нет билетов данного типа в наличии.");
        }

        _context.UpdateSelectedTicket(ticket);
        _context.SetState(new WaitingForMoneyState(_context));
    }

    public void InsertMoney(decimal amount) => throw new InvalidOperationException("Вы не выбрали билет.");
    public void DispenseTicket() => throw new InvalidOperationException("Выбранный билет отсутствует.");
    public void Cancel() => throw new InvalidOperationException("Просьба не отменять операцию.");
}
