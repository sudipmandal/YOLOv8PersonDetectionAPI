using YoloPersonDetectionAPI.Models;

namespace YoloPersonDetectionAPI.Services
{
    public class EnvDefaultService : IHostedService
    {

        ILogger _logger;

        public EnvDefaultService(ILogger<EnvDefaultService> logger)
        {
            this._logger = logger;
            
        }

        void setDefault(string envVar, string defaultVal, VARTYPE type)
        {
            if (String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(envVar)))
            {
                _logger.LogInformation($"Env var ${envVar} is not set, using default {defaultVal}");
                Environment.SetEnvironmentVariable(envVar, defaultVal);
            }
                

            switch (type)
            {
                case VARTYPE.String:
                    break;
                case VARTYPE.Numeric:
                    if(!Decimal.TryParse(Environment.GetEnvironmentVariable(envVar),out var decimalVal))
                    {
                        _logger.LogInformation($"Env var ${envVar} is not valid num, using default {defaultVal}");
                        Environment.SetEnvironmentVariable(envVar, defaultVal);
                    }
                    break;
                case VARTYPE.Bool:
                    if(!Boolean.TryParse(Environment.GetEnvironmentVariable(envVar),out var boolVal))
                    {
                        _logger.LogInformation($"Env var ${envVar} is not valid bool, using default {defaultVal}");
                        Environment.SetEnvironmentVariable(envVar, defaultVal);
                    }
                    break;
                default:
                    break;
            }

            
        }

        void verifyEnvironmentValues()
        {
            foreach (var item in Defaults.GetDefaults())
            {
                setDefault(item.variableName, item.defaultValue, item.type);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation("EnvDefaultSvc Start Called");
            return Task.Run(() => verifyEnvironmentValues());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
