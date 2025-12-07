using System;
using System.Collections.Generic;

namespace DesignPatternsLab13.Task3
{
    /// <summary>
    /// Класс администратора (может выполнять функции преподавателя и студента)
    /// </summary>
    public class Administrator : Teacher
    {
        public Administrator(string name, string email, string password) 
            : base(name, email, password)
        {
        }

        public void ManageUserAccount(string userId, string action)
        {
            Console.WriteLine($"✅ Пользователь {userId}: {action}");
        }

        public void CreateUser(string role, string name, string email)
        {
            string userId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Console.WriteLine($"✅ Создан новый пользователь:");
            Console.WriteLine($"   Роль: {role}");
            Console.WriteLine($"   Имя: {name}");
            Console.WriteLine($"   Email: {email}");
            Console.WriteLine($"   ID: {userId}");
        }

        public void DeleteUser(string userId)
        {
            Console.WriteLine($"❌ Пользователь {userId} удален из системы");
        }

        public void BlockUser(string userId)
        {
            Console.WriteLine($"🚫 Пользователь {userId} заблокирован");
        }

        public void UnblockUser(string userId)
        {
            Console.WriteLine($"✅ Пользователь {userId} разблокирован");
        }

        public void ManageCourseCategory(string categoryName, string action)
        {
            Console.WriteLine($"✅ Категория '{categoryName}': {action}");
        }

        public void CreateCategory(string categoryName, string description)
        {
            Console.WriteLine($"✅ Создана новая категория курсов:");
            Console.WriteLine($"   Название: {categoryName}");
            Console.WriteLine($"   Описание: {description}");
        }

        public void ViewSystemAnalytics()
        {
            Random rnd = new Random();
            Console.WriteLine("\n📊 Аналитика системы:");
            Console.WriteLine("════════════════════════════════════════");
            Console.WriteLine($"👥 Всего пользователей: {rnd.Next(500, 2000)}");
            Console.WriteLine($"   • Студентов: {rnd.Next(400, 1800)}");
            Console.WriteLine($"   • Преподавателей: {rnd.Next(50, 150)}");
            Console.WriteLine($"   • Администраторов: {rnd.Next(2, 10)}");
            Console.WriteLine();
            Console.WriteLine($"📚 Всего курсов: {rnd.Next(50, 200)}");
            Console.WriteLine($"   • Активных: {rnd.Next(40, 180)}");
            Console.WriteLine($"   • В разработке: {rnd.Next(5, 20)}");
            Console.WriteLine();
            Console.WriteLine($"🔥 Популярные курсы:");
            Console.WriteLine($"   1. C# для начинающих ({rnd.Next(100, 500)} студентов)");
            Console.WriteLine($"   2. Паттерны проектирования ({rnd.Next(80, 300)} студентов)");
            Console.WriteLine($"   3. ASP.NET Core ({rnd.Next(70, 250)} студентов)");
            Console.WriteLine();
            Console.WriteLine($"📈 Средняя успеваемость: {rnd.Next(70, 90)}%");
            Console.WriteLine($"⭐ Средний рейтинг курсов: {rnd.Next(40, 50) / 10.0}/5.0");
        }

        public void ViewCoursePopularity()
        {
            Random rnd = new Random();
            Console.WriteLine("\n🔥 Популярность курсов:");
            Console.WriteLine("════════════════════════════════════════");
            for (int i = 1; i <= 5; i++)
            {
                string courseId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                int students = rnd.Next(50, 300);
                double rating = rnd.Next(35, 50) / 10.0;
                Console.WriteLine($"{i}. Курс {courseId}");
                Console.WriteLine($"   Студентов: {students}");
                Console.WriteLine($"   Рейтинг: {rating}/5.0");
                Console.WriteLine();
            }
        }

        public override void ShowMenu()
        {
            Console.WriteLine("\n👨‍💼 Меню администратора:");
            Console.WriteLine("1. Управление учетными записями");
            Console.WriteLine("2. Создать пользователя");
            Console.WriteLine("3. Удалить пользователя");
            Console.WriteLine("4. Заблокировать/Разблокировать пользователя");
            Console.WriteLine("5. Управление категориями курсов");
            Console.WriteLine("6. Просмотр аналитики системы");
            Console.WriteLine("7. Просмотр популярности курсов");
            Console.WriteLine("8. [Как преподаватель] Создать курс");
            Console.WriteLine("9. [Как студент] Записаться на курс");
        }

        public override string GetRole()
        {
            return "Администратор";
        }
    }
}

