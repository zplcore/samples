// data-binding: binds a list of C# records to one label layout: one label per record, as one ZPL file and one PDF.
// Usage: dotnet run
using ZplCore.Labels;

List<Shipment> shipments =
[
    new("Jane Doe", "12 Main Street", "Springfield 12345", "PKG-2026-0001"),
    new("John Smith", "7 Harbour Lane", "Shelbyville 54321", "PKG-2026-0002"),
    new("Erika Muster", "3 Station Road", "Ogdenville 67890", "PKG-2026-0003"),
];

// the layout: fields are bound by property name (DataField) or computed (Expression)
var label = new Label(LabelUnit.Mm, 100, 60)
    .Add(new TextItem { X = 5, Y = 4, DataField = "Name", FontSize = 16 })
    .Add(new TextItem { X = 5, Y = 12, DataField = "Street", FontSize = 11 })
    .Add(new TextItem { X = 5, Y = 18, DataField = "City", FontSize = 11 })
    .Add(new BarcodeItem { X = 5, Y = 28, DataField = "TrackingNo", BarHeight = 15, ModuleWidth = 2 })
    .Add(new TextItem { X = 72, Y = 4, Expression = "\"No. \" + LABEL_NUMBER", FontSize = 11 });

var binding = label.For<Shipment>();
string zpl = binding.ToZpl(shipments, dpi: 203);
byte[] pdf = binding.RenderPdf(shipments, dpi: 203);

Directory.CreateDirectory("output");
File.WriteAllText("output/shipments.zpl", zpl);
File.WriteAllBytes("output/shipments.pdf", pdf);
Console.WriteLine($"{shipments.Count} records -> output/shipments.zpl ({zpl.Length} characters), output/shipments.pdf ({pdf.Length} bytes)");

record Shipment(string Name, string Street, string City, string TrackingNo);
