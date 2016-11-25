using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TripPlanner.Models
{
    public class User : IdentityUser
    {
        public DateTime FIrstTrip { get; set; }
    }
}