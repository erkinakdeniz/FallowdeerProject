using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.MainFeatures.WebOptions.Queries.SliderCategory;
public class GetSCategoriesQuery : IRequest<List<SCategoryResponse>>
{
    public class GetSCategoriesQueryHandler : IRequestHandler<GetSCategoriesQuery, List<SCategoryResponse>>
    {
        private readonly ISCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetSCategoriesQueryHandler(ISCategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<SCategoryResponse>> Handle(GetSCategoriesQuery request, CancellationToken cancellationToken)
        {
            var sCategories = _categoryRepository.Query().ToList();
            var sCategoryResponses = new List<SCategoryResponse>();
            if (sCategories is not null)
                sCategoryResponses = _mapper.Map<List<SCategoryResponse>>(sCategories);
            return sCategoryResponses;
        }
    }
}
