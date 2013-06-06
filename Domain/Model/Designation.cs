using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Designation
    {
        public virtual int Id { get; protected set; }
        public virtual string Title { get; set; }
        public virtual string Desc { get; set; }
        public virtual string Note { get; set; }
    }
}
