// zebra-nucleus-print: sends a ZPL file through Zebra Nucleus (Zebra Cloud Print API) to a printer that is addressed
// only by its serial number - no VPN, no port forwarding, no network path to the printer. Shows the Nucleus job id and
// the printer's alerts; with --wait it waits until Nucleus reports the job COMPLETED.
// The API token comes from ZEBRA_NUCLEUS_TOKEN or ZEBRA_NUCLEUS_TOKEN_FILE (Nucleus: ID icon > API Tokens).
// Without a token or serial number the sample runs as a dry run and only shows what would be sent.
// Usage: dotnet run                                   (dry run)
//        dotnet run -- <serial> [label.zpl] [--wait]
using System.Text;
using ZplCore.Printing;

bool wait = args.Contains("--wait");
string[] rest = args.Where(a => a != "--wait").ToArray();
string? serial = rest.Length > 0 ? rest[0] : null;
string input = rest.Length > 1 ? rest[1] : "label.zpl";
string zpl = File.ReadAllText(input);

bool hasToken = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(NucleusCloudPrintOptions.TokenVariable))
             || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(NucleusCloudPrintOptions.TokenFileVariable));
if (serial is null || !hasToken)
{
    Console.WriteLine($"Dry run: would send {input} ({Encoding.UTF8.GetByteCount(zpl)} bytes) through Zebra Nucleus to printer {serial ?? "<serial>"}");
    Console.WriteLine("To print: set ZEBRA_NUCLEUS_TOKEN (or ZEBRA_NUCLEUS_TOKEN_FILE), then: dotnet run -- XXZJJ174600974 --wait");
    return;
}

// NucleusPrinterConnection is an IPrinterConnection like TCP or USB: whatever prints to a connection prints through Nucleus
await using var printer = new NucleusPrinterConnection(serial)
{
    WaitForCompletion = wait,                    // SendAsync returns when the job is COMPLETED (throws on FAILED/CANCELLED)
    CompletionTimeout = TimeSpan.FromMinutes(2),
};
printer.JobAccepted += r => Console.WriteLine($"Nucleus accepted job {r.JobId} for printer {r.SerialNumber}");

await printer.SendAsync(zpl);

Console.WriteLine($"Job id: {printer.LastJobId}");
if (printer.LastJob is { } job)
    Console.WriteLine($"Status: {job.Status} ({job.RawStatus}), completed {job.CompletedAt:u}");
// the printer's current alerts as Nucleus reports them: "Paper out", "Head open" ...
Console.WriteLine(printer.LastAlerts.Count == 0 ? "Alerts: none" : "Alerts: " + string.Join(", ", printer.LastAlerts));
