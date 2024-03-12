using Bogus;
using Filegenerator.Enums;
using Filegenerator.Factories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Filegenerator;

class Program
{
    private static readonly Faker Faker = new("ru");

    static void Main(string[] args)
    {
        var rootDirectory = "D:\\GeneratedFiles";

        if (!Directory.Exists(rootDirectory))
        {
            Directory.CreateDirectory(rootDirectory);
        }

        var totalSize = 0L;
        var mustHaveSize = 10L * 1024 * 1024 * 1024;

        // Пока общий размер файлов не достигнет 10 ГБ
        while (totalSize < mustHaveSize)
        {
            var textData = Faker.Lorem.Sentences(10_000);
            var numericData = Faker.Random.Decimal();
            GenerateFiles(rootDirectory, textData, numericData, 0, ref totalSize);
        }

        Console.WriteLine("Генерация завершена.");
    }

    /// <summary>
    /// Метод генерации всех видов файлов со вложенностью в глубину 5 папок
    /// </summary>
    /// <param name="directoryPath">Путь сохранения</param>
    /// <param name="textData">Содержимый текст в файлах</param>
    /// <param name="numericData">Числа в экселе</param>
    /// <param name="depth">Глубина вложенности</param>
    /// <param name="totalSize"></param>
    private static void GenerateFiles(string directoryPath, string textData, decimal numericData, int depth,
        ref long totalSize)
    {
        while (depth <= 5)
        {
            var directorySubName = Faker.Lorem.Word();

            var directoryName = $"Level_{depth}_{directorySubName}";
            var directoryPathWithDepth = Path.Combine(directoryPath, directoryName);

            Directory.CreateDirectory(directoryPathWithDepth);

            var imagePath = GeneratePhoto();
            var fileType = GetRandomFileType().ToString();

            for (var i = 0; i < 10; i++)
            {
                var documentGenerator = DocumentGeneratorFactory.CreateDocumentGenerator(fileType);
                documentGenerator.Generate(directoryPath, textData, numericData, imagePath, ref totalSize);

                directoryPath = directoryPathWithDepth;
            }

            depth++;
        }
    }

    /// <summary>
    /// Получение рандомного генератора файла
    /// </summary>
    /// <returns></returns>
    static FileType GetRandomFileType()
    {
        var random = new Random();
        var fileTypes = Enum.GetValues(typeof(FileType));

        return (FileType)fileTypes.GetValue(random.Next(fileTypes.Length))!;
    }

    /// <summary>
    /// Генерация изображения
    /// </summary>
    /// <returns>Возвращает путь к созданной фотографиии</returns>
    private static string GeneratePhoto()
    {
        const int width = 800;
        const int height = 600;
        const string outputPath = "output.png";

        using (Image<Rgba32> image = new Image<Rgba32>(width, height))
        {
            // Заполняем изображение белым цветом
            image.Mutate(ctx => ctx.BackgroundColor(Color.Azure));

            // Сохраняем изображение
            image.Save(outputPath); // Сохраняем в формате PNG
        }

        return outputPath;
    }
}