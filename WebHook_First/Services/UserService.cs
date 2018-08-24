using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebHook_First.Models;
using WebHook_First.Models.Repository;

namespace WebHook_First.Services
{
    public class UserService : IDisposable
    {
        UserRepository userRepository = new UserRepository();

        public void AddUser(string userId, string userName)
        {
            if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(userName))
            {
                var user = userRepository.GetAll().FirstOrDefault(x => x.UserID == userId);
                if (user == null)
                {
                    user = new User()
                    {
                        UserID = userId,
                        UserName = userName
                    };
                    userRepository.Create(user);
                    userRepository.SaveChanges();
                }
            }
        }

        public IList<User> GetAll()
        {
            return userRepository.GetAll().ToList();
        }

        public void Dispose()
        {
            if (userRepository != null)
            {
                userRepository.Dispose();
                userRepository = null;
            }
        }
        
    }
}