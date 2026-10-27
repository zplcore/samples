# ZPL to EPL2

Converts a ZPL label into an EPL2 print job for EPL printers (the label as an EPL2 graphic). Send the file unchanged to TCP port 9100 of the printer.

## Run

```sh
cd dotnet-csharp/zpl-to-epl2
dotnet run    # input: label.zpl
dotnet run -- my-label.zpl    # your own input
```

Results are written to the `output` folder.

## Expected output

```text
output/label-1.epl: 41446 bytes
```

![Console output](images/console.png)

## API

`ZplRenderer.ProcessCommands` with `ZplOutputFormat.Epl` - see the [documentation](https://zplcore.com/docs/) and the [API reference](https://zplcore.com/api-reference/).

Without a licence key the SDK runs in evaluation mode: rendered images carry a "zplCore.com TRIAL" watermark and generated ZPL contains small TRIAL fields. See the [main README](../../README.md#licence-key).
