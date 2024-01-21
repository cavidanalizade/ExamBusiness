namespace ExamBusiness.Helper
{
    public static class FileManager
    {
        public static string UploadFile(this IFormFile file , string env , string folderName)
        {
            string newFileName= Guid.NewGuid().ToString()+file.FileName;
            string path = env+folderName+newFileName;

            using (FileStream stream = new FileStream(path , FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return newFileName;
        }
        public static bool CheckContent(this IFormFile file , string content)
        {
            return file.ContentType.Contains(content);
        }
        public static void RemoveFile(this string ImageUrl , string env , string folderName)
        {
            string path = env+ folderName+ImageUrl;
            try
            {
                File.Delete(path);
                Console.WriteLine("Silindi");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
