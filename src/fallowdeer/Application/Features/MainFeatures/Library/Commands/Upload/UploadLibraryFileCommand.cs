using Application.Services.MainServices.FileService;
using Core.Application.Pipelines.Authorization;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.Library.Commands.Upload;
public class UploadLibraryFileCommand : IRequest<Unit>, IRoleRequest
{
    public IFormFile[] FormFiles { get; set; }
    public string Directory { get; set; }

    public string[] Roles => new[] { Admin, SuperAdmin, Editor };

    public class UploadLibraryFileCommandHandler : IRequestHandler<UploadLibraryFileCommand, Unit>
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;

        public UploadLibraryFileCommandHandler(IFileService fileService, IWebHostEnvironment env)
        {
            _fileService = fileService;
            _env = env;
        }

        public async Task<Unit> Handle(UploadLibraryFileCommand request, CancellationToken cancellationToken)
        {
            string path = Path.Combine(_env.WebRootPath, request.Directory);
            foreach (var item in request.FormFiles)
                await _fileService.SaveFile(item, path);
            return await Unit.Task;

        }
    }
}
