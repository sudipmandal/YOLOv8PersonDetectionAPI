using Compunet.YoloV8;
using YoloPersonDetectionAPI.Models;

namespace YoloPersonDetectionAPI.Services
{
    public class LocalCacheService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            LocalCache.predictor = new YoloPredictor("Assets/yolov8n-uint8.onnx");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            LocalCache.predictor.Dispose();
            return Task.CompletedTask;
        }
    }
}
