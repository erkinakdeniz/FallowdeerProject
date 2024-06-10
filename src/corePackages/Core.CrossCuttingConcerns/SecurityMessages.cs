using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns;
public static class SecurityMessages
{
    public const string AuthorizationException = "Erişim izniniz yok!";
    public const string AuthenticatedException = "Kimliğiniz Doğrulanmadı!";
}
