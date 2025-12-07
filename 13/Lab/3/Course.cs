using System;
using System.Collections.Generic;

namespace DesignPatternsLab13.Task3
{
    /// <summary>
    /// Класс курса
    /// </summary>
    public class Course
    {
        public string Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string AuthorId { get; private set; }
        public List<string> Materials { get; private set; }
        public List<string> Tests { get; private set; }
        public List<Review> Reviews { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int EnrolledStudents { get; set; }

        public Course(string title, string description, string category, string authorId)
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Title = title;
            Description = description;
            Category = category;
            AuthorId = authorId;
            Materials = new List<string>();
            Tests = new List<string>();
            Reviews = new List<Review>();
            CreatedAt = DateTime.Now;
            EnrolledStudents = 0;
        }

        public void AddMaterial(string materialName)
        {
            Materials.Add(materialName);
        }

        public void AddTest(string testName)
        {
            Tests.Add(testName);
        }

        public void AddReview(Review review)
        {
            Reviews.Add(review);
        }

        public double GetAverageRating()
        {
            if (Reviews.Count == 0) return 0;
            
            double sum = 0;
            foreach (var review in Reviews)
            {
                sum += review.Rating;
            }
            return sum / Reviews.Count;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"\n📚 {Title}");
            Console.WriteLine($"🆔 ID: {Id}");
            Console.WriteLine($"📝 Описание: {Description}");
            Console.WriteLine($"📂 Категория: {Category}");
            Console.WriteLine($"👨‍🏫 Автор ID: {AuthorId}");
            Console.WriteLine($"👥 Записано студентов: {EnrolledStudents}");
            Console.WriteLine($"📄 Материалов: {Materials.Count}");
            Console.WriteLine($"📝 Тестов: {Tests.Count}");
            Console.WriteLine($"⭐ Рейтинг: {GetAverageRating():F1}/5.0 ({Reviews.Count} отзывов)");
            Console.WriteLine($"📅 Создан: {CreatedAt:dd.MM.yyyy}");
        }
    }
}

