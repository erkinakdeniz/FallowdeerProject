using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Entities;
public class Unicode:Entity<int>
{
    public string Code { get; set; }
    public string Email { get; set; }
    public DateTime ExpiredDate { get; set; }= DateTime.Now.AddMinutes(1);
}
