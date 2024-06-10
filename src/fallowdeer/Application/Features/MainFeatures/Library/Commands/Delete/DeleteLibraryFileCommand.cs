using Application.Services.MainServices.FileService;
using Core.Application.Pipelines.Authorization;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.Library.Commands.Delete;
public class DeleteLibraryFileCommand : IRequest<Unit>, IRoleRequest
{
    public string ImageSrc { get; set; }

    public string[] Roles => new[] { Admin, SuperAdmin, Editor };

    public class DeleteLibraryFileCommandHandler : IRequestHandler<DeleteLibraryFileCommand, Unit>
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;

        public DeleteLibraryFileCommandHandler(IFileService fileService, IWebHostEnvironment env)
        {
            _fileService = fileService;
            _env = env;
        }

        public async Task<Unit> Handle(DeleteLibraryFileCommand request, CancellationToken cancellationToken)
        {
            string path = Path.Combine(_env.WebRootPath, request.ImageSrc);
            _fileService.DeleteFile(path);
            return Unit.Task.Result;
        }
    }
}
