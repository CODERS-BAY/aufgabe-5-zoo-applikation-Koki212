using ZooAPI.Model;
using ZooAPI.Controller;

namespace ZooAPI.Service
{
    public class TierpflegerService
    {
        private readonly DBConnection _dbConnection;

        // Konstruktor: Injektion der Datenbankverbindung
        public TierpflegerService(DBConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Methode zum Abrufen von Tieren basierend auf der Pfleger-ID
        public async Task<List<Tier>> GetTiereByPflegerIdAsync(int pflegerId)
        {
            var result = new List<Tier>();
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            // SQL-Abfrage: Tiere basierend auf der Pfleger-ID
            command.CommandText =
                "SELECT t.* FROM Zoo.tiere t JOIN Zoo.gehege g ON t.gehege_id = g.Id JOIN Zoo.mitarbeiter m ON g.mitarbeiter_id = m.id WHERE m.id = @pflegerId";
            command.Parameters.AddWithValue("@pflegerId", pflegerId);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var tier = new Tier(
                    reader.GetInt32("id"),
                    reader.GetString("gattung"),
                    reader.GetString("nahrung"),
                    reader.GetInt32("gehege_id")

                );
                result.Add(tier);
            }

            return result;
        }

        // Methode zum Aktualisieren eines Tiers in der Datenbank
        public async Task UpdateTierAsync(int id, Tier updatedTier)
        {
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            // SQL-Update-Befehl für das Tier
            command.CommandText =
                "UPDATE Zoo.tiere SET gattung = @gattung, nahrung = @nahrung, gehege_id = @gehegeId WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@gattung", updatedTier.Gattung);
            command.Parameters.AddWithValue("@nahrung", updatedTier.Nahrung);
            command.Parameters.AddWithValue("@gehegeId", updatedTier.GehegeId);
            await command.ExecuteNonQueryAsync();
        }
    }
}