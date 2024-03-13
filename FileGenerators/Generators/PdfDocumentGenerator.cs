using Filegenerator.Interfaces;
using iText.Kernel.Pdf;

namespace Filegenerator.Generators;

/// <summary>
/// Генератор пдфовских документов
/// </summary>
public class PdfDocumentGenerator : IDocumentGenerator
{
    /// <summary>
    /// Генерация pdf документа
    /// </summary>
    /// <param name="directoryPath">Путь куда будет сгенерирован файл</param>
    public void Generate(string directoryPath, string sourceFile)
    {
        var reader = new PdfReader(sourceFile);
        var pdfDoc = new PdfDocument(reader);
        using (var writer = new PdfWriter(directoryPath))
        {
            var newPdfDoc = new PdfDocument(writer);
            pdfDoc.CopyPagesTo(1, pdfDoc.GetNumberOfPages(), newPdfDoc);
            newPdfDoc.Close();
        }

        pdfDoc.Close();
    }
}