namespace ZooAPI.Model
{
    // Tier-Modell
    public class Tier
    {
        // Konstruktor
        public Tier(int id, string gattung, string nahrung, int gehegeId)
        {
            Id = id; // Tier-ID
            Gattung = gattung; // Tiergattung (z.B. Löwe, Tiger)
            Nahrung = nahrung; // Nahrung des Tiers
            GehegeId = gehegeId; // ID des Geheges, in dem das Tier sich befindet
        }

        public int Id { get; set; } // Tier-ID
        public string Gattung { get; set; } // Tiergattung
        public string Nahrung { get; set; } // Nahrung des Tiers
        public int GehegeId { get; set; } // Gehege-ID
    }
}