using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.GoogleAuthenticator;
public class AuthenticatorOptions
{
   public GoogleAuthenticator GoogleAuthenticator { get; set; }
   public string AppName => "Kodkop";
   public string Description => "Kodkop Teknoloji A.S";
}
public class GoogleAuthenticator
{
    public string Key { get; set; }
}
