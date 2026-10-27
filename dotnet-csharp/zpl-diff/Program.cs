// zpl-diff: compares two ZPL versions pixel by pixel and line by line and writes an overlay image
// (grey = unchanged, red = only in the first file, green = only in the second file).
// Usage: dotnet run -- [before.zpl] [after.zpl]
using ZplCore;
using ZplCore.Tools;

string a = args.Length > 0 ? args[0] : "before.zpl";
string b = args.Length > 1 ? args[1] : "after.zpl";

var result = ZplDiff.Compare(File.ReadAllText(a), File.ReadAllText(b), new ZplRenderSettings { Dpi = 203 });

Console.WriteLine($"Pixel identical: {result.PixelIdentical}, changed pixels: {result.ChangedPixels}");
Console.WriteLine($"Lines added: {result.LinesAdded}, lines removed: {result.LinesRemoved}");
foreach (var line in result.TextDiff.Where(l => l.Kind != ZplDiffLineKind.Unchanged))
    Console.WriteLine($"{(line.Kind == ZplDiffLineKind.Added ? "+" : "-")} {line.Text}");

Directory.CreateDirectory("output");
foreach (var label in result.Labels.Where(l => l.OverlayPng is not null))
{
    string output = $"output/diff-{label.Index + 1}.png";
    File.WriteAllBytes(output, label.OverlayPng!);
    Console.WriteLine($"{output}: {label.Regions.Count} changed region(s)");
}
