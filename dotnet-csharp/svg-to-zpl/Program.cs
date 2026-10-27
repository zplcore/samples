// svg-to-zpl: converts an SVG drawing into a ZPL label with a ^GFA graphic at the given height.
// Usage: dotnet run -- [logo.svg] [heightDots]
using ZplCore.Graphics;

string input = args.Length > 0 ? args[0] : "logo.svg";
int heightDots = args.Length > 1 ? int.Parse(args[1]) : 300;

var (graphic, width, height) = SvgImage.ToGraphicField(File.ReadAllText(input), heightDots);
string zpl = $"^XA^PW{width + 40}^LL{height + 40}^FO20,20{graphic}^FS^XZ";

Directory.CreateDirectory("output");
File.WriteAllText("output/logo.zpl", zpl);
Console.WriteLine($"output/logo.zpl: {width} x {height} dots, {zpl.Length} characters");
