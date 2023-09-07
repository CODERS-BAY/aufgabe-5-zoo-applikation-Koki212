using System.Data;
using MySqlConnector;

namespace ZooAPI.Controller
{
    // Datenbankverbindungsklasse
    public class DBConnection
    {
        private readonly string _connectionString; // Verbindungszeichenfolge zur Datenbank

        // Konstruktor mit Konfiguration
        public DBConnection(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("ZooDb"); // Verbindungszeichenfolge aus Konfiguration holen
        }

        // Überladener Konstruktor mit Verbindungszeichenfolge und Konfiguration
        public DBConnection(string connectionString, IConfiguration configuration)
        {
            _connectionString = connectionString;
        }

        // Asynchrone Methode zur Herstellung einer sicheren Datenbankverbindung
        public async Task<MySqlConnection> GetConnectionAsync()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new Exception($"Verbindungszeichenfolge ist nicht definiert" + $".");
            }

            var conn = new MySqlConnection(_connectionString); // Neue Verbindung erstellen

            await conn.OpenAsync(); // Verbindung asynchron öffnen

            if (conn.State != ConnectionState.Open)
            {
                throw new Exception("Verbindung konnte nicht geöffnet werden.");
            }

            return conn; // Verbindung zurückgeben
        }
    }
}