using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.MainServices.FileService;
public interface IFileService
{
    Task<FileInfoDto> SaveImage(IFormFile formFile, string path);
    Task<FileInfoDto> SaveImage(IFormFile formFile, string path, int width, int height);
    Task<FileInfoDto> SaveFile(IFormFile formFile, string path);
    Task<FileInfoDto> SaveImage(IFormFile formFile, string path, string imageName);
    Task<FileInfoDto> SaveImage(IFormFile formFile, string path, string imageName, int width, int height);
    
    string ReadImage(string filePath);
    string ReadFile(string path);
    void DeleteFile(string path);
    void DeleteFolder(string path);
    void CreateFolder(string path);
    Task<List<FileInfoResponse>> GetFiles(string path);
    Task<List<FileInfoDto>> GetOnlyFiles(string path);
    List<FileInfoDto> GetOnlyFilesForSlider(string path);
    FolderDto GetFolders(string path);
}
