
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
using Business.Handlers.Cities.ValidationRules;

namespace Business.Handlers.Cities.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCityCommand : IRequest<IResult>
    {
        public int CreatedUserId { get; set; }
        public bool Status { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }


        public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, IResult>
        {
            private readonly ICityRepository _cityRepository;
            private readonly IMediator _mediator;
            public CreateCityCommandHandler(ICityRepository cityRepository, IMediator mediator)
            {
                _cityRepository = cityRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateCityValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateCityCommand request, CancellationToken cancellationToken)
            {
                var isThereCityRecord = _cityRepository.Query().Any(u => u.Name == request.Name && u.CountryId == request.CountryId);

                if (isThereCityRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedCity = new City
                {
                    CreatedDate = System.DateTime.Now,
                    LastUpdatedUserId = request.CreatedUserId,
                    LastUpdatedDate = System.DateTime.Now,
                    CreatedUserId = request.CreatedUserId,
                    Status = request.Status,
                    isDeleted = false,
                    Name = request.Name,
                    CountryId = request.CountryId,

                };

                _cityRepository.Add(addedCity);
                await _cityRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}