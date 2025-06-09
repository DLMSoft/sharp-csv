using System.Text;
using DLMSoft.SharpCSV;
using DLMSoft.SharpCSV.Test;

using var stream = File.Open("items.csv", FileMode.Open, FileAccess.Read);

var table = CSVTable.FromStream(stream, new CSVOptions {
    HasHeader = true,
    Encoding = Encoding.UTF8
});

var rows = table.ToModel<Item>();

Console.WriteLine(rows.Count());