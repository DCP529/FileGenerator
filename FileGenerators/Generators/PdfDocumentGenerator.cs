using Filegenerator.Interfaces;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Image = iText.Layout.Element.Image;

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
    /// <param name="textData">Текстовое наполенение</param>
    /// <param name="totalSize"></param>
    public void Generate(string directoryPath, string textData, decimal numericData, string imagePath, ref long totalSize)
    {
        var filePath = Path.Combine(directoryPath, "example.pdf");

        var writer = new PdfWriter(filePath);
        var pdf = new PdfDocument(writer);

        var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var fontFilePath = Path.Combine(currentDirectory, "arial.ttf");

        var font = PdfFontFactory.CreateFont(fontFilePath, PdfEncodings.IDENTITY_H, pdf);
        var document = new Document(pdf);

        // Устанавливаем кодировку UTF-8
        var para = new Paragraph(textData).SetFont(font);
        document.Add(para);

        var imageData = ImageDataFactory.Create(imagePath);
        var image = new Image(imageData);

        image.SetAutoScale(true);

        document.Add(image);

        document.Close();

        // Обновляем общий размер файлов
        totalSize += new FileInfo(filePath).Length;
    }
}