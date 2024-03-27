using models.AanwezigHeid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.Bezoeker
{
    public class GetBezoekerModel : BaseBezoekerModel
    {
        public Guid IdBezoeker { get; set; }
        public ICollection<Guid> IdAanwezigHeden { get; set; }
    }
}
