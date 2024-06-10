using Application.Features.MainFeatures.WebOptions.Rules;
using MediatR;

namespace Application.Features.MainFeatures.Auth.Commands.ExternalRegistrationControl;
public class ExternalRegistrationControlCommand : IRequest
{
    public class ExternalRegistrationControlCommandHandler : IRequestHandler<ExternalRegistrationControlCommand>
    {
        private readonly WebOptionBusinessRules _webOptionBusinessRules;

        public ExternalRegistrationControlCommandHandler(WebOptionBusinessRules webOptionBusinessRules)
        {
            _webOptionBusinessRules = webOptionBusinessRules;
        }

        public async Task Handle(ExternalRegistrationControlCommand request, CancellationToken cancellationToken)
        {
            await _webOptionBusinessRules.IsActiveExternalRegistration();
        }
    }
}
