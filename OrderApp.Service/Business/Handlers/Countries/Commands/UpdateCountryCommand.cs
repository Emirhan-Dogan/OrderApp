
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Countries.ValidationRules;


namespace Business.Handlers.Countries.Commands
{


    public class UpdateCountryCommand : IRequest<IResult>
    {
        public int LastUpdatedUserId { get; set; }
        public bool Status { get; set; }
        public bool isDeleted { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, IResult>
        {
            private readonly ICountryRepository _countryRepository;
            private readonly IMediator _mediator;

            public UpdateCountryCommandHandler(ICountryRepository countryRepository, IMediator mediator)
            {
                _countryRepository = countryRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateCountryValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
            {
                var isThereCountryRecord = await _countryRepository.GetAsync(u => u.Id == request.Id);

                isThereCountryRecord.LastUpdatedUserId = request.LastUpdatedUserId;
                isThereCountryRecord.LastUpdatedDate = System.DateTime.Now;
                isThereCountryRecord.Status = request.Status;
                isThereCountryRecord.isDeleted = request.isDeleted;
                isThereCountryRecord.Name = request.Name;


                _countryRepository.Update(isThereCountryRecord);
                await _countryRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

