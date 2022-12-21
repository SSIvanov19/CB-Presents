using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBPresents.Services.Contracts;

public interface ITimeService
{
    Task SetTime(string time);
        
    Task SetExplicitTime(DateTime time);

    Task<DateTime?> GetTime();
}
