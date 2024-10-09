using Business.Constants;
using Business.Services.Authentication;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Mail;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Authorizations.Queries
{
    public class LoginUserQuery : IRequest<IDataResult<AccessToken>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, IDataResult<AccessToken>>
        {
            private readonly IUserRepository _userRepository;
            private readonly IGroupRepository _groupRepository;
            private readonly IUserGroupRepository _userGroupRepository;
            private readonly ITokenHelper _tokenHelper;
            private readonly IMediator _mediator;
            private readonly ICacheManager _cacheManager;

            private readonly IMailService _mailService;


            public LoginUserQueryHandler(IUserRepository userRepository, ITokenHelper tokenHelper, IMediator mediator, ICacheManager cacheManager, IGroupRepository groupRepository, IUserGroupRepository userGroupRepository, IMailService mailService)
            {
                _userRepository = userRepository;
                _tokenHelper = tokenHelper;
                _mediator = mediator;
                _cacheManager = cacheManager;
                _groupRepository = groupRepository;
                _userGroupRepository = userGroupRepository;
                _mailService = mailService;
            }


            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<AccessToken>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetAsync(u => u.Email == request.Email && u.Status);

                if (user == null)
                {
                    return new ErrorDataResult<AccessToken>(Messages.UserNotFound);
                }

                if (!HashingHelper.VerifyPasswordHash(request.Password, user.PasswordSalt, user.PasswordHash))
                {
                    return new ErrorDataResult<AccessToken>(Messages.PasswordError);
                }

                var claims = _userRepository.GetClaims(user.UserId);

                var accessToken = _tokenHelper.CreateToken<DArchToken>(user);
                accessToken.Claims = claims.Select(x => x.Name).ToList();

                user.RefreshToken = accessToken.RefreshToken;
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

                _cacheManager.Add($"{CacheKeys.UserIdForClaim}={user.UserId}", claims.Select(x => x.Name));

                // Eklendi
                accessToken.User = user;
                var userGroups = _userGroupRepository.GetList(o => o.UserId == user.UserId).ToList();
                var groups = _groupRepository.GetList().ToList();
                List<Group> usrgroups = new List<Group>();
                foreach (var userGroup in userGroups)
                {
                    foreach (var group in groups)
                    {
                        if (userGroup.GroupId == group.Id)
                        {
                            usrgroups.Add(group);
                        }
                    }
                }
                accessToken.Groups = usrgroups;
                return new SuccessDataResult<AccessToken>(accessToken, Messages.SuccessfulLogin);
            }
        }
    }
}