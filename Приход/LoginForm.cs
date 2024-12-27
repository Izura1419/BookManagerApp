using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Приход
{
    public partial class LoginForm : MaterialForm
    {
        public LoginForm()
        {
            InitializeComponent();
            InitializeLoginForm();
        }

        private void InitializeLoginForm()
        {
            //создание админа, чтобы он всегда был при запуске формы
            var newUser = new User("admin", "admin", "admin");
            UserRepository.Instance.AddUser(newUser);
            //создание обычного пользователя, чтобы можно было быстро перейти к форме библиотеки
            newUser = new User("user", "user", "user");
            UserRepository.Instance.AddUser(newUser);


            // Установка размеров формы
            this.Text = "Авторизация";
            this.Size = new System.Drawing.Size(800, 480);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Создание элементов
            var loginInput = new MaterialSkin.Controls.MaterialSingleLineTextField
            {
                Hint = "Введите ваш логин",
                Width = 250,
                Top = 100,
            };

            var passwordInput = new MaterialSkin.Controls.MaterialSingleLineTextField
            {
                Hint = "Введите ваш пароль",
                Width = 250,
                Top = 150,
                PasswordChar = '*'
            };

            var loginButton = new MaterialSkin.Controls.MaterialRaisedButton
            {
                Text = "Войти",
                Width = 150,
                Top = 200
            };

            var registerButton = new MaterialSkin.Controls.MaterialFlatButton
            {
                Text = "Создать аккаунт",
                Width = 150,
                Top = 250
            };

            // Центрирование элементов
            loginInput.Left = (this.ClientSize.Width - loginInput.Width) / 2;
            passwordInput.Left = (this.ClientSize.Width - passwordInput.Width) / 2;
            loginButton.Left = (this.ClientSize.Width - loginButton.Width) / 2;
            registerButton.Left = (this.ClientSize.Width - registerButton.Width) / 2;

            // Обработка событий
            loginButton.Click += (sender, args) =>
            {
                string email = loginInput.Text;
                string password = passwordInput.Text; 

                // Проверка на пустые поля
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Пожалуйста, введите email и пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 1. Поиск пользователя в коллекции
                User user = UserRepository.Instance.GetUserByEmail(email);

                // 2. Проверка результата и переход или сообщение
                if (user != null && user.Password == password && user.Role == "user")
                {
                    // Пользователь найден, переходим на страницу библиотеки (замените на ваш код перехода)
                    MessageBox.Show("Авторизация успешна!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var bookManager = new BookManager();
                    List<Book> books = bookManager.GetAllBooks();

                    var mainForm = new MainForm(books);
                    mainForm.Show();
                    this.Hide();


                }

                else if (user.Role == "admin")
                {
                    var adminPanelForm = new AdminPanelForm();
                    adminPanelForm.Show();
                    this.Hide();
                }

                else if (loginInput.Text == "user" && passwordInput.Text == "user")
                {
                    var bookManager = new BookManager();
                    List<Book> books = bookManager.GetAllBooks();

                    var mainForm = new MainForm(books);
                    mainForm.Show();
                    this.Hide();
                }

                else
                {
                    // Пользователь не найден, выводим сообщение об ошибке
                    MessageBox.Show("Неверный email или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            registerButton.Click += (sender, args) =>
            {
                var registerForm = new RegisterForm();
                registerForm.Show();
                this.Hide();
            };

            // Добавление элементов на форму
            Controls.Add(loginInput);
            Controls.Add(passwordInput);
            Controls.Add(loginButton);
            Controls.Add(registerButton);
        }
    }
}
