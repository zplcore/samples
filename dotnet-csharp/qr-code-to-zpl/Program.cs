// qr-code-to-zpl: creates a ZPL label with a native QR Code (^BQ) for the given text or URL.
// Usage: dotnet run -- [text]
using ZplCore.Labels;

string data = args.Length > 0 ? args[0] : "https://zplcore.com";

// for QR codes BarHeight is the magnification (dots per module, 1-10)
var label = new Label(LabelUnit.Mm, 40, 40)
    .Add(new BarcodeItem { Symbology = Symbology.QrCode, Code = data, X = 4, Y = 4, BarHeight = 6, QrErrorCorrection = 'M' });

string zpl = label.ToZpl(dpi: 203);
Directory.CreateDirectory("output");
File.WriteAllText("output/qr-code.zpl", zpl);
Console.WriteLine(zpl);
