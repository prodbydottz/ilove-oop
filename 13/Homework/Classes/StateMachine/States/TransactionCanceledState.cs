namespace DesignPatternsModuleHomework13.Classes.StateMachine.States;

public class TransactionCanceledState : ITicketMachineState
{
    private readonly TicketMachine _context;

    public TransactionCanceledState(TicketMachine context)
    {
        _context = context;
        Console.WriteLine("Операция отменена. Возврат в исходное состояние.");
    }

    public void SelectTicket(Classes.Models.TicketType ticket)
    {
        Console.WriteLine("Автомат готов к новой покупке.");
        _context.SetState(new IdleState(_context));
        _context.SelectTicket(ticket);
    }

    public void InsertMoney(decimal amount)
    {
        Console.WriteLine("Автомат готов к новой покупке.");
        _context.SetState(new IdleState(_context));
        _context.InsertMoney(amount);
    }

    public void DispenseTicket() => Console.WriteLine("Транзакция отменена. Новый билет не выбран.");

    public void Cancel()
    {
        Console.WriteLine("Автомат готов к новой покупке.");
        _context.SetState(new IdleState(_context));
    }
}
