using System.Text;

namespace DLMSoft.SharpCSV.Test;

public class CSVHelperTest {
    [Fact]
    public void FormatCSVStringTest()
    {
        var str = "   1234, \"Hello, world !\"\r\nThis is a test !";        
        Assert.Equal("\"   1234, \"\"Hello, world !\"\"\r\nThis is a test !\"", CSVHelper.FormatCSVString(str));
    }

    [Fact]
    public void ReadCSVTokenTest()
    {
        var str = "aaa,bbb,\r\nccc\r,ddd,eee";
        using var stream = new MemoryStream(Encoding.Default.GetBytes(str));
        using var reader = new StreamReader(stream);
        
        var token = reader.ReadCSVToken();
        Assert.Equal(CSVTokenStatus.NONE, token.Status);
        Assert.Equal("aaa", token.Field);

        token = reader.ReadCSVToken();
        Assert.Equal(CSVTokenStatus.NONE, token.Status);
        Assert.Equal("bbb", token.Field);

        token = reader.ReadCSVToken();
        Assert.Equal(CSVTokenStatus.END_OF_LINE, token.Status);
        Assert.Equal("", token.Field);
        
        token = reader.ReadCSVToken();
        Assert.Equal(CSVTokenStatus.NONE, token.Status);
        Assert.Equal("ccc\r", token.Field);
        
        token = reader.ReadCSVToken();
        Assert.Equal(CSVTokenStatus.NONE, token.Status);
        Assert.Equal("ddd", token.Field);
        
        token = reader.ReadCSVToken();
        Assert.Equal(CSVTokenStatus.END_OF_FILE, token.Status);
        Assert.Equal("eee", token.Field);
    }

    [Fact]
    public void ReadCSVTokenAdvancedTest()
    {
        var str = "\"Hello, world !\",\"\"\"Hey !\r\nYou should't been here !\"\"\"           \r\n\"adsfasfdas\"           ";
        using var stream = new MemoryStream(Encoding.Default.GetBytes(str));
        using var reader = new StreamReader(stream);
        
        var token = reader.ReadCSVToken();
        Assert.Equal(CSVTokenStatus.NONE, token.Status);
        Assert.Equal("Hello, world !", token.Field);

        token = reader.ReadCSVToken();
        Assert.Equal(CSVTokenStatus.END_OF_LINE, token.Status);
        Assert.Equal("\"Hey !\r\nYou should't been here !\"", token.Field);

        token = reader.ReadCSVToken();
        Assert.Equal(CSVTokenStatus.END_OF_FILE, token.Status);
        Assert.Equal("adsfasfdas", token.Field);
    }
}
