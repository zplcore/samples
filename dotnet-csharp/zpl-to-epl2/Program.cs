// zpl-to-epl2: converts a ZPL label into an EPL2 print job (the label as an EPL2 graphic).
// Usage: dotnet run -- [label.zpl]
using ZplCore;

string input = args.Length > 0 ? args[0] : "label.zpl";

var renderer = new ZplRenderer(new ZplRenderSettings { Dpi = 203, OutputFormat = ZplOutputFormat.Epl });
List<byte[]> jobs = renderer.ProcessCommands(File.ReadAllText(input));

Directory.CreateDirectory("output");
for (int i = 0; i < jobs.Count; i++)
{
    string output = $"output/label-{i + 1}.epl";
    File.WriteAllBytes(output, jobs[i]);   // send unchanged to port 9100 of an EPL2 printer
    Console.WriteLine($"{output}: {jobs[i].Length} bytes");
}
