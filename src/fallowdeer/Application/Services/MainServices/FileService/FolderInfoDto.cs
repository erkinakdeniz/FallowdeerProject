using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.MainServices.FileService;
public class FolderInfoDto
{
    public string FolderName { get; set; }
    public string FolderPath { get; set; }
    public string Directory { get; set; }
    public int Lenght { get; set; } = 0;
    public long Size { get; set; } = 0;
}
