# ZPL to PDF

Renders all labels of a ZPL file into one PDF, one page per label (vector PDF with searchable text).

## Run

```sh
cd dotnet-csharp/zpl-to-pdf
dotnet run    # input: labels.zpl
dotnet run -- my-labels.zpl    # your own input
```

Results are written to the `output` folder.

## Expected output

```text
output/labels.pdf: 22274 bytes
```

![The three pages of labels.pdf](images/labels-pdf.png)

## API

`ZplRenderer.ProcessCommands` with `ZplOutputFormat.Pdf` - see the [documentation](https://zplcore.com/docs/) and the [API reference](https://zplcore.com/api-reference/).

Without a licence key the SDK runs in evaluation mode: rendered images carry a "zplCore.com TRIAL" watermark and generated ZPL contains small TRIAL fields. See the [main README](../../README.md#licence-key).
