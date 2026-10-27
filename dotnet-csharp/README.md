# zplCore SDK samples for .NET (C#)

Small console programs, one per task: an input file goes in, the result is written to `output/`.
Each sample is a self-contained .NET 10 project with a single `Program.cs` that references the
[ZplCore](https://www.nuget.org/packages/ZplCore) NuGet package.

```sh
cd dotnet-csharp/zpl-to-png
dotnet run
```

| Sample | What it does |
|---|---|
| [zpl-to-png](zpl-to-png/) | Renders every label of a ZPL file to a PNG image (label size from `^PW`/`^LL`, 203 dpi). |
| [zpl-to-jpg](zpl-to-jpg/) | Renders every label of a ZPL file to a JPG image. |
| [zpl-to-svg](zpl-to-svg/) | Renders every label of a ZPL file to a vector SVG: text as glyph outlines, barcodes as rectangles, sharp at any zoom. |
| [zpl-to-pdf](zpl-to-pdf/) | Renders all labels of a ZPL file into one PDF, one page per label (vector PDF with searchable text). |
| [zpl-linter](zpl-linter/) | Renders a ZPL file the way a printer would and lists everything a printer would skip or misread, with a stable code, severity and line:column. |
| [zpl-dpi-converter](zpl-dpi-converter/) | Converts ZPL written for one printer resolution to another (default 203 dpi to 300 dpi): positions, fonts, barcodes, boxes and `^GF` graphics are recalculated. |
| [zpl-formatter](zpl-formatter/) | Pretty-prints a ZPL file (one command per line, field blocks indented) and writes a minified copy. |
| [zpl-diff](zpl-diff/) | Compares two ZPL versions pixel by pixel and line by line and writes an overlay image: grey = unchanged, red = only in the first file, green = only in the second file. |
| [label-compliance-check](label-compliance-check/) | Checks a ZPL label against the GS1 logistic label rules (X-dimension, bar height, quiet zones, human readable text, AI structure) on the rendered geometry and writes the report as text, JSON and SARIF. |
| [barcode-scan-proof](barcode-scan-proof/) | Decodes every barcode of a ZPL label back from the rendered image with two independent decoders, compares the result with the field data and grades it A-F (modelled on ISO/IEC 15416/15415). |
| [rfid-encode](rfid-encode/) | Encodes a GTIN and serial number as SGTIN-96 EPC (GS1 EPC Tag Data Standard) and writes the ZPL commands that program the tag. |
| [rfid-decode](rfid-decode/) | Decodes an EPC read from an RFID tag (hex) into its GS1 keys, Tag URI and GS1 Digital Link. |
| [rfid-check](rfid-check/) | Checks that the EPC written to the RFID tag (`^RF`) matches the barcodes printed on the same label. |
| [image-to-zpl](image-to-zpl/) | Converts a PNG, JPG, BMP or GIF image into a ZPL label with a compressed 1-bit `^GFA` graphic. |
| [svg-to-zpl](svg-to-zpl/) | Converts an SVG drawing into a ZPL label with a `^GFA` graphic at the given height in dots. |
| [pdf-to-native-zpl](pdf-to-native-zpl/) | Converts a PDF label into native ZPL: text as `^A` fields, barcodes as native `^B` commands (decoded, regenerated and verified), lines and boxes as `^GB`, and `^GF` only for the rest. |
| [epl2-to-zpl](epl2-to-zpl/) | Converts an EPL2 label (text, barcodes, lines, boxes) into ZPL II. |
| [zpl-to-epl2](zpl-to-epl2/) | Converts a ZPL label into an EPL2 print job for EPL printers (the label as an EPL2 graphic). |
| [zpl-to-escpos](zpl-to-escpos/) | Converts a ZPL label into an ESC/POS raster print job for receipt and label printers. |
| [zpl-to-pcl](zpl-to-pcl/) | Converts a ZPL label into a PCL raster print job for office laser printers. |
| [barcode-to-zpl](barcode-to-zpl/) | Creates a ZPL label with a native Code 128 barcode (`^BC`) for the given data. |
| [qr-code-to-zpl](qr-code-to-zpl/) | Creates a ZPL label with a native QR Code (`^BQ`) for the given text or URL. |
| [gs1-ai-128-to-zpl](gs1-ai-128-to-zpl/) | Validates GS1 Application Identifiers (check digits, dates, lengths) and creates a ZPL label with a native GS1-128 barcode. |
| [gs1-ai-datamatrix-to-zpl](gs1-ai-datamatrix-to-zpl/) | Validates GS1 Application Identifiers and creates a ZPL label with a GS1 DataMatrix (FNC1 first) as `^GFA` graphic. |
| [gs1-ai-qr-code-to-zpl](gs1-ai-qr-code-to-zpl/) | Validates GS1 Application Identifiers and creates a ZPL label with a GS1 QR Code (FNC1 first) as `^GFA` graphic. |
| [gs1-ai-databar-to-zpl](gs1-ai-databar-to-zpl/) | Validates GS1 Application Identifiers and creates a ZPL label with a GS1 DataBar Expanded barcode as `^GFA` graphic. |
| [check-barcode](check-barcode/) | Checks GS1 barcode data, one code per line: AI structure, check digits, dates and lengths. |
| [sunrise-2027-migration](sunrise-2027-migration/) | Adds a QR Code with a GS1 Digital Link URI next to the EAN/UPC of an existing ZPL label (in free space, with quiet zones) and runs the Sunrise 2027 pre-check. |
| [json-template-to-zpl](json-template-to-zpl/) | Fills a label template (JSON) with one record (JSON) and writes the ZPL and a PNG preview. |
| [data-binding](data-binding/) | Binds a list of C# records to one label layout: one label per record, as one ZPL file and one PDF. |
| [print-to-network-printer](print-to-network-printer/) | Sends a ZPL file to a network label printer over raw TCP (port 9100), with a dry run when no printer address is given. |
