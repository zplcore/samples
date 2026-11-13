# Print through Zebra Nucleus

Sends a ZPL file through the Zebra Cloud Print API of Zebra Nucleus to a printer that is addressed only by its serial
number. The printer keeps its own WebLink connection to Nucleus: no VPN, no port forwarding, no network route from
your application to the printer. `NucleusPrinterConnection` is an `IPrinterConnection` like TCP or USB, so everything
that prints to a connection prints through Nucleus.

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
cd dotnet-csharp/zebra-nucleus-print
dotnet run                                       # dry run
dotnet run -- XXZJJ174600974                     # print label.zpl, returns when Nucleus accepted the job
dotnet run -- XXZJJ174600974 --wait              # wait until Nucleus reports COMPLETED
dotnet run -- XXZJJ174600974 my-label.zpl --wait
```

## Expected output (example)

```text
Nucleus accepted job 7f3c9e2a-1b4d-4c8e-9a51-0d2f6b8e4c13 for printer XXZJJ174600974
Job id: 7f3c9e2a-1b4d-4c8e-9a51-0d2f6b8e4c13
Status: Completed (COMPLETED), completed 2026-11-12 14:03:51Z
Alerts: none
```

The alerts are the printer's current conditions as Nucleus reports them, for example `ERROR: Paper out` or
`ERROR: Head open`. With `--wait`, a job that Nucleus reports `FAILED` or `CANCELLED` throws a `NucleusException`
(`JobFailed`, `JobCancelled`).

## API

`NucleusPrinterConnection`, `NucleusCloudPrintClient`, `NucleusException` (namespace `ZplCore.Printing`) - see
[Zebra Nucleus in the documentation](https://zplcore.com/docs/printing/#nucleus) and the
[API reference](https://zplcore.com/api-reference/). Other Nucleus samples: [zebra-nucleus-data-binding](../zebra-nucleus-data-binding/), [zebra-nucleus-job-status](../zebra-nucleus-job-status/), [zebra-nucleus-error-handling](../zebra-nucleus-error-handling/).
Server instead of SDK: [docker/zebra-nucleus](../../docker/zebra-nucleus/).

Without a licence key the SDK runs in evaluation mode: generated ZPL contains small TRIAL fields. See the [main README](../../README.md#licence-key).

Zebra and Nucleus are trademarks of Zebra Technologies Corp.
