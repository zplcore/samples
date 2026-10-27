// rfid-decode: decodes an EPC read from an RFID tag (hex) into its GS1 keys and URIs.
// Usage: dotnet run -- [epcHex]
using ZplCore.Rfid;

string hex = args.Length > 0 ? args[0] : "30364433400D1EC000001267";

Epc epc = ZplRfid.DecodeEpc(hex);

Console.WriteLine($"Scheme:         {epc.Scheme}");
Console.WriteLine($"Filter:         {epc.Filter}");
Console.WriteLine($"GTIN:           {epc.Gtin}");
Console.WriteLine($"Serial:         {epc.Serial}");
Console.WriteLine($"Tag URI:        {epc.TagUri}");
Console.WriteLine($"Digital Link:   {epc.DigitalLinkUri}");
foreach (var warning in epc.Warnings)
    Console.WriteLine($"Warning:        {warning}");
