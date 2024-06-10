using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.MainServices.FileService;
public class FileInfoResponse
{
    public FolderInfoDto Folder { get; set; }
    public FileInfoDto[] Files { get; set; }
}
