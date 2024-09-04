using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MVC.PL.Helpers
{
    public static class DocumentSettings
    {
        public static async Task<string> UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Location Folder Path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files",folderName);

            if (Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            // 2. Get FileName and Make it Unique
            string FileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // 3. Get File Path
            string FilePath = Path.Combine(FolderPath,FileName);

            // 4. Save File as Streams [Data per Time]
            var FileStram = new FileStream(FilePath,FileMode.Create);

            await file.CopyToAsync(FileStram);

            return FileName;

        }

        public static void DeleteFile(string folderName,string fileName)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName,fileName);
            if(File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}
