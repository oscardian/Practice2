using ClassLibrary1.Domain.Files;
using ClassLibrary1.Service.Files.Repository.Models;
using Npgsql;
using Tools;


namespace ClassLibrary1.Service.Files.Repository;

public class FileRepository
{
   private const String connectionString = "Host=localhost;Port=5432;Username=postgres;Password=ваш_пароль;Database=postgres";

    public Result SaveFile(FileBlank fileBlank)
        {
        String sql = "INSERT INTO hashsum(id, hashsum, filename , type, isremoved)"
           + "values(@p_id, @p_hashsum, @p_filename, @p_type, @p_isremoved)"
           + "ON CONFLICT(id) DO UPDATE SET hashsum = @p_hashsum, filename = @p_filename, type = @p_type";


        NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        connection.Open();
        using (NpgsqlCommand sqlCommand = new NpgsqlCommand(sql, connection))
        {
            sqlCommand.Parameters.AddWithValue("p_id", Guid.NewGuid());
            sqlCommand.Parameters.AddWithValue("p_hashsum", fileBlank.HashSum);
            sqlCommand.Parameters.AddWithValue("p_filename", fileBlank.Name);
            sqlCommand.Parameters.AddWithValue("p_type", (Int32)fileBlank.FileType);
            sqlCommand.Parameters.AddWithValue("p_isremoved", false);

            int rowsAffected = sqlCommand.ExecuteNonQuery();

        }

        connection.Close();

        return Result.Success();
    }

    public Domain.Files.File[] GetFiles()
    {
        string sql = "Select * from hashsum where isRemoved = false";
        NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        connection.Open();
    
        using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
        {
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                List<FileDB> fileDBs = new List<FileDB>();

                while (reader.Read())
                {
                    FileDB fileDB = FileDB.FromDateReader(reader);
                    fileDBs.Add(fileDB); 
                }

                connection.Close();
                Domain.Files.File[] files = fileDBs.Select(fileDB => fileDB.ToFile()).ToArray();
                return files;
            }
        }
    }

}
