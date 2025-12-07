using DesignPatternsModuleHomework13.Classes.Models;
using DesignPatternsModuleHomework13.Classes.StateMachine.States;
using DesignPatternsModuleHomework13.Classes.Services;

namespace DesignPatternsModuleHomework13.Classes.StateMachine;

public interface ITicketMachineState
{
    void SelectTicket(TicketType ticket);
    void InsertMoney(decimal amount);
    void DispenseTicket();
    void Cancel();
}

public class TicketMachine
{
    private readonly PaymentService _paymentService;
    private readonly InventoryService _inventoryService;

    public ITicketMachineState CurrentState { get; private set; }
    public TicketType? SelectedTicket { get; private set; }

    public TicketMachine(PaymentService paymentService, InventoryService inventoryService)
    {
        _paymentService = paymentService;
        _inventoryService = inventoryService;
        CurrentState = new IdleState(this);
    }

    public void SetState(ITicketMachineState state) => CurrentState = state;

    public void SelectTicket(TicketType ticket) => CurrentState.SelectTicket(ticket);
    public void InsertMoney(decimal amount) => CurrentState.InsertMoney(amount);
    public void DispenseTicket() => CurrentState.DispenseTicket();
    public void Cancel() => CurrentState.Cancel();

    public void UpdateSelectedTicket(TicketType? ticket) => SelectedTicket = ticket;

    public PaymentService Payments => _paymentService;
    public InventoryService Inventory => _inventoryService;
}
