using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using Filegenerator.Interfaces;

namespace Filegenerator.Generators
{
    /// <summary>
    /// Генератор вордовских документов с использованием Open XML SDK
    /// </summary>
    public class WordDocumentGenerator : IDocumentGenerator
    {
        /// <summary>
        /// Генерация вордовского документа
        /// </summary>
        /// <param name="directoryPath">Путь куда будет сгенерирован файл</param>
        /// <param name="sourceFile">Путь к исходному файлу, если необходимо использовать его содержимое</param>
        public void Generate(string directoryPath, string sourceFile)
        {
            // Создаем новый документ
            using (WordprocessingDocument firstDocument = WordprocessingDocument.Open(sourceFile, false))
            {
                foreach (var part in firstDocument.Parts)
                {
                    // Создаем новый файл и копируем содержимое из исходного
                    using (WordprocessingDocument newDocument = WordprocessingDocument.Create(directoryPath, WordprocessingDocumentType.Document))
                    {
                        newDocument.AddPart(part.OpenXmlPart, part.RelationshipId);
                    }
                }
            }
        }
    }
}