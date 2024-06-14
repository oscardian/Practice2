using ClassLibrary1.Domain.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace ClassLibrary1.Domain.Services;

public interface iFileInterface
{
    public Result SaveFile(FileBlank fileBlank);

    public Domain.Files.File[] GetFiles();
}
