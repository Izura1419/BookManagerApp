using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Windows.Forms;

namespace Приход
{
    public partial class RegisterForm : MaterialForm
    {
        public RegisterForm()
        {
            InitializeComponent();
            ApplyMaterialSkin(); // Применяем оформление MaterialSkin
            InitializeRegisterForm(); // Инициализируем элементы формы
        }

        // Метод для настройки общего стиля с помощью MaterialSkin
        private void ApplyMaterialSkin()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT; // Устанавливаем светлую тему
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.BlueGrey800, Primary.BlueGrey900,
                Primary.BlueGrey500, Accent.LightBlue200,
                TextShade.WHITE);
        }

        // Метод для добавления элементов управления на форму
        private void InitializeRegisterForm()
        {
            // Поле для ввода электронной почты
            var emailInput = new MaterialSingleLineTextField
            {
                Hint = "Введите вашу почту", // Подсказка внутри поля
                Top = 120,
                Left = 260,
                Width = 300
            };

            // Кнопка для регистрации
            var registerButton = new MaterialRaisedButton
            {
                Text = "Зарегистрироваться", // Текст на кнопке
                Top = 170,
                Left = 300,
                Width = 200
            };

            // Обработчик нажатия на кнопку регистрации
            registerButton.Click += (sender, args) =>
            {
                // Здесь реализуется логика регистрации пользователя

                string email = emailInput.Text.Trim();

                // 1. Валидация email
                if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                {
                    MessageBox.Show("Введите корректный email.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Проверка на существование email
                if (EmailRepository.EmailExists(email))
                {
                    MessageBox.Show("Email уже зарегистрирован!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. Добавление email в коллекцию
                EmailRepository.AddEmail(email);

                MessageBox.Show("Email успешно зарегистрирован!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var loginForm = new LoginForm();
                loginForm.Show();
                this.Hide();
            };

            // Добавляем элементы управления на форму
            Controls.Add(emailInput);
            Controls.Add(registerButton);
        }
    }
}
