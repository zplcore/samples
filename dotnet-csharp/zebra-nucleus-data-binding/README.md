# Data binding to a Zebra Nucleus printer

Binds a list of C# records to one label layout (50 x 20 mm) and prints all labels as a single job through Zebra
Nucleus. Only the connection is Nucleus-specific: swap it for a `ZplPrinterClient` and the same code prints on a
network printer. All labels go in one call, because every call counts against the tenant's budget.

## Before you start

1. Enroll the printer in Zebra Nucleus (app Zebra Nucleus Connector) and note its serial number.
2. In Nucleus, create an API token (ID icon > API Tokens) and give it to the sample through the environment - never
   put it into code or a committed file:

```sh
export ZEBRA_NUCLEUS_TOKEN=zbr_nuc_...            # or: export ZEBRA_NUCLEUS_TOKEN_FILE=/path/to/token.txt
```

```powershell
$env:ZEBRA_NUCLEUS_TOKEN = "zbr_nuc_..."           # Windows PowerShell
```

Without a token the sample runs as a dry run, so it also works without Nucleus. The Cloud Print API is a Zebra add-on
to Nucleus, licensed per printer; without a licence a tenant has a small number of free calls per day (50).

## Run

```sh
cd dotnet-csharp/zebra-nucleus-data-binding
dotnet run                       # dry run: output/shipments.zpl and output/shipments.pdf
dotnet run -- XXZJJ174600974     # print the three labels in one job, wait for COMPLETED
```

## Expected output (example)

```text
3 labels printed on XXZJJ174600974: job 7f3c9e2a-1b4d-4c8e-9a51-0d2f6b8e4c13, Completed
```

The core of the sample:

```csharp
string zpl = label.For<Shipment>().ToZpl(shipments, dpi: 203);   // one ZPL job, one label per record
await using var printer = new NucleusPrinterConnection(serial) { WaitForCompletion = true };
await printer.SendAsync(zpl);
```

## API

`NucleusPrinterConnection`, `NucleusCloudPrintClient`, `NucleusException` (namespace `ZplCore.Printing`) - see
[Zebra Nucleus in the documentation](https://zplcore.com/docs/printing/#nucleus) and the
[API reference](https://zplcore.com/api-reference/). Other Nucleus samples: [zebra-nucleus-print](../zebra-nucleus-print/), [zebra-nucleus-job-status](../zebra-nucleus-job-status/), [zebra-nucleus-error-handling](../zebra-nucleus-error-handling/).
Server instead of SDK: [docker/zebra-nucleus](../../docker/zebra-nucleus/).

Without a licence key the SDK runs in evaluation mode: generated ZPL contains small TRIAL fields. See the [main README](../../README.md#licence-key).

Zebra and Nucleus are trademarks of Zebra Technologies Corp.
