namespace ncrypt.Library;

public enum RenderClass
{
    Cipher,
    Format,
    Hash
}

public class RenderUI : Attribute 
{
    public RenderClass Class { get; set; }
}