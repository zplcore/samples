// barcode-scan-proof: decodes every barcode of a ZPL label back from the rendered image with two independent
// decoders, compares it with the field data and grades it (A-F, modelled on ISO/IEC 15416/15415).
// A pre-check, not an ISO verification.
// Usage: dotnet run -- [label.zpl]
using ZplCore;
using ZplCore.ScanProof;

string input = args.Length > 0 ? args[0] : "label.zpl";

ScanProofReport report = ZplScanProof.Verify(File.ReadAllText(input));

Console.WriteLine(report.ToText());
Directory.CreateDirectory("output");
File.WriteAllText("output/report.json", report.ToJson());
