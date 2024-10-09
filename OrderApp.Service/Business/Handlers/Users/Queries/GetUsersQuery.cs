using AutoMapper;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Users.Queries
{
    public class GetUsersQuery : IRequest<IDataResult<IEnumerable<UserDto>>>
    {
        public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IDataResult<IEnumerable<UserDto>>>
        {
            private readonly IUserRepository _userRepository;
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;

            public GetUsersQueryHandler(IUserRepository userRepository, ICustomerRepository customerRepository,IMapper mapper)
            {
                _userRepository = userRepository;
                _customerRepository = customerRepository;
                _mapper = mapper;
            }

            [SecuredOperation(Priority = 1)]
            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                var userList = await _userRepository.GetListAsync(p => p.isDeleted == false);
                var customers = await _customerRepository.GetListAsync(p => p.isDeleted == false);
                List<User> users = new List<User>();
                foreach (var user in userList)
                {
                    bool isOk = false;
                    foreach (var customer in customers)
                    {
                        if(user.UserId == customer.UserId)
                        {
                            isOk = true;
                        }
                    }
                    if (!isOk)
                    {
                        users.Add(user);
                    }
                }

                var userDtoList = users.Select(user => _mapper.Map<UserDto>(user)).ToList();
                return new SuccessDataResult<IEnumerable<UserDto>>(userDtoList);
            }
        }
    }
}