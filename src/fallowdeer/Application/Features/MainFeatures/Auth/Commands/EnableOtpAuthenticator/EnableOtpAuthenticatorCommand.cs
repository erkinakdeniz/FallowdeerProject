using Application.Features.MainFeatures.Auth.Rules;
using Application.Services.MainServices.AuthenticatorService;
using Application.Services.MainServices.UsersService;
using Application.Services.Repositories;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Transaction;
using Core.Security.Entities;
using MediatR;
using QRCoder;
using static QRCoder.PayloadGenerator;

namespace Application.Features.MainFeatures.Auth.Commands.EnableOtpAuthenticator;

public class EnableOtpAuthenticatorCommand : IRequest, ISecuredAndUserId, ITransactionalRequest
{
    public Guid UserId { get; set; }

    public class EnableOtpAuthenticatorCommandHandler : IRequestHandler<EnableOtpAuthenticatorCommand>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IOtpAuthenticatorRepository _otpAuthenticatorRepository;
        private readonly IUserService _userService;

        public EnableOtpAuthenticatorCommandHandler(
            IUserService userService,
            IOtpAuthenticatorRepository otpAuthenticatorRepository,
            AuthBusinessRules authBusinessRules,
            IAuthenticatorService authenticatorService
        )
        {
            _userService = userService;
            _otpAuthenticatorRepository = otpAuthenticatorRepository;
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
        }

        public async Task Handle(
            EnableOtpAuthenticatorCommand request,
            CancellationToken cancellationToken
        )
        {
            User? user = await _userService.GetAsync(predicate: u => u.Id == request.UserId, cancellationToken: cancellationToken);
            await _authBusinessRules.UserShouldBeExistsWhenSelected(user);
            await _authBusinessRules.UserShouldNotBeHaveOtpAuthenticator(user!);

            OtpAuthenticator? doesExistOtpAuthenticator = await _otpAuthenticatorRepository.GetAsync(
                predicate: o => o.UserId == request.UserId,
                cancellationToken: cancellationToken
            );
            await _authBusinessRules.OtpAuthenticatorThatVerifiedShouldNotBeExists(doesExistOtpAuthenticator);
            if (doesExistOtpAuthenticator is not null)
                await _otpAuthenticatorRepository.DeleteAsync(doesExistOtpAuthenticator);

            OtpAuthenticator newOtpAuthenticator = await _authenticatorService.CreateOtpAuthenticator(user!);
            var qrGenerator = new QRCodeGenerator();
            var oneTimePassword = new OneTimePassword();
            oneTimePassword.Secret = await _authenticatorService.ConvertSecretKeyToString(newOtpAuthenticator.SecretKey);
            oneTimePassword.Issuer = "Kodkop.com";
            oneTimePassword.Label = user.Email;
            oneTimePassword.Type = OneTimePassword.OneTimePasswordAuthType.TOTP;

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(oneTimePassword, QRCodeGenerator.ECCLevel.Q);
            var pngByteQRCode = new PngByteQRCode(qrCodeData);
            var qrcode = pngByteQRCode.GetGraphic(10);
            newOtpAuthenticator.ImageBase64 = "data:image/png;base64," + Convert.ToBase64String(qrcode);
            OtpAuthenticator addedOtpAuthenticator = await _otpAuthenticatorRepository.AddAsync(newOtpAuthenticator);


            //user.AuthenticatorTypes.Add(Core.Security.Enums.AuthenticatorType.Otp);
            //await _userService.UpdateAsync(user);




            // EnabledOtpAuthenticatorResponse enabledOtpAuthenticatorDto =
            //new() { SecretKey = await _authenticatorService.ConvertSecretKeyToString(addedOtpAuthenticator.SecretKey), ImageBase64 = "data:image/png;base64," + Convert.ToBase64String(qrcode) };


            // return enabledOtpAuthenticatorDto;
        }
    }
}
