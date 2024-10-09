
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
using Business.Handlers.Orders.ValidationRules;
using Core.Aspects.Autofac.Transaction;
using Entities.Dtos;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Business.Handlers.Orders.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateOrderCommand : IRequest<IResult>
    {
        public int CreatedUserId { get; set; }
        public bool Status { get; set; }
        public int CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public System.DateTime DateOfReceipt { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public string DeliveryNotes { get; set; }
        public System.Collections.Generic.ICollection<ProductOrder> ProductOrders { get; set; }


        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, IResult>
        {
            private readonly MsDbContext _context;
            private readonly IMediator _mediator;
            private readonly JsonSerializerSettings _serializerSettings;

            public CreateOrderCommandHandler(MsDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;

                _serializerSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
            }
            //[TransactionScopeAspect]
            [ValidationAspect(typeof(CreateOrderValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                // Stok kontrolü için hata listesi
                var errors = new List<string>();

                foreach (var productOrder in request.ProductOrders)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Id == productOrder.ProductId, cancellationToken);

                    if (product == null)
                    {
                        errors.Add($"Product with ID {productOrder.ProductId} does not exist.");
                        continue;
                    }

                    if (product.StockCount < productOrder.Piece)
                    {
                        errors.Add($"Insufficient stock for product with ID {productOrder.ProductId}. Available: {product.StockCount}, Requested: {productOrder.Piece}");
                    }
                }

                if (errors.Any())
                {
                    return new ErrorDataResult<List<string>>(errors, "Stock validation failed.");
                }

                var addedOrder = new Order
                {
                    CreatedUserId = request.CreatedUserId,
                    CreatedDate = DateTime.Now,
                    LastUpdatedUserId = request.CreatedUserId,
                    LastUpdatedDate = DateTime.Now,
                    Status = request.Status,
                    isDeleted = false,
                    CustomerId = request.CustomerId,
                    TotalPrice = request.TotalPrice,
                    DateOfReceipt = request.DateOfReceipt,
                    DeliveryStatus = request.DeliveryStatus,
                    DeliveryNotes = request.DeliveryNotes,
                    ProductOrders = request.ProductOrders
                };

                _context.Orders.Add(addedOrder);
                await _context.SaveChangesAsync(cancellationToken);

                foreach (var productOrder in request.ProductOrders)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Id == productOrder.ProductId, cancellationToken);

                    if (product != null)
                    {
                        product.StockCount -= productOrder.Piece;
                        _context.Products.Update(product);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);

                var serializedOrder = JsonConvert.SerializeObject(addedOrder, _serializerSettings);

                return new SuccessResult(Messages.Added);
            }

        }

    }
}