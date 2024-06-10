using Core.Security.Entities;

namespace Application.Services.MainServices.WebOptionService;
public interface IWebOptionService : IBaseService<WebOption>
{
    List<WebOption> GetAll();
    void CreateJson();
    WebOption Update(WebOption entity);
}
