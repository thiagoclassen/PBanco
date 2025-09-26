namespace Clients.API.Client.CreateClient;

public class CreateClientRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string CPF { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}