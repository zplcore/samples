// print-to-network-printer: sends a ZPL file to a network label printer (raw TCP, port 9100).
// Without a printer address it runs as a dry run and only shows what would be sent.
// Usage: dotnet run                              (dry run)
//        dotnet run -- <printer-ip> [port] [label.zpl]
using ZplCore.Printing;

string? host = args.Length > 0 ? args[0] : null;
int port = args.Length > 1 ? int.Parse(args[1]) : 9100;
string input = args.Length > 2 ? args[2] : "label.zpl";

string zpl = File.ReadAllText(input);

if (host is null)
{
    Console.WriteLine($"Dry run: would send {input} ({zpl.Length} characters) to <printer-ip>:{port}");
    Console.WriteLine("Run with the printer address to print: dotnet run -- 192.168.1.50");
    return;
}

var printer = new ZplPrinterClient(host, port) { Timeout = TimeSpan.FromSeconds(5) };
await printer.SendAsync(zpl);
Console.WriteLine($"Sent {input} ({zpl.Length} characters) to {host}:{port}");
