using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace NEA.classes
{
    public class UserManager
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public static DataTable GetUserDataById(int userId)
public int GetUserId(string username, string password)
{
    int userId = 0;
    string query = "SELECT UserId FROM UsersTbl WHERE Username = @Username";
    try
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {   
          // If the reader returns a row, get the user ID from the first column of the result set.
                    if (reader.Read())
                    {
                        userId = reader.GetInt32(0);
                    }
                }
            }
        }
    }
    catch (SqlException ex)
    {
throw;
          }
    catch (Exception ex)
    {
throw;
            }
    return userId;
}

   public static int GetInvoiceId()
{
    int invoiceId = 0;
    try
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            // Opens the database connection
            con.Open();

            // Creates a SqlCommand object with SQL query
string query = "SELECT MAX(InvoiceId) FROM InvoicesTbl";
            using(SqlCommand cmd = new SqlCommand(query, con))
{

            // Executes the query and retrieve the invoiceId value
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                invoiceId = (int)result;
            }

            // Increments the invoiceId value to get the next invoice number
            invoiceId++;
}
        }
    }
    catch (SqlException ex)
    {
        // Handle the exception by displaying an error message
        //re-throw the exception to be handled by the calling method
        throw;
    }
    catch (Exception ex)
    {
     throw;
    }
    return invoiceId;
}
 public bool UserExists(string username)
        {
            bool exists = false;

            // connect to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // create a SQL command to check for the user
                string sql = "SELECT COUNT(*) FROM UsersTbl WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // add parameters to the command to prevent SQL injection
                    command.Parameters.AddWithValue("@Username", username);

                    // execute the command and check the result
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    exists = (count > 0);
                }
            }

            return exists;
        }


    }
    
}

