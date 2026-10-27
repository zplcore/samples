// gs1-ai-128-to-zpl: validates GS1 Application Identifiers and creates a ZPL label with a native GS1-128 barcode.
// Usage: dotnet run -- ["(01)09506000134352(10)LOT42(17)271231"]
using ZplCore.Barcodes;
using ZplCore.Labels;

string data = args.Length > 0 ? args[0] : "(01)09506000134352(10)LOT42(17)271231";

Gs1Result gs1 = Gs1.Parse(data);   // check digits, dates, lengths and AI combinations
foreach (var error in gs1.Errors)
    Console.WriteLine($"GS1 error ({error.Ai}): {error.Message}");
if (!gs1.IsValid) return 1;

var label = new Label(LabelUnit.Mm, 100, 40)
    .Add(new BarcodeItem { Symbology = ZplCore.Labels.Symbology.Gs1_128, Code = data, X = 5, Y = 4, BarHeight = 25, ModuleWidth = 2 });

Directory.CreateDirectory("output");
string zpl = label.ToZpl(dpi: 203);
File.WriteAllText("output/gs1-128.zpl", zpl);
Console.WriteLine(zpl);
return 0;
