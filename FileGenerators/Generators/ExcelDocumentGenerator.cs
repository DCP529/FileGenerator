using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Filegenerator.Interfaces;

namespace Filegenerator.Generators;

public class ExcelDocumentGenerator : IDocumentGenerator
{
    /// <summary>
    /// Генерация excel(xlsx) файла
    /// </summary>
    /// <param name="directoryPath">Путь куда будет сгенерирован файл</param>
    /// <param name="sourceFile">Путь к исходному файлу, если необходимо использовать его содержимое</param>
    public void Generate(string directoryPath, string sourceFile)
    {
        using (var sourceStream = File.Open(sourceFile, FileMode.Open, FileAccess.Read))
        using (var destinationStream = File.Create(directoryPath))
        {
            // Открываем исходный файл
            using (var sourceDocument = SpreadsheetDocument.Open(sourceStream, false))
            {
                // Создаем новый документ Excel
                using (var destinationDocument = SpreadsheetDocument.Create(destinationStream, SpreadsheetDocumentType.Workbook))
                {
                    // Создаем новую часть Workbook для целевого документа
                    var destinationWorkbookPart = destinationDocument.AddWorkbookPart();

                    // Копируем содержимое Workbook из исходного документа в целевой
                    using (var writer = destinationWorkbookPart.GetStream(FileMode.Create))
                    {
                        sourceDocument.WorkbookPart.Workbook.Save(writer);
                    }

                    // Копируем содержимое всех листов из исходного документа в новый документ
                    foreach (var sheetPart in sourceDocument.WorkbookPart.WorksheetParts)
                    {
                        var newSheetPart = destinationWorkbookPart.AddNewPart<WorksheetPart>();
                        sheetPart.Worksheet.Save(newSheetPart);
                    }
                }
            }
        }
    }
}