using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.DTO;
using WebApplication2.Models;

namespace WebApplication2.Controller;

[ApiController]
[Route("api/trips/{idTrip}/clients")]
public class ClientTripsController : ControllerBase
{
    private readonly TripContext _context;

    public ClientTripsController(TripContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] AssignClientDto dto)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        if (trip == null)
        {
            return NotFound("Trip not found.");
        }

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);
        if (client == null)
        {
            client = new Client { Pesel = dto.Pesel, Name = dto.Name };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        if (await _context.ClientTrips.AnyAsync(ct => ct.ClientId == client.Id && ct.TripId == idTrip))
        {
            return BadRequest("Client is already assigned to this trip.");
        }

        var clientTrip = new ClientTrip
        {
            ClientId = client.Id,
            TripId = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = dto.PaymentDate
        };

        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();

        return Ok();
    }
}