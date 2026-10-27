// rfid-encode: encodes a GTIN and serial number as SGTIN-96 EPC and writes the ZPL commands that program the tag.
// Usage: dotnet run -- [gtin] [serial] [companyPrefixLength]
using ZplCore.Rfid;

string gtin = args.Length > 0 ? args[0] : "09506000134352";
string serial = args.Length > 1 ? args[1] : "4711";
int prefixLength = args.Length > 2 ? int.Parse(args[2]) : 7;

Epc epc = ZplRfid.EncodeSgtin(gtin, serial, prefixLength, filter: 1);   // filter 1 = point of sale item

Console.WriteLine($"EPC (hex):      {epc.Hex}");
Console.WriteLine($"Tag URI:        {epc.TagUri}");
Console.WriteLine($"Pure identity:  {epc.PureIdentityUri}");
Console.WriteLine($"Digital Link:   {epc.DigitalLinkUri}");

string zpl = ZplRfid.BuildCommands(epc);   // ^RS / ^RF commands to put into your label
Directory.CreateDirectory("output");
File.WriteAllText("output/rfid.zpl", zpl);
Console.WriteLine();
Console.WriteLine("output/rfid.zpl:");
Console.WriteLine(zpl);
