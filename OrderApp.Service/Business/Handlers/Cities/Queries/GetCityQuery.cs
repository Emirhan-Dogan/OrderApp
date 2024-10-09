
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Microsoft.EntityFrameworkCore;


namespace Business.Handlers.Cities.Queries
{
    public class GetCityQuery : IRequest<IDataResult<City>>
    {
        public int Id { get; set; }

        public class GetCityQueryHandler : IRequestHandler<GetCityQuery, IDataResult<City>>
        {
            private readonly ICityRepository _cityRepository;
            private readonly IMediator _mediator;

            public GetCityQueryHandler(ICityRepository cityRepository, IMediator mediator)
            {
                _cityRepository = cityRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<City>> Handle(GetCityQuery request, CancellationToken cancellationToken)
            {
                var city = await _cityRepository.GetAsync(
                    p => p.Id == request.Id && p.isDeleted == false,
                    include: q=>q.Include(s=>s.Country)
                    );

                return new SuccessDataResult<City>(city);
            }
        }
    }
}
