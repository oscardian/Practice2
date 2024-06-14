using ClassLibrary1.Domain.Files;
using ClassLibrary1.Domain.Services;
using ClassLibrary1.Service.Files.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace ClassLibrary1.Service.Files;

public class FileService : iFileInterface
{
    private readonly FileRepository repository;

    public FileService()
    {
        repository = new FileRepository();
    }

    public Domain.Files.File[] GetFiles()
    {

        return repository.GetFiles();
    }

    public Result SaveFile(FileBlank fileBlank)
    {
        if (fileBlank is not { } fileblank) throw new Exception("FileBlank был null");

        if (fileblank.Name is not { } name) throw new Exception("Свойство Name в FileBlank было null");

        if (fileblank.HashSum is not { } hashSum) throw new Exception("Свойство HashSum было null");

        repository.SaveFile(fileBlank);

        return Result.Success();
    }
}
