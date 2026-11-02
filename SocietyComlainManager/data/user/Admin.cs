public class Admin : User
{
    public string destination { get; set; }

    public Admin(int Id, string Name, string Email, string Role, string destination) : base(Id, Name, Email, Role)
    {
        super(Id, Name, Email, Role);
        this.destination = destination;
    }
}