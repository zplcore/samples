# ZPL to PCL

Converts a ZPL label into a PCL raster print job for office laser printers.

## Run

```sh
cd dotnet-csharp/zpl-to-pcl
dotnet run    # input: label.zpl
dotnet run -- my-label.zpl    # your own input
```

Results are written to the `output` folder.

## Expected output

```text
output/label-1.pcl: 17634 bytes
```

![Console output](images/console.png)

## API

`ZplRenderer.ProcessCommands` with `ZplOutputFormat.Pcl` - see the [documentation](https://zplcore.com/docs/) and the [API reference](https://zplcore.com/api-reference/).

Without a licence key the SDK runs in evaluation mode: rendered images carry a "zplCore.com TRIAL" watermark and generated ZPL contains small TRIAL fields. See the [main README](../../README.md#licence-key).
