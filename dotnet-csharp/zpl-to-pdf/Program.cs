// zpl-to-pdf: renders all labels of a ZPL file into one PDF, one page per label.
// Usage: dotnet run -- [labels.zpl]
using ZplCore;

string input = args.Length > 0 ? args[0] : "labels.zpl";

var renderer = new ZplRenderer(new ZplRenderSettings { Dpi = 203, OutputFormat = ZplOutputFormat.Pdf });
byte[] pdf = renderer.ProcessCommands(File.ReadAllText(input))[0];   // vector PDF with searchable text

Directory.CreateDirectory("output");
File.WriteAllBytes("output/labels.pdf", pdf);
Console.WriteLine($"output/labels.pdf: {pdf.Length} bytes");
