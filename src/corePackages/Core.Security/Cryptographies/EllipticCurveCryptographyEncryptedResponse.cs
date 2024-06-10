using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Cryptographies;
public class EllipticCurveCryptographyEncryptedResponse
{
    public string EncryptedValue { get; set; }
    public string IV { get; set; }
    public string Tag { get; set; }
    public string Key { get; set; }
}
