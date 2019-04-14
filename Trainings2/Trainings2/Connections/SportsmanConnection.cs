using System.Data.SqlClient;

namespace Trainings2.Connections
{
    class SportsmanConnection
    {
        const string path = @"Data Source=DESKTOP-NIMN87C;Initial Catalog=DBTraining;User Id=SPORTSMAN_LOG; Password=2222";
        static public SqlConnection GetConnection()
        {
            return new SqlConnection(path);
        }
    }
}
