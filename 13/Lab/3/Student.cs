using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsLab13.Task3
{
    /// <summary>
    /// Класс студента
    /// </summary>
    public class Student : User
    {
        public List<string> EnrolledCourses { get; private set; }
        public Dictionary<string, int> TestResults { get; private set; }
        public Dictionary<string, int> CourseProgress { get; private set; }

        public Student(string name, string email, string password) 
            : base(name, email, password)
        {
            EnrolledCourses = new List<string>();
            TestResults = new Dictionary<string, int>();
            CourseProgress = new Dictionary<string, int>();
        }

        public void EnrollToCourse(string courseId)
        {
            if (!EnrolledCourses.Contains(courseId))
            {
                EnrolledCourses.Add(courseId);
                CourseProgress[courseId] = 0;
                Console.WriteLine($"✅ Вы успешно записались на курс!");
            }
            else
            {
                Console.WriteLine("ℹ️ Вы уже записаны на этот курс.");
            }
        }

        public void TakeTest(string courseId, int score)
        {
            if (EnrolledCourses.Contains(courseId))
            {
                TestResults[courseId] = score;
                Console.WriteLine($"✅ Тест пройден! Ваш результат: {score} баллов");
            }
            else
            {
                Console.WriteLine("❌ Вы не записаны на этот курс!");
            }
        }

        public void UpdateProgress(string courseId, int progress)
        {
            if (EnrolledCourses.Contains(courseId))
            {
                CourseProgress[courseId] = Math.Min(100, progress);
                Console.WriteLine($"📊 Прогресс обновлен: {CourseProgress[courseId]}%");
            }
        }

        public void ViewProgress()
        {
            Console.WriteLine("\n📊 Ваш прогресс:");
            Console.WriteLine("════════════════════════════════════════");
            foreach (var courseId in EnrolledCourses)
            {
                int progress = CourseProgress.ContainsKey(courseId) ? CourseProgress[courseId] : 0;
                int testScore = TestResults.ContainsKey(courseId) ? TestResults[courseId] : 0;
                Console.WriteLine($"📚 Курс ID: {courseId}");
                Console.WriteLine($"   Прогресс: {progress}%");
                Console.WriteLine($"   Результат теста: {testScore} баллов");
                Console.WriteLine();
            }
        }

        public override void ShowMenu()
        {
            Console.WriteLine("\n👨‍🎓 Меню студента:");
            Console.WriteLine("1. Просмотр доступных курсов");
            Console.WriteLine("2. Записаться на курс");
            Console.WriteLine("3. Пройти тест");
            Console.WriteLine("4. Просмотр прогресса");
            Console.WriteLine("5. Оставить отзыв");
        }

        public override string GetRole()
        {
            return "Студент";
        }
    }
}

