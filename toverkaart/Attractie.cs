using MySql.Data.MySqlClient;
using System.Data;

namespace toverkaart.Pages
{
    public class Attractie
    {
        private DatabaseService _databaseService;

        public int Id { get; private set; }
        public string Naam { get; private set; }
        public int Capaciteit { get; private set; }
        public int TijdsduurSec { get; private set; }
        public int AttractieType { get; private set; }
        public Gebied? AttractieGebied { get; private set; }
        public bool Status { get; private set; }

        public Attractie() { }

        public Attractie(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public Attractie(int id, string naam, int capaciteit, int tijdsduurSec, int attractieType, Gebied? attractieGebied, bool status)
        {
            Id = id;
            Naam = naam;
            Capaciteit = capaciteit;
            TijdsduurSec = tijdsduurSec;
            AttractieType = attractieType;
            AttractieGebied = attractieGebied;
            Status = status;

            AttractieGebied?.AddAttractie(this);
        }

        public List<Attractie> GetAllAttracties(string attractieNaam)
        {
            string query = "SELECT * FROM attracties WHERE naam LIKE @Naam";

            var parameters = new[]
            {
                new MySqlParameter("@Naam", "%" + attractieNaam + "%")
            };

            var result = _databaseService.ExecuteQuery(query, parameters);

            List<Attractie> attractielijst = new List<Attractie>();

            foreach (DataRow row in result.Rows)
            {
                if (row["naam"] != DBNull.Value)
                {
                    var attractie = DataAttractie(row);

                    var gebiedId = Convert.ToInt32(row["gebied_id"]);
                    var gebied = new Gebied(_databaseService);
                    attractie.AttractieGebied = gebied.GetAllGebieden().FirstOrDefault(g => g.Id == gebiedId);

                    attractielijst.Add(attractie);
                }
            }
            return attractielijst;
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
                return DataAttractie(row);
            }
            return null;
        }

        public Attractie? GetAttractie(int gebiedId)
        {
            string query = "SELECT * FROM attracties WHERE gebied_id = @GebiedId";

            var parameters = new[]
            {
                new MySqlParameter("@GebiedId", gebiedId)
            };

            var result = _databaseService.ExecuteQuery(query, parameters);
            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[9];
                return DataAttractie(row);
            }
            return null;
        }

        private Attractie DataAttractie(DataRow row)
        {
            return new Attractie(
                id: Convert.ToInt32(row["id"]),
                naam: row["naam"].ToString()!,
                capaciteit: Convert.ToInt32(row["capaciteit"]),
                tijdsduurSec: Convert.ToInt32(row["tijdsduur_sec"]),
                attractieType: Convert.ToInt32(row["attractie_type_id"]),
                attractieGebied: null,
                status: Convert.ToBoolean(row["status"])
            );
        }
    }
}
