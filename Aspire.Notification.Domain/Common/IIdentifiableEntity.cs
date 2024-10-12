using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Domain.Common
{
    public interface IIdentifiableEntity<TIdentity>
    {
        TIdentity Id { get; set; }
    }
}
