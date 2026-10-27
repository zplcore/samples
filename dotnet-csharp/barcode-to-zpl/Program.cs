// barcode-to-zpl: creates a ZPL label with a native Code 128 barcode (^BC) for the given data.
// Usage: dotnet run -- [data]
using ZplCore.Labels;

string data = args.Length > 0 ? args[0] : "ZPLCORE-0042";

var label = new Label(LabelUnit.Mm, 70, 30)
    .Add(new BarcodeItem { Symbology = Symbology.Code128, Code = data, X = 5, Y = 4, BarHeight = 15, ModuleWidth = 2 });

string zpl = label.ToZpl(dpi: 203);
Directory.CreateDirectory("output");
File.WriteAllText("output/barcode.zpl", zpl);
Console.WriteLine(zpl);
