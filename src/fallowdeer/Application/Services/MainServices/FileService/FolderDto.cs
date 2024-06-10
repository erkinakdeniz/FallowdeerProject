using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.MainServices.FileService;
public class FolderDto
{
    public string FolderName { get; set; }
    public string Directory { get; set; }
    public ICollection<FolderDto> SubFolders { get; set; }
}
