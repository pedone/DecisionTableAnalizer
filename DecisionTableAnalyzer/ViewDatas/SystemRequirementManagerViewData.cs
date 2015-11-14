using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;

namespace ViewDatas
{
    [EntityType(typeof(RequirementManager))]
    public class SystemRequirementManagerViewData : ViewData<RequirementManager>
    {

        public List<SystemRequirementViewData> FunctionalRequirements { get; set; }
        public List<SystemRequirementViewData> NonFunctionalRequirements { get; set; }

        protected override void CopyFromEntity(RequirementManager entity)
        {
            FunctionalRequirements = CopyViewDatasFromEntities<Requirement, SystemRequirementViewData>(entity.FunctionalRequirements);
            NonFunctionalRequirements = CopyViewDatasFromEntities<Requirement, SystemRequirementViewData>(entity.NonFunctionalRequirements);
        }

        protected override void CopyToEntity(RequirementManager entity)
        {
        }
    }
}
