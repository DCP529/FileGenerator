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
        switch (fileType)
        {
            case "Word":
                return new WordDocumentGenerator();
            case "Excel":
                return new ExcelDocumentGenerator();
            case "PDF":
                return new PdfDocumentGenerator();
            default:
                throw new ArgumentException("Unsupported file type");
        }
    }
}