using ClassLibrary1.Domain.Files;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Service.Files.Repository.Models;

internal class FileDB
{
    public string Name { get; set; }

    public FileType FileType { get; set; }

    public string HashSum { get; set; }

    public static FileDB FromDateReader(NpgsqlDataReader reader)
    {
        FileDB fileDB = new FileDB();

        fileDB.Name = reader.GetString("filename");
        fileDB.FileType = (FileType)reader.GetInt32("type");
        fileDB.HashSum = reader.GetString("hashsum");

        return fileDB;
    }

    public Domain.Files.File ToFile()
    {
        return new(Name, FileType, HashSum);
    }
}
