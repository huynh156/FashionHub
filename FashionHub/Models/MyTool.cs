namespace FashionHub.Models
{
    public class MyTool
    {
        public static string UploadImageToFolder(IFormFile myfile, string folder = "Products")

        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folder, myfile.FileName);
                using (var newFile = new FileStream(filePath, FileMode.Create))
                {
                    myfile.CopyTo(newFile);
                }
                return myfile.FileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
    }
}