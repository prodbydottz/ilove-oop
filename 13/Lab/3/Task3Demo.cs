using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsLab13.Task3
{
    /// <summary>
    /// Демонстрация работы системы управления онлайн-курсами
    /// </summary>
    public static class Task3Demo
    {
        private static List<User> users = new List<User>();
        private static List<Course> courses = new List<Course>();
        private static User currentUser = null;

        public static void Run()
        {
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  Задание №3: Система управления онлайн-курсами               ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            InitializeTestData();

            while (true)
            {
                if (currentUser == null)
                {
                    ShowLoginMenu();
                }
                else
                {
                    ShowUserMenu();
                }
            }
        }

        private static void InitializeTestData()
        {
            // Создаем тестовых пользователей
            var admin = new Administrator("Иван Админов", "admin@example.com", "admin123");
            var teacher1 = new Teacher("Мария Учителева", "teacher1@example.com", "teacher123");
            var teacher2 = new Teacher("Петр Преподавателев", "teacher2@example.com", "teacher123");
            var student1 = new Student("Алексей Студентов", "student1@example.com", "student123");
            var student2 = new Student("Ольга Ученикова", "student2@example.com", "student123");

            users.Add(admin);
            users.Add(teacher1);
            users.Add(teacher2);
            users.Add(student1);
            users.Add(student2);

            // Создаем тестовые курсы
            var course1 = new Course("C# для начинающих", "Основы программирования на C#", "Программирование", teacher1.Id);
            var course2 = new Course("Паттерны проектирования", "Изучение основных паттернов проектирования", "Программирование", teacher1.Id);
            var course3 = new Course("ASP.NET Core MVC", "Разработка веб-приложений на ASP.NET Core", "Web-разработка", teacher2.Id);

            course1.EnrolledStudents = 45;
            course2.EnrolledStudents = 32;
            course3.EnrolledStudents = 28;

            courses.Add(course1);
            courses.Add(course2);
            courses.Add(course3);

            teacher1.CreatedCourses.Add(course1.Id);
            teacher1.CreatedCourses.Add(course2.Id);
            teacher2.CreatedCourses.Add(course3.Id);
        }

        private static void ShowLoginMenu()
        {
            Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("🔐 Вход в систему");
            Console.WriteLine("\n📋 Тестовые аккаунты:");
            Console.WriteLine("1. Администратор - admin@example.com / admin123");
            Console.WriteLine("2. Преподаватель 1 - teacher1@example.com / teacher123");
            Console.WriteLine("3. Преподаватель 2 - teacher2@example.com / teacher123");
            Console.WriteLine("4. Студент 1 - student1@example.com / student123");
            Console.WriteLine("5. Студент 2 - student2@example.com / student123");
            Console.WriteLine("\n6. Регистрация нового пользователя");
            Console.WriteLine("0. Вернуться в главное меню");
            Console.Write("\nВыберите действие: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Login("admin@example.com", "admin123");
                    break;
                case "2":
                    Login("teacher1@example.com", "teacher123");
                    break;
                case "3":
                    Login("teacher2@example.com", "teacher123");
                    break;
                case "4":
                    Login("student1@example.com", "student123");
                    break;
                case "5":
                    Login("student2@example.com", "student123");
                    break;
                case "6":
                    RegisterNewUser();
                    break;
                case "0":
                    currentUser = null;
                    return;
                default:
                    Console.WriteLine("❌ Неверный выбор!");
                    break;
            }
        }

        private static void Login(string email, string password)
        {
            var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                currentUser = user;
                Console.WriteLine($"✅ Добро пожаловать, {user.Name}!");
                Console.WriteLine($"👤 Роль: {user.GetRole()}");
            }
            else
            {
                Console.WriteLine("❌ Неверный email или пароль!");
            }
        }

        private static void RegisterNewUser()
        {
            Console.WriteLine("📝 Регистрация нового пользователя");
            Console.WriteLine("\nВыберите роль:");
            Console.WriteLine("1. Студент");
            Console.WriteLine("2. Преподаватель");
            Console.Write("\nВаш выбор: ");

            var roleChoice = Console.ReadLine();

            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите email: ");
            string email = Console.ReadLine();

            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();

            User newUser = null;
            switch (roleChoice)
            {
                case "1":
                    newUser = new Student(name, email, password);
                    break;
                case "2":
                    newUser = new Teacher(name, email, password);
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор роли!");
                    return;
            }

            users.Add(newUser);
            Console.WriteLine($"\n✅ Пользователь успешно зарегистрирован!");
            Console.WriteLine($"🆔 ID: {newUser.Id}");
            Console.WriteLine($"📧 Email: {newUser.Email}");

            // Автоматический вход
            currentUser = newUser;
        }

        private static void ShowUserMenu()
        {
            Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine($"👤 Пользователь: {currentUser.Name} ({currentUser.GetRole()})");

            if (currentUser is Administrator admin)
            {
                ShowAdministratorMenu(admin);
            }
            else if (currentUser is Teacher teacher)
            {
                ShowTeacherMenu(teacher);
            }
            else if (currentUser is Student student)
            {
                ShowStudentMenu(student);
            }
        }

        private static void ShowStudentMenu(Student student)
        {
            student.ShowMenu();
            Console.WriteLine("0. Выход из аккаунта");
            Console.Write("\nВыберите действие: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ViewAvailableCourses();
                    break;
                case "2":
                    EnrollToCourse(student);
                    break;
                case "3":
                    TakeTest(student);
                    break;
                case "4":
                    student.ViewProgress();
                    break;
                case "5":
                    LeaveReview(student);
                    break;
                case "0":
                    currentUser = null;
                    Console.WriteLine("👋 До свидания!");
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор!");
                    break;
            }
        }

        private static void ShowTeacherMenu(Teacher teacher)
        {
            teacher.ShowMenu();
            Console.WriteLine("0. Выход из аккаунта");
            Console.Write("\nВыберите действие: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    CreateCourse(teacher);
                    break;
                case "2":
                    EditCourse(teacher);
                    break;
                case "3":
                    AddMaterials(teacher);
                    break;
                case "4":
                    CreateTest(teacher);
                    break;
                case "5":
                    ViewStatistics(teacher);
                    break;
                case "6":
                    ModerateReviews(teacher);
                    break;
                case "7":
                    EnrollToCourse(teacher);
                    break;
                case "8":
                    TakeTest(teacher);
                    break;
                case "0":
                    currentUser = null;
                    Console.WriteLine("👋 До свидания!");
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор!");
                    break;
            }
        }

        private static void ShowAdministratorMenu(Administrator admin)
        {
            admin.ShowMenu();
            Console.WriteLine("0. Выход из аккаунта");
            Console.Write("\nВыберите действие: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ManageUserAccounts(admin);
                    break;
                case "2":
                    CreateUserByAdmin(admin);
                    break;
                case "3":
                    DeleteUserByAdmin(admin);
                    break;
                case "4":
                    BlockUnblockUser(admin);
                    break;
                case "5":
                    ManageCategories(admin);
                    break;
                case "6":
                    admin.ViewSystemAnalytics();
                    break;
                case "7":
                    admin.ViewCoursePopularity();
                    break;
                case "8":
                    CreateCourse(admin);
                    break;
                case "9":
                    EnrollToCourse(admin);
                    break;
                case "0":
                    currentUser = null;
                    Console.WriteLine("👋 До свидания!");
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор!");
                    break;
            }
        }

        private static void ViewAvailableCourses()
        {
            Console.WriteLine("\n📚 Доступные курсы:");
            Console.WriteLine("════════════════════════════════════════");
            
            if (courses.Count == 0)
            {
                Console.WriteLine("Нет доступных курсов.");
                return;
            }

            foreach (var course in courses)
            {
                course.DisplayInfo();
                Console.WriteLine();
            }
        }

        private static void EnrollToCourse(Student student)
        {
            ViewAvailableCourses();
            Console.Write("\nВведите ID курса для записи: ");
            string courseId = Console.ReadLine();

            var course = courses.FirstOrDefault(c => c.Id == courseId);
            if (course != null)
            {
                student.EnrollToCourse(courseId);
                course.EnrolledStudents++;
            }
            else
            {
                Console.WriteLine("❌ Курс не найден!");
            }
        }

        private static void TakeTest(Student student)
        {
            if (student.EnrolledCourses.Count == 0)
            {
                Console.WriteLine("❌ Вы не записаны ни на один курс!");
                return;
            }

            Console.WriteLine("\n📝 Ваши курсы:");
            for (int i = 0; i < student.EnrolledCourses.Count; i++)
            {
                var courseId = student.EnrolledCourses[i];
                var course = courses.FirstOrDefault(c => c.Id == courseId);
                Console.WriteLine($"{i + 1}. {(course != null ? course.Title : courseId)}");
            }

            Console.Write("\nВыберите курс: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= student.EnrolledCourses.Count)
            {
                string courseId = student.EnrolledCourses[index - 1];
                Console.WriteLine("\n📝 Прохождение теста...");
                System.Threading.Thread.Sleep(1000);
                int score = new Random().Next(60, 100);
                student.TakeTest(courseId, score);
                
                // Обновляем прогресс
                int currentProgress = student.CourseProgress.ContainsKey(courseId) ? student.CourseProgress[courseId] : 0;
                student.UpdateProgress(courseId, Math.Min(100, currentProgress + 25));
            }
            else
            {
                Console.WriteLine("❌ Неверный выбор!");
            }
        }

        private static void LeaveReview(Student student)
        {
            if (student.EnrolledCourses.Count == 0)
            {
                Console.WriteLine("❌ Вы не записаны ни на один курс!");
                return;
            }

            Console.WriteLine("\n📝 Оставить отзыв на курс:");
            for (int i = 0; i < student.EnrolledCourses.Count; i++)
            {
                var courseId = student.EnrolledCourses[i];
                var course = courses.FirstOrDefault(c => c.Id == courseId);
                Console.WriteLine($"{i + 1}. {(course != null ? course.Title : courseId)}");
            }

            Console.Write("\nВыберите курс: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= student.EnrolledCourses.Count)
            {
                string courseId = student.EnrolledCourses[index - 1];
                var course = courses.FirstOrDefault(c => c.Id == courseId);

                if (course != null)
                {
                    Console.Write("Оценка (1-5): ");
                    if (int.TryParse(Console.ReadLine(), out int rating))
                    {
                        Console.Write("Комментарий: ");
                        string comment = Console.ReadLine();

                        var review = new Review(student.Id, student.Name, courseId, rating, comment);
                        course.AddReview(review);
                        Console.WriteLine("✅ Отзыв отправлен на модерацию!");
                    }
                }
            }
        }

        private static void CreateCourse(Teacher teacher)
        {
            Console.WriteLine("\n📚 Создание нового курса");
            Console.Write("Название курса: ");
            string title = Console.ReadLine();

            Console.Write("Описание: ");
            string description = Console.ReadLine();

            Console.Write("Категория: ");
            string category = Console.ReadLine();

            var course = new Course(title, description, category, teacher.Id);
            courses.Add(course);
            teacher.CreateCourse(title, description, category);
            teacher.CreatedCourses.Add(course.Id);
        }

        private static void EditCourse(Teacher teacher)
        {
            if (teacher.CreatedCourses.Count == 0)
            {
                Console.WriteLine("❌ У вас нет созданных курсов!");
                return;
            }

            Console.WriteLine("\n✏️ Ваши курсы:");
            for (int i = 0; i < teacher.CreatedCourses.Count; i++)
            {
                var courseId = teacher.CreatedCourses[i];
                var course = courses.FirstOrDefault(c => c.Id == courseId);
                Console.WriteLine($"{i + 1}. {(course != null ? course.Title : courseId)}");
            }

            Console.Write("\nВыберите курс для редактирования: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= teacher.CreatedCourses.Count)
            {
                string courseId = teacher.CreatedCourses[index - 1];
                Console.Write("Новое название: ");
                string newTitle = Console.ReadLine();
                Console.Write("Новое описание: ");
                string newDescription = Console.ReadLine();

                teacher.EditCourse(courseId, newTitle, newDescription);
            }
        }

        private static void AddMaterials(Teacher teacher)
        {
            if (teacher.CreatedCourses.Count == 0)
            {
                Console.WriteLine("❌ У вас нет созданных курсов!");
                return;
            }

            Console.WriteLine("\n📄 Добавление материалов");
            Console.Write("ID курса: ");
            string courseId = Console.ReadLine();

            Console.Write("Название материала: ");
            string materialName = Console.ReadLine();

            Console.Write("Тип (видео/презентация/документ): ");
            string materialType = Console.ReadLine();

            teacher.AddMaterial(courseId, materialName, materialType);
        }

        private static void CreateTest(Teacher teacher)
        {
            if (teacher.CreatedCourses.Count == 0)
            {
                Console.WriteLine("❌ У вас нет созданных курсов!");
                return;
            }

            Console.WriteLine("\n📝 Создание теста");
            Console.Write("ID курса: ");
            string courseId = Console.ReadLine();

            Console.Write("Название теста: ");
            string testName = Console.ReadLine();

            Console.Write("Максимальный балл: ");
            if (int.TryParse(Console.ReadLine(), out int maxScore))
            {
                teacher.CreateTest(courseId, testName, maxScore);
            }
        }

        private static void ViewStatistics(Teacher teacher)
        {
            if (teacher.CreatedCourses.Count == 0)
            {
                Console.WriteLine("❌ У вас нет созданных курсов!");
                return;
            }

            Console.Write("Введите ID курса: ");
            string courseId = Console.ReadLine();
            teacher.ViewStudentStatistics(courseId);
        }

        private static void ModerateReviews(Teacher teacher)
        {
            Console.WriteLine("\n📝 Модерация отзывов");
            
            var coursesWithReviews = courses.Where(c => teacher.CreatedCourses.Contains(c.Id) && c.Reviews.Count > 0).ToList();
            
            if (coursesWithReviews.Count == 0)
            {
                Console.WriteLine("❌ Нет отзывов для модерации!");
                return;
            }

            foreach (var course in coursesWithReviews)
            {
                Console.WriteLine($"\n📚 Курс: {course.Title}");
                var unapprovedReviews = course.Reviews.Where(r => !r.IsApproved).ToList();
                
                foreach (var review in unapprovedReviews)
                {
                    review.Display();
                    Console.Write("\nОдобрить отзыв? (y/n): ");
                    string response = Console.ReadLine();
                    
                    bool approve = response?.ToLower() == "y";
                    teacher.ModerateReview(review.Id, approve);
                    review.IsApproved = approve;
                }
            }
        }

        private static void ManageUserAccounts(Administrator admin)
        {
            Console.WriteLine("\n👥 Управление учетными записями");
            Console.WriteLine("════════════════════════════════════════");
            
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                Console.WriteLine($"{i + 1}. {user.Name} ({user.GetRole()}) - {user.Email}");
            }

            Console.Write("\nВведите номер пользователя для управления: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= users.Count)
            {
                var user = users[index - 1];
                Console.WriteLine($"\nВыбран: {user.Name}");
                Console.WriteLine("1. Просмотр информации");
                Console.WriteLine("2. Изменить роль");
                Console.Write("\nВыберите действие: ");
                
                string action = Console.ReadLine();
                admin.ManageUserAccount(user.Id, action == "1" ? "Просмотр информации" : "Изменение роли");
            }
        }

        private static void CreateUserByAdmin(Administrator admin)
        {
            Console.Write("Выберите роль (1-Студент, 2-Преподаватель, 3-Администратор): ");
            string roleChoice = Console.ReadLine();
            
            Console.Write("Имя: ");
            string name = Console.ReadLine();
            
            Console.Write("Email: ");
            string email = Console.ReadLine();

            string role = roleChoice switch
            {
                "1" => "Студент",
                "2" => "Преподаватель",
                "3" => "Администратор",
                _ => "Студент"
            };

            admin.CreateUser(role, name, email);
        }

        private static void DeleteUserByAdmin(Administrator admin)
        {
            Console.Write("Введите ID пользователя для удаления: ");
            string userId = Console.ReadLine();
            admin.DeleteUser(userId);
        }

        private static void BlockUnblockUser(Administrator admin)
        {
            Console.Write("Введите ID пользователя: ");
            string userId = Console.ReadLine();
            
            Console.Write("Заблокировать (b) или разблокировать (u)? ");
            string action = Console.ReadLine();

            if (action?.ToLower() == "b")
            {
                admin.BlockUser(userId);
            }
            else
            {
                admin.UnblockUser(userId);
            }
        }

        private static void ManageCategories(Administrator admin)
        {
            Console.WriteLine("\n📂 Управление категориями");
            Console.WriteLine("1. Создать категорию");
            Console.WriteLine("2. Удалить категорию");
            Console.Write("\nВыберите действие: ");
            
            string choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.Write("Название категории: ");
                string name = Console.ReadLine();
                
                Console.Write("Описание: ");
                string description = Console.ReadLine();
                
                admin.CreateCategory(name, description);
            }
            else if (choice == "2")
            {
                Console.Write("Название категории для удаления: ");
                string name = Console.ReadLine();
                admin.ManageCourseCategory(name, "Удалена");
            }
        }
    }
}

