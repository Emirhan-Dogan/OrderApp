
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Countries.ValidationRules;

namespace Business.Handlers.Countries.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCountryCommand : IRequest<IResult>
    {
        public int CreatedUserId { get; set; }
        public bool Status { get; set; }
        public string Name { get; set; }


        public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, IResult>
        {
            private readonly ICountryRepository _countryRepository;
            private readonly IMediator _mediator;
            public CreateCountryCommandHandler(ICountryRepository countryRepository, IMediator mediator)
            {
                _countryRepository = countryRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateCountryValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
            {
                var isThereCountryRecord = _countryRepository.Query().Any(u => u.Name == request.Name);

                if (isThereCountryRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedCountry = new Country
                {
                    CreatedDate = System.DateTime.Now,
                    LastUpdatedUserId = request.CreatedUserId,
                    LastUpdatedDate = System.DateTime.Now,
                    Status = request.Status,
                    isDeleted = false,
                    Name = request.Name,
                    CreatedUserId = request.CreatedUserId,
                };

                _countryRepository.Add(addedCountry);
                await _countryRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}