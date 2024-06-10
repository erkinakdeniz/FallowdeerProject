using Application.Features.MainFeatures.WebOptions.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Entities;

namespace Application.Features.MainFeatures.WebOptions.Rules;
public class WebOptionBusinessRules : BaseBusinessRules
{
    private readonly IWebOptionRepository _webOptionRepository;

    public WebOptionBusinessRules(IWebOptionRepository webOptionRepository)
    {
        _webOptionRepository = webOptionRepository;
    }

    public Task WebOptionShouldBeExistsWhenSelected(WebOption? entity)
    {
        if (entity == null)
            throw new BusinessException(WebOptionMessages.WebOptionDontExists);
        return Task.CompletedTask;
    }
    public Task WebOptionIsThere(WebOption? entity)
    {
        if (entity != null)
            throw new BusinessException(WebOptionMessages.WebOptionIsThere);
        return Task.CompletedTask;
    }
    public Task IsRun(WebOption? entity)
    {
        if (entity is not null)
            if (entity.Key.ToLower().Equals("run"))
                throw new BusinessException(WebOptionMessages.ValueCannotBeDeleted);
        return Task.CompletedTask;
    }
    public Task IsActiveExternalRegistration()
    {
        bool isActive = _webOptionRepository.Any(x => x.Key.Contains("ExternalRegistration") && x.Value.Contains("True"));
        if (!isActive)
            throw new BusinessException(WebOptionMessages.IsActiveExternalRegistration);

        return Task.CompletedTask;
    }

}
