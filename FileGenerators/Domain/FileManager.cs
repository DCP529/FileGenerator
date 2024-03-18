using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace Filegenerator.Domain;

public static class FileManager
{
    /// <summary>
    /// Метод для скачивания архива из облака
    /// </summary>
    /// <param name="url">Адрес архива</param>
    /// <param name="destinationDirectory">Целевая папка</param>
    public static async Task DownloadArchiveAsync(string url, string destinationDirectory)
    {
        var filePath = Path.Combine(destinationDirectory, "sourceArchive.zip");

        if (File.Exists(filePath))
        {
            return;
        }

        using (var client = new HttpClient())
        {
            client.Timeout = TimeSpan.FromHours(1);
            using (var response = await client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    await using var contentStream = await response.Content.ReadAsStreamAsync();
                    await using var fileStream =
                        new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

                    await contentStream.CopyToAsync(fileStream);
                }
                else
                {
                    throw new Exception($"Ошибка загрузки архива: {response.StatusCode}");
                }
            }
        }
    }

    /// <summary>
    /// Метод разархивирования
    /// </summary>
    /// <param name="sourceDirectory">Папка где будет развернут архив</param>
    public static string ExtractArchive(string sourceDirectory)
    {
        var zipFilePath = Path.Combine(sourceDirectory, "sourceArchive.zip");
        var extractPath = Path.Combine(sourceDirectory, "extracted");

        Directory.CreateDirectory(extractPath);

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using (var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Read, Encoding.GetEncoding(866)))
        {
            foreach (var entry in zip.Entries)
            {
                // Игнорируем вложенные директории и извлекаем только файлы
                if (!entry.FullName.EndsWith("/") && !string.IsNullOrEmpty(entry.Name))
                {
                    var entryFileName = Path.GetFileName(entry.FullName);

                    // Путь для сохранения файла в указанной директории
                    var destinationPath = Path.Combine(extractPath, entryFileName);

                    // Извлекаем файл из архива
                    entry.ExtractToFile(destinationPath);
                }
            }
        }

        return extractPath;
    }
}