using MySql.Data.MySqlClient;

namespace toverkaart
{
    public class mensen
    {
        private DatabaseService _databaseService;
        public mensen(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

    }
}
