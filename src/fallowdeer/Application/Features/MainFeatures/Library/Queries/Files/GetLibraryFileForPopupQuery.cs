using Application.Services.MainServices.FileService;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.Library.Queries.Files;
public class GetLibraryFileForPopupQuery : IRequest<List<LibraryFileForPopupResponse>>, IRoleRequest
{
    public string Directory { get; set; }

    public string[] Roles => new[] { Admin, SuperAdmin, Editor, User };

    public class GetLibraryFileForPopupQueryHandler : IRequestHandler<GetLibraryFileForPopupQuery, List<LibraryFileForPopupResponse>>
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public GetLibraryFileForPopupQueryHandler(IFileService fileService, IWebHostEnvironment env, IMapper mapper)
        {
            _fileService = fileService;
            _env = env;
            _mapper = mapper;
        }

        public async Task<List<LibraryFileForPopupResponse>> Handle(GetLibraryFileForPopupQuery request, CancellationToken cancellationToken)
        {
            string path = Path.Combine(_env.WebRootPath, request.Directory);
            List<FileInfoDto> fileInfoResponses = _fileService.GetOnlyFilesForSlider(path);
            List<LibraryFileForPopupResponse> responses = _mapper.Map<List<LibraryFileForPopupResponse>>(fileInfoResponses);
            return responses;
        }
    }
}

