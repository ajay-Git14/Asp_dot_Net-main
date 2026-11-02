namespace FoundationBatch2.Oops
{
    public class Employee:Person
    {
        public string EmployeeId { get; set; }
        public decimal Salary { get; set; }

        public void DisplayInfo()
        {
            base.DisplayInfo(); // Call the base class method
            Console.WriteLine($"Employee ID: {EmployeeId}");
            Console.WriteLine($"Salary: {Salary}");
        }
    }

}
         

