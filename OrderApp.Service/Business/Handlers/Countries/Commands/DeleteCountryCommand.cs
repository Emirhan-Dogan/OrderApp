
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


namespace Business.Handlers.Countries.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteCountryCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, IResult>
        {
            private readonly ICountryRepository _countryRepository;
            private readonly IMediator _mediator;

            public DeleteCountryCommandHandler(ICountryRepository countryRepository, IMediator mediator)
            {
                _countryRepository = countryRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
            {
                var countryToDelete = _countryRepository.Get(p => p.Id == request.Id);
                countryToDelete.isDeleted = true;

                _countryRepository.Update(countryToDelete);
                await _countryRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

