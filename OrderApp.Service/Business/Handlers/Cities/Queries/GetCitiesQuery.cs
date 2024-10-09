
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
using Microsoft.EntityFrameworkCore;

namespace Business.Handlers.Cities.Queries
{

    public class GetCitiesQuery : IRequest<IDataResult<IEnumerable<City>>>
    {
        public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, IDataResult<IEnumerable<City>>>
        {
            private readonly ICityRepository _cityRepository;
            private readonly IMediator _mediator;

            public GetCitiesQueryHandler(ICityRepository cityRepository, IMediator mediator)
            {
                _cityRepository = cityRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<City>>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<City>>(await _cityRepository.GetListAsync(
                    p => p.isDeleted == false,
                    include: q => q.Include(s => s.Country)));
            }
        }
    }
}