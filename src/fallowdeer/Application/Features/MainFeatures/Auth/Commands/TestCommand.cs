using Core.Security.Cryptographies;
using MediatR;

namespace Application.Features.MainFeatures.Auth.Commands;
public class TestCommand:IRequest<string>
{
    public class TestCommandHandler : IRequestHandler<TestCommand,string>
    {
        private readonly IECCCryptography _eCCCryptography;

        public TestCommandHandler(IECCCryptography eCCCryptography)
        {
            _eCCCryptography = eCCCryptography;
        }

        public async Task<string> Handle(TestCommand request, CancellationToken cancellationToken)
        {
           var enc= _eCCCryptography.Encryption(DateTime.Now.ToLongDateString());
            var dec = _eCCCryptography.Decryption(enc.EncryptedValue,enc.IV,enc.Tag,enc.Key);
            //var dec = _eCCCryptography.Decryption("4TrghnazaaFE53uLlY3LMEZ33Q==", "CLb17jlvVGAFeO0j", "V+kWE2Vx9w7zAArpRHyXtQ==", "VvUH9icVirklHuCHf4vgR3hwIk3gKYI+00bFzQvFZvI=");
            return dec.Value;
        }
    }
}
