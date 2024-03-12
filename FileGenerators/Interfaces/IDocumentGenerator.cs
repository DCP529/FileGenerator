namespace Filegenerator.Interfaces;

public interface IDocumentGenerator
{
    /// <summary>
    /// Генерация файла
    /// </summary>
    /// <param name="directoryPath">Путь куда будет сгенерирован файл</param>
    /// <param name="textData">Текстовое наполенение</param>
    /// <param name="numericData">число</param>
    /// <param name="imagePath">Картинка</param>
    /// <param name="totalSize">Текущий размер файла</param>
    void Generate(string directoryPath, string textData, decimal numericData, string imagePath, ref long totalSize);
}