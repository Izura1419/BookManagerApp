using System;
using System.Windows.Forms;

namespace Приход
{
    public partial class AdminPanelForm : Form
    {
        public AdminPanelForm()
        {
            InitializeComponent();
            LoadEmails(); // Загружаем email при инициализации формы
        }

        private void LoadEmails()
        {
            emailsListBox.Items.Clear();
            var emails = EmailRepository.GetAllEmails();
            foreach (var email in emails)
            {
                emailsListBox.Items.Add($"{email.Id} - {email.Email}"); // Отображаем ID - email
            }
        }

        private async void generatePasswordButton_Click_1(object sender, EventArgs e)
        {
            try
            {

                if (emailsListBox.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите email из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedEmail = emailsListBox.SelectedItem.ToString();
                int emailId = int.Parse(selectedEmail.Split('-')[0].Trim());
                string email = EmailRepository.GetEmailById(emailId);

                if (UserRepository.Instance.UserExists(email))
                {
                    MessageBox.Show($"Пользователь с email {email} уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 1. Генерация пароля
                string password = PasswordGenerator.GenerateRandomPassword();

                // 2. Отправка email
                string smtpServer = "smtp.yandex.ru";
                int smtpPort = 587;
                string smtpUsername = "uchihaizura1419@yandex.ru";
                string smtpPassword = "xvvjkehclexezlly";


                await EmailSender.SendEmailAsync(email, "Ваш пароль для регистрации",
                        $"Ваш сгенерированный пароль: {password}", smtpServer, smtpPort, smtpUsername, smtpPassword);

                var newUser = new User(email, password, "user");
                UserRepository.Instance.AddUser(newUser);


                // Удаление из ListBox
                emailsListBox.Items.Remove(emailsListBox.SelectedItem);

                MessageBox.Show($"Пароль для {email} успешно отправлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в процессе: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            var loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }
    }
}
