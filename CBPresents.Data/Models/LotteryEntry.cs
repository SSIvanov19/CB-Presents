using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBPresents.Data.Models;

public class LotteryEntry
{
    public string? Id { get; set; }
    
    public string? Email { get; set; }
    
    public bool? IsWinner { get; set; }
}
