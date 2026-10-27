// zpl-formatter: pretty-prints a ZPL file (one command per line, fields indented) and also writes a minified copy.
// Usage: dotnet run -- [label.zpl]
using ZplCore.Tools;

string input = args.Length > 0 ? args[0] : "label.zpl";
string zpl = File.ReadAllText(input);

string pretty = ZplFormatter.Format(zpl);   // field data is copied byte for byte
string minified = ZplFormatter.Minify(zpl);

Directory.CreateDirectory("output");
File.WriteAllText("output/label.pretty.zpl", pretty);
File.WriteAllText("output/label.min.zpl", minified);
Console.WriteLine(pretty);
Console.WriteLine($"output/label.pretty.zpl: {pretty.Length} characters, output/label.min.zpl: {minified.Length} characters");
