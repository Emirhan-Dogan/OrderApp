
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
using Business.Handlers.ProductOrders.ValidationRules;


namespace Business.Handlers.ProductOrders.Commands
{


    public class UpdateProductOrderCommand : IRequest<IResult>
    {
        public int LastUpdatedUserId { get; set; }
        public bool Status { get; set; }
        public bool isDeleted { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Piece { get; set; }
        public double UnitInPrice { get; set; }

        public class UpdateProductOrderCommandHandler : IRequestHandler<UpdateProductOrderCommand, IResult>
        {
            private readonly IProductOrderRepository _productOrderRepository;
            private readonly IMediator _mediator;

            public UpdateProductOrderCommandHandler(IProductOrderRepository productOrderRepository, IMediator mediator)
            {
                _productOrderRepository = productOrderRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateProductOrderValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateProductOrderCommand request, CancellationToken cancellationToken)
            {
                var isThereProductOrderRecord = await _productOrderRepository.GetAsync(u => u.Id == request.Id);

                isThereProductOrderRecord.LastUpdatedUserId = request.LastUpdatedUserId;
                isThereProductOrderRecord.LastUpdatedDate = System.DateTime.Now;
                isThereProductOrderRecord.Status = request.Status;
                isThereProductOrderRecord.isDeleted = request.isDeleted;
                isThereProductOrderRecord.ProductId = request.ProductId;
                isThereProductOrderRecord.OrderId = request.OrderId;
                isThereProductOrderRecord.Piece = request.Piece;
                isThereProductOrderRecord.UnitInPrice = request.UnitInPrice;


                _productOrderRepository.Update(isThereProductOrderRecord);
                await _productOrderRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

