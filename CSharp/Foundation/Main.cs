string input = "101";

bool isValid = int.TryParse(input, out int complaintId);

if (isValid)
{
    Console.WriteLine($"Complaint ID is {complaintId}");
}
else
{
    Console.WriteLine("Invalid complaint ID");
}