using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Entities;
public class WebOption:Entity<int>
{
    
    public WebOption()
    {
        
    }

    public WebOption(string key, string value, string alias, string ınputType, string? @params)
    {
        Key = key;
        Value = value;
        Alias = alias;
        InputType = ınputType;
        Params = @params;
    }

    public string Key { get; set; }
    public string Value { get; set; }
    public string Alias { get; set; }
    public string InputType { get; set; }
    public string? Params { get; set; }
}
public class ImageParams
{
    public int Width { get; set; }
    public int Height { get; set; }
}
