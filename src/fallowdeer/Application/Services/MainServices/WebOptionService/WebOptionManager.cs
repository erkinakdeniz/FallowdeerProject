using Application.Features.MainFeatures.WebOptions.Rules;
using Application.Services.Repositories;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Text;

namespace Application.Services.MainServices.WebOptionService;
public class WebOptionManager : IWebOptionService
{
    private readonly IWebOptionRepository _webOptionRepository;
    private readonly WebOptionBusinessRules _webOptionBusinessRules;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public WebOptionManager(IWebOptionRepository webOptionRepository, WebOptionBusinessRules webOptionBusinessRules, IWebHostEnvironment webHostEnvironment)
    {
        _webOptionRepository = webOptionRepository;
        _webOptionBusinessRules = webOptionBusinessRules;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<WebOption> AddAsync(WebOption entity)
    {
        await _webOptionBusinessRules.WebOptionShouldBeExistsWhenSelected(entity);
        WebOption webOption = await _webOptionRepository.AddAsync(entity);
        return webOption;
    }

    public void CreateJson()
    {

        var webOptions = _webOptionRepository.Query().ToList();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("{");
        for (int i = 0; i < webOptions.Count; i++)
        {
            if (i < webOptions.Count - 1)
            {
                stringBuilder.Append($"\"{webOptions[i].Key}\":\"{webOptions[i].Value ?? ""}\"");
                stringBuilder.Append(",");
            }
            else
            {
                stringBuilder.Append($"\"{webOptions[i].Key}\":\"{webOptions[i].Value ?? ""}\"");
            }

        }

        stringBuilder.Append("}");
        var myStringData = stringBuilder.ToString();
        string fileName = Path.Combine(_webHostEnvironment.WebRootPath, "wo.json");
        File.WriteAllText(fileName, myStringData);
    }

    public async Task<WebOption> DeleteAsync(WebOption entity, bool permanent = false)
    {
        WebOption webOption = await _webOptionRepository.DeleteAsync(entity, permanent);
        return webOption;
    }

    public List<WebOption> GetAll()
    {
        return _webOptionRepository.Query().Where(x => x.Key.ToLower() != "run").ToList();
    }

    public async Task<WebOption?> GetAsync(Expression<Func<WebOption, bool>> predicate, Func<IQueryable<WebOption>, IIncludableQueryable<WebOption, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        //TODO bu kod incelenecek
        WebOption? webOption = await _webOptionRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return webOption;
    }
    public async Task<IPaginate<WebOption>?> GetListAsync(Expression<Func<WebOption, bool>>? predicate = null, Func<IQueryable<WebOption>, IOrderedQueryable<WebOption>>? orderBy = null, Func<IQueryable<WebOption>, IIncludableQueryable<WebOption, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IPaginate<WebOption> paginate = await _webOptionRepository.GetListAsync(
           predicate,
           orderBy,
           include,
           index,
           size,
           withDeleted,
           enableTracking,
           cancellationToken
       );
        return paginate;
    }
    public async Task<WebOption> UpdateAsync(WebOption entity)
    {
        WebOption webOption = await _webOptionRepository.UpdateAsync(entity);
        CreateJson();
        return webOption;
    }
    public WebOption Update(WebOption entity)
    {
        WebOption webOption = _webOptionRepository.Update(entity);
        CreateJson();
        return webOption;
    }
}
