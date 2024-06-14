using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Domain.Files;

public class File
{
    public string Name { get; }

    public FileType FileType { get;}

    public string HashSum { get; }

    public File(String name,FileType fileType,string hashSum)
    {
        Name = name;

        FileType = fileType;

        HashSum = hashSum;
    }
}
