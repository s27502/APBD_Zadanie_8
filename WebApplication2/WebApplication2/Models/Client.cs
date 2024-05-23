using WebApplication2.Context;

namespace WebApplication2.Models;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Pesel { get; set; }

    public ICollection<ClientTrip> ClientTrips { get; set; }
}