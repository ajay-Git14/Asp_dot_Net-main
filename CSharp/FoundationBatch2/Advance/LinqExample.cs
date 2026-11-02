namespace LinqExample
{
    public class LinqExample
    {
        public static void Main(string[] args)
        {
            List<string> names = new List<string> { "Alice", "Bob", "Charlie", "David", "Eve" };

            // Using LINQ to filter names that start with 'A'
            var filteredNames = names.Where(name => name.StartsWith("A"));

            Console.WriteLine("Names that start with 'A':");
            foreach (var name in filteredNames)
            {
                Console.WriteLine(name);
            }
        }
    }
}