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
