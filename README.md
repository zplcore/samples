# zplCore SDK Samples

[zplCore](https://zplcore.com/) is a high-performance label print engine for .NET: it renders ZPL and EPL to PNG, PDF
and SVG, builds labels from templates and data, checks barcodes and compliance rules and prints to label printers.
This repository contains small, runnable samples for the .NET SDK and for the zplCore Server and Designer containers.

## Samples

### [.NET (C#)](dotnet-csharp/) – one console program per task

| Sample | Sample | Sample |
|---|---|---|
| [zpl-to-png](dotnet-csharp/zpl-to-png/) | [zpl-to-jpg](dotnet-csharp/zpl-to-jpg/) | [zpl-to-svg](dotnet-csharp/zpl-to-svg/) |
| [zpl-to-pdf](dotnet-csharp/zpl-to-pdf/) | [zpl-linter](dotnet-csharp/zpl-linter/) | [zpl-dpi-converter](dotnet-csharp/zpl-dpi-converter/) |
| [zpl-formatter](dotnet-csharp/zpl-formatter/) | [zpl-diff](dotnet-csharp/zpl-diff/) | [label-compliance-check](dotnet-csharp/label-compliance-check/) |
| [barcode-scan-proof](dotnet-csharp/barcode-scan-proof/) | [rfid-encode](dotnet-csharp/rfid-encode/) | [rfid-decode](dotnet-csharp/rfid-decode/) |
| [rfid-check](dotnet-csharp/rfid-check/) | [image-to-zpl](dotnet-csharp/image-to-zpl/) | [svg-to-zpl](dotnet-csharp/svg-to-zpl/) |
| [pdf-to-native-zpl](dotnet-csharp/pdf-to-native-zpl/) | [epl2-to-zpl](dotnet-csharp/epl2-to-zpl/) | [zpl-to-epl2](dotnet-csharp/zpl-to-epl2/) |
| [zpl-to-escpos](dotnet-csharp/zpl-to-escpos/) | [zpl-to-pcl](dotnet-csharp/zpl-to-pcl/) | [barcode-to-zpl](dotnet-csharp/barcode-to-zpl/) |
| [qr-code-to-zpl](dotnet-csharp/qr-code-to-zpl/) | [gs1-ai-128-to-zpl](dotnet-csharp/gs1-ai-128-to-zpl/) | [gs1-ai-datamatrix-to-zpl](dotnet-csharp/gs1-ai-datamatrix-to-zpl/) |
| [gs1-ai-qr-code-to-zpl](dotnet-csharp/gs1-ai-qr-code-to-zpl/) | [gs1-ai-databar-to-zpl](dotnet-csharp/gs1-ai-databar-to-zpl/) | [check-barcode](dotnet-csharp/check-barcode/) |
| [sunrise-2027-migration](dotnet-csharp/sunrise-2027-migration/) | [json-template-to-zpl](dotnet-csharp/json-template-to-zpl/) | [data-binding](dotnet-csharp/data-binding/) |
| [print-to-network-printer](dotnet-csharp/print-to-network-printer/) | | |

Descriptions: [dotnet-csharp/README.md](dotnet-csharp/README.md).

### [Docker and cloud](docker/) – zplCore Server and Designer

| Sample | What it shows |
|---|---|
| [quickstart](docker/quickstart/) | Server and designer with `docker compose` (local test) |
| [api-key](docker/api-key/) | API keys from a `.env` file |
| [templates-volume](docker/templates-volume/) | Label templates from a mounted folder |
| [kubernetes](docker/kubernetes/) | Plain Kubernetes manifests |
| [helm](docker/helm/) | Minimal sample Helm chart |
| [aws-ecs-fargate](docker/aws-ecs-fargate/) | AWS ECS on Fargate |
| [azure-container-apps](docker/azure-container-apps/) | Azure Container Apps |
| [google-cloud-run](docker/google-cloud-run/) | Google Cloud Run |

## Prerequisites

- .NET samples: [.NET 10 SDK](https://dotnet.microsoft.com/download) on Windows, Linux or macOS. The samples
  reference the [ZplCore](https://www.nuget.org/packages/ZplCore) NuGet package; `dotnet run` restores it.
- Docker samples: Docker with Compose v2; the cloud samples need the CLI of the platform.

```sh
git clone https://github.com/zplcore/samples.git
cd samples/dotnet-csharp/zpl-to-png
dotnet run
```

## Licence key

zplCore is commercial software. Without a licence key the SDK, the server and the designer run in **evaluation mode**:
everything works, but images, PDF and SVG carry a "zplCore.com TRIAL" watermark and generated ZPL contains small
TRIAL fields. The screenshots in this repository were made in evaluation mode.

- Trial key: [zplcore.com/get-started](https://zplcore.com/get-started/) · licences: [zplcore.com/pricing](https://zplcore.com/pricing/)
- SDK: set the environment variable `ZPLCORE_LICENSE` (or `ZPLCORE_LICENSE_FILE`, or put a file `zplcore.license` next
  to the application), or call `ZplCore.Licensing.ZplCoreLicense.Register(key)` at start-up.
- Server: `ZPLCORE__LICENSE__KEY` or a mounted secret file `/run/secrets/zplcore-license`; designer: `ZPLCORE_LICENSE`.

The key is checked offline. Keep it secret and never commit it.

## Documentation

- [Documentation](https://zplcore.com/docs/) · [API reference](https://zplcore.com/api-reference/)
- [zplCore Server](https://zplcore.com/docs/server/) · [zplCore Designer](https://zplcore.com/docs/designer/)

## License

The sample code in this repository is licensed under the [MIT License](LICENSE). The zplCore SDK, Server and Designer
themselves are commercial software under the [zplCore licence terms](https://zplcore.com/legal/).
