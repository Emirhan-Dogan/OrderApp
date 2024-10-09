
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
using Business.Handlers.Products.ValidationRules;


namespace Business.Handlers.Products.Commands
{


    public class UpdateProductCommand : IRequest<IResult>
    {
       
        public int LastUpdatedUserId { get; set; }
        public bool Status { get; set; }
        public bool isDeleted { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public double Price { get; set; }
        public SizeCode SizeCode { get; set; }
        public int ColorId { get; set; }
        public int StockCount { get; set; }
        public int CategoryId { get; set; }
        public int StorageId { get; set; }

        //public System.Collections.Generic.ICollection<ProductOrder> ProductOrders { get; set; }

        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, IResult>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMediator _mediator;

            public UpdateProductCommandHandler(IProductRepository productRepository, IMediator mediator)
            {
                _productRepository = productRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateProductValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                var isThereProductRecord = await _productRepository.GetAsync(u => u.Id == request.Id);

                isThereProductRecord.LastUpdatedUserId = request.LastUpdatedUserId;
                isThereProductRecord.LastUpdatedDate = System.DateTime.Now;
                isThereProductRecord.Status = request.Status;
                isThereProductRecord.isDeleted = request.isDeleted;
                isThereProductRecord.Name = request.Name;
                isThereProductRecord.ProductCode = request.ProductCode;
                isThereProductRecord.Description = request.Description;
                isThereProductRecord.ImagePath = request.ImagePath;
                isThereProductRecord.Price = request.Price;
                isThereProductRecord.SizeCode = request.SizeCode;
                isThereProductRecord.ColorId = request.ColorId;
                isThereProductRecord.StockCount = request.StockCount;
                isThereProductRecord.CategoryId = request.CategoryId;
                isThereProductRecord.StorageId = request.StorageId;

                _productRepository.Update(isThereProductRecord);
                await _productRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

