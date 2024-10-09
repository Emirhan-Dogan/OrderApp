
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.Countries.Queries
{

    public class GetCountriesQuery : IRequest<IDataResult<IEnumerable<Country>>>
    {
        public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IDataResult<IEnumerable<Country>>>
        {
            private readonly ICountryRepository _countryRepository;
            private readonly IMediator _mediator;

            public GetCountriesQueryHandler(ICountryRepository countryRepository, IMediator mediator)
            {
                _countryRepository = countryRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Country>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Country>>(await _countryRepository.GetListAsync(p => p.isDeleted == false));
            }
        }
    }
}