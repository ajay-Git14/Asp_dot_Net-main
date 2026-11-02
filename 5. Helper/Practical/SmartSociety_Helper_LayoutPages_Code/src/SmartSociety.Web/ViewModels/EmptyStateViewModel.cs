namespace SmartSociety.Web.ViewModels;

public class EmptyStateViewModel
{
    public string Title { get; set; } = "No data found";

    public string Message { get; set; } = "There are no records to display.";

    public string? ActionText { get; set; }

    public string? ActionController { get; set; }

    public string? ActionName { get; set; }
}
