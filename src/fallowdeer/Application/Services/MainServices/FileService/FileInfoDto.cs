namespace Application.Services.MainServices.FileService;
public class FileInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; } //Dosyanın Adı
    public string FullName { get; set; } //Dosyanın path dahil hali
    public string Extension { get; set; } //tipi
    public long Size { get; set; } // Dosyanın Boyutu
    public string DirectoryName { get; set; } // Bulunduğu dizinin adı
    public string ImgSrc { get; set; } // html kısmında imgsrc'ye yazılacak değer
    public DateTime LastWriteTime { get; set; }
    public string Thumbnail { get; set; }

}
