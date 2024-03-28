namespace Filegenerator.Interfaces;

public interface IDocumentGenerator
{
    /// <summary>
    /// Генерация файла
    /// </summary>
    /// <param name="directoryPath">Путь куда будет сгенерирован файл</param>
    void Generate(string directoryPath, string sourceFile);
}