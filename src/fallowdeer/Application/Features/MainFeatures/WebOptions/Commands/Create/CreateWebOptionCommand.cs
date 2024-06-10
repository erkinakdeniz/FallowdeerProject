using Application.Features.MainFeatures.WebOptions.Rules;
using Application.Services.MainServices.WebOptionService;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Transaction;
using Core.Security.Entities;
using MediatR;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.WebOptions.Commands.Create;
public class CreateWebOptionCommand : IRequest, IRoleRequest, ITransactionalRequest
{
    public CreateWebOptionCommand()
    {
        Title = string.Empty;
        Content = string.Empty;
        Alias = string.Empty;
        InputType = string.Empty;
        Params = string.Empty;
    }

    public string Title { get; set; }
    public string Content { get; set; }
    public string Alias { get; set; }
    public string InputType { get; set; }
    public string Params { get; set; }

    public string[] Roles => new[] { Admin, SuperAdmin };

    public class CreateWebOptionCommandHandler : IRequestHandler<CreateWebOptionCommand>
    {
        private readonly IWebOptionService _webOptionService;
        private readonly WebOptionBusinessRules _webOptionBusinessRules;

        public CreateWebOptionCommandHandler(IWebOptionService webOptionService, WebOptionBusinessRules webOptionBusinessRules)
        {
            _webOptionService = webOptionService;
            _webOptionBusinessRules = webOptionBusinessRules;
        }

        public async Task Handle(CreateWebOptionCommand request, CancellationToken cancellationToken)
        {
            WebOption? currentWebOption = await _webOptionService.GetAsync(x => x.Key.Trim().ToLower() == request.Title.Trim().ToLower());
            await _webOptionBusinessRules.WebOptionIsThere(currentWebOption);
            var webOption = new WebOption(request.Title.Trim(), request.Content.Trim(), request.Alias.Trim(), request.InputType.Trim(), request.Params.Trim());
            await _webOptionService.AddAsync(webOption);

        }
    }
}
