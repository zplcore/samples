// zpl-to-pcl: converts a ZPL label into a PCL raster print job for office laser printers.
// Usage: dotnet run -- [label.zpl]
using ZplCore;

string input = args.Length > 0 ? args[0] : "label.zpl";

var renderer = new ZplRenderer(new ZplRenderSettings { Dpi = 203, OutputFormat = ZplOutputFormat.Pcl });
List<byte[]> jobs = renderer.ProcessCommands(File.ReadAllText(input));

Directory.CreateDirectory("output");
for (int i = 0; i < jobs.Count; i++)
{
    string output = $"output/label-{i + 1}.pcl";
    File.WriteAllBytes(output, jobs[i]);   // send unchanged to TCP port 9100 of a PCL printer
    Console.WriteLine($"{output}: {jobs[i].Length} bytes");
}
