using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBPresents.Shared.Models;

public class User
{
    public string Email { get; set; }
    
    public string Name { get; set; }
    
    public bool IsWinner { get; set; }
}
