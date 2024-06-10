using Core.CrossCuttingConcerns.Extensions;
using Core.Security.JWT;
using MediatR;

namespace Application.Features.MainFeatures.Auth.Commands.PageGuard;
public class PageGuardCommand : IRequest<bool>
{
    public string Token { get; set; }
    public class PageGuardCommandHandler : IRequestHandler<PageGuardCommand,bool>
    {
        private readonly ITokenHelper _token;

        public PageGuardCommandHandler(ITokenHelper token)
        {
            _token = token;
        }

        public async Task<bool> Handle(PageGuardCommand request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.Token))
                return false;
            string token = "";
            string[] jwtToken = request.Token.Split(" ");
            if (jwtToken.Length > 1)
            {
                token = jwtToken[1];
            }
            else
            {
                token = request.Token;
            }
            try
            {
                var jwt = _token.TokenVerify(token);
                if (!jwt.Payload.TryGetValue(MyClaimTypes.ID, out var userId))
                    return false;
            }
            catch (Exception)
            {

                return false;
            }
           

            return true;
            
        }
    }
}
