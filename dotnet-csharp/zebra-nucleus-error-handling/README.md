# Error handling for Zebra Nucleus

Prints through Zebra Nucleus the way production code should: every `NucleusErrorKind` mapped to what to do next,
retries only when nothing reached Nucleus (`NucleusException.Retryable`), and never a blind resend when the outcome
is unknown - that is how labels get printed twice. The token never appears in an error message.

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
cd dotnet-csharp/zebra-nucleus-error-handling
dotnet run                       # dry run: every error kind and what the sample does
dotnet run -- XXZJJ174600974     # print label.zpl with the error handling
```

## The rules

| Error kind | Meaning | What to do |
|---|---|---|
| `Unavailable` | Nucleus unreachable, nothing accepted | retry - it cannot print twice |
| `UnknownOutcome` | the request may have arrived, no answer | do **not** resend blindly; check the job or the printer |
| `Unauthenticated` | token missing, expired or revoked | new token in Nucleus |
| `PermissionDenied` | wrong scope or another tenant | token of the printer's tenant |
| `NotFound` | serial number or job id not in the tenant | check the printer in Nucleus |
| `ResourceExhausted` | 50 free calls of the day used up | wait for the daily reset, or license the printer |
| `MissingNucleusLicense`, `MissingCloudPrintLicense` | printer not licensed | license the printer |
| `PayloadTooLarge` | more than 10 MB inline | `PrintRemoteFileAsync` (Nucleus downloads the file) |
| `JobFailed`, `JobCancelled` | accepted, then failed or cancelled at Nucleus | check the printer alerts |

zplCore itself repeats a call only when no connection could be opened (`ConnectRetries`). Everything else is your
decision - this sample retries `Unavailable` twice with a growing delay and stops on all other errors.

## API

`NucleusPrinterConnection`, `NucleusCloudPrintClient`, `NucleusException` (namespace `ZplCore.Printing`) - see
[Zebra Nucleus in the documentation](https://zplcore.com/docs/printing/#nucleus) and the
[API reference](https://zplcore.com/api-reference/). Other Nucleus samples: [zebra-nucleus-print](../zebra-nucleus-print/), [zebra-nucleus-data-binding](../zebra-nucleus-data-binding/), [zebra-nucleus-job-status](../zebra-nucleus-job-status/).
Server instead of SDK: [docker/zebra-nucleus](../../docker/zebra-nucleus/).

Without a licence key the SDK runs in evaluation mode: generated ZPL contains small TRIAL fields. See the [main README](../../README.md#licence-key).

Zebra and Nucleus are trademarks of Zebra Technologies Corp.
