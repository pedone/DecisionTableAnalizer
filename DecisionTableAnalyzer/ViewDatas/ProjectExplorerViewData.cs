using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;

namespace ViewDatas
{
    [EntityType(typeof(DTProject))]
    public class ProjectExplorerViewData : ViewData<DTProject>
    {

        public SystemProjectViewData Project { get; set; }
        public SystemRequirementManagerViewData RequirementManager { get; set; }
        public SystemDecisionTableManagerViewData DecisionTableManager { get; set; }

        protected override void CopyFromEntity(DTProject entity)
        {
            Project = CopyViewDataFromEntity<DTProject, SystemProjectViewData>(entity);
            RequirementManager = CopyViewDataFromEntity<RequirementManager, SystemRequirementManagerViewData>(entity.RequirementManager);
            DecisionTableManager = CopyViewDataFromEntity<DecisionTableManager, SystemDecisionTableManagerViewData>(entity.DecisionTableManager);
        }

        protected override void CopyToEntity(DTProject entity)
        {
        }
    }
}
