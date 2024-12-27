using System;
using System.Drawing;
using System.Windows.Forms;

namespace Приход
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox emailInput;
        private System.Windows.Forms.TextBox loginInput;
        private System.Windows.Forms.Button registerButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.ClientSize = new System.Drawing.Size(800, 480); // Устанавливаем размер клиентской области
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Регистрация";

            this.SuspendLayout();

            this.ResumeLayout(false);
            this.PerformLayout();
        }


        private void RegisterButton_Click(object sender, EventArgs e)
        {
            // Логика обработки нажатия на кнопку регистрации
            MessageBox.Show("Регистрация завершена!");
        }

    }
}
