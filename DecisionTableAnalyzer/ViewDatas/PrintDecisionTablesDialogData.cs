using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;

namespace ViewDatas
{
    [EntityType(typeof(DecisionTableManager))]
    public class PrintDecisionTablesDialogData : ViewData<DecisionTableManager>
    {

        public List<SystemDecisionTableViewData> DecisionTables { get; set; }

        protected override void CopyFromEntity(DecisionTableManager entity)
        {
            DecisionTables = CopyViewDatasFromEntities<DecisionTable, SystemDecisionTableViewData>(entity.DecisionTables);
        }

        protected override void CopyToEntity(DecisionTableManager entity)
        {
        }
    }
}
