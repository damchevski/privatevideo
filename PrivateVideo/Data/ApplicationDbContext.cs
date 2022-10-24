using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrivateVideo.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateVideo.Data
{
    public class ApplicationDbContext : IdentityDbContext<PrivateUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
