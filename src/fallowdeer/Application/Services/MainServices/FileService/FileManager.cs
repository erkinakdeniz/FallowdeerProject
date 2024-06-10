using Core.CrossCuttingConcerns.Exceptions.Types;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using static Core.CrossCuttingConcerns.Extensions.ImageExtensions;

namespace Application.Services.MainServices.FileService;
public class FileManager : IFileService
{
    public void DeleteFolder(string path)
    {
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        else
            throw new NotFoundException("Dosya bulunamadı");
    }
    public void DeleteFile(string path)
    {

        if (File.Exists(path))
            File.Delete(path);
    }
    public async Task<List<FileInfoResponse>> GetFiles(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Invalid folderName");

        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException("Folder not found");

        var result = new List<FileInfoResponse>();
        string[] subDirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
        foreach (var item in subDirs)
        {
            var folder = new FolderInfoDto();
            var folderInfo = new FileInfo(item);
            folder.FolderName = folderInfo.Name;
            folder.FolderPath = item;
            folder.Directory = item.Split("wwwroot")[1].Replace("\\", "/").Substring(1);
            string[] files = Directory.GetFiles(item, "*.*", SearchOption.TopDirectoryOnly);
            var fileInfos = new List<FileInfoDto>();
            foreach (string file in files)
            {
                // Dosya bilgisini oluştur
                var fileInfoDto = new FileInfoDto();
                var fileInfo = new FileInfo(file);
                fileInfoDto.Name = fileInfo.Name;
                fileInfoDto.Extension = fileInfo.Extension;
                fileInfoDto.FullName = file;
                fileInfoDto.Size = fileInfo.Length;
                fileInfoDto.DirectoryName = folderInfo.Name;
                fileInfoDto.LastWriteTime = fileInfo.LastWriteTime;
                fileInfoDto.ImgSrc = $"{folderInfo.Name}/{fileInfo.Name}";
                string thum = await GetThumbnail(fileInfo);
                if (!string.IsNullOrEmpty(thum))
                    fileInfoDto.Thumbnail = $"data:image/{Path.GetExtension(file).ToLower().Substring(1)};base64," + thum;
                else
                    fileInfoDto.Thumbnail = "";

                fileInfos.Add(fileInfoDto);
            }
            folder.Lenght = fileInfos.Count;
            folder.Size = fileInfos.Sum(f => f.Size);
            result.Add(new FileInfoResponse { Folder = folder, Files = fileInfos.ToArray() });
        }
        return result;
    }
    public void CreateFolder(string path)
    {

        Directory.CreateDirectory(path);
    }
    public string ReadFile(string path)
    {
        // Check if the path is valid
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Invalid path");

        // Check if the file exists
        if (!File.Exists(path))
            throw new FileNotFoundException("File not found");

        // Read the file bytes
        byte[] bytes = File.ReadAllBytes(path);

        // Convert the bytes to base64 string
        string base64 = Convert.ToBase64String(bytes);

        // Return the base64 string
        return base64;
    }
    public string ReadImage(string filePath)
    {
        string base64 = "";
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found");
        using (var image = Image.FromFile(filePath))
        // Convert the image to base64 string
        using (var ms = new MemoryStream())
        {
            image.Save(ms, image.RawFormat);
            byte[] bytes = ms.ToArray();
            base64 = Convert.ToBase64String(bytes);
        }
        return base64;
    }
    public async Task<string> GetThumbnail(IFormFile formFile,string imagePath)
    {
        string[] extensions = new[] { ".jpg", ".jpeg", ".png" };
        if (formFile.ContentType.StartsWith("image/"))
        {
            if (extensions.Contains(Path.GetExtension(formFile.FileName)))
            {
                using (var image = Image.FromFile(imagePath))
                {
                    int width = 100;
                    int height = 100;
                    using (var resized = new Bitmap(width, height))
                    {
                        using (var graphics = Graphics.FromImage(resized))
                        {

                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;


                            graphics.DrawImage(image, 0, 0, width, height);
                        }

                        return Save(resized, image);
                    }
                }
            }
            else
                return ReadFile(imagePath);
        }
        else
            return "";
        

    }
    public async Task<string> GetThumbnail(FileInfo fileInfo)
    {
        string[] extensions = new[] { ".jpg", ".jpeg", ".png" };
        try
        {
            if (extensions.Contains(fileInfo.Extension.ToLower()))
            {
                using (var image = Image.FromFile(fileInfo.FullName))
                {
                    int width = 100;
                    int height = 100;
                    using (var resized = new Bitmap(width, height))
                    {
                        using (var graphics = Graphics.FromImage(resized))
                        {

                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;


                            graphics.DrawImage(image, 0, 0, width, height);
                        }

                        return Save(resized, image);
                    }
                }

            }
            else if (fileInfo.Extension.ToLower() == ".webp")
            { return ReadFile(fileInfo.FullName); }
            else
                return "";
        }
        catch (Exception)
        {

            return "";
        }

        

    }
    public async Task<FileInfoDto> SaveImage(IFormFile formFile, string path)
    {
        IsImage(formFile);

        string imageName = GetFileName(formFile);
        string imageFullName = imageName + Path.GetExtension(formFile.FileName).ToLower();

        Directory.CreateDirectory(path);
        string imagePath = Path.Combine(path, imageFullName);

        FileInfoDto fileInfoDto = new();
        fileInfoDto.Name = imageName;
        fileInfoDto.FullName = imageFullName;

        string directoryName = GetDirectoryName(path);
        fileInfoDto.DirectoryName = directoryName;
         
        await Save(formFile, path);
        fileInfoDto.ImgSrc= $"{directoryName}/{imageName}{Path.GetExtension(formFile.FileName)}";

        fileInfoDto.LastWriteTime = DateTime.Now;
        fileInfoDto.Thumbnail = await GetThumbnail(formFile,imagePath);
        fileInfoDto.Size = formFile.Length;

        return fileInfoDto;
    }
    public async Task<FileInfoDto> SaveImage(IFormFile formFile, string path,string imageName)
    {
        IsImage(formFile);

        string imageFullName = imageName + Path.GetExtension(formFile.FileName).ToLower();

        Directory.CreateDirectory(path);
        string imagePath = Path.Combine(path, imageFullName);

        FileInfoDto fileInfoDto = new();
        fileInfoDto.Name = imageName;
        fileInfoDto.FullName = imageFullName;

        string directoryName = GetDirectoryName(path);
        fileInfoDto.DirectoryName = directoryName;

        await Save(formFile, path);
        fileInfoDto.ImgSrc = $"{directoryName}/{imageName}{Path.GetExtension(formFile.FileName)}";

        fileInfoDto.LastWriteTime = DateTime.Now;
        fileInfoDto.Thumbnail = await GetThumbnail(formFile,imagePath);
        fileInfoDto.Size = formFile.Length;

        return fileInfoDto;
    }
    public async Task<FileInfoDto> SaveImage(IFormFile formFile, string path,int width,int height)
    {
        IsImage(formFile);

        string imageName = GetFileName(formFile);
        string imageFullName = imageName + Path.GetExtension(formFile.FileName).ToLower();

        Directory.CreateDirectory(path);
        string imagePath = Path.Combine(path, imageFullName);

        FileInfoDto fileInfoDto = new();
        fileInfoDto.Name = imageName;
        fileInfoDto.FullName = imageFullName;

        string directoryName = GetDirectoryName(path);
        fileInfoDto.DirectoryName = directoryName;

        
        fileInfoDto.ImgSrc = await SaveResizeImage(formFile,imagePath,directoryName,imageName,FileMode.Create,width,height);

        fileInfoDto.LastWriteTime = DateTime.Now;
        fileInfoDto.Thumbnail = await GetThumbnail(formFile, imagePath);
        fileInfoDto.Size = formFile.Length;

        return fileInfoDto;
    }
    public async Task<FileInfoDto> SaveImage(IFormFile formFile, string path,string imageName, int width, int height)
    {
        IsImage(formFile);

        string imageFullName = imageName + Path.GetExtension(formFile.FileName).ToLower();

        Directory.CreateDirectory(path);
        string imagePath = Path.Combine(path, imageFullName);

        FileInfoDto fileInfoDto = new();
        fileInfoDto.Name = imageName;
        fileInfoDto.FullName = imageFullName;

        string directoryName = GetDirectoryName(path);
        fileInfoDto.DirectoryName = directoryName;


        fileInfoDto.ImgSrc = await SaveResizeImage(formFile, imagePath, directoryName, imageName, FileMode.Create, width, height);

        fileInfoDto.LastWriteTime = DateTime.Now;
        fileInfoDto.Thumbnail = await GetThumbnail(formFile, imagePath);
        fileInfoDto.Size = formFile.Length;

        return fileInfoDto;
    }
    public FileInfoDto SaveTempImage(IFormFile formFile, int width = 0, int height = 0)
    {
        if (formFile == null && !formFile.ContentType.StartsWith("image/"))
            throw new BusinessException("Geçersiz Dosya");

        string imageName = formFile.FileName.Split('.')[0] + "_" + DateTime.Now.ToFileTime();
        string imageFullName = imageName + Path.GetExtension(formFile.FileName).ToLower();
        var fileInfoDto = new FileInfoDto();
        fileInfoDto.Name = imageName;
        fileInfoDto.FullName = imageFullName;
        fileInfoDto.LastWriteTime = DateTime.Now;
        fileInfoDto.Size = formFile.Length;


        using (var ms = new MemoryStream())
        {

            // Copy the formFile to the memory stream
            formFile.CopyTo(ms);

            // Load the image from the memory stream
            using (var image = Image.FromStream(ms))
            {

                if (width != 0 && height != 0)
                    // resize the image to 150x150
                    using (var resizedImage = image.Resize(width, height))
                        // save the resized image to the stream
                        resizedImage.Save(ms, ImageFormat.Png);
                // Convert the image to base64 string
                byte[] bytes = ms.ToArray();
                fileInfoDto.ImgSrc = $"data:image/{Path.GetExtension(formFile.FileName).ToLower().Substring(1)};base64," + Convert.ToBase64String(bytes);
            }
        }
        return fileInfoDto;
    }
    public async Task<FileInfoDto> SaveFile(IFormFile formFile, string path)
    {

        Directory.CreateDirectory(path);
        var fileInfo = new FileInfoDto();
        fileInfo.Name = formFile.Name;
        fileInfo.FullName = formFile.FileName;
        fileInfo.DirectoryName = path.Split("wwwroot")[1].Replace("\\", "/").Substring(1);
        fileInfo.LastWriteTime = DateTime.Now;
        fileInfo.Size = formFile.Length;
        string filePath = Path.Combine(path, formFile.FileName);
        await Save(formFile, filePath);

       
        return fileInfo;

    }
    public FolderDto GetFolders(string folderParth)
    {

        var folder = new FolderDto();
        var folderInfo = new DirectoryInfo(folderParth);
        folder.FolderName = folderInfo.Name;
        folder.Directory = folderParth.Split("wwwroot")[1].Replace("\\", "/").Substring(1);
        // Alt klasörleri doldurun
        folder.SubFolders = new List<FolderDto>();
        string[] subDirs = Directory.GetDirectories(folderParth, "*", SearchOption.TopDirectoryOnly);
        foreach (var subDir in subDirs)
            folder.SubFolders.Add(GetFolders(subDir));
        return folder;

    }
    public async Task<List<FileInfoDto>> GetOnlyFiles(string path)
    {

        PathControl(path);


        string[] files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
        var fileInfos = new List<FileInfoDto>();
        int id = 0;
        foreach (string file in files)
        {

            // Dosya bilgisini oluştur
            var fileInfoDto = new FileInfoDto();
            var fileInfo = new FileInfo(file);
            fileInfoDto.Name = fileInfo.Name;
            //fileInfoDto.FullName = file;
            fileInfoDto.Size = fileInfo.Length;
            string directoryName = GetDirectoryName(path);
            fileInfoDto.DirectoryName = directoryName;
            fileInfoDto.LastWriteTime = fileInfo.LastWriteTime;
            fileInfoDto.ImgSrc = $"{directoryName}/{fileInfo.Name}";
            fileInfoDto.Extension = fileInfo.Extension;
            fileInfoDto.Id = ++id;
            string thum = await GetThumbnail(fileInfo);
            if (!string.IsNullOrEmpty(thum))
                fileInfoDto.Thumbnail = $"data:image/{Path.GetExtension(file).ToLower().Substring(1)};base64," + thum;
            else
                fileInfoDto.Thumbnail = "";
            fileInfos.Add(fileInfoDto);
        }
        return fileInfos;
    }
    public List<FileInfoDto> GetOnlyFilesForSlider(string path)
    {
        PathControl(path);

        string[] files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
        var fileInfos = new List<FileInfoDto>();
        int id = 0;
        foreach (string file in files)
        {
            var fileInfoDto = new FileInfoDto();
            var fileInfo = new FileInfo(file);
            string directoryName = path.Split("wwwroot")[1].Replace("\\", "/").Substring(1);
            fileInfoDto.ImgSrc = $"{directoryName}/{fileInfo.Name}";
            fileInfoDto.Id = ++id;
            fileInfos.Add(fileInfoDto);
        }
        return fileInfos;
    }

    private async Task<string> Save(IFormFile formFile, string filePath)
    {
        string base64 = "";
        using (var memoryStream = new MemoryStream())
        {
            await formFile.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();
            base64 = Convert.ToBase64String(fileBytes);
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                await stream.WriteAsync(fileBytes, 0, fileBytes.Length);


        }
        return base64;
    }
    private string Save(Bitmap bitmap, Image image)
    {
        string base64 = "";
        using (var ms = new MemoryStream())
        {
            bitmap.Save(ms, image.RawFormat);
            byte[] bytes = ms.ToArray();
            base64 = Convert.ToBase64String(bytes);
        }
        return base64;
    }
    private void IsImage(IFormFile formFile)
    {
        if (formFile == null && !formFile.ContentType.StartsWith("image/"))
            throw new BusinessException("Geçersiz Dosya");
    }
    private string GetFileName(IFormFile formFile, string suffix = "")
    {
        if (string.IsNullOrEmpty(suffix))
            return formFile.FileName.Split('.')[0] + "_" + DateTime.Now.ToFileTime();
        else
            return formFile.FileName.Split('.')[0] + "_" + suffix;
    }
    private string GetDirectoryName(string path)
    {
        return path.Split("wwwroot")[1].Replace("\\", "/").Substring(1);
    }
    private void PathControl(string path)
    {
        // Check if the folderName is valid
        if (string.IsNullOrEmpty(path))
            throw new NotFoundException("Invalid folderName");



        // Check if the folder exists
        if (!Directory.Exists(path))
            throw new NotFoundException("Folder not found");
    }
    private async Task<string> SaveResizeImage(IFormFile formFile, string imagePath, string directoryName, string imageName, FileMode fileMode, int width, int height)
    {
        string imageDirectory = "";
        string[] extensions = new[] { ".jpg", ".jpeg", ".png" };
        if (extensions.Contains(Path.GetExtension(formFile.FileName)))
        {
            using (var stream = new FileStream(imagePath, fileMode))
            {
                using (var image = Image.FromStream(formFile.OpenReadStream()))
                {
                    using (var resizedImage = image.Resize(width, height))
                    {
                        if (Path.GetExtension(formFile.FileName).Contains(".png"))
                        {
                            resizedImage.Save(stream, ImageFormat.Png);
                            imageDirectory = $"{directoryName}/{imageName}.png";
                        }
                        else if (Path.GetExtension(formFile.FileName).Contains(".jpg"))
                        {
                            resizedImage.Save(stream, ImageFormat.Jpeg);
                            imageDirectory = $"{directoryName}/{imageName}.jpg";
                        }
                        else
                        {
                            formFile.CopyTo(stream);
                            imageDirectory = $"{directoryName}/{imageName}{Path.GetExtension(formFile.FileName)}";
                        }

                    }
                }

            }
        }
        else
        {
            await Save(formFile, imagePath);
            imageDirectory = $"{directoryName}/{imageName}{Path.GetExtension(formFile.FileName)}";
        }


        return imageDirectory;

    }
}

