// image-to-zpl: converts a PNG/JPG/BMP/GIF image into a ZPL label with a ^GFA graphic (1 bit, compressed).
// Usage: dotnet run -- [logo.png]
using SkiaSharp;
using ZplCore.Graphics;

string input = args.Length > 0 ? args[0] : "logo.png";

using SKBitmap image = SKBitmap.Decode(input);
string zpl = ZplGraphicEncoder.ToZpl(image, dither: DitherMode.Threshold, x: 20, y: 20);

Directory.CreateDirectory("output");
File.WriteAllText("output/logo.zpl", zpl);
Console.WriteLine($"output/logo.zpl: {image.Width} x {image.Height} dots, {zpl.Length} characters");
