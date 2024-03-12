using Filegenerator.Interfaces;
using NPOI.Util;
using NPOI.XWPF.UserModel;

namespace Filegenerator.Generators;

/// <summary>
/// Генератор вордовских документов
/// </summary>
public class WordDocumentGenerator : IDocumentGenerator
{
    /// <summary>
    /// Генерация вордовского документа
    /// </summary>
    /// <param name="directoryPath">Путь куда будет сгенерирован файл</param>
    /// <param name="textData">Текстовое наполенение</param>
    /// <param name="numericData"></param>
    /// <param name="imagePath">Картинка</param>
    /// <param name="totalSize"></param>
    public void Generate(string directoryPath, string textData, decimal numericData, string imagePath, ref long totalSize)
    {
        var filePath = Path.Combine(directoryPath, "example.docx");

        using (var doc = new XWPFDocument())
        {
            var para = doc.CreateParagraph();
            var run = para.CreateRun();

            run.SetText(textData);

            using (var imgStream = File.OpenRead(imagePath))
            {
                var imgPara = doc.CreateParagraph();
                var imgRun = imgPara.CreateRun();

                imgRun.AddPicture(imgStream, (int)PictureType.PNG, "example.png", Units.ToEMU(200), Units.ToEMU(200));
            }

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                doc.Write(fs);
            }
        }

        // Обновляем общий размер файлов
        totalSize += new FileInfo(filePath).Length;
    }
}