using System;
using System.Data.SqlClient;
namespace NEA.classes
{
public class ClientRepository
    {
        private readonly string _connectionString;

 public ClientRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

public DataTable GetClientsByUserId(int userId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
var query = "SELECT * FROM ClientsTbl WHERE UserId=@UserId";
                using (var cmd = new SqlCommand(query, con))
{
cmd.Parameters.AddWithValue("@UserId", userId);
using (var da = new SqlDataAdapter(cmd))
{
var dt = new DataTable();
da.Fill(dt);
return dt;     } }   }   }

public void AddClient(ClientModel client)
{
    // Create a new SqlConnection object with the provided connection string.
    using (var con = new SqlConnection(_connectionString))
    {
        using (var cmd = new SqlCommand("INSERT INTO Clients(Name, Gender, EmailAddress, MobileNo, HomeAddress, UserId) VALUES(@Name, @Gender, @EmailAddress, @MobileNo, @HomeAddress, @UserId)", con))
        {
            // Add parameters to the SqlCommand object to prevent SQL injection attacks.
            cmd.Parameters.AddWithValue("@Name", client.Name);
            cmd.Parameters.AddWithValue("@Gender", client.Gender);
            cmd.Parameters.AddWithValue("@EmailAddress", client.EmailAddress);
            cmd.Parameters.AddWithValue("@MobileNo", client.MobileNo);
            cmd.Parameters.AddWithValue("@HomeAddress", client.HomeAddress);
            cmd.Parameters.AddWithValue("@UserId", client.UserId);           
            // Open the SqlConnection object to connect to the database.
            con.Open();          
            cmd.ExecuteNonQuery();
        }
    }
}



      }
}

