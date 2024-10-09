
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
using ServiceStack;


namespace Business.Handlers.Storages.Queries
{
    public class GetStorageQuery : IRequest<IDataResult<Storage>>
    {
        public int Id { get; set; }

        public class GetStorageQueryHandler : IRequestHandler<GetStorageQuery, IDataResult<Storage>>
        {
            private readonly IStorageRepository _storageRepository;
            private readonly IMediator _mediator;

            public GetStorageQueryHandler(IStorageRepository storageRepository, IMediator mediator)
            {
                _storageRepository = storageRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Storage>> Handle(GetStorageQuery request, CancellationToken cancellationToken)
            {
                var storage = await _storageRepository.GetAsync(p => p.Id == request.Id && p.isDeleted == false,
                    include: q => q.Include(s => s.Address).ThenInclude(s => s.City)
                    .Include(s => s.Address).ThenInclude(s => s.Country));
                return new SuccessDataResult<Storage>(storage);
            }
        }
    }
}
