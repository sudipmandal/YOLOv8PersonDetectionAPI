using Compunet.YoloV8;
using YoloPersonDetectionAPI.Models;

namespace YoloPersonDetectionAPI.Detectors
{
    public class PersonDetector
    {
        ILogger logger;
        public PersonDetector(ILogger logger)
        {
            this.logger = logger;
        }
        public async Task<int> GetHumansInImage(byte[] imgData)
        {
            var result = await LocalCache.predictor.DetectAsync(imgData);

#if DEBUG
            foreach (var item in result)
                logger.LogInformation($"Detected -> {item.Name.Name} with Confidence {item.Confidence}");
#endif
            var thresholdConfidence = float.Parse(Environment.GetEnvironmentVariable(Constants.SENSITIVITY) ?? Defaults.GetDefault(Constants.SENSITIVITY));
            int r = result.Count(x => x.Name.Name.ToLower() == "person" && x.Confidence > thresholdConfidence);
            return r;
        }
    }
}
