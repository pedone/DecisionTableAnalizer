using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;
using DTEnums;

namespace ViewDatas
{
    [EntityType(typeof(DecisionTableManager))]
    public class DecisionTableManagerViewData : ViewData<DecisionTableManager>
    {

        public List<DecisionTableInfoViewData> DecisionTables { get; set; }

        protected override void CopyFromEntity(DecisionTableManager entity)
        {
            DecisionTables = CopyViewDatasFromEntities<DecisionTable, DecisionTableInfoViewData>(entity.DecisionTables);
        }

        protected override void CopyToEntity(DecisionTableManager entity)
        {
        }

    }
}
