using Application.Features.MainFeatures.WebOptions.Constants;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Entities;

namespace Application.Features.MainFeatures.WebOptions.Rules;
public class SliderBusinessRules : BaseBusinessRules
{
    public Task WebOptionShouldBeExistsWhenSelected(Slider? entity)
    {
        if (entity == null)
            throw new BusinessException(SliderMessages.SliderDontExists);
        return Task.CompletedTask;
    }
    public Task WebOptionIsThere(Slider? entity)
    {
        if (entity != null)
            throw new BusinessException(SliderMessages.SliderIsThere);
        return Task.CompletedTask;
    }
}
