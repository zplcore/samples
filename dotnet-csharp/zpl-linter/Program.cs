// zpl-linter: renders a ZPL file like a printer would and lists everything the printer would skip or misread.
// Usage: dotnet run -- [label.zpl]
using ZplCore;

string input = args.Length > 0 ? args[0] : "label.zpl";

var renderer = new ZplRenderer(new ZplRenderSettings { Dpi = 203 });
var results = renderer.Render(File.ReadAllText(input));

int count = 0;
for (int i = 0; i < results.Count; i++)
{
    foreach (var d in results[i].Diagnostics)   // stable code, severity, message and source position
    {
        Console.WriteLine($"label {i + 1}: {d}");
        count++;
    }
    results[i].Bitmap.Dispose();
}
Console.WriteLine($"{results.Count} label(s), {count} finding(s)");
return count == 0 ? 0 : 1;
