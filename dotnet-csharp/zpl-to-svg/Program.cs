// zpl-to-svg: renders every label of a ZPL file to a vector SVG (text as outlines, barcodes as rectangles).
// Usage: dotnet run -- [label.zpl]
using ZplCore;

string input = args.Length > 0 ? args[0] : "label.zpl";

var renderer = new ZplRenderer(new ZplRenderSettings { Dpi = 203 });
List<string> svgs = renderer.RenderSvg(File.ReadAllText(input));

Directory.CreateDirectory("output");
for (int i = 0; i < svgs.Count; i++)
{
    string output = $"output/label-{i + 1}.svg";
    File.WriteAllText(output, svgs[i]);
    Console.WriteLine($"{output}: {svgs[i].Length} characters");
}
