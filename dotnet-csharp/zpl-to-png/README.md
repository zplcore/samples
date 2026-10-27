# ZPL to PNG

Renders every label of a ZPL file to a PNG image (label size from `^PW`/`^LL`, 203 dpi).

## Run

```sh
cd dotnet-csharp/zpl-to-png
dotnet run    # input: label.zpl
dotnet run -- my-labels.zpl    # your own input
```

Results are written to the `output` folder.

## Expected output

```text
output/label-1.png: 20072 bytes
```

![Rendered label](images/label-1.png)

## API

`ZplRenderer.ProcessCommands` - see the [documentation](https://zplcore.com/docs/) and the [API reference](https://zplcore.com/api-reference/).

Without a licence key the SDK runs in evaluation mode: rendered images carry a "zplCore.com TRIAL" watermark and generated ZPL contains small TRIAL fields. See the [main README](../../README.md#licence-key).
