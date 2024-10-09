using AutoMapper;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Users.Queries
{
    public class GetUserQuery : IRequest<IDataResult<UserDto>>
    {
        public int UserId { get; set; }

        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, IDataResult<UserDto>>
        {
            private readonly IUserRepository _userRepository;
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;

            public GetUserQueryHandler(IUserRepository userRepository, ICustomerRepository customerRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _customerRepository = customerRepository;
                _mapper = mapper;
            }

            [SecuredOperation(Priority = 1)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetAsync(p => p.UserId == request.UserId && p.isDeleted == false);
                var customer = await _customerRepository.GetAsync(p => p.UserId == request.UserId);
                if (customer != null) {
                    if (customer.Id != 0)
                    {
                        return new ErrorDataResult<UserDto>();
                    }
                }
                
                var userDto = _mapper.Map<UserDto>(user);
                return new SuccessDataResult<UserDto>(userDto);
            }
        }
    }
}