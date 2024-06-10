using Application.Features.MainFeatures.WebOptions.Rules;
using Application.Services.MainServices.WebOptionService;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Transaction;
using Core.Security.Entities;
using MediatR;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.WebOptions.Commands.Delete;
public class DeleteWebOptionCommand : IRequest, IRoleRequest, ITransactionalRequest
{
    public int Id { get; set; }

    public string[] Roles => new[] { SuperAdmin, Admin };

    public class DeleteWebOptionCommandHandler : IRequestHandler<DeleteWebOptionCommand>
    {
        private readonly IWebOptionService _webOptionService;
        private readonly WebOptionBusinessRules _webOptionBusinessRules;

        public DeleteWebOptionCommandHandler(IWebOptionService webOptionService, WebOptionBusinessRules webOptionBusinessRules)
        {
            _webOptionService = webOptionService;
            _webOptionBusinessRules = webOptionBusinessRules;
        }

        public async Task Handle(DeleteWebOptionCommand request, CancellationToken cancellationToken)
        {
            WebOption? webOption = await _webOptionService.GetAsync(x => x.Id == request.Id);
            await _webOptionBusinessRules.WebOptionShouldBeExistsWhenSelected(webOption!);
            await _webOptionBusinessRules.IsRun(webOption!);
            await _webOptionService.DeleteAsync(webOption!,true);

        }
    }
}
