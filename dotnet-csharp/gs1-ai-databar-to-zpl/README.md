# GS1 DataBar to ZPL

Validates GS1 Application Identifiers and creates a ZPL label with a GS1 DataBar Expanded barcode as `^GFA` graphic.

## Run

```sh
cd dotnet-csharp/gs1-ai-databar-to-zpl
dotnet run    # sample data
dotnet run -- "(01)09506000134352(3103)000750(15)271231"
```

Results are written to the `output` folder.

## Expected output

```text
output/gs1-databar.zpl: 2160 characters
```

![The generated ZPL rendered](images/gs1-databar.png)

## API

`Gs1.Parse`, `Barcode` with `Symbology.DataBarExpanded`, `Barcode.ToZplGraphic` - see the [documentation](https://zplcore.com/docs/) and the [API reference](https://zplcore.com/api-reference/).

Without a licence key the SDK runs in evaluation mode: rendered images carry a "zplCore.com TRIAL" watermark and generated ZPL contains small TRIAL fields. See the [main README](../../README.md#licence-key).
