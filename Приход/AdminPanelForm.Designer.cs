namespace Приход
{
    partial class AdminPanelForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.adminPanLab = new System.Windows.Forms.Label();
            this.emailsListBox = new System.Windows.Forms.ListBox();
            this.adminPanInfo = new System.Windows.Forms.Label();
            this.generatePasswordButton = new System.Windows.Forms.Button();
            this.exitBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // adminPanLab
            // 
            this.adminPanLab.AutoSize = true;
            this.adminPanLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.adminPanLab.Location = new System.Drawing.Point(280, 9);
            this.adminPanLab.Name = "adminPanLab";
            this.adminPanLab.Size = new System.Drawing.Size(226, 33);
            this.adminPanLab.TabIndex = 0;
            this.adminPanLab.Text = "Панель админа";
            // 
            // emailsListBox
            // 
            this.emailsListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.emailsListBox.FormattingEnabled = true;
            this.emailsListBox.IntegralHeight = false;
            this.emailsListBox.ItemHeight = 24;
            this.emailsListBox.Location = new System.Drawing.Point(38, 117);
            this.emailsListBox.Name = "emailsListBox";
            this.emailsListBox.Size = new System.Drawing.Size(330, 264);
            this.emailsListBox.TabIndex = 1;
            // 
            // adminPanInfo
            // 
            this.adminPanInfo.AutoSize = true;
            this.adminPanInfo.Location = new System.Drawing.Point(94, 69);
            this.adminPanInfo.Name = "adminPanInfo";
            this.adminPanInfo.Size = new System.Drawing.Size(594, 13);
            this.adminPanInfo.TabIndex = 2;
            this.adminPanInfo.Text = "Выберите электронную почту и нажмите \"создать пользователя\", чтобы отправить поль" +
    "зователю письмо на почту";
            // 
            // generatePasswordButton
            // 
            this.generatePasswordButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.generatePasswordButton.Location = new System.Drawing.Point(499, 117);
            this.generatePasswordButton.Name = "generatePasswordButton";
            this.generatePasswordButton.Size = new System.Drawing.Size(189, 46);
            this.generatePasswordButton.TabIndex = 3;
            this.generatePasswordButton.Text = "Создать пользователя";
            this.generatePasswordButton.UseVisualStyleBackColor = true;
            this.generatePasswordButton.Click += new System.EventHandler(this.generatePasswordButton_Click_1);
            // 
            // exitBtn
            // 
            this.exitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.exitBtn.Location = new System.Drawing.Point(12, 8);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(86, 34);
            this.exitBtn.TabIndex = 4;
            this.exitBtn.Text = "Выйти";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // AdminPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 441);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.generatePasswordButton);
            this.Controls.Add(this.adminPanInfo);
            this.Controls.Add(this.emailsListBox);
            this.Controls.Add(this.adminPanLab);
            this.Name = "AdminPanelForm";
            this.Text = "Панель админа";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label adminPanLab;
        private System.Windows.Forms.ListBox emailsListBox;
        private System.Windows.Forms.Label adminPanInfo;
        private System.Windows.Forms.Button generatePasswordButton;
        private System.Windows.Forms.Button exitBtn;
    }
}