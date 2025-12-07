using DesignPatternsModuleHomework13.Classes.Models;

namespace DesignPatternsModuleHomework13.Classes.Services;

public class InventoryService
{
    private readonly Dictionary<string, int> _stock = new();

    public void SeedStock(IEnumerable<(TicketType ticket, int quantity)> items)
    {
        foreach (var (ticket, quantity) in items)
        {
            _stock[ticket.Code] = quantity;
        }
    }

    public bool HasStock(TicketType ticket) => _stock.TryGetValue(ticket.Code, out var qty) && qty > 0;

    public void Reserve(TicketType ticket)
    {
        if (!HasStock(ticket))
        {
            throw new InvalidOperationException("Нет доступных билетов.");
        }

        _stock[ticket.Code]--;
    }
}
