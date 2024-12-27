using System;
using System.Windows.Forms;

namespace Приход
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Устанавливаем LoginForm как стартовую форму
            Application.Run(new LoginForm());
        }
    }
}

/*
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Приход
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Создаем экземпляр BookManager для получения списка книг
            var bookManager = new BookManager();
            List<Book> books = bookManager.GetAllBooks();  // Получаем список книг

            // Передаем список книг в MainForm
            Application.Run(new MainForm(books));
        }
    }
}

*/