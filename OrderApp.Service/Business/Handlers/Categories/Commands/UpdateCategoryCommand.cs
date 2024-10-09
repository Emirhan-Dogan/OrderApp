
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
using Business.Handlers.Categories.ValidationRules;


namespace Business.Handlers.Categories.Commands
{


    public class UpdateCategoryCommand : IRequest<IResult>
    {
        public int LastUpdatedUserId { get; set; }
        public bool Status { get; set; }
        public bool isDeleted { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, IResult>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMediator _mediator;

            public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMediator mediator)
            {
                _categoryRepository = categoryRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateCategoryValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                var isThereCategoryRecord = await _categoryRepository.GetAsync(u => u.Id == request.Id);

                isThereCategoryRecord.LastUpdatedUserId = request.LastUpdatedUserId;
                isThereCategoryRecord.LastUpdatedDate = System.DateTime.Now;
                isThereCategoryRecord.Status = request.Status;
                isThereCategoryRecord.isDeleted = request.isDeleted;
                isThereCategoryRecord.Name = request.Name;


                _categoryRepository.Update(isThereCategoryRecord);
                await _categoryRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

