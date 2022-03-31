using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SMS_Sender.Modals;
using SMS_Sender.Services;
using Vonage;
using Vonage.Request;

namespace SMS_Sender
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private DbHelper _db;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _db = new DbHelper();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await File.AppendAllTextAsync("c:\\service\\data.txt", $"\nExecuted :: at {DateTimeOffset.Now.ToString()}", cancellationToken);
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    await SendSmSs();
                }
                catch (Exception ex)
                {
                    await File.AppendAllTextAsync("c:\\service\\data.txt", $"\nError :: {ex.Message} at {DateTimeOffset.Now.ToString()}", stoppingToken);
                    _logger.LogError(ex, "An Error Occurred");
                }
                finally
                {
                    await Task.Delay(1000, stoppingToken);

                }
            }
        }

        private async Task SendSmSs()
        {
            var messages = _db.GetAllSmSes();

            var tasks = messages.Select(SendSms).ToList();

            await Task.WhenAll(tasks);
        }

        public async Task SendSms(SmS _SmS)
        {
            try
            {
                var credentials = Credentials.FromApiKeyAndSecret(
                    apiKey: AppSettings.ApiKey,
                    apiSecret: AppSettings.ApiSecret
                );

                var VonageClient = new VonageClient(credentials);

                var response = await VonageClient.SmsClient.SendAnSmsAsync(new Vonage.Messaging.SendSmsRequest()
                {
                    To = $"00{_SmS.MobileNum}",
                    From = "Al-Medad Soft",
                    Text = _SmS.MessageText
                });

                _db.UpdateSmsToFinish(smS: _SmS);

                await File.AppendAllTextAsync("c:\\service\\data.txt", $"\nSent :: to 00{_SmS.MobileNum} at {DateTimeOffset.Now.ToString()}");

                _logger.LogInformation($"Sent to 00{_SmS.MobileNum}");
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync("c:\\service\\data.txt", $"\nError on Sending :: to 00{_SmS.MobileNum}{ex.Message} at {DateTimeOffset.Now.ToString()}");
                _logger.LogError(ex, "Message: {0}", ex.Message);
                _logger.LogInformation($"Failed to 00{_SmS.MobileNum}");

            }
        }

    }
}
