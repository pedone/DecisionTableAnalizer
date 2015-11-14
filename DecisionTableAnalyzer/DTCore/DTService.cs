using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCore
{
    public abstract class DTService
    {

        protected IEntityService EntityService
        {
            get { return DTCore.EntityService.Instance; }
        }

        public abstract object ExecuteOperation(string operationId, params object[] args);

    }
}
