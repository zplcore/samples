// zebra-nucleus-error-handling: prints through Zebra Nucleus the way production code should - every error kind of the
// Cloud Print API mapped to what to do next, retries only when nothing reached Nucleus, and never a blind resend when
// the outcome is unknown (that is how labels get printed twice).
// The API token comes from ZEBRA_NUCLEUS_TOKEN or ZEBRA_NUCLEUS_TOKEN_FILE.
// Usage: dotnet run                 (dry run: lists the error kinds and what the sample does for each)
//        dotnet run -- <serial>
using ZplCore.Printing;

string? serial = args.Length > 0 ? args[0] : null;
bool hasToken = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(NucleusCloudPrintOptions.TokenVariable))
             || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(NucleusCloudPrintOptions.TokenFileVariable));
if (serial is null || !hasToken)
{
    Console.WriteLine("Dry run. What this sample does for each NucleusErrorKind:");
    foreach (NucleusErrorKind kind in Enum.GetValues<NucleusErrorKind>())
        Console.WriteLine($"  {kind,-26} {Advice(kind)}");
    Console.WriteLine("To print: set ZEBRA_NUCLEUS_TOKEN, then: dotnet run -- XXZJJ174600974");
    return;
}

string zpl = File.ReadAllText("label.zpl");
await using var printer = new NucleusPrinterConnection(serial, new NucleusCloudPrintOptions
{
    CallTimeout = TimeSpan.FromSeconds(20),
    ConnectRetries = 2,                         // zplCore itself repeats only when no connection could be opened
    ConnectRetryDelay = TimeSpan.FromSeconds(1),
})
{ WaitForCompletion = true, CompletionTimeout = TimeSpan.FromMinutes(1) };

for (int attempt = 1; ; attempt++)
{
    try
    {
        await printer.SendAsync(zpl);
        Console.WriteLine($"printed: job {printer.LastJobId}, {printer.LastJob?.Status}");
        break;
    }
    catch (NucleusException e) when (e.Retryable && attempt < 3)
    {
        // Unavailable: nothing was accepted, so a new attempt cannot print twice
        Console.WriteLine($"attempt {attempt}: Nucleus unreachable, trying again in {attempt * 5} s");
        await Task.Delay(TimeSpan.FromSeconds(attempt * 5));
    }
    catch (NucleusException e)
    {
        // the message never contains the token; GrpcStatus is the raw gRPC status code (-1 when there was none)
        Console.WriteLine($"{e.Kind} (gRPC {e.GrpcStatus}, request sent: {e.RequestSent}): {e.Message}");
        Console.WriteLine($"-> {Advice(e.Kind)}");
        if (printer.LastJobId is { } id)
            Console.WriteLine($"   job id {id} - check it with: dotnet run --project ../zebra-nucleus-job-status -- status {id}");
        foreach (NucleusAlert a in printer.LastAlerts.Where(a => a.IsSet))
            Console.WriteLine($"   printer alert: {a}");
        Environment.ExitCode = 1;
        break;
    }
}

static string Advice(NucleusErrorKind kind) => kind switch
{
    NucleusErrorKind.Unauthenticated => "token missing, expired or revoked: create a new one in Nucleus (ID icon > API Tokens)",
    NucleusErrorKind.PermissionDenied => "token lacks the scope or belongs to another tenant than the printer",
    NucleusErrorKind.NotFound => "serial number (or job id) is not in the token's tenant: check the printer in Nucleus",
    NucleusErrorKind.MissingNucleusLicense => "the printer needs a Nucleus licence (free calls of the day used up)",
    NucleusErrorKind.MissingCloudPrintLicense => "the printer needs a Cloud Print API licence (free calls of the day used up)",
    NucleusErrorKind.ResourceExhausted => "free budget of 50 calls per day used up: wait for the daily reset or license the printer",
    NucleusErrorKind.PayloadTooLarge => "more than 10 MB inline: host the file and use PrintRemoteFileAsync",
    NucleusErrorKind.JobFailed => "accepted, but the printer failed it: check the printer alerts, then print again",
    NucleusErrorKind.JobCancelled => "accepted, but cancelled in Nucleus before it printed",
    NucleusErrorKind.NotCancellable => "the job had already printed or reached a final state",
    NucleusErrorKind.Unavailable => "Nucleus unreachable, nothing accepted: safe to retry",
    NucleusErrorKind.UnknownOutcome => "the request may have arrived: do NOT resend blindly, check the job / the printer first",
    NucleusErrorKind.InvalidArgument => "malformed request: content or serial number missing",
    NucleusErrorKind.RemoteFile => "Nucleus could not download the remote file: check URL and access",
    NucleusErrorKind.Internal => "error inside Nucleus: try again later",
    NucleusErrorKind.Unimplemented => "wrong endpoint or schema",
    NucleusErrorKind.Protocol => "the answer did not follow gRPC/protobuf: wrong endpoint or a proxy in between",
    _ => "not classified: see the gRPC status and message",
};
