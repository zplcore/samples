// zebra-nucleus-data-binding: binds a list of C# records to one label layout and prints all labels in a single job
// through Zebra Nucleus. Swap the connection for a ZplPrinterClient and the same code prints on a network printer.
// The API token comes from ZEBRA_NUCLEUS_TOKEN or ZEBRA_NUCLEUS_TOKEN_FILE. Without token or serial number the sample
// writes the job to output/shipments.zpl and output/shipments.pdf instead of printing.
// Usage: dotnet run                 (dry run: files in output/)
//        dotnet run -- <serial>
using ZplCore.Labels;
using ZplCore.Printing;

List<Shipment> shipments =
[
    new("Jane Doe", "12 Main Street", "Springfield 12345", "PKG-2026-0001"),
    new("John Smith", "7 Harbour Lane", "Shelbyville 54321", "PKG-2026-0002"),
    new("Erika Muster", "3 Station Road", "Ogdenville 67890", "PKG-2026-0003"),
];

// a small 50 x 20 mm label; fields are bound by property name (DataField) or computed (Expression)
var label = new Label(LabelUnit.Mm, 50, 20)
    .Add(new TextItem { X = 2, Y = 1.5, DataField = "Name", FontSize = 9 })
    .Add(new TextItem { X = 2, Y = 5.5, DataField = "City", FontSize = 6 })
    .Add(new BarcodeItem { X = 2, Y = 9, DataField = "TrackingNo", BarHeight = 7, ModuleWidth = 1 })
    .Add(new TextItem { X = 40, Y = 1.5, Expression = "\"#\" + LABEL_NUMBER", FontSize = 6 });

var binding = label.For<Shipment>();
string zpl = binding.ToZpl(shipments, dpi: 203);   // one ZPL job, one label per record

string? serial = args.Length > 0 ? args[0] : null;
bool hasToken = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(NucleusCloudPrintOptions.TokenVariable))
             || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(NucleusCloudPrintOptions.TokenFileVariable));
if (serial is null || !hasToken)
{
    Directory.CreateDirectory("output");
    File.WriteAllText("output/shipments.zpl", zpl);
    File.WriteAllBytes("output/shipments.pdf", binding.RenderPdf(shipments, dpi: 203));
    Console.WriteLine($"Dry run: {shipments.Count} labels -> output/shipments.zpl ({zpl.Length} characters), output/shipments.pdf");
    Console.WriteLine("To print: set ZEBRA_NUCLEUS_TOKEN, then: dotnet run -- XXZJJ174600974");
    return;
}

// all labels in one Nucleus call: every call counts against the tenant's budget (50 free calls per day without licence)
await using var printer = new NucleusPrinterConnection(serial) { WaitForCompletion = true };
await printer.SendAsync(zpl);
Console.WriteLine($"{shipments.Count} labels printed on {serial}: job {printer.LastJobId}, {printer.LastJob?.Status}");

record Shipment(string Name, string Street, string City, string TrackingNo);
