// pdf-to-native-zpl: converts a PDF label into native ZPL: text as ^A fields, barcodes as native ^B commands
// (decoded, regenerated and verified), lines and boxes as ^GB, and ^GF only for the rest.
// Usage: dotnet run -- [label.pdf]
using System.Globalization;
using ZplCore.VectorLift;

string input = args.Length > 0 ? args[0] : "label.pdf";

VectorLiftResult result = VectorLift.Convert(File.ReadAllBytes(input), new VectorLiftOptions { Dpi = 203 });

Directory.CreateDirectory("output");
File.WriteAllText("output/label.zpl", result.Zpl);
Console.WriteLine($"output/label.zpl: {result.ZplBytes} bytes (as a full ^GF graphic: {result.RasterZplBytes} bytes)");
Console.WriteLine($"Fidelity against the PDF: {result.Fidelity?.ToString("P1", CultureInfo.InvariantCulture)}");
