using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using ClosedXML.Excel; // Для ClosedXML
using iTextSharp.text.pdf; //Для PDFWriter
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions; //Для обработки текста.
using DocumentFormat.OpenXml;
using System.Drawing.Printing;
using QRCoder;
using System.Drawing.Imaging;

namespace Приход
{
    // Основная форма приложения
    public partial class MainForm : MaterialForm
    {
        private readonly BookManager _bookManager = new BookManager();
        private readonly List<Book> _userBooks;
        private ListBox booksList; 

        public MainForm(List<Book> userBooks)
        {
            _userBooks = userBooks;
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Настройка MaterialSkin
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);

            // Настройка элементов интерфейса
            this.Text = "Book Manager";
            this.Width = 1100;
            this.Height = 500;

            // Поля ввода
            var titleInput = new MaterialSingleLineTextField { Top = 80, Left = 20, Width = 200, Hint = "Название" };
            var authorInput = new MaterialSingleLineTextField { Top = 120, Left = 20, Width = 200, Hint = "Автор" };
            var yearInput = new MaterialSingleLineTextField { Top = 160, Left = 20, Width = 200, Hint = "Год" };
            var searchInput = new MaterialSingleLineTextField { Top = 320, Left = 20, Width = 200, Hint = "Поиск" };

            // Кнопки
            var addButton = new MaterialRaisedButton { Text = "Добавить книгу", Top = 200, Left = 20, Width = 200 };
            var removeButton = new MaterialRaisedButton { Text = "Удалить книгу", Top = 240, Left = 20, Width = 200 };
            var viewButton = new MaterialRaisedButton { Text = "Показать все книги", Top = 280, Left = 20, Width = 200 };
            var findByNameButton = new MaterialRaisedButton { Text = "Найти по названию", Top = 360, Left = 20, Width = 200 };
            var findByAuthorButton = new MaterialRaisedButton { Text = "Найти книгу по автору", Top = 400, Left = 20, Width = 200 };

            // Стандартный ListBox для вывода книг
            booksList = new ListBox
            {
                Top = 120,
                Left = 240,
                Width = 250,
                Height = 360,
                DrawMode = DrawMode.OwnerDrawVariable, // Важный параметр!
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.FromArgb(33, 33, 33),
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            booksList.DrawItem += BooksList_DrawItem;
            booksList.MeasureItem += BooksList_MeasureItem;
            booksList.BackColor = System.Drawing.Color.FromArgb(255, 255, 255); // Белый фон
            booksList.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33); // Темный текст
            booksList.Font = new System.Drawing.Font("Segoe UI", 10);

            // Стандартный ListBox для вывода результатов поиска
            var searchResultsList = new ListBox
            {
                Top = 120,
                Left = 500,
                Width = 250,
                Height = 360,
                DrawMode = DrawMode.OwnerDrawVariable, // Важный параметр!
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.FromArgb(33, 33, 33),
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            searchResultsList.DrawItem += BooksList_DrawItem;
            searchResultsList.MeasureItem += BooksList_MeasureItem;
            searchResultsList.BackColor = System.Drawing.Color.FromArgb(255, 255, 255); // Белый фон
            searchResultsList.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33); // Темный текст
            searchResultsList.Font = new System.Drawing.Font("Segoe UI", 10);

            // Добавляем PictureBox
            var qrCodeBox = new PictureBox { Top = 120, Left = 760, Width = 250, Height = 250, BackColor = System.Drawing.Color.White };

            booksList.SelectedIndexChanged += (sender, args) =>
            {
                if (booksList.SelectedItem is Book selectedBook)
                {
                    GenerateQrCode(selectedBook, qrCodeBox);
                }
                else
                {
                    qrCodeBox.Image = null; // Убираем QR-код если ничего не выбрано
                }
            };

            // Надписи "Список книг" и "Выборка"
            var showLabel = new MaterialLabel { Text = "Список книг", Top = 80, Left = 240, TextAlign = ContentAlignment.MiddleLeft };
            var findLabel = new MaterialLabel { Text = "Выборка:", Top = 80, Left = 500, TextAlign = ContentAlignment.MiddleLeft };
            var qrLabel = new MaterialLabel { Text = "QR-код книги", Top = 80, Left = 760, TextAlign = ContentAlignment.MiddleLeft };


            Controls.Add(showLabel);
            Controls.Add(findLabel);
            Controls.Add(qrLabel);

            // Обработчики событий
            addButton.Click += (sender, args) =>
            {
                if (int.TryParse(yearInput.Text, out var year))
                {
                    _bookManager.AddBook(titleInput.Text, authorInput.Text, year);
                    titleInput.Clear();
                    authorInput.Clear();
                    yearInput.Clear();
                    MessageBox.Show("Книга успешно добавлена");
                }
                else
                {
                    MessageBox.Show("Год книги введён неправильно");
                }
            };
            removeButton.Click += (sender, args) =>
            {
                if (booksList.SelectedItem is Book selectedBook)
                {
                    _bookManager.RemoveBook(selectedBook.Id);
                    MessageBox.Show("Книга успешно удалена");
                    UpdateBookList(booksList);
                }
                else
                {
                    MessageBox.Show("Выберите книгу для удаления");
                }
            };

            viewButton.Click += (sender, args) =>
            {
                UpdateBookList(booksList);
            };

            findByNameButton.Click += (sender, args) =>
            {
                var searchTerm = searchInput.Text;
                var results = _bookManager.FindBookByName(searchTerm);
                UpdateSearchResults(searchResultsList, results);
            };

            findByAuthorButton.Click += (sender, args) =>
            {
                var searchTerm = searchInput.Text;
                var results = _bookManager.FindBookByAuthor(searchTerm);
                UpdateSearchResults(searchResultsList, results);
            };

            // Надписи "Импорт" и "Экспорт"
            var importLabel = new MaterialLabel { Text = "Импорт:", Top = 30, Left = 240, TextAlign = ContentAlignment.MiddleLeft };
            var exportLabel = new MaterialLabel { Text = "Экспорт:", Top = 30, Left = 440, TextAlign = ContentAlignment.MiddleLeft };

            // Кнопки экспорта и импорта
            var comboBoxImport = new System.Windows.Forms.ComboBox { Top = 30, Left = 350, Width = 50, FlatStyle = FlatStyle.Flat };
            var comboBoxExport = new System.Windows.Forms.ComboBox { Top = 30, Left = 550, Width = 50, FlatStyle = FlatStyle.Flat };

            // Убираем рамку у выпадающего списка
            comboBoxImport.FlatStyle = FlatStyle.Flat;
            comboBoxExport.FlatStyle = FlatStyle.Flat;

            // Устанавливаем нужные цвета
            comboBoxImport.BackColor = System.Drawing.Color.White;
            comboBoxImport.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33);
            comboBoxExport.BackColor = System.Drawing.Color.White;
            comboBoxExport.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33);

            // Устанавливаем нужный шрифт
            comboBoxImport.Font = new System.Drawing.Font("Segoe UI", 10);
            comboBoxExport.Font = new System.Drawing.Font("Segoe UI", 10);

            // Заполняем ComboBox для импорта
            comboBoxImport.Items.AddRange(new string[] { "csv", "json", "docx", "pdf" });
            comboBoxImport.SelectedIndex = 0; // Выбираем первый элемент по умолчанию

            // Заполняем ComboBox для экспорта
            comboBoxExport.Items.AddRange(new string[] { "csv", "json", "docx", "pdf" });
            comboBoxExport.SelectedIndex = 0;  // Выбираем первый элемент по умолчанию

            comboBoxImport.SelectedIndexChanged += comboBoxImport_SelectedIndexChanged;
            comboBoxExport.SelectedIndexChanged += comboBoxExport_SelectedIndexChanged;


            Controls.Add(titleInput);
            Controls.Add(authorInput);
            Controls.Add(yearInput);

            Controls.Add(searchInput);
            Controls.Add(addButton);
            Controls.Add(removeButton);
            Controls.Add(viewButton);

            Controls.Add(findByNameButton);
            Controls.Add(findByAuthorButton);

            Controls.Add(booksList);
            Controls.Add(searchResultsList);
            Controls.Add(qrCodeBox);

            Controls.Add(importLabel);
            Controls.Add(exportLabel);
            Controls.Add(comboBoxImport);
            Controls.Add(comboBoxExport);

            // Добавляем кнопку печати
            var printButton = new MaterialRaisedButton { Text = "Распечатать", Top = 440, Left = 20, Width = 200 };

            this.Controls.Add(printButton);

            // Обработчик нажатия на кнопку "Распечатать"
            printButton.Click += (sender, args) =>
            {
                try
                {
                    // 1. Сообщение "Подготовка к печати"
                    MessageBox.Show("Подготовка к печати...", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Создание объекта PrintDocument
                    PrintDocument printDocument = new PrintDocument();
                    printDocument.PrintPage += PrintDocument_PrintPage;

                    // Создание PrintPreviewDialog
                    PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                    printPreviewDialog.Document = printDocument;

                    // Показываем диалог предпросмотра
                    if (printPreviewDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 2. Сообщение "Печать завершена"
                        MessageBox.Show("Печать завершена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // 3. Сообщение "Ошибка печати"
                    MessageBox.Show($"Ошибка печати: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }
        private void BooksList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var book = _bookManager.GetAllBooks()[e.Index];

            string text = book.ToString();

            e.DrawBackground();
            e.DrawFocusRectangle();

            using (var stringFormat = new StringFormat())
            {
                stringFormat.FormatFlags = StringFormatFlags.LineLimit;
                stringFormat.Alignment = StringAlignment.Near;

                e.Graphics.DrawString(text, booksList.Font, Brushes.Black, e.Bounds, stringFormat);
            }
        }
        private void BooksList_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0) return;

            var book = _bookManager.GetAllBooks()[e.Index];
            string text = book.ToString();
            SizeF size = e.Graphics.MeasureString(text, booksList.Font, booksList.Width);
            e.ItemHeight = (int)size.Height + 5; // Устанавливаем высоту элемента на основе размера текста
        }
        private void comboBoxImport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                string selectedFormat = comboBox.SelectedItem.ToString();
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = GetFileFilter(selectedFormat);
                    openFileDialog.Title = "Выберите файл для импорта";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            ImportBooks(openFileDialog.FileName, selectedFormat);
                            UpdateBookList(booksList);
                            MessageBox.Show("Импорт завершен!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка импорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void comboBoxExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                string selectedFormat = comboBox.SelectedItem.ToString();
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = GetFileFilter(selectedFormat);
                    saveFileDialog.Title = "Выберите место для сохранения файла";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            ExportBooks(saveFileDialog.FileName, selectedFormat);
                            MessageBox.Show("Экспорт завершен!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void ImportBooks(string filePath, string format)
        {
            switch (format)
            {
                case "csv":
                    _bookManager.ImportBooksFromCsv(filePath);
                    break;
                case "json":
                    _bookManager.ImportBooksFromJson(filePath);
                    break;
                case "docx":
                    _bookManager.ImportBooksFromDocx(filePath);
                    break;
                case "pdf":
                    _bookManager.ImportBooksFromPdf(filePath);
                    break;
            }
        }

        private void ExportBooks(string filePath, string format)
        {
            switch (format)
            {
                case "svg":
                    _bookManager.ExportBooksToCsv(filePath);
                    break;
                case "json":
                    _bookManager.ExportBooksToJson(filePath);
                    break;
                case "docx":
                    ExportBooksToDocx(filePath);
                    break;
                case "pdf":
                    ExportBooksToPdf(filePath);
                    break;
            }
        }

        private string GetFileFilter(string format)
        {
            switch (format)
            {
                case "csv":
                    return "CSV Files (*.csv)|*.csv";
                case "json":
                    return "JSON Files (*.json)|*.json";
                case "docx":
                    return "Word Documents (*.docx)|*.docx";
                case "pdf":
                    return "PDF Files (*.pdf)|*.pdf";
                default:
                    return "All Files (*.*)|*.*";
            }
        }

        private void ExportBooksToDocx(string filePath)
        {
            try
            {
                using (WordprocessingDocument document = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = document.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    foreach (var book in _bookManager.GetAllBooks())
                    {
                        Paragraph para = body.AppendChild(new Paragraph());
                        Run run = para.AppendChild(new Run());
                        run.AppendChild(new Text($"{book.Title} by {book.Author} ({book.Year})"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта DOCX: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportBooksToPdf(string filePath)
        {
            using (var document = new iTextSharp.text.Document())
            {
                try
                {
                    iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                    document.Open();
                    var font = iTextSharp.text.FontFactory.GetFont("Arial", iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

                    foreach (var book in _bookManager.GetAllBooks())
                    {
                        var paragraph = new iTextSharp.text.Paragraph($"{book.Title} by {book.Author} ({book.Year})", font);
                        document.Add(paragraph);
                    }
                }
                finally
                {
                    document.Close();
                }
            }
        }
        private void UpdateBookList(ListBox lb)
        {
            lb.Items.Clear();
            foreach (var book in _bookManager.GetAllBooks())
            {
                lb.Items.Add(book);
            }
        }

        private void UpdateSearchResults(ListBox lb, System.Collections.Generic.List<Book> books)
        {
            lb.Items.Clear();
            foreach (var book in books)
            {
                lb.Items.Add(book);
            }
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            float y = e.MarginBounds.Top; // Установка начальной Y-координаты
            float lineHeight = 20; // Высота строки

            foreach (var book in _bookManager.GetAllBooks())
            {
                // Рисуем строку с информацией о книге
                e.Graphics.DrawString(book.ToString(), new System.Drawing.Font("Arial", 12), Brushes.Black, e.MarginBounds.Left, y);
                y += lineHeight; // Увеличиваем Y-координату для следующей строки

                // Если текст выходит за пределы страницы, то переносим на следующую
                if (y > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }
        }
        private void GenerateQrCode(Book book, PictureBox qrCodeBox)
        {
            string qrData = $"ID: {book.Id}\nTitle: {book.Title}\nAuthor: {book.Author}\nYear: {book.Year}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(3);

            qrCodeBox.Image = qrCodeImage;
        }
    }
}