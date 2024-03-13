using Bogus;
using Filegenerator.Factories;

namespace Filegenerator;

class Program
{
    private static readonly Faker Faker = new("ru");

    static void Main(string[] args)
    {
        var sourceDirectory = "D:\\Киги";
        var rootDirectory = "D:\\GeneratedFiles";

        if (!Directory.Exists(rootDirectory))
        {
            Directory.CreateDirectory(rootDirectory);
        }

        long folderSize;
        var tenGBInBytes = 10L * 1024 * 1024 * 1024; 

        try
        {
            Parallel.ForEach(Directory.GetFiles(sourceDirectory),
                (filePath, state) =>
                {
                    folderSize = GetDirectorySize(rootDirectory);

                    if (folderSize <= tenGBInBytes)
                    {
                        try
                        {
                            GenerateFiles(rootDirectory, filePath, 0);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Какойто файлик не прошел, а точнее {filePath}");
                            throw;
                        }
                    }
                    else
                    {
                        state.Stop();
                    }
                });
        }
        finally
        {
            Console.WriteLine("Папка заполнена, генерация завершена.");
        }
    }

    static long GetDirectorySize(string directoryPath)
    {
        long size = 0;

        var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

        FileInfo fileInfo;
        foreach (var file in files)
        {
            fileInfo = new FileInfo(file);
            size += fileInfo.Length;
        }

        return size;
    }

    /// <summary>
    /// Метод генерации всех видов файлов со вложенностью в глубину 5 папок
    /// </summary>
    /// <param name="directoryPath">Путь сохранения</param>
    /// <param name="depth">Глубина вложенности</param>
    private static void GenerateFiles(string directoryPath, string filePath, int depth)
    {
        while (depth <= 5)
        {
            var directorySubName = Faker.Lorem.Word();
            var directoryName = $"Level_{depth}_{directorySubName}";
            var directoryPathWithDepth = Path.Combine(directoryPath, directoryName);
            Directory.CreateDirectory(directoryPathWithDepth);

            var fileType = Path.GetExtension(filePath);

            var newFileName = $"{directorySubName}{fileType}";
            var newPath = Path.Combine(directoryPathWithDepth, newFileName);

            var documentGenerator = DocumentGeneratorFactory.CreateDocumentGenerator(fileType);
            documentGenerator.Generate(newPath, filePath);

            GenerateFiles(directoryPathWithDepth, newPath, depth + 1);

            depth++;
        }
    }
}