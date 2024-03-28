using Bogus;
using Filegenerator.Domain;
using Filegenerator.Factories;

namespace Filegenerator
{
    class Program
    {
        private static readonly Faker Faker = new("ru");
        private static string[] files = Array.Empty<string>();
        static async Task Main(string[] args)
        {
            //внимательно с урлом в гугл диске, в конце должен быть идентификатор файла
            var sourceArchiveUrl =
                "https://drive.usercontent.google.com/download?id=1rxVJ4D74JHrN-zDxAZUW_WOd8mUl8MQw&export=download&authuser=0&confirm=t&uuid=162fa6c4-39fe-4262-88e7-0b48446cf3c4&at=APZUnTU42JaJES7XtXsNRQ-aV8sE:1710770106976\n";

            var sourceDirectory = "D:\\";
            var rootDirectory = "D:\\GeneratedFiles";

            if (!Directory.Exists(rootDirectory))
            {
                Directory.CreateDirectory(rootDirectory);
            }

            long folderSize;
            var tenGBInBytes = 12L * 1024 * 1024;

            try
            {
                await FileManager.DownloadArchiveAsync(sourceArchiveUrl, sourceDirectory);
                sourceDirectory = FileManager.ExtractArchive(sourceDirectory);

                files = Directory.GetFiles(sourceDirectory);
                Parallel.ForEach(files, (filePath, state) =>
                {
                    folderSize = GetDirectorySize(rootDirectory);

                    if (folderSize <= tenGBInBytes)
                    {
                        try
                        {
                            GenerateFiles(rootDirectory, filePath, 0);
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine($"Файл {filePath} не доступен для обработки. Ошибка: {ex.Message}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Какой-то файл не прошел обработку, а именно: {filePath}. Ошибка: {e}");
                            throw;
                        }
                    }
                    else
                    {
                        state.Stop();
                    }
                });
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Папка заполнена, генерация завершена.");
            }
        }

        /// <summary>
        /// Получить текущий размер папки
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
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
        /// <param name="filePath">Путь к файлу</param>
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

                for (var i = 0; i < Random.Shared.Next(1, 8); i++)
                {
                    var fileIndex = Random.Shared.Next(1, 50);
                    var someFileType = Path.GetExtension(files[fileIndex]);
                    
                    directorySubName = Faker.Lorem.Word();
                    var newSomeFileName = $"{directorySubName}{someFileType}";
                    var newSomePath = Path.Combine(directoryPathWithDepth, newSomeFileName);
                    
                    documentGenerator = DocumentGeneratorFactory.CreateDocumentGenerator(someFileType);
                    documentGenerator.Generate(newSomePath, files[fileIndex]);
                }

                GenerateFiles(directoryPathWithDepth, newPath, depth + 1);

                depth++;
            }
        }
    }
}