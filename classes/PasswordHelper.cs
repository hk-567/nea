using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace NEA.classes
{
    public class PasswordHelper
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; // from web.config file
   private static string HashPassword(string password, string salt)
       {
            
return BCrypt.Net.BCrypt.HashPassword(password + salt, BCrypt.Net.BCrypt.GenerateSalt());
}

// Verify a password
        private static bool VerifyPassword(string password, string salt, string hashedPassword) {
if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt) || string.IsNullOrEmpty(hashedPassword))
    {
        return false;
    }
            return BCrypt.Net.BCrypt.Verify(password + salt, hashedPassword); // called in login button event  
        }

 // Store the password in the database
        public static void StorePassword(string username, string email, string password)
        {
            // Generates a new salt for each password
            string salt = Guid.NewGuid().ToString();

            // Hashes the password
            string hashedPassword = HashPassword(password, salt);

            string activationCode = Guid.NewGuid().ToString();

            // Stores the username, hashed password, and salt in the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO UsersTbl (Username, Email, Password, Salt ) VALUES (@Username, @Email, @Password, @Salt)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Salt", salt);
                    command.Parameters.AddWithValue("@ActivationCode", activationCode);


                    command.ExecuteNonQuery();
                }
                               connection.Close();
            }

        }

        // Verify the password when logging in
       public static bool VerifyLogin(string username, string password)
{
    // Retrieve the hashed password and salt for the username from the database

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        string sql = "SELECT Password,Salt FROM UsersTbl WHERE Username = @Username";

        using (SqlCommand command = new SqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@Username", username);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    string storedHashedPassword = reader["Password"] as string;
                    if (storedHashedPassword != null)
                    {
                        Debug.WriteLine("stored" + reader["Password"]);

                        string salt = reader["Salt"] as string;
                        string hashedPassword = HashPassword(password, salt); // arguments are the user entered password and the salt from db

                        // Compares the hashed passwords
                        bool myBool = VerifyPassword(password, salt, storedHashedPassword);
                        return myBool;
                    }

                }
            }
        }
        return false;
    }

}

    }
}

