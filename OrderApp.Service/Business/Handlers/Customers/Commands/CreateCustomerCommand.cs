
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
using Business.Handlers.Customers.ValidationRules;
using Core.Entities.Concrete;
using System;

namespace Business.Handlers.Customers.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCustomerCommand : IRequest<IResult>
    {
        public int CreatedUserId { get; set; }
        public bool Status { get; set; }
        public string CustomerCode { get; set; }
        public Address Address { get; set; }

        public long CitizenId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobilePhones { get; set; }
        public DateTime BirthDate { get; set; }
        public int Gender { get; set; }
        public string Notes { get; set; }
        public string ImagePath { get; set; }

        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, IResult>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IMediator _mediator;
            public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMediator mediator)
            {
                _customerRepository = customerRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateCustomerValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var isThereCustomerRecord = _customerRepository.Query().Any(u => u.User.Email == request.Email);

                if (isThereCustomerRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedCustomer = new Customer
                {
                    CreatedDate = System.DateTime.Now,
                    LastUpdatedUserId = request.CreatedUserId,
                    LastUpdatedDate = System.DateTime.Now,
                    Status = request.Status,
                    isDeleted = false,
                    User = new User()
                    {
                        Email = request.Email,
                        CitizenId = request.CitizenId,
                        Gender = request.Gender,
                        FullName = request.FullName,
                        BirthDate = request.BirthDate,
                        MobilePhones = request.MobilePhones,
                        Notes = request.Notes,
                        ImagePath = request.ImagePath,
                        Status = request.Status,
                        isDeleted=false,
                        RecordDate = System.DateTime.Now,
                        UpdateContactDate = System.DateTime.Now,
                    },
                    CustomerCode = request.CustomerCode,
                    Address = request.Address,

                };

                _customerRepository.Add(addedCustomer);
                await _customerRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}