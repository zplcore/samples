// check-barcode: checks GS1 barcode data (one code per line): AI structure, check digits, dates and lengths.
// Usage: dotnet run -- [codes.txt]
using ZplCore.Barcodes;

string input = args.Length > 0 ? args[0] : "codes.txt";

foreach (string code in File.ReadLines(input).Where(l => l.Trim().Length > 0))
{
    Gs1Result result = Gs1.Parse(code.Trim());
    Console.WriteLine($"{code.Trim()}  ->  {(result.IsValid ? "valid" : "INVALID")}");
    foreach (var element in result.Elements)
        Console.WriteLine($"    ({element.Ai}) {element.Value}");
    foreach (var error in result.Errors)
        Console.WriteLine($"    error ({error.Ai}): {error.Message}");
}
