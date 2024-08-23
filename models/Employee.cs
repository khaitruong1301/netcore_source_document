using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
[Serializable]
public class Employee
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Position { get; set; }

    [NonSerialized] // Không tuần tự hóa trường này
    private decimal salary;

    [NonSerialized] // Không tuần tự hóa trường này
    private string sensitiveData;
    public Employee(string name, int age, string position, decimal salary, string sensitiveData)
    {
        Name = name;
        Age = age;
        Position = position;
        this.salary = salary;
        this.sensitiveData = sensitiveData;
    }
}


