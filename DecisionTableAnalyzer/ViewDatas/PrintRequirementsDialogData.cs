using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;

namespace ViewDatas
{
    [EntityType(typeof(RequirementManager))]
    public class PrintRequirementsDialogData : ViewData<RequirementManager>
    {

        public List<RequirementViewData> FunctionalRequirements { get; set; }
        public List<RequirementViewData> NonFunctionalRequirements { get; set; }

        protected override void CopyFromEntity(RequirementManager entity)
        {
            FunctionalRequirements = CopyViewDatasFromEntities<Requirement, RequirementViewData>(entity.FunctionalRequirements);
            NonFunctionalRequirements = CopyViewDatasFromEntities<Requirement, RequirementViewData>(entity.NonFunctionalRequirements);
        }

        protected override void CopyToEntity(RequirementManager entity)
        {
        }

    }
}
