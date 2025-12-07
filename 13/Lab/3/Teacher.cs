using System;
using System.Collections.Generic;

namespace DesignPatternsLab13.Task3
{
    /// <summary>
    /// Класс преподавателя (может выполнять функции студента)
    /// </summary>
    public class Teacher : Student
    {
        public List<string> CreatedCourses { get; private set; }

        public Teacher(string name, string email, string password) 
            : base(name, email, password)
        {
            CreatedCourses = new List<string>();
        }

        public string CreateCourse(string title, string description, string category)
        {
            string courseId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            CreatedCourses.Add(courseId);
            Console.WriteLine($"✅ Курс '{title}' создан успешно! ID: {courseId}");
            return courseId;
        }

        public void EditCourse(string courseId, string newTitle, string newDescription)
        {
            if (CreatedCourses.Contains(courseId))
            {
                Console.WriteLine($"✅ Курс {courseId} обновлен!");
            }
            else
            {
                Console.WriteLine("❌ Вы не являетесь автором этого курса!");
            }
        }

        public void AddMaterial(string courseId, string materialName, string materialType)
        {
            if (CreatedCourses.Contains(courseId))
            {
                Console.WriteLine($"✅ Материал '{materialName}' ({materialType}) добавлен к курсу {courseId}");
            }
            else
            {
                Console.WriteLine("❌ Вы не являетесь автором этого курса!");
            }
        }

        public void CreateTest(string courseId, string testName, int maxScore)
        {
            if (CreatedCourses.Contains(courseId))
            {
                Console.WriteLine($"✅ Тест '{testName}' создан для курса {courseId}");
                Console.WriteLine($"   Максимальный балл: {maxScore}");
            }
            else
            {
                Console.WriteLine("❌ Вы не являетесь автором этого курса!");
            }
        }

        public void ViewStudentStatistics(string courseId)
        {
            if (CreatedCourses.Contains(courseId))
            {
                Console.WriteLine($"\n📊 Статистика курса {courseId}:");
                Console.WriteLine("════════════════════════════════════════");
                Console.WriteLine($"Студентов записано: {new Random().Next(10, 100)}");
                Console.WriteLine($"Средний прогресс: {new Random().Next(40, 90)}%");
                Console.WriteLine($"Средний балл: {new Random().Next(60, 95)}");
            }
            else
            {
                Console.WriteLine("❌ Вы не являетесь автором этого курса!");
            }
        }

        public void ModerateReview(string reviewId, bool approve)
        {
            if (approve)
            {
                Console.WriteLine($"✅ Отзыв {reviewId} одобрен");
            }
            else
            {
                Console.WriteLine($"❌ Отзыв {reviewId} отклонен");
            }
        }

        public override void ShowMenu()
        {
            Console.WriteLine("\n👨‍🏫 Меню преподавателя:");
            Console.WriteLine("1. Создать курс");
            Console.WriteLine("2. Редактировать курс");
            Console.WriteLine("3. Добавить материалы");
            Console.WriteLine("4. Создать тест");
            Console.WriteLine("5. Просмотр статистики студентов");
            Console.WriteLine("6. Модерация отзывов");
            Console.WriteLine("7. [Как студент] Записаться на курс");
            Console.WriteLine("8. [Как студент] Пройти тест");
        }

        public override string GetRole()
        {
            return "Преподаватель";
        }
    }
}

