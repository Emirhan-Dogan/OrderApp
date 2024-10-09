
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Countries.Queries
{
    public class GetCountryQuery : IRequest<IDataResult<Country>>
    {
        public int Id { get; set; }

        public class GetCountryQueryHandler : IRequestHandler<GetCountryQuery, IDataResult<Country>>
        {
            private readonly ICountryRepository _countryRepository;
            private readonly IMediator _mediator;

            public GetCountryQueryHandler(ICountryRepository countryRepository, IMediator mediator)
            {
                _countryRepository = countryRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Country>> Handle(GetCountryQuery request, CancellationToken cancellationToken)
            {
                var country = await _countryRepository.GetAsync(p => p.Id == request.Id && p.isDeleted == false);
                return new SuccessDataResult<Country>(country);
            }
        }
    }
}
