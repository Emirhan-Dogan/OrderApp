
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Cities.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteCityCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, IResult>
        {
            private readonly ICityRepository _cityRepository;
            private readonly IMediator _mediator;

            public DeleteCityCommandHandler(ICityRepository cityRepository, IMediator mediator)
            {
                _cityRepository = cityRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
            {
                var cityToDelete = _cityRepository.Get(p => p.Id == request.Id);
                cityToDelete.isDeleted = true;

                _cityRepository.Update(cityToDelete);
                await _cityRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

