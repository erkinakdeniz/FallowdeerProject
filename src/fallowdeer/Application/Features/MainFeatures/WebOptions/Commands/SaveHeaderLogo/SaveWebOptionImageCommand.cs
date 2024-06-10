using Application.Features.MainFeatures.WebOptions.Rules;
using Application.Services.MainServices.FileService;
using Application.Services.MainServices.WebOptionService;
using Application.Services.Repositories;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.WebOptions.Commands.SaveHeaderLogo;
public class SaveWebOptionImageCommand : IRequest<FileInfoDto>, IRoleRequest
{
    public IFormFile Image { get; set; }
    public int Id { get; set; }
    public string[] Roles => new[] { Admin, SuperAdmin };
    public class SaveHeaderLogoCommandHandler : IRequestHandler<SaveWebOptionImageCommand, FileInfoDto>
    {
        private readonly IFileService _fileService;
        private readonly IWebOptionRepository _webOptionRepository;
        private readonly WebOptionBusinessRules _webOptionBusinessRules;
        private readonly IWebHostEnvironment _env;
        private readonly IWebOptionService _webOptionService;

        public SaveHeaderLogoCommandHandler(IFileService fileService, IWebOptionRepository webOptionRepository, WebOptionBusinessRules webOptionBusinessRules, IWebHostEnvironment env,IWebOptionService webOptionService)
        {
            _fileService = fileService;
            _webOptionRepository = webOptionRepository;
            _webOptionBusinessRules = webOptionBusinessRules;
            _env = env;
            _webOptionService = webOptionService;
        }

        public async Task<FileInfoDto> Handle(SaveWebOptionImageCommand request, CancellationToken cancellationToken)
        {


            WebOption? webOption = _webOptionRepository.Get(x => x.Id == request.Id);
            await _webOptionBusinessRules.WebOptionShouldBeExistsWhenSelected(webOption);
            ImageParams ımageParams = JsonSerializer.Deserialize<ImageParams>(webOption.Params);
            if(!string.IsNullOrEmpty(webOption.Value))
            _fileService.DeleteFile(Path.Combine(_env.WebRootPath, webOption.Value));

            var info = new FileInfoDto();
            if (ımageParams is not null)
                info =await _fileService.SaveImage(request.Image, Path.Combine(_env.WebRootPath, "Uploads/Systems"), webOption.Key, ımageParams.Width, ımageParams.Height);
            else
                info = await _fileService.SaveImage(request.Image, Path.Combine(_env.WebRootPath, "Uploads/Systems"), webOption.Key);

            webOption.Value = info.ImgSrc;
            //_webOptionRepository.Update(webOption);
            _webOptionService.Update(webOption);
            return info;
        }
    }
}
