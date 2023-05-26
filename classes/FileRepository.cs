using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
namespace NEA.classes
{
public class FileRepository
    {
        private readonly string connectionString;

public FileRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

   public IEnumerable<FileModel> GetAllFiles()
        {
            using (var con = new SqlConnection(this.connectionString))
            {
        var query = "SELECT FileId, Name FROM FilesTbl WHERE Name LIKE @SearchKeyword";
                using (var cmd = new SqlCommand(query, con)){
                    con.Open();
                    using (var reader = cmd.ExecuteReader()) {
                        while (reader.Read()){
                            yield return new FileModel {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                        }
                    }
                } } }

public FileModel GetFileById(int id)
{
    
    using (var con = new SqlConnection(this.connectionString))
    {
var query = "select Name, Data, ContentType from FilesTbl where FileId=@FileId";
        // Create a new SqlCommand object with a parameterized query that retrieves the file's name, data, and content type based on the file ID.
        using (var cmd = new SqlCommand(query, con))
        {
          
            cmd.Parameters.AddWithValue("@FileId", id);
            con.Open();
            // Execute the query and return a SqlDataReader object.
            using (var reader = cmd.ExecuteReader())
            {
                // If the SqlDataReader object doesn't contain any data, return null.
                if (!reader.Read())
                {
                    return null;
                }
                // If the SqlDataReader object contains data, create a new FileModel object with the retrieved data and return it.
                return new FileModel
                {
                    Id = id,
                    Name = reader.GetString(0),
                    Data = (byte[])reader.GetValue(1),
                    ContentType = reader.GetString(2)
                };
            }
        }
    }
}



    }
}

