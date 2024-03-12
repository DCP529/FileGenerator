using Filegenerator.Interfaces;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Filegenerator.Generators;

/// <summary>
/// Генератор екселевских документов
/// </summary>
public class ExcelDocumentGenerator : IDocumentGenerator
{
    /// <summary>
    /// Генерация excel(xlsx) файла
    /// </summary>
    /// <param name="directoryPath">Путь куда будет сгенерирован файл</param>
    /// <param name="textData"></param>
    /// <param name="numericData">число</param>
    /// <param name="imagePath"></param>
    /// <param name="totalSize"></param>
    public void Generate(string directoryPath, string textData, decimal numericData, string imagePath,
        ref long totalSize)
    {
        var filePath = Path.Combine(directoryPath, "example.xlsx");

        var workbook = new XSSFWorkbook();

        var sheet = workbook.CreateSheet("Sheet1");

        var value = textData.Split();

        var rand = new Random();
        
        for (var row = 0; row < 100; row++)
        {
            var excelRow = sheet.CreateRow(row);
            for (var col = 0; col < 100; col++)
            {
                var cell = excelRow.CreateCell(col);
                cell.SetCellValue(value[col + rand.NextInt64(1, 1000)]); // Пример генерации случайных чисел
            }
        }

        using (var fs = new FileStream(filePath, FileMode.Create))
        {
            workbook.Write(fs);
        }

        // Обновляем общий размер файлов
        totalSize += new FileInfo(filePath).Length;
    }
}