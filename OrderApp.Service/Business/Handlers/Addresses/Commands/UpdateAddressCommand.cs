
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
using Business.Handlers.Addresses.ValidationRules;


namespace Business.Handlers.Addresses.Commands
{


    public class UpdateAddressCommand : IRequest<IResult>
    {
        public int LastUpdatedUserId { get; set; }
        public bool Status { get; set; }
        public bool isDeleted { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public string AddressDetail { get; set; }
        public System.DateTime DateOfReceipt { get; set; }

        public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, IResult>
        {
            private readonly IAddressRepository _addressRepository;
            private readonly IMediator _mediator;

            public UpdateAddressCommandHandler(IAddressRepository addressRepository, IMediator mediator)
            {
                _addressRepository = addressRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateAddressValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
            {
                var isThereAddressRecord = await _addressRepository.GetAsync(u => u.Id == request.Id);

                isThereAddressRecord.LastUpdatedUserId = request.LastUpdatedUserId;
                isThereAddressRecord.LastUpdatedDate = System.DateTime.Now;
                isThereAddressRecord.Status = request.Status;
                isThereAddressRecord.isDeleted = request.isDeleted;
                isThereAddressRecord.Name = request.Name;
                isThereAddressRecord.CityId = request.CityId;
                isThereAddressRecord.CountryId = request.CountryId;
                isThereAddressRecord.AddressDetail = request.AddressDetail;
                isThereAddressRecord.DateOfReceipt = request.DateOfReceipt;


                _addressRepository.Update(isThereAddressRecord);
                await _addressRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

