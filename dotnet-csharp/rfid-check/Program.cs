// rfid-check: checks that the EPC written to the RFID tag (^RF) matches the barcodes printed on the same label.
// Usage: dotnet run -- [label.zpl]
using ZplCore.Rfid;

string input = args.Length > 0 ? args[0] : "label.zpl";

var report = ZplRfid.Check(File.ReadAllText(input));

Console.WriteLine(report.ToText(input));
