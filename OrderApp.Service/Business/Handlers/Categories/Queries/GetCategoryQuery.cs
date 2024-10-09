
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Categories.Queries
{
    public class GetCategoryQuery : IRequest<IDataResult<Category>>
    {
        public int Id { get; set; }

        public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, IDataResult<Category>>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMediator _mediator;

            public GetCategoryQueryHandler(ICategoryRepository categoryRepository, IMediator mediator)
            {
                _categoryRepository = categoryRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Category>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
            {
                var category = await _categoryRepository.GetAsync(p => p.Id == request.Id && p.isDeleted == false);
                return new SuccessDataResult<Category>(category);
            }
        }
    }
}
