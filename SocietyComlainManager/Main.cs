using System.Reflection.Metadata;
using System.Threading.Tasks;

public class Program
{
     public async static void Main()
     {

        Task<int> sumTask = Task.Run(async () =>
        {
            await Task.Delay(2000).Wait(); // Simulate some work
            int a = 5;
            int b = 10;
            return a + b;
        });
        Console.WriteLine($"Sum: {sumTask.Result}");


        Console.WriteLine("Hello, World!");
    //     List<User> users = new List<User>
    //     {
    //         new User(1, "Alice", "deepak@gmail.com", "Admin"),
    //         new User(2, "Bob", "madhuari@gmail.com", "User"),
    //         new User(3, "Charlie", "charlie.com", "User")
    //     };

    //     foreach (User user in users)
    //     {
    //         Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, Email: {user.Email}, Role: {user.Role}");
    //     }   

    //     List<User> admins = users.Where(u => u.Role == "Admin").ToList();

    //     Console.WriteLine("\nAdmins:");  

    //     //Nullable
    //     string? nullableString = null;

    //     Console.WriteLine(nullableString??"User email is null");
    //     //Task
    //     //which will return sum Value
    //     //Which will return any value
    //     Task<int> sumTask = Task.Run(() =>
    //     {
        
    //         int a = 5;
    //         int b = 10;
    //         return a + b;
    //     });


    try
        {
         int result = await sumTask;
         Console.WriteLine($"Sum: {result}");   
        }
        catch(Exception ex)
        {
            
        }
        catch(Nullable ex)
        {
            
        }
        finally{}
        }




}
