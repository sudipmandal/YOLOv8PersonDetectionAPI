using Compunet.YoloV8;

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
            using (var predictor = new YoloV8("Assets/yolov8s.onnx"))
            {

                var result = await predictor.DetectAsync(imgData);

#if DEBUG
                foreach (var item in result.Boxes)
                    logger.LogInformation($"Detected -> {item.Class.Name} with Confidence {item.Confidence}");
#endif

                int r = result.Boxes.Where(x => x.Class.Name.ToLower() == "person" && x.Confidence > 0.5).Count();
                return r;
            }
        }
    }
}
