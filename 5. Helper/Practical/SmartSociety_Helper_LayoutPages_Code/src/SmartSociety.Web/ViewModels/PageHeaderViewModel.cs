namespace SmartSociety.Web.ViewModels;

public class PageHeaderViewModel
{
    public string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;

    public string? ActionText { get; set; }

    public string? ActionController { get; set; }

    public string? ActionName { get; set; }
}
