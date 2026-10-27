// label-compliance-check: checks a ZPL label against the GS1 logistic label rules (X-dimension, bar height,
// quiet zones, human readable text, AI structure) on the rendered geometry. A pre-check, not a certification.
// Usage: dotnet run -- [label.zpl]
using ZplCore.Compliance;

string input = args.Length > 0 ? args[0] : "label.zpl";

ComplianceReport report = ZplCompliance.Check(File.ReadAllText(input), CompliancePacks.Gs1LogisticLabel);

Console.WriteLine(report.ToText(input));
Directory.CreateDirectory("output");
File.WriteAllText("output/report.json", report.ToJson());
File.WriteAllText("output/report.sarif", report.ToSarif(input));   // for code scanning in CI
