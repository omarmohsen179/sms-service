using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMS_Sender.Modals;

namespace SMS_Sender.Services
{
    public class AppDbContext : DbContext
    {
        public DbSet<SmS> SMSWantedToSend { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }
}
