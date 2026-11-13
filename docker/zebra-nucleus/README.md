# zplCore Server with Zebra Nucleus printers

zplCore Server prints through the [Zebra Cloud Print API](https://zplcore.com/docs/server/#nucleus) of Zebra Nucleus:
your ERP or WMS posts templates and data to the REST API, the server renders the labels, checks them with the printer
twin and hands them to Nucleus. The printer keeps its own WebLink connection to Nucleus - no VPN, no port forwarding,
no network route from the server to the printer.

`zplcore.yaml` configures two printers:

| Printer | What it shows |
|---|---|
| `store-hamburg` | one fixed printer (`serial`), printer twin, wait for `COMPLETED` |
| `stores` | a fleet behind one entry (`serials` with `*` patterns): the request chooses the printer with `?serial=` |

## Before you start

1. Enroll the printer in Zebra Nucleus (app Zebra Nucleus Connector) and note its serial number.
2. In Nucleus, create an API token (ID icon > API Tokens) and save it as `secrets/zebra-nucleus.txt`. The folder is
   in `.gitignore`; the token never belongs into `zplcore.yaml` (the server refuses to start with it there).
3. Replace the serial numbers in `zplcore.yaml` with yours.

The Cloud Print API is a Zebra add-on to Nucleus, licensed per printer; without a licence a tenant has a small number
of free calls per day (50). Every print request below is one call.

## Run

```sh
cd docker/zebra-nucleus
docker compose up -d

# the printers: transport zebracloud, serial / serials, queue - never the token
curl -s localhost:8080/v1/printers | jq '.[] | {name, transport, serial, serials}'

# print a shipping label on store-hamburg (template + data, rendered at the printer's dpi)
curl -X POST 'localhost:8080/v1/print/store-hamburg?template=shipping/label-100x150' \
  -H 'Content-Type: application/json' \
  -d '{"Service":"EXPRESS","Name":"Jane Doe","Street":"12 Main Street","City":"Springfield 12345","Weight":"2.4","Reference":"PO-10045","TrackingNo":"1Z999AA10123456784"}'

# three labels in one job on a printer of the fleet, chosen at runtime
curl -X POST 'localhost:8080/v1/print/stores?template=shipping/label-100x150&serial=50J204201210' \
  -H 'Content-Type: application/json' \
  -d '[{"Name":"Jane Doe","TrackingNo":"PKG-0001"},{"Name":"John Smith","TrackingNo":"PKG-0002"},{"Name":"Erika Muster","TrackingNo":"PKG-0003"}]'

# ZPL as it is, without a template
curl -X POST 'localhost:8080/v1/print/stores?serial=50J204201210' \
  -H 'Content-Type: application/vnd.zpl' --data-binary '^XA^FO40,40^A0N,40,40^FDHello Nucleus^FS^XZ'

# a serial number that is not on the list is refused (403) - nothing is sent to Nucleus
curl -i -X POST 'localhost:8080/v1/print/stores?serial=XYZ123' -H 'Content-Type: application/vnd.zpl' --data-binary '^XA^XZ'
```

This sample runs without a login (`ZPLCORE__AUTH__MODE: none`) - only for a local test. In production use API keys
or JWT: [authentication of the server](https://zplcore.com/docs/server/#auth).

## Asynchronous jobs

```sh
# 202 + Location: /v1/jobs/{id}
curl -i -X POST 'localhost:8080/v1/print/store-hamburg?template=shipping/label-100x150&async=true' \
  -H 'Content-Type: application/json' -d '{"Name":"Jane Doe","TrackingNo":"PKG-0004"}'

curl -s localhost:8080/v1/jobs/<id> | jq .print
# { "printer": "store-hamburg", "serial": "XXZJJ174600974", "state": "sent",
#   "remoteJobId": "7f3c9e2a-…", "remoteStatus": "COMPLETED", "alerts": [] }

# cancel while the server still waits for COMPLETED: cancels the job at Nucleus if it has not started printing
curl -X DELETE localhost:8080/v1/jobs/<id>
```

## What happens on errors

| Situation | Answer |
|---|---|
| Serial number not in the tenant, no licence, invalid content | 502 or 422 with Zebra's reason |
| Label fails the printer twin (`twin: block`) | 422, nothing sent |
| `?serial=` not on the `serials` list | 403, nothing sent |
| More than 10 MB of ZPL | 413 |
| Nucleus unreachable | repeated (`retries`) - nothing had been accepted |
| Outcome unknown, or no `COMPLETED` within `completionTimeout` | the job fails (504) and is **not** resent: no double print |

`GET /v1/printers/{name}/status` answers 501 for Nucleus printers (the Cloud Print API has no status query) and
includes the alerts of the last job.

## More

- Configuration reference: [zplCore Server - Zebra Nucleus printers](https://zplcore.com/docs/server/#nucleus)
- From .NET without a server: [zebra-nucleus-print](../../dotnet-csharp/zebra-nucleus-print/) and the other
  `zebra-nucleus-*` samples
- Command line: `zplcore print label.zpl --nucleus XXZJJ174600974 --token-file token.txt --wait`

Zebra and Nucleus are trademarks of Zebra Technologies Corp.
