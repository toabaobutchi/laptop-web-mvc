using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyLaptopWebsite.Areas.Admin.Models
{
    public class FileUtils
    {
        public readonly static string[] AcceptedImageExtensions = { ".jpeg", ".jpg", ".png", ".webp", ".svg" };

        // max length uploaded file is about ~ 3M
        public readonly static int MAX_LENGTH_UPLOADED_FILE = 3000000;

        public readonly static string UPLOADED_FILE_PATH = @"wwwroot/images";

        public static bool CheckUploadedFile(IFormFile file, out string msg, bool allowNull = false)
        {
            bool flag = true;
            msg = "";
            if (file == null || file.Length == 0)
            {
                if(!allowNull)
                {
                    msg = "Hãy tải lên ảnh làm logo cho nhà sản xuất";
                    flag = false;
                }
                
            }
            else
            {
                string etx = Path.GetExtension(file.FileName);
                if (!AcceptedImageExtensions.Contains(etx))
                {
                    msg = "Xin lỗi, định dạng ảnh không được hỗ trợ bởi hệ thống!";
                    flag = false;
                }
                else if (MAX_LENGTH_UPLOADED_FILE < file.Length)
                {
                    msg = $"Dung lượng file {file.Length} byte không được hỗ trợ. Hãy chọn ảnh có dung lượng dưới {MAX_LENGTH_UPLOADED_FILE} byte";
                    flag = false;
                }
            }
            return flag;
        }
        public static void SaveUploadedFile(string path, IFormFile file)
        {
            using (var stream = System.IO.File.Create(path + "/" + file.FileName))
            {
                file.CopyTo(stream);
            }
        }
    }
}
