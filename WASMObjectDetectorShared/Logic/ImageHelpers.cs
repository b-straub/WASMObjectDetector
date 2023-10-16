using Compunet.YoloV8.Plotting;
using SixLabors.Fonts;

namespace WASMObjectDetectorShared.Logic;

public class WADetectionPlottingOptions : IDetectionPlottingOptions
{
    public float TextHorizontalPadding { get; set; }

    public float BoxBorderThickness { get; set; }

    public ColorPalette ColorPalette { get; set; }

    public FontFamily FontFamily { get; set; }

    public float FontSize { get; set; }

    public WADetectionPlottingOptions()
    {
        TextHorizontalPadding = 5F;
        BoxBorderThickness = 1F;
        ColorPalette = ColorPalette.Default;
    }

    public static async Task<WADetectionPlottingOptions> CreateAsync(HttpClient httpClient)
    {
        WADetectionPlottingOptions plottingOptions = new();

        var fontBytes = await httpClient.GetByteArrayAsync("_content/WASMObjectDetectorShared/fonts/OpenSans-Regular.ttf");
        using var fontStream = new MemoryStream(fontBytes);
        FontCollection collection = new();
        collection.Add(fontStream);

        plottingOptions.FontFamily = collection.Get("Open Sans");
        plottingOptions.FontSize = 12F;
        return plottingOptions;
    }
}