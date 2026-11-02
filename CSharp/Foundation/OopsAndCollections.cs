using System;
using System.Collections.Generic;

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }
}

public interface IEmployeeService
{
    void AddEmployee(Employee employee);

    List<Employee> GetEmployees();
}

public class EmployeeService : IEmployeeService
{
    private readonly List<Employee> employees = new List<Employee>();

    public void AddEmployee(Employee employee)
    {
        employees.Add(employee);
    }

    public List<Employee> GetEmployees()
    {
        return employees;
    }
}

public class Program
{
    public static void Main()
    {
        IEmployeeService employeeService = new EmployeeService();

        employeeService.AddEmployee(new Employee
        {
            Id = 1,
            Name = "Amit Sharma",
            Department = "IT",
            Salary = 60000
        });

        employeeService.AddEmployee(new Employee
        {
            Id = 2,
            Name = "Priya Verma",
            Department = "HR",
            Salary = 55000
        });

        employeeService.AddEmployee(new Employee
        {
            Id = 3,
            Name = "Rahul Mehta",
            Department = "Finance",
            Salary = 70000
        });

        List<Employee> employees = employeeService.GetEmployees();

        foreach (Employee employee in employees)
        {
            Console.WriteLine($"{employee.Id} - {employee.Name} - {employee.Department} - {employee.Salary}");
        }
    }
}