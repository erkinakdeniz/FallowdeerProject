using Application.Services.MainServices.FileService;
using Core.Application.Pipelines.Authorization;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.Library.Commands.Create;
public class CreateLibraryFolderCommand : IRequest<Unit>, IRoleRequest
{
    public string Directory { get; set; }

    public string[] Roles => new[] { Admin, SuperAdmin, Editor };

    public class CreateLibraryFolderCommandHandler : IRequestHandler<CreateLibraryFolderCommand, Unit>
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;

        public CreateLibraryFolderCommandHandler(IFileService fileService, IWebHostEnvironment env)
        {
            _fileService = fileService;
            _env = env;
        }

        public async Task<Unit> Handle(CreateLibraryFolderCommand request, CancellationToken cancellationToken)
        {
            string path = Path.Combine(_env.WebRootPath, "Library", request.Directory);
            _fileService.CreateFolder(path);
            return await Unit.Task;
        }
    }
}
