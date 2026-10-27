// sunrise-2027-migration: adds a QR Code with a GS1 Digital Link URI next to the EAN/UPC of an existing ZPL label
// (in free space, with quiet zones) and runs the Sunrise 2027 pre-check. A migration assistant, not a certification.
// Usage: dotnet run -- [label.zpl]
using ZplCore.Tools;

string input = args.Length > 0 ? args[0] : "label.zpl";

Gs1MigrationResult result = ZplGs1Migration.Migrate(File.ReadAllText(input));

foreach (var label in result.Labels)
    Console.WriteLine($"label: {Gs1MigrationResult.StatusName(label.Status)}");
foreach (var warning in result.Warnings)
    Console.WriteLine($"warning: {warning}");
Console.WriteLine($"Sunrise 2027 errors after migration: {result.SunriseErrors.Count}");

Directory.CreateDirectory("output");
File.WriteAllText("output/label-migrated.zpl", result.MigratedZpl);
Console.WriteLine("output/label-migrated.zpl written");
