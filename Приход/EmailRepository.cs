using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Приход
{
    public static class EmailRepository
    {
        private static List<EmailAddress> _emails = new List<EmailAddress>();
        private static int _nextId = 1;

        public static void AddEmail(string email)
        {
            var newEmail = new EmailAddress(_nextId++, email);
            _emails.Add(newEmail);
        }

        public static List<EmailAddress> GetAllEmails()
        {
            return new List<EmailAddress>(_emails); // Возвращаем копию для безопасности
        }

        public static string GetEmailById(int id)
        {
            return _emails.FirstOrDefault(e => e.Id == id)?.Email;
        }

        public static bool EmailExists(string email)
        {
            return _emails.Exists(e => e.Email == email);
        }
    }
}
