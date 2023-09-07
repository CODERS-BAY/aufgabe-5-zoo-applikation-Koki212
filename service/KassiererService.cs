using Microsoft.Extensions.Options;
using ZooAPI.Model;
using ZooAPI.Controller;

namespace ZooAPI.Service
{
    // Service für Kassierer-Funktionen
    public class KassiererService
    {
        private readonly DBConnection _dbConnection; // Datenbankverbindung
        private readonly IOptions<TicketPrices> _ticketPrices; // Ticketpreise

        // Konstruktor: DB & Ticketpreise Injektion
        public KassiererService(DBConnection dbConnection, IOptions<TicketPrices> ticketPrices)
        {
            _dbConnection = dbConnection;
            _ticketPrices = ticketPrices;
        }

        // Ticketkauf
        public async Task BuyTicket(TicketType type)
        {
            // Ticketpreis bestimmen
            var ticketPrice = type switch
            {
                TicketType.Kinder => _ticketPrices.Value.Kinder,
                TicketType.Erwachsener => _ticketPrices.Value.Erwachsene,
                TicketType.Senioren => _ticketPrices.Value.Senioren,
                _ => throw new ArgumentException("Ungültiger Tickettyp")
            };

            // Ticket erstellen
            var ticket = new Ticket
            {
                Type = type,
                Preis = ticketPrice,
                Verkaufsdatum = DateTime.Now
            };

            // Ticket in DB einfügen
            await InsertTicketAsync(ticket);
        }

        // Alle verkauften Tickets abrufen
        public async Task<List<Ticket>> GetAllSoldTicketsAsync()
        {
            var result = new List<Ticket>();
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            // SQL-Abfrage: Alle Tickets
            command.CommandText = "SELECT * FROM Zoo.tickets";
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var ticket = new Ticket
                {
                    Id = reader.GetInt32("id"),
                    Type = reader.IsDBNull(reader.GetOrdinal("type"))
                        ? default
                        : (TicketType)Enum.Parse(typeof(TicketType), reader.GetString("type")),
                    Preis = reader.GetDecimal("preis"),
                    Verkaufsdatum = reader.GetDateTime("verkaufsdatum")
                };
                result.Add(ticket);
            }

            return result;
        }

        // Ticket in DB einfügen
        public async Task InsertTicketAsync(Ticket ticket)
        {
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            // SQL-Insert
            command.CommandText =
                "INSERT INTO Zoo.tickets (type, preis, verkaufsdatum) VALUES (@type, @preis, @verkaufsdatum)";
            command.Parameters.AddWithValue("@type", ticket.Type);
            command.Parameters.AddWithValue("@preis", ticket.Preis);
            command.Parameters.AddWithValue("@verkaufsdatum", ticket.Verkaufsdatum);
            await command.ExecuteNonQueryAsync();
        }

        // Tickets nach Datum abrufen und Gesamtpreis berechnen
        public async Task<(List<Ticket> Tickets, decimal Total)> GetTicketsByDate(DateTime date)
        {
            var tickets = new List<Ticket>();
            decimal total = 0;
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            // SQL-Abfrage: Tickets nach Datum 
            command.CommandText = "SELECT * FROM Zoo.tickets WHERE verkaufsdatum = @date";
            command.Parameters.AddWithValue("@date", date);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var ticket = new Ticket
                {
                    Id = reader.GetInt32("id"),
                    Preis = reader.GetDecimal("preis"),
                    Verkaufsdatum = reader.GetDateTime("verkaufsdatum")
                };
                tickets.Add(ticket);
                total += ticket.Preis;
            }

            return (tickets, Total: total);
        }
    }
}