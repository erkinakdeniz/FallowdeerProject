using Application.Services.MainServices.FileService;
using Core.Application.Pipelines.Authorization;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.Library.Queries.Folders;
public class GetAllLibraryFoldersQuery : IRequest<FolderDto>, IRoleRequest
{
    public string[] Roles => new[] { Admin, SuperAdmin, Editor, User };

    public class GetFoldersQueryHandler : IRequestHandler<GetAllLibraryFoldersQuery, FolderDto>
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;

        public GetFoldersQueryHandler(IFileService fileService, IWebHostEnvironment env)
        {
            _fileService = fileService;
            _env = env;
        }

        public async Task<FolderDto> Handle(GetAllLibraryFoldersQuery request, CancellationToken cancellationToken)
        {
            string path = Path.Combine(_env.WebRootPath, "Library");
            FolderDto folderInfos = _fileService.GetFolders(path);
            return folderInfos;
        }
    }
}
