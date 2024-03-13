using Filegenerator.Generators;
using Filegenerator.Interfaces;

namespace Filegenerator.Factories;

/// <summary>
/// Фабрика генераторов документов
/// </summary>
public static class DocumentGeneratorFactory
{
    /// <summary>
    /// Создание фабрики в зависимости от типа файла
    /// </summary>
    /// <param name="fileType">Тип файла</param>
    public static IDocumentGenerator CreateDocumentGenerator(string fileType)
    {
        return fileType switch
        {
            ".docx" => new WordDocumentGenerator(),
            ".xlsx" => new ExcelDocumentGenerator(),
            ".pdf" => new PdfDocumentGenerator(),
            _ => throw new ArgumentException("Unsupported file type")
        };
    }
}