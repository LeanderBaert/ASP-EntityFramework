using models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.Persoon
{
    public class GetPersoonModel : BasePersoonModel
    {
        public Guid IdPersoon { get; set; }
        public ICollection<Guid> IdAanwezigheiden { get; set; }
    }
}
