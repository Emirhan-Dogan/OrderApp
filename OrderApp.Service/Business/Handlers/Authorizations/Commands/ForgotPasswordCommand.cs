using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Mail;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Toolkit;
using DataAccess.Abstract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Authorizations.Commands
{
    public class ForgotPasswordCommand : IRequest<IResult>
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, IResult>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMailService _mailService;

            public ForgotPasswordCommandHandler(IUserRepository userRepository, IMailService mailService)
            {
                _userRepository = userRepository;
                _mailService = mailService;
            }

            /// <summary>
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            //[SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetAsync(u => u.Email == request.Email && u.FullName == request.FullName);

                if (user == null)
                {
                    return new ErrorResult(Messages.WrongCitizenId);
                }

                var generatedPassword = RandomPassword.CreateRandomPassword(14);
                HashingHelper.CreatePasswordHash(generatedPassword, out var passwordSalt, out var passwordHash);
                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHash;

                _userRepository.Update(user);
                _userRepository.SaveChanges();
                var emailMessage = new EmailMessage
                {
                    Subject = "Şifre Sıfırlama",
                    //Content = "yeni şifreniz " + generatedPassword,
                    Content = @"
                        <!DOCTYPE html>
                        <html lang='tr'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title>Şifre Sıfırlama</title>
                            <style>
                                body {
                                    font-family: Arial, sans-serif;
                                    background-color: #f4f4f4;
                                    margin: 0;
                                    padding: 0;
                                }
                                .container {
                                    width: 100%;
                                    max-width: 600px;
                                    margin: auto;
                                    background-color: #ffffff;
                                    border-radius: 8px;
                                    overflow: hidden;
                                    box-shadow: 0 0 10px rgba(0,0,0,0.1);
                                    border: 1px solid #e1e1e1;
                                }
                                .header {
                                    background-color: #007bff;
                                    padding: 20px;
                                    text-align: center;
                                }
                                .header img {
                                    max-width: 200px;
                                    height: auto;
                                }
                                .content {
                                    padding: 20px;
                                    text-align: center;
                                }
                                .content h2 {
                                    color: #333333;
                                }
                                .content p {
                                    color: #555555;
                                    line-height: 1.5;
                                }
                                .footer {
                                    background-color: #f1f1f1;
                                    padding: 10px;
                                    text-align: center;
                                    font-size: 14px;
                                    color: #888888;
                                }
                                .btn {
                                    display: inline-block;
                                    padding: 10px 20px;
                                    font-size: 16px;
                                    color: #ffffff;
                                    background-color: #007bff;
                                    text-decoration: none;
                                    border-radius: 5px;
                                }
                                .btn:hover {
                                    background-color: #0056b3;
                                }
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <div class='header'>
                                    <img src='https://docs.kariyer.net/job/jobtemplate/000/000/238/avatar/23828120220428114616685.jpeg' alt='Şirket Logosu'>
                                </div>
                                <div class='content'>
                                    <h2>Yeni Şifreniz</h2>
                                    <p>Merhaba,</p>
                                    <p>Hesabınız için yeni şifreniz: <strong>" + generatedPassword + @"</strong></p>
                                    <p>Şifrenizi güvenli bir şekilde saklayın ve başkalarıyla paylaşmayın.</p>
                                    <p>Eğer bu isteği siz yapmadıysanız, lütfen destek ekibimizle iletişime geçin.</p>
                                </div>
                                <div class='footer'>
                                    <p>© 2024 Şirket Adı. Tüm hakları saklıdır.</p>
                                </div>
                            </div>
                        </body>
                        </html>
                            ",

                    ToAddresses = new List<EmailAddress>
                        {
                            new EmailAddress { Name = user.FullName, Address = user.Email }
                        },
                    FromAddresses = new List<EmailAddress>
                        {
                            new EmailAddress { Name = "Emirhan Doğan", Address = "emirhan.dogan16@outlook.com" }
                        },
                };
                _mailService.Send(emailMessage);

                return new SuccessResult("Mailinizi kontrol ediniz..");
            }
        }
    }
}