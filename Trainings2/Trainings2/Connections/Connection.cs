using System.Data.SqlClient;

namespace Trainings2.Connections
{
    class Connection
    {
        const string path = @"Data Source=DESKTOP-NIMN87C;Initial Catalog=DBTraining;Integrated Security=True";
        static public SqlConnection GetConnection()
        {
            return new SqlConnection(path);
        }
    }
}
