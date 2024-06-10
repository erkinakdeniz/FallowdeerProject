using Core.Application.Responses;

namespace Application.Features.MainFeatures.WebOptions.Queries.WebOptions;
public class GetWebOptionsResponse : IResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Alias { get; set; }
    public string InputType { get; set; }
    public string Params { get; set; }
}
