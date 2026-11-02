using System.ComponentModel.DataAnnotations;

namespace FoundationBatch2.Oops
{
    public class OopsAndCollection
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Oops and Collection Batch 2!");
            /**
            Polymorphism: Polymorphism allows objects of different classes to be treated as objects of a common superclass. It enables a single interface to represent different underlying forms (data types). There are two types of polymorphism: compile-time (method overloading) and runtime (method overriding).
            Inheritance: Inheritance is a fundamental OOP concept where a new class (called a
            Abstract Class: An abstract class is a class that cannot be instantiated and is meant to be subclassed. It can contain abstract methods (without implementation) that must be implemented by derived classes.
            Interface: An interface is a contract that defines a set of methods and properties that a class must implement. It allows for multiple inheritance and is used to achieve abstraction.
            Encapsulation: Encapsulation is the bundling of data and methods that operate on that data within a single unit (class). It restricts direct access to some of the object's components, which can help prevent unintended interference and misuse of the data.
            Absraction: Abstraction is the concept of hiding the complex implementation details and showing only the necessary features of an object. It allows developers to focus on what an object does rather than how it does it.
            Collection: A collection is a data structure that can hold multiple values or objects. In C#, there are various types of collections, such as arrays, lists, dictionaries, and sets, which provide different ways to store and manipulate data. Collections are part of the System.Collections namespace and its sub-namespaces, and they offer methods for adding, removing, and accessing elements efficiently.   
            Access Modifiers: Access modifiers are keywords used to specify the accessibility of classes, methods, and other members. The common access modifiers in C# are public, private, protected, internal, and protected internal. They control the visibility and accessibility of members from other parts of the code.

            **/

            Person person  = new Employee
            {
                Name = "Jane Smith",
                Age = 25,
                Gender = "Female",
                Address = "456 Oak Ave",
                EmployeeId = "E001",
                Salary = 50000m
            };

            Console.WriteLine("Employee Information:"+person.Name);

            person.DisplayInfo();

    }

    //Polymorphism

            public  static void Polymorphism()
            {
                Console.WriteLine("Polymorphism in C#");
            }
            public  static void Polymorphism(int a,int b)
            {
                Console.WriteLine(a+b);
            }
              public  static void Polymorphism(string a,string b)
            {
                Console.WriteLine(a+b);
            }
    }

    
}


