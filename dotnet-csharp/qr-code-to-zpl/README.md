# QR Code to ZPL

Creates a ZPL label with a native QR Code (`^BQ`) for the given text or URL.

## Run

```sh
cd dotnet-csharp/qr-code-to-zpl
dotnet run    # sample data
dotnet run -- "https://example.com"
```

Results are written to the `output` folder.

## Expected output

![The generated ZPL rendered](images/qr-code.png)

## API

Label model: `Label` + `BarcodeItem`, `Label.ToZpl` - see the [documentation](https://zplcore.com/docs/) and the [API reference](https://zplcore.com/api-reference/).

Without a licence key the SDK runs in evaluation mode: rendered images carry a "zplCore.com TRIAL" watermark and generated ZPL contains small TRIAL fields. See the [main README](../../README.md#licence-key).
