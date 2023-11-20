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
            foreach (var item in result.Boxes)
                logger.LogInformation($"Detected -> {item.Class.Name} with Confidence {item.Confidence}");
#endif
            float thresholdConfidence = float.Parse(Environment.GetEnvironmentVariable(Constants.SENSITIVITY) ?? Defaults.GetDefault(Constants.SENSITIVITY));
            int r = result.Boxes.Where(x => x.Class.Name.ToLower() == "person" && x.Confidence > thresholdConfidence).Count();
            return r;
        }
    }
}
