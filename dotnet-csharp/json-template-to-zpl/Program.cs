// json-template-to-zpl: fills a label template (JSON) with one record (JSON) and writes ZPL and a PNG preview.
// Usage: dotnet run -- [product-label.json] [record.json]
using System.Text.Json;
using ZplCore.Labels;

string templateFile = args.Length > 0 ? args[0] : "product-label.json";
string recordFile = args.Length > 1 ? args[1] : "record.json";

// strict: true rejects unknown properties, so typos in the template fail early
Label label = Label.FromJson(File.ReadAllText(templateFile), strict: true);
JsonElement record = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(recordFile));

var binding = label.For<JsonElement>();
string zpl = binding.ToZpl([record], dpi: 203);
byte[] png = binding.Render([record], dpi: 203).First();   // PNG preview of the same label

Directory.CreateDirectory("output");
File.WriteAllText("output/label.zpl", zpl);
File.WriteAllBytes("output/label.png", png);
Console.WriteLine($"output/label.zpl: {zpl.Length} characters");
Console.WriteLine($"output/label.png: {png.Length} bytes");
