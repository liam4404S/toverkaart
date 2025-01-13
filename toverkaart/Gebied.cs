using System.Collections.Generic;
using System.Data;
using toverkaart.Pages;

namespace toverkaart
{
    public class Gebied
    {
        private DatabaseService _databaseService;

        public int Id { get; private set; }
        public string Naam { get; private set; } = string.Empty;
        public List<Attractie> Attracties { get; private set; }

        public Gebied() { }

        public Gebied(int id, string naam)
        {
            Id = id;
            Naam = naam;
        }

        public Gebied(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public void AddAttractie(Attractie attractie)
        {
            if (attractie != null && attractie.AttractieGebied?.Id == Id)
            {
                Attracties.Add(attractie);
            }
        }

        public List<Attractie> GetAttracties()
        {
            return Attracties;
        }

        public List<Gebied> GetAllGebieden()
        {
            string query = "SELECT id, naam FROM gebieden";
            var result = _databaseService.ExecuteQuery(query);

            List<Gebied> gebiedenLijst = new List<Gebied>();

            foreach (DataRow row in result.Rows)
            {
                if (row["id"] != DBNull.Value && row["naam"] != DBNull.Value)
                {
                    int id = Convert.ToInt32(row["id"]);
                    string naam = row["naam"].ToString()!;

                    gebiedenLijst.Add(new Gebied(id, naam));
                }
            }
            return gebiedenLijst;
        }
    }
}
