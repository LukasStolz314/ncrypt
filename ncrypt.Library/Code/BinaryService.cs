using System.Text;

namespace ncrypt.Library.Code;

[RenderUI (Class = RenderClass.Format)]
public class BinaryService
{
    [RenderUI]
    public String Encode(String data)
    {
        StringBuilder sb = new ();
 
        foreach (Char c in data.ToCharArray())        
            sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));

        return sb.ToString();
    }

    [RenderUI]
    public String Decode(String data)
    {
        List<Byte> bytes = new ();
 
        for (Int32 i = 0; i < data.Length; i += 8)
            bytes.Add(Convert.ToByte(data.Substring(i, 8), 2));

        return Converter.ToString(bytes.ToArray(), ConvertType.ASCII);
    }
}
