namespace CSharpDevelopment
{
    public class Complaint
    {
       public int Id {  get; set; }
       public string title { get; set; }
        public string description { get; set; }
        public DateTime resolvedDate { get; set; } = DateTime.Now;
        public int priorityOder {  get; set; }

    }


    public class MainClass
    {
        public static void Main(string[] args)
        {
            List<Complaint> list = new List<Complaint>() { new Complaint
            {
                Id=1,
                title="Water Leakage",
                description="Facing from last two days",
                priorityOder=10
            },
            new Complaint
            {
                Id=2,
                title="Lift Problem",
                description="Facing from last two days",
                priorityOder=10
            },
            new Complaint
            {
                Id=3,
                title="Plumbering ",
                description="Facing from last two days",
                priorityOder=7
            }};

            List<Complaint>selectedComplaints = list.Where<Complaint>(c=>c.Id==1).ToList();

            bool checkExistance = list.Any<Complaint>(c => c.Id == 1);

           
            Console.WriteLine("Hello World");

            selectedComplaints.ForEach((complaint) =>
            {
                Console.WriteLine(complaint.title);
            });
            //Null Point Safety

            string? name = null;
            string result = name ?? "I am dummy TEst";
            Console.WriteLine(result);


           try
            {
                int result1 = int.Parse("10asfds");
                Console.WriteLine(result);
            }
            catch (FormatException)
            {
                Console.WriteLine("Unable to Convert String to Number");
            }
            finally
            {
                Console.WriteLine("I will Execute");
            }

            
         



            


        }
       
    }




}


