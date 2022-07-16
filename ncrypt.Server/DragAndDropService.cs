namespace ncrypt.Server;

// Based on: https://www.radzen.com/blog/blazor-drag-and-drop/
public class DragAndDropService
{
    public Object? Data { get; set; }
    public String? Zone { get; set; }

    public void StartDrag(Object data, String zone)
    {
        this.Data = data;
        this.Zone = zone;
    }

    public Boolean Accepts(String zone)
    {
        return this.Zone == zone;
    }
}
