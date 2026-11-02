namespace FoundationBatch2.Oops
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }

        public void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine($"Gender: {Gender}"); 
            Console.WriteLine($"Address: {Address}");
        }

        public void speak()
        {
            Console.WriteLine("Person is speaking");
        }
        
    }

}