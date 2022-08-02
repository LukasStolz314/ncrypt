using Microsoft.AspNetCore.Mvc;

namespace ncrypt.Server.Controller;

[Route("upload")]
public partial class UploadController : ControllerBase
{
    [HttpPost()]
    public async Task<IActionResult> UploadService(IFormFile file)
    {
        try
        {
            if (!file.FileName.Split('.').Last().Equals("dll"))
                throw new Exception("Uploaded has to be a .dll");

            String path = Path.Combine(Environment.CurrentDirectory,
                "PluginServices", file.FileName);

            using (Stream stream = new FileStream(path, FileMode.Create))
                await file.CopyToAsync(stream);


            // Put your code here
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
