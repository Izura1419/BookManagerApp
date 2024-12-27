using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions; //Для обработки текста
using System.Windows.Forms; //Для MessageBox
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Приход
{
    // Класс для управления коллекцией книг
    public class BookManager
    {
        private readonly List<Book> _books = new List<Book>();

        public void AddBook(string title, string author, int year)
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = title,
                Author = author,
                Year = year
            };
            _books.Add(book);
        }

        public bool RemoveBook(Guid id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _books.Remove(book);
                return true;
            }
            return false;
        }

        public List<Book> FindBookByName(string name)
        {
            return _books.Where(b => b.Title.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        public List<Book> FindBookByAuthor(string author)
        {
            return _books.Where(b => b.Author.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        public List<Book> GetAllBooks()
        {
            return _books;
        }
        public void PrintAllBooks(Action<string> outputAction)
        {
            foreach (var book in _books)
            {
                outputAction?.Invoke($"{book.Id} - {book.Title} by {book.Author} ({book.Year})");
            }
        }
        // Метод для импорта книг из CSV-файла
        public void ImportBooksFromCsv(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3) // Убедимся, что строка содержит 3 элемента (название, автор, год)
                    {
                        var title = parts[0].Trim();
                        var author = parts[1].Trim();
                        var year = int.TryParse(parts[2].Trim(), out var parsedYear) ? parsedYear : 0;

                        if (year != 0) // Только если год корректен
                        {
                            AddBook(title, author, year);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing books: {ex.Message}");
            }
        }
        // Метод для экспорта книг в CSV-файл
        public void ExportBooksToCsv(string filePath)
        {
            try
            {
                var lines = new List<string>();

                foreach (var book in _books)
                {
                    var line = $"{book.Title},{book.Author},{book.Year}";
                    lines.Add(line);
                }

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting books: {ex.Message}");
            }
        }
        // Метод для импорта книг из JSON-файла
        public void ImportBooksFromJson(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var books = JsonConvert.DeserializeObject<List<Book>>(json);

                foreach (var book in books)
                {
                    AddBook(book.Title, book.Author, book.Year);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing books: {ex.Message}");
            }
        }
        // Метод для экспорта книг в JSON-файл
        public void ExportBooksToJson(string filePath)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_books, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting books: {ex.Message}");
            }
        }
        public void ImportBooksFromDocx(string filePath)
        {
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
                {
                    if (wordDoc != null)
                    {
                        Body body = wordDoc.MainDocumentPart.Document.Body;
                        if (body != null)
                        {
                            string text = "";
                            foreach (var paragraph in body.Descendants<Paragraph>())
                            {
                                text += paragraph.InnerText + "\n";
                            }

                            // Регулярное выражение для извлечения данных книги
                            string pattern = @"(.*?)\sby\s(.*?)\s\((.*?)\)";
                            foreach (Match match in Regex.Matches(text, pattern))
                            {
                                if (match.Groups.Count == 4)
                                {
                                    string title = match.Groups[1].Value.Trim();
                                    string author = match.Groups[2].Value.Trim();
                                    if (int.TryParse(match.Groups[3].Value.Trim(), out int year))
                                    {
                                        AddBook(title, author, year);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка импорта DOCX: {ex.Message}");
            }
        }
        public void ImportBooksFromPdf(string filePath)
        {
            try
            {
                using (PdfReader reader = new PdfReader(filePath))
                {
                    string text = "";
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        text += PdfTextExtractor.GetTextFromPage(reader, i);
                    }
                    // Регулярное выражение для извлечения данных книги
                    string pattern = @"(.*?)\sby\s(.*?)\s\((.*?)\)";
                    foreach (Match match in Regex.Matches(text, pattern))
                    {
                        if (match.Groups.Count == 4)
                        {
                            string title = match.Groups[1].Value.Trim();
                            string author = match.Groups[2].Value.Trim();
                            if (int.TryParse(match.Groups[3].Value.Trim(), out int year))
                            {
                                AddBook(title, author, year);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка импорта PDF: {ex.Message}");
            }
        }

    }
}
