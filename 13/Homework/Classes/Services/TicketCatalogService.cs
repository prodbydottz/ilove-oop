using DesignPatternsModuleHomework13.Classes.Models;

namespace DesignPatternsModuleHomework13.Classes.Services;

public class TicketCatalogService
{
    private readonly Dictionary<string, TicketType> _tickets = new(StringComparer.OrdinalIgnoreCase);

    public void Seed(params TicketType[] tickets)
    {
        foreach (var ticket in tickets)
        {
            _tickets[ticket.Code] = ticket;
        }
    }

    public TicketType? Find(string code) => _tickets.TryGetValue(code, out var ticket) ? ticket : null;

    public IEnumerable<TicketType> All() => _tickets.Values;
}
