// gs1-ai-qr-code-to-zpl: validates GS1 Application Identifiers and creates a ZPL label with a GS1 QR Code
// (FNC1 first) as ^GFA graphic.
// Usage: dotnet run -- ["(01)09506000134352(17)271231(10)LOT42"]
using ZplCore.Barcodes;

string data = args.Length > 0 ? args[0] : "(01)09506000134352(17)271231(10)LOT42";

Gs1Result gs1 = Gs1.Parse(data);
foreach (var error in gs1.Errors)
    Console.WriteLine($"GS1 error ({error.Ai}): {error.Message}");
if (!gs1.IsValid) return 1;

var barcode = new Barcode
{
    Symbology = Symbology.Gs1QrCode,
    Code = data,
    Dpi = 203,
    Unit = BarcodeUnit.Millimeter,
    BarWidth = 0.75,   // module size
};

Directory.CreateDirectory("output");
string zpl = $"^XA^PW320^LL320{barcode.ToZplGraphic(x: 30, y: 30)}^XZ";
File.WriteAllText("output/gs1-qr-code.zpl", zpl);
Console.WriteLine($"output/gs1-qr-code.zpl: {zpl.Length} characters");
return 0;
