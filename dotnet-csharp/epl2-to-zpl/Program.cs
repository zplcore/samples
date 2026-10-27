// epl2-to-zpl: converts an EPL2 label (text, barcodes, lines, boxes) into ZPL II.
// Usage: dotnet run -- [label.epl]
using ZplCore;

string input = args.Length > 0 ? args[0] : "label.epl";

var converter = new EplRenderer(new ZplRenderSettings { Dpi = 203 });
string zpl = converter.ToZpl(File.ReadAllText(input));

Directory.CreateDirectory("output");
File.WriteAllText("output/label.zpl", zpl);
Console.WriteLine(zpl);
