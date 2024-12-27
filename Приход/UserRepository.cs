using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Приход
{
    public class UserRepository
    {
        private static UserRepository _instance; //  приватное статическое поле для хранения экземпляра
        public List<User> Users { get; set; }

        private UserRepository() //  приватный конструктор
        {
            Users = new List<User>();
        }

        public static UserRepository Instance //  публичное статическое свойство для доступа к экземпляру
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserRepository();
                }
                return _instance;
            }
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public bool UserExists(string email)
        {
            return Users.Exists(u => u.Email == email);
        }

        public User GetUserByEmail(string email)
        {
            return Users.FirstOrDefault(u => u.Email == email);
        }
    }
}