using Application.Services.MainServices.FileService;
using Core.Application.Pipelines.Authorization;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.Library.Commands.Delete;
public class DeleteLibraryFolderCommand : IRequest<Unit>, IRoleRequest
{
    public string Directory { get; set; }

    public string[] Roles => new[] { Admin, SuperAdmin, Editor };

    public class DeleteLibraryFolderCommandHandler : IRequestHandler<DeleteLibraryFolderCommand, Unit>
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;

        public DeleteLibraryFolderCommandHandler(IFileService fileService, IWebHostEnvironment env)
        {
            _fileService = fileService;
            _env = env;
        }

        public async Task<Unit> Handle(DeleteLibraryFolderCommand request, CancellationToken cancellationToken)
        {
            string path = Path.Combine(_env.WebRootPath, request.Directory);
            _fileService.DeleteFolder(path);
            return Unit.Task.Result;
        }
    }
}
