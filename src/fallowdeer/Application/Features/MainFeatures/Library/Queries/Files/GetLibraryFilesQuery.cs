using Application.Services.MainServices.FileService;
using Core.Application.Pipelines.Authorization;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using static Application.Features.Constants.GeneralOperationClaims;
namespace Application.Features.MainFeatures.Library.Queries.Files;
public class GetLibraryFilesQuery : IRequest<List<FileInfoResponse>>, IRoleRequest
{
    public string[] Roles => new[] { Admin, SuperAdmin, Editor, User };

    public class GetLibraryFilesQueryHandler : IRequestHandler<GetLibraryFilesQuery, List<FileInfoResponse>>
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;

        public GetLibraryFilesQueryHandler(IFileService fileService, IWebHostEnvironment env)
        {
            _fileService = fileService;
            _env = env;
        }

        public async Task<List<FileInfoResponse>> Handle(GetLibraryFilesQuery request, CancellationToken cancellationToken)
        {
            string path = Path.Combine(_env.WebRootPath, "Library");
            return await _fileService.GetFiles(path);
        }
    }
}
