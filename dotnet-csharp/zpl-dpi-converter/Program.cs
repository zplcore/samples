// zpl-dpi-converter: converts ZPL written for one printer resolution to another (here 203 dpi -> 300 dpi).
// Usage: dotnet run -- [label.zpl] [fromDpi] [toDpi]
using ZplCore.Tools;

string input = args.Length > 0 ? args[0] : "label.zpl";
int fromDpi = args.Length > 1 ? int.Parse(args[1]) : 203;
int toDpi = args.Length > 2 ? int.Parse(args[2]) : 300;

// positions, fonts, barcodes, boxes and ^GF graphics are recalculated
string zpl = ZplRescaler.Rescale(File.ReadAllText(input), fromDpi, toDpi);

Directory.CreateDirectory("output");
string output = $"output/label-{toDpi}dpi.zpl";
File.WriteAllText(output, zpl);
Console.WriteLine($"{output}:");
Console.WriteLine(zpl);
