using Microsoft.EntityFrameworkCore.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Helpers
{
    public static class FileManager
    {
       
        public static string Save(IFormFile file, string folder)
        {
            var newFileName = Guid.NewGuid().ToString() +file.FileName;
            string path = Path.Combine(folder, newFileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return newFileName;
        }
    }
}
