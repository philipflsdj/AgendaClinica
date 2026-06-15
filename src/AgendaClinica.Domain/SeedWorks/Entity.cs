using AgendaClinica.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Domain.SeedWorks
{
    public class Entity : Notifiable, IEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual object? GetLogContent()
        {
            return this;
        }

        public virtual string GetOwner()
        {
            return CreatedBy ?? string.Empty;
        }
    }
}
