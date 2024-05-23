﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;

namespace WebApplication2.Controller;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly TripContext _context;

    public ClientsController(TripContext context)
    {
        _context = context;
    }
    
    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.Id == idClient);

        if (client == null)
        {
            return NotFound();
        }

        if (client.ClientTrips.Any())
        {
            return BadRequest("Client has assigned trips and cannot be deleted.");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}