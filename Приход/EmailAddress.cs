using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Приход
{
    public class EmailAddress
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public EmailAddress(int id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}
