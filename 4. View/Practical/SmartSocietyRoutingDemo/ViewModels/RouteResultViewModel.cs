namespace SmartSocietyRoutingDemo.ViewModels;

public class RouteResultViewModel
{
    public string Heading { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public string RequestPath { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = string.Empty;
    public string EndpointName { get; set; } = string.Empty;
    public Dictionary<string, string> RouteValues { get; set; } = [];
    public Dictionary<string, string> QueryValues { get; set; } = [];
    public List<string> Messages { get; set; } = [];
}
