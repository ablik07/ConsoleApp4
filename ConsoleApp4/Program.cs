using System;
using System.Collections.Generic;

public abstract class Person
{
    private int id;
    private string name;
    private int age;
    private string email;

    protected Person(int id, string name, int age, string email)
    {
        this.id = id;
        this.name = name;
        this.age = age;
        this.email = email;
    }

    public int Id => id;
    public string Name => name;
    public int Age => age;
    public string Email => email;

    public abstract string GetRole();

    public virtual string GetInfo()
    {
        return $"ID: {id}, Имя: {name}, Возраст: {age}";
    }
}

public class Student : Person
{
    private List<Course> courses;
    private string major;

    public Student(int id, string name, int age, string email, string major)
        : base(id, name, age, email)
    {
        courses = new List<Course>();
        this.major = major;
    }

    public List<Course> Courses => courses;
    public string Major => major;

    public override string GetRole() => "Студент";

    public override string GetInfo()
    {
        return base.GetInfo() + $", Специальность: {major}";
    }

    public void Enroll(Course course)
    {
        if (!courses.Contains(course))
        {
            courses.Add(course);
            course.AddStudent(this);
        }
    }

    public string GetCourses()
    {
        if (courses.Count == 0)
            return "Нет активных курсов";

        string result = "";
        foreach (var course in courses)
        {
            result += $"- {course.Title}\n";
        }
        return result;
    }
}

public class Teacher : Person
{
    private string department;
    private List<Course> teachingCourses;

    public Teacher(int id, string name, int age, string email, string department)
        : base(id, name, age, email)
    {
        this.department = department;
        teachingCourses = new List<Course>();
    }

    public string Department => department;
    public List<Course> TeachingCourses => teachingCourses;

    public override string GetRole() => "Преподаватель";

    public override string GetInfo()
    {
        return base.GetInfo() + $", Кафедра: {department}";
    }

    public void Assign(Course course)
    {
        if (!teachingCourses.Contains(course))
        {
            teachingCourses.Add(course);
            course.SetTeacher(this);
        }
    }
}

public class Course
{
    private int id;
    private string title;
    private string description;
    private Teacher teacher;
    private List<Student> students;

    public Course(int id, string title, string description)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        students = new List<Student>();
        teacher = null;
    }

    public int Id => id;
    public string Title => title;
    public string Description => description;
    public Teacher Teacher => teacher;
    public List<Student> Students => students;

    public string GetInfo()
    {
        string teacherInfo = teacher != null ? teacher.Name : "Не назначен";
        return $"Курс: {title}\nОписание: {description}\n" +
               $"Преподаватель: {teacherInfo}\n" +
               $"Количество студентов: {students.Count}";
    }

    public void SetTeacher(Teacher teacher)
    {
        this.teacher = teacher;
    }

    public void AddStudent(Student student)
    {
        if (!students.Contains(student))
        {
            students.Add(student);
        }
    }

    public string GetStudents()
    {
        if (students.Count == 0)
            return "На курсе нет студентов";

        string result = "";
        foreach (var student in students)
        {
            result += $"- {student.Name} ({student.Major})\n";
        }
        return result;
    }
}

public class University
{
    private List<Student> students;
    private List<Teacher> teachers;
    private List<Course> courses;

    public University()
    {
        students = new List<Student>();
        teachers = new List<Teacher>();
        courses = new List<Course>();
        CreateData();
    }

    private void CreateData()
    {
        AddTeacher("Анна Иванова", 35, "anna@mail.ru", "Информатика");
        AddTeacher("Петр Сидоров", 42, "petr@mail.ru", "Математика");

        AddStudent("Мария Козлова", 20, "maria@mail.ru", "Программирование");
        AddStudent("Алексей Новиков", 21, "alex@mail.ru", "Математика");

        AddCourse("Основы C#");
        AddCourse("Алгебра");

        AssignTeacher(1, 1);
        AssignTeacher(2, 2);

        EnrollStudent(1, 1);
        EnrollStudent(2, 2);
    }

    public void AddStudent(string name, int age, string email, string major)
    {
        var id = students.Count + 1;
        var student = new Student(id, name, age, email, major);
        students.Add(student);
    }

    public Student FindStudent(int id)
    {
        return students.Find(student => student.Id == id);
    }

    public void AddTeacher(string name, int age, string email, string department)
    {
        var id = teachers.Count + 1;
        var teacher = new Teacher(id, name, age, email, department);
        teachers.Add(teacher);
    }

    public Teacher FindTeacher(int id)
    {
        return teachers.Find(teacher => teacher.Id == id);
    }

    public void AddCourse(string title)
    {
        var id = courses.Count + 1;
        var course = new Course(id, title, "");
        courses.Add(course);
    }

    public Course FindCourse(int id)
    {
        return courses.Find(course => course.Id == id);
    }

    public List<Student> AllStudents() => students;
    public List<Teacher> AllTeachers() => teachers;
    public List<Course> AllCourses() => courses;

    public bool EnrollStudent(int studentId, int courseId)
    {
        var student = FindStudent(studentId);
        var course = FindCourse(courseId);

        if (student != null && course != null)
        {
            student.Enroll(course);
            return true;
        }
        return false;
    }

    public bool AssignTeacher(int teacherId, int courseId)
    {
        var teacher = FindTeacher(teacherId);
        var course = FindCourse(courseId);

        if (teacher != null && course != null)
        {
            teacher.Assign(course);
            return true;
        }
        return false;
    }

    public string StudentCourses(int studentId)
    {
        var student = FindStudent(studentId);
        return student?.GetCourses() ?? "Студент не найден";
    }

    public string CourseStudents(int courseId)
    {
        var course = FindCourse(courseId);
        return course?.GetStudents() ?? "Курс не найден";
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var university = new University();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Университетская система");
            Console.WriteLine("1. Список студентов");
            Console.WriteLine("2. Список преподавателей");
            Console.WriteLine("3. Список курсов");
            Console.WriteLine("4. Выход");
            Console.Write("Введите номер: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ShowStudents(university);
                    break;
                case "2":
                    ShowTeachers(university);
                    break;
                case "3":
                    ShowCourses(university);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный ввод");
                    break;
            }
            Console.WriteLine("\nНажмите Enter для продолжения");
            Console.ReadKey();
        }
    }

    private static void ShowStudents(University university)
    {
        var students = university.AllStudents();
        Console.WriteLine("\nВсе студенты:");
        foreach (var student in students)
        {
            Console.WriteLine(student.GetInfo());
        }
    }

    private static void ShowTeachers(University university)
    {
        var teachers = university.AllTeachers();
        Console.WriteLine("\nВсе преподаватели:");
        foreach (var teacher in teachers)
        {
            Console.WriteLine(teacher.GetInfo());
        }
    }

    private static void ShowCourses(University university)
    {
        var courses = university.AllCourses();
        Console.WriteLine("\nВсе курсы:");
        foreach (var course in courses)
        {
            Console.WriteLine(course.GetInfo());
            Console.WriteLine();
        }
    }
}