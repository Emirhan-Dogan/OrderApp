﻿
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
using Business.Handlers.Storages.ValidationRules;
using Microsoft.EntityFrameworkCore;

namespace Business.Handlers.Storages.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateStorageCommand : IRequest<IResult>
    {
        public int CreatedUserId { get; set; }
        public bool Status { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }

        public class CreateStorageCommandHandler : IRequestHandler<CreateStorageCommand, IResult>
        {
            private readonly IStorageRepository _storageRepository;
            private readonly IMediator _mediator;
            public CreateStorageCommandHandler(IStorageRepository storageRepository, IMediator mediator)
            {
                _storageRepository = storageRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateStorageValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateStorageCommand request, CancellationToken cancellationToken)
            {
                //var isThereStorageRecord = _storageRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                //if (isThereStorageRecord == true)
                //    return new ErrorResult(Messages.NameAlreadyExist);

                var addedStorage = new Storage
                {
                    CreatedDate = System.DateTime.Now,
                    CreatedUserId = request.CreatedUserId,
                    LastUpdatedUserId = request.CreatedUserId,
                    LastUpdatedDate = System.DateTime.Now,
                    Status = request.Status,
                    isDeleted = false,
                    Name = request.Name,
                    Address = request.Address,

                };

                _storageRepository.Add(addedStorage, q=>q.Include(s=>s.Address));
                await _storageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}