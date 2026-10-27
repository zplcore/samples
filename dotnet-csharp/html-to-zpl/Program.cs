// html-to-zpl: renders an HTML label with headless Chromium (Playwright) at the label size and printer resolution,
// converts the image into a ZPL ^GFA graphic and renders the ZPL again as a check.
// Usage: dotnet run -- [label.html]
using Microsoft.Playwright;
using SkiaSharp;
using ZplCore;
using ZplCore.Graphics;

string input = args.Length > 0 ? args[0] : "label.html";
const double widthInches = 4, heightInches = 6;
const int dpi = 203;

// CSS works in 96 px per inch: a 4 x 6 in viewport scaled by 203/96 gives exactly 812 x 1218 printer dots
using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync();
var page = await browser.NewPageAsync(new()
{
    ViewportSize = new() { Width = (int)(widthInches * 96), Height = (int)(heightInches * 96) },
    DeviceScaleFactor = dpi / 96f,
});
await page.SetContentAsync(File.ReadAllText(input));
byte[] png = await page.ScreenshotAsync();

using SKBitmap image = SKBitmap.Decode(png);
string zpl = ZplGraphicEncoder.ToZpl(image, dither: DitherMode.Threshold);

// check: render the ZPL like a printer would
var renderer = new ZplRenderer(new ZplRenderSettings { Dpi = dpi }.WithLabelSizeInches(widthInches, heightInches));
byte[] check = renderer.ProcessCommands(zpl)[0];

Directory.CreateDirectory("output");
File.WriteAllBytes("output/label-html.png", png);
File.WriteAllText("output/label.zpl", zpl);
File.WriteAllBytes("output/label-zpl.png", check);
Console.WriteLine($"output/label-html.png: {image.Width} x {image.Height} px");
Console.WriteLine($"output/label.zpl: {zpl.Length} characters");
Console.WriteLine("output/label-zpl.png: the ZPL rendered by zplCore");
