namespace FoundationBatch2.Oops
{
   public class Collections
    {
        public void Run()
        {
            //List<T>
            List<string> names = new List<string>();
            names.Add("Alice");
            names.Add("Bob");
            names.Add("Charlie");

            Console.WriteLine("Names in the list:");
            foreach (var name in names)
            {
                Console.WriteLine(name);
            }

            //Dictionary<TKey, TValue>
            Dictionary<int, string> studentGrades = new Dictionary<int, string>();
            studentGrades.Add(1, "A");
            studentGrades.Add(2, "B");
            studentGrades.Add(3, "C");

            Console.WriteLine("\nStudent Grades:");
            foreach (var kvp in studentGrades)
            {
                Console.WriteLine($"Student ID: {kvp.Key}, Grade: {kvp.Value}");
            }

        }
    }

}