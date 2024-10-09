
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
using Business.Handlers.Products.ValidationRules;

namespace Business.Handlers.Products.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateProductCommand : IRequest<IResult>
    {
        public int CreatedUserId { get; set; }
        public bool Status { get; set; }
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


        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, IResult>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMediator _mediator;
            public CreateProductCommandHandler(IProductRepository productRepository, IMediator mediator)
            {
                _productRepository = productRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateProductValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                //var isThereProductRecord = _productRepository.Query().Any(u => u.CreatedDate == request.);

                //if (isThereProductRecord == true)
                //    return new ErrorResult(Messages.NameAlreadyExist);

                var addedProduct = new Product
                {
                    CreatedUserId=request.CreatedUserId,
                    CreatedDate = System.DateTime.Now,
                    LastUpdatedUserId = request.CreatedUserId,
                    LastUpdatedDate = System.DateTime.Now,
                    Status = request.Status,
                    isDeleted = false,
                    Name = request.Name,
                    ProductCode = request.ProductCode,
                    Description = request.Description,
                    ImagePath = request.ImagePath,
                    Price = request.Price,
                    SizeCode = request.SizeCode,
                    ColorId = request.ColorId,
                    StockCount = request.StockCount,
                    CategoryId = request.CategoryId,
                    StorageId = request.StorageId
                };

                _productRepository.Add(addedProduct);
                await _productRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}