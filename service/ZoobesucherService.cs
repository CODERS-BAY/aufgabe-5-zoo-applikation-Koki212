using ZooAPI.Controller;
using ZooAPI.Model;

namespace ZooAPI.Service
{
    public class ZoobesucherService
    {
        private readonly DBConnection _dbConnection;

        // Konstruktor: DB Verbindung Injektion
        public ZoobesucherService(DBConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Alle Tiere abrufen
        public async Task<List<Tier>> GetAllTiere()
        {
            var result = new List<Tier>();
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            // SQL-Abfrage: Alle Tiere
            command.CommandText = "SELECT * FROM Zoo.tiere";
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

        // Tier nach Gattung abrufen
        public async Task<Tier?> GetTierByGattung(string Gattung)
        {
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();

            // SQL-Abfrage: Tier nach Gattung
            command.CommandText = "SELECT * FROM KokiZoo.Tiere WHERE Gattung = @Gattung";
            command.Parameters.AddWithValue("@gattung", Gattung);
            await using var reader = await command.ExecuteReaderAsync();
            Tier? tier = null;
            if (await reader.ReadAsync())
            {
                tier = new Tier(
                    reader.GetInt32("id"),
                    reader.GetString("gattung"),
                    reader.GetString("nahrung"),
                    reader.GetInt32("gehege_id")
                );
            }

            return tier;
        }
    }
}