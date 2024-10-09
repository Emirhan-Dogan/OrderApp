
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


namespace Business.Handlers.Addresses.Queries
{
    public class GetAddressQuery : IRequest<IDataResult<Address>>
    {
        public int Id { get; set; }

        public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, IDataResult<Address>>
        {
            private readonly IAddressRepository _addressRepository;
            private readonly IMediator _mediator;

            public GetAddressQueryHandler(IAddressRepository addressRepository, IMediator mediator)
            {
                _addressRepository = addressRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Address>> Handle(GetAddressQuery request, CancellationToken cancellationToken)
            {
                var address = await _addressRepository.GetAsync(
                    p => p.Id == request.Id && p.isDeleted == false,
                    include: q => q.Include(s => s.City).Include(s => s.Country)
                    );
                return new SuccessDataResult<Address>(address);
            }
        }
    }
}
