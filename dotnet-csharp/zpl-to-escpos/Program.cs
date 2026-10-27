// zpl-to-escpos: converts a ZPL label into an ESC/POS raster print job for receipt and label printers.
// Usage: dotnet run -- [label.zpl]
using ZplCore;

string input = args.Length > 0 ? args[0] : "label.zpl";

var renderer = new ZplRenderer(new ZplRenderSettings { Dpi = 203, OutputFormat = ZplOutputFormat.EscPos });
List<byte[]> jobs = renderer.ProcessCommands(File.ReadAllText(input));

Directory.CreateDirectory("output");
for (int i = 0; i < jobs.Count; i++)
{
    string output = $"output/label-{i + 1}.escpos";
    File.WriteAllBytes(output, jobs[i]);   // send unchanged to the printer (TCP 9100, USB or serial)
    Console.WriteLine($"{output}: {jobs[i].Length} bytes");
}
