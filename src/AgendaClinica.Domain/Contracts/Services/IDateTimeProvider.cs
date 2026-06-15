using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Domain.Contracts.Services
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
