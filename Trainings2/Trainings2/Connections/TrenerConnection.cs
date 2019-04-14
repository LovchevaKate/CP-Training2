using System.Data.SqlClient;

namespace Trainings2.Connections
{
    class TrenerConnection
    {
        const string path = @"Data Source=DESKTOP-NIMN87C;Initial Catalog=DBTraining;Integrated Security=False;User Id=TRENER_LOG; Password=1111";
        static public SqlConnection GetConnection()
        {
            return new SqlConnection(path);
        }
    }
}
