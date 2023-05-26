using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HabibaKhatun_NEA.classes
{
    public class UserManager
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; // from web.config file

        public static DataTable GetUserDataById(int userID)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("View_User_By_ID", sqlConn);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("@UserId", userID);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                return dtbl;
            }
        }

        public int GetUserId(string username, string password) // method to get UserId
        {
            int userId = 0;
            string query = "SELECT UserId FROM UsersTbl WHERE Username = @Username";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetInt32(0);
                        }
                    }
                }
            }
            return userId;
        }

        public static int GetInvoiceId()
        {
            int invoiceId = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Opens the database connection
                con.Open();

                // Creates a SqlCommand object with  SQL query
                SqlCommand cmd = new SqlCommand("SELECT MAX(InvoiceId) FROM InvoicesTbl;", con); 

                // Executes the query and retrieve the invoiceId value
                invoiceId = (int)cmd.ExecuteScalar();

                // Increments the invoiceId value to get the next invoice number
                invoiceId++;

                // then used on the generated pdf... 

            }
            return invoiceId;

        }
        public string GetUserType(int userId)
        {
            string employeeType = null;

            // create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // create a command to select the employee type from the database
                string sql = "SELECT Type FROM UsersTbl WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                // open the database connection and execute the command
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // if a row was returned, retrieve the employee type from the reader
                if (reader.Read())
                {
                    employeeType = reader.GetString(0);
                }

                // close the reader and connection
                reader.Close();
                connection.Close();
            }

            return employeeType;
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
