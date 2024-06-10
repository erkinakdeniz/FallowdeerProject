using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.GoogleAuthenticator;
public interface IGoogleAuthenticatorService
{
    GoogleAuthenticatorDto AuthenticateCreate();
    GoogleAuthenticatorDto AuthenticateCreate(string secretKey);
    bool Verify(string pin);
    bool Verify(string secretKey, string pin);
}
