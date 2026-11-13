// zebra-nucleus-job-status: works with Nucleus print jobs directly through NucleusCloudPrintClient - print on one or
// several printers in one call, read a job's status, wait for a final state and cancel a job that has not printed yet.
// The API token comes from ZEBRA_NUCLEUS_TOKEN or ZEBRA_NUCLEUS_TOKEN_FILE.
// Usage: dotnet run -- print <serial> [<serial> ...]   label.zpl on every printer, one job per printer
//        dotnet run -- status <job-id>                 status of a job
//        dotnet run -- wait <job-id> [seconds]         wait for COMPLETED, FAILED or CANCELLED (default 60 s)
//        dotnet run -- cancel <job-id>                 cancel a job that is still pending
// Without a command or token it only lists the commands (dry run).
using System.Text;
using ZplCore.Printing;

bool hasToken = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(NucleusCloudPrintOptions.TokenVariable))
             || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(NucleusCloudPrintOptions.TokenFileVariable));
if (args.Length < 2 || !hasToken)
{
    Console.WriteLine("Dry run (no token or no command). Commands:");
    Console.WriteLine("  dotnet run -- print XXZJJ174600974 XXZJJ174600975");
    Console.WriteLine("  dotnet run -- status <job-id>");
    Console.WriteLine("  dotnet run -- wait <job-id> 60");
    Console.WriteLine("  dotnet run -- cancel <job-id>");
    return;
}

// without a Token the options read ZEBRA_NUCLEUS_TOKEN / ZEBRA_NUCLEUS_TOKEN_FILE; endpoint https://api.nucleus.zebra.com
using var nucleus = new NucleusCloudPrintClient(new NucleusCloudPrintOptions { CallTimeout = TimeSpan.FromSeconds(30) });

switch (args[0])
{
    case "print":
    {
        byte[] zpl = Encoding.UTF8.GetBytes(File.ReadAllText("label.zpl"));
        // one result per printer: its job id or its error - an unknown serial number does not fail the others
        foreach (NucleusPrintResult r in await nucleus.PrintAsync(zpl, args[1..]))
        {
            Console.WriteLine(r.Accepted ? $"{r.SerialNumber}: job {r.JobId}" : $"{r.SerialNumber}: {r.Error!.Kind} - {r.Error.Message}");
            foreach (NucleusAlert a in r.ActiveErrors)
                Console.WriteLine($"  printer error: {a.Description}");
        }
        break;
    }
    case "status":
        Show(await nucleus.GetJobAsync(args[1]));
        break;
    case "wait":
    {
        var timeout = TimeSpan.FromSeconds(args.Length > 2 ? int.Parse(args[2]) : 60);
        NucleusPrintJob job = await nucleus.WaitAsync(args[1], timeout);   // polls until a final state or the timeout
        Show(job);
        if (!job.IsFinal) Console.WriteLine($"not final after {timeout.TotalSeconds:0} s");
        break;
    }
    case "cancel":
        // false: the job had already printed or reached another final state
        Console.WriteLine(await nucleus.CancelAsync(args[1]) ? "cancelled" : "not cancelled (already final)");
        break;
    default:
        Console.WriteLine($"unknown command {args[0]}");
        break;
}

static void Show(NucleusPrintJob job) =>
    Console.WriteLine($"{job.JobId}: {job.Status} ({job.RawStatus}), created {job.CreatedAt:u}, completed {job.CompletedAt:u}"
                      + (job.ErrorDetails is { Length: > 0 } e ? $", error: {e}" : ""));
