using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.GoogleAuthenticator;
public class GoogleAuthenticatorDto
{
    public string Base64Image { get; set; }
    public string ManualEntryKey { get; set;}
}
