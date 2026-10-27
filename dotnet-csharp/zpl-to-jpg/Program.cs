// zpl-to-jpg: renders every label of a ZPL file to a JPG image.
// Usage: dotnet run -- [label.zpl]
using ZplCore;

string input = args.Length > 0 ? args[0] : "label.zpl";

var renderer = new ZplRenderer(new ZplRenderSettings { Dpi = 203, OutputFormat = ZplOutputFormat.Jpeg });
List<byte[]> images = renderer.ProcessCommands(File.ReadAllText(input));

Directory.CreateDirectory("output");
for (int i = 0; i < images.Count; i++)
{
    string output = $"output/label-{i + 1}.jpg";
    File.WriteAllBytes(output, images[i]);
    Console.WriteLine($"{output}: {images[i].Length} bytes");
}
