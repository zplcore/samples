# Zebra Nucleus print jobs: status, wait, cancel

Uses `NucleusCloudPrintClient` directly: print on one or several printers in one call (one result per printer - an
unknown serial number does not fail the others), read a job's status, wait for a final state and cancel a job that
has not printed yet.

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
cd dotnet-csharp/zebra-nucleus-job-status
dotnet run                                              # dry run: lists the commands
dotnet run -- print XXZJJ174600974 XXZJJ174600975       # label.zpl on both printers
dotnet run -- status 7f3c9e2a-1b4d-4c8e-9a51-0d2f6b8e4c13
dotnet run -- wait 7f3c9e2a-1b4d-4c8e-9a51-0d2f6b8e4c13 60
dotnet run -- cancel 7f3c9e2a-1b4d-4c8e-9a51-0d2f6b8e4c13
```

## Expected output (example)

```text
XXZJJ174600974: job 7f3c9e2a-1b4d-4c8e-9a51-0d2f6b8e4c13
XXZJJ174600975: NotFound - printer XXZJJ174600975 is not in the token's tenant
```

```text
7f3c9e2a-1b4d-4c8e-9a51-0d2f6b8e4c13: Completed (COMPLETED), created 2026-11-12 14:03:49Z, completed 2026-11-12 14:03:51Z
```

`cancel` answers `not cancelled (already final)` when the job has already printed. The same from the command line:
`zplcore nucleus status <job-id>`, `zplcore nucleus cancel <job-id>`.

## API

`NucleusPrinterConnection`, `NucleusCloudPrintClient`, `NucleusException` (namespace `ZplCore.Printing`) - see
[Zebra Nucleus in the documentation](https://zplcore.com/docs/printing/#nucleus) and the
[API reference](https://zplcore.com/api-reference/). Other Nucleus samples: [zebra-nucleus-print](../zebra-nucleus-print/), [zebra-nucleus-data-binding](../zebra-nucleus-data-binding/), [zebra-nucleus-error-handling](../zebra-nucleus-error-handling/).
Server instead of SDK: [docker/zebra-nucleus](../../docker/zebra-nucleus/).

Without a licence key the SDK runs in evaluation mode: generated ZPL contains small TRIAL fields. See the [main README](../../README.md#licence-key).

Zebra and Nucleus are trademarks of Zebra Technologies Corp.
