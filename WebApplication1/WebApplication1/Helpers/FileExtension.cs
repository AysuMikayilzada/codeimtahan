using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Runtime.CompilerServices;

namespace WebApplication1.Helpers
{
    public static class FileExtension
    {
        public static bool CheckFileLength(this IFormFile file,int length)
        {
            return (file.Length > length*1024);
        }
        public static bool CheckFileType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }
        public static string CreateFile(this IFormFile file,string envpath,string folder)
        {
            string filename=Guid.NewGuid().ToString()+file.FileName;
            string path=Path.Combine(envpath,folder,filename);
            using(FileStream fileStream=new FileStream(path,FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return filename;
        }
        public static void DeleteFile(this string Image,string envpath,string folder)
        {
            string path = Path.Combine(envpath, folder, Image);
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }
      

    }
}
