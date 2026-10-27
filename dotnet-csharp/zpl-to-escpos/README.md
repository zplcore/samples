# ZPL to ESC/POS

Converts a ZPL label into an ESC/POS raster print job for receipt and label printers.

## Run

```sh
cd dotnet-csharp/zpl-to-escpos
dotnet run    # input: label.zpl
dotnet run -- my-label.zpl    # your own input
```

Results are written to the `output` folder.

## Expected output

```text
output/label-1.escpos: 41426 bytes
```

![Console output](images/console.png)

## API

`ZplRenderer.ProcessCommands` with `ZplOutputFormat.EscPos` - see the [documentation](https://zplcore.com/docs/) and the [API reference](https://zplcore.com/api-reference/).

Without a licence key the SDK runs in evaluation mode: rendered images carry a "zplCore.com TRIAL" watermark and generated ZPL contains small TRIAL fields. See the [main README](../../README.md#licence-key).
