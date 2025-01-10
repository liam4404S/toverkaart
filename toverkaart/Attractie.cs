using MySql.Data.MySqlClient;
using System.Data;

namespace toverkaart.Pages
{
    public class Attractie
    {
        private DatabaseService _databaseService;

        public int Id { get; set; }
        public string Naam {  get; set; } = string.Empty;
        public int Capaciteit { get; set; } = 0;
        public int TijdsduurSec {  get; set; } = 0;
        public int AttractieType { get; set; } = 0;
        public int AttractieGebied { get; set; } = 0;
        public bool Status {  get; set; } = false;
        public List<string> AttractieLijst { get; set; }

        public Attractie() { }
        public Attractie(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<string> GetAllAttracties()
        {
            string query = "SELECT naam FROM attracties";

            var result = _databaseService.ExecuteQuery(query);
            
            List<string> attractielijst = new List<string>();

            foreach (DataRow row in result.Rows)
            {
                if (row["naam"] != DBNull.Value)
                {
                    attractielijst.Add(row["naam"].ToString()!);
                }
            }
            return AttractieLijst;
        }

        public Attractie? GetAttractie(string attractieNaam)
        {
            string query = "SELECT * FROM attracties WHERE naam = @Naam";

            var parameters = new[]
            {
                new MySqlParameter("@Naam", attractieNaam)
            };

            var result = _databaseService.ExecuteQuery(query, parameters);
            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                return new Attractie()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Naam = row["naam"].ToString(),
                    Capaciteit = Convert.ToInt32(row["capaciteit"]),
                    TijdsduurSec = Convert.ToInt32(row["tijdsduur_sec"]),
                    AttractieType = Convert.ToInt32(row["attractie_type_id"]),
                    AttractieGebied = Convert.ToInt32(row["gebied_id"]),
                    Status = Convert.ToBoolean(row["status"])
                };
            }
            return null;
        }

        public Attractie? GetAttractie(int gebied)
        {
            string query = "SELECT * FROM attracties WHERE gebied = @Gebied";

            var parameters = new[]
            {
                new MySqlParameter("@Gebied", gebied)
            };

            var result = _databaseService.ExecuteQuery(query, parameters);
            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[9];
                return new Attractie()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Naam = row["naam"].ToString(),
                    Capaciteit = Convert.ToInt32(row["capaciteit"]),
                    TijdsduurSec = Convert.ToInt32(row["tijdsduur_sec"]),
                    AttractieType = Convert.ToInt32(row["attractie_type_id"]),
                    AttractieGebied = Convert.ToInt32(row["gebied_id"]),
                    Status = Convert.ToBoolean(row["status"])
                };
            }
            return null;
        }
    }
}
