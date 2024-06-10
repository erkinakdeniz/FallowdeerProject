using Application.Services.MainServices.FileService;
using Core.Application.Pipelines.Authorization;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.Library.Queries.Files;
public class GetLibraryFileQuery : IRequest<List<FileInfoDto>>, IRoleRequest
{
    public string Directory { get; set; }

    public string[] Roles => new[] { Admin, SuperAdmin, Editor };

    public class GetFileQueryHandler : IRequestHandler<GetLibraryFileQuery, List<FileInfoDto>>
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;

        public GetFileQueryHandler(IFileService fileService, IWebHostEnvironment env)
        {
            _fileService = fileService;
            _env = env;
        }

        public async Task<List<FileInfoDto>> Handle(GetLibraryFileQuery request, CancellationToken cancellationToken)
        {
            string path = Path.Combine(_env.WebRootPath, request.Directory);
            List<FileInfoDto> fileInfoResponses = await _fileService.GetOnlyFiles(path);
            return fileInfoResponses;
        }
    }
}
