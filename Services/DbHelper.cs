using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMS_Sender.Modals;

namespace SMS_Sender.Services
{
    public class DbHelper
    {
        private AppDbContext _dbContext;

        private DbContextOptions<AppDbContext> GetAllOptions()
        {
            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionBuilder.UseSqlServer(AppSettings.ConnectionString);
            return optionBuilder.Options;
        }

        public List<SmS> GetAllSmSes()
        {
            using (_dbContext = new AppDbContext(GetAllOptions()))
            {
                var messages = _dbContext.SMSWantedToSend.Where(sms => sms.Finished.ToUpper() == "F").ToList();
                return messages;
            }
        }

        public void UpdateSmsToFinish(SmS smS)
        {
            smS.Finished = "T";

            using (_dbContext = new AppDbContext(GetAllOptions()))
            {
                _dbContext.SMSWantedToSend.Update(smS);
                _dbContext.SaveChanges();
            }
        }

        public void RemoveSmS(SmS smS)
        {
            using (_dbContext = new AppDbContext(GetAllOptions()))
            {
                _dbContext.SMSWantedToSend.Remove(smS);
                _dbContext.SaveChanges();
            }
        }
    }
}
