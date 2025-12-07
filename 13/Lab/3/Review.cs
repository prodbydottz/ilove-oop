using System;

namespace DesignPatternsLab13.Task3
{
    /// <summary>
    /// Класс отзыва на курс
    /// </summary>
    public class Review
    {
        public string Id { get; private set; }
        public string StudentId { get; private set; }
        public string StudentName { get; private set; }
        public string CourseId { get; private set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsApproved { get; set; }

        public Review(string studentId, string studentName, string courseId, int rating, string comment)
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            StudentId = studentId;
            StudentName = studentName;
            CourseId = courseId;
            Rating = Math.Max(1, Math.Min(5, rating)); // Ограничение от 1 до 5
            Comment = comment;
            CreatedAt = DateTime.Now;
            IsApproved = false;
        }

        public void Display()
        {
            Console.WriteLine($"\n⭐ Рейтинг: {Rating}/5");
            Console.WriteLine($"👤 Автор: {StudentName}");
            Console.WriteLine($"💬 Отзыв: {Comment}");
            Console.WriteLine($"📅 Дата: {CreatedAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"✅ Статус: {(IsApproved ? "Одобрен" : "На модерации")}");
        }
    }
}

