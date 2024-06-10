using Application.Features.MainFeatures.WebOptions.Rules;
using Application.Services.MainServices.WebOptionService;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Transaction;
using Core.Security.Entities;
using MediatR;
using static Application.Features.Constants.GeneralOperationClaims;
namespace Application.Features.MainFeatures.WebOptions.Commands.Update;
public class UpdateWebOptionCommand : IRequest, IRoleRequest, ITransactionalRequest
{
    public UpdateWebOptionCommand()
    {
        Title = string.Empty;
        Content = string.Empty;
        Alias = string.Empty;
        InputType = string.Empty;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Alias { get; set; }
    public string InputType { get; set; }
    public string[] Roles => new[] { SuperAdmin, Admin };
    public class UpdateWebOptionCommandHandler : IRequestHandler<UpdateWebOptionCommand>
    {
        private readonly IWebOptionService _webOptionService;
        private readonly WebOptionBusinessRules _webOptionBusinessRules;

        public UpdateWebOptionCommandHandler(IWebOptionService webOptionService, WebOptionBusinessRules webOptionBusinessRules)
        {
            _webOptionService = webOptionService;
            _webOptionBusinessRules = webOptionBusinessRules;
        }

        public async Task Handle(UpdateWebOptionCommand request, CancellationToken cancellationToken)
        {
            WebOption? webOption = await _webOptionService.GetAsync(x => x.Id == request.Id);
            await _webOptionBusinessRules.WebOptionShouldBeExistsWhenSelected(webOption);
            await _webOptionBusinessRules.IsRun(webOption);
            webOption!.Key = request.Title.Trim();
            webOption.Value = request.Content.Trim();
            webOption.Alias = request.Alias.Trim();
            webOption.InputType = request.InputType.Trim();
            //await _webOptionService.UpdateAsync(webOption);
            _webOptionService.Update(webOption);
            

        }
    }
}
