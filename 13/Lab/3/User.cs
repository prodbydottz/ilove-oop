using System;
using System.Collections.Generic;

namespace DesignPatternsLab13.Task3
{
    /// <summary>
    /// Базовый класс пользователя системы
    /// </summary>
    public abstract class User
    {
        public string Id { get; protected set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; protected set; }
        public DateTime RegisteredAt { get; protected set; }

        protected User(string name, string email, string password)
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Name = name;
            Email = email;
            Password = password;
            RegisteredAt = DateTime.Now;
        }

        public abstract void ShowMenu();
        public abstract string GetRole();
    }
}

