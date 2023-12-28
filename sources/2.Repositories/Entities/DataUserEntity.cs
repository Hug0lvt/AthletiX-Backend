using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class DataUserEntity
    {
        public List<int> SessionsIds { get; set; } = new List<int>();

        public DataUserEntity(List<int> sessionsIds)
        {
            SessionsIds = sessionsIds;
        }
    }
}
