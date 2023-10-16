using Compunet.YoloV8;
using Compunet.YoloV8.Plotting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using WASMObjectDetectorShared.Logic;

namespace WASMObjectDetectorShared.Services
{
    public sealed class ObjectDetectorService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private YoloV8? _detector;
        private WADetectionPlottingOptions? _plottingOptions;

        public ObjectDetectorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task InitializeAsync()
        {
            using var detectorStream = await _httpClient.GetStreamAsync("_content/WASMObjectDetectorShared/models/yolov8s.onnx");

            _detector = new(detectorStream);
            _plottingOptions = await WADetectionPlottingOptions.CreateAsync(_httpClient);
        }

        public bool IsInitialized()
        {
            return _detector is not null && _plottingOptions is not null;
        }

        public async Task<string> DetectAsync(string imageURL)
        {
            ArgumentNullException.ThrowIfNull(_detector, nameof(_detector));
            ArgumentNullException.ThrowIfNull(_plottingOptions, nameof(_plottingOptions));

            using var imageStream = await _httpClient.GetStreamAsync(imageURL);

            var imageSharp = await Image.LoadAsync(imageStream);
            var detection = await _detector.DetectAsync(imageSharp);

            Console.WriteLine(detection.Speed.ToString());

            using var ploted = await detection.PlotImageAsync(imageSharp, _plottingOptions);
            return ploted.ToBase64String(PngFormat.Instance);
        }

        public void Dispose()
        {
            _detector?.Dispose();
        }
    }
}
