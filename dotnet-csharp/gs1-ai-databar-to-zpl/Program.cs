// gs1-ai-databar-to-zpl: validates GS1 Application Identifiers and creates a ZPL label with a
// GS1 DataBar Expanded barcode as ^GFA graphic.
// Usage: dotnet run -- ["(01)09506000134352(3103)000750(15)271231"]
using ZplCore.Barcodes;

string data = args.Length > 0 ? args[0] : "(01)09506000134352(3103)000750(15)271231";

Gs1Result gs1 = Gs1.Parse(data);
foreach (var error in gs1.Errors)
    Console.WriteLine($"GS1 error ({error.Ai}): {error.Message}");
if (!gs1.IsValid) return 1;

var barcode = new Barcode
{
    Symbology = Symbology.DataBarExpanded,
    Code = data,
    Dpi = 203,
    Unit = BarcodeUnit.Millimeter,
    BarWidth = 0.33,   // X-dimension
    BarHeight = 12,
};

Directory.CreateDirectory("output");
string zpl = $"^XA^PW900^LL200{barcode.ToZplGraphic(x: 30, y: 30)}^XZ";
File.WriteAllText("output/gs1-databar.zpl", zpl);
Console.WriteLine($"output/gs1-databar.zpl: {zpl.Length} characters");
return 0;
