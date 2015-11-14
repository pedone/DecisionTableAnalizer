using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;

namespace ViewModels
{
    [ViewDataType(typeof(StateViewData))]
    public class StateViewModel : ViewModel<StateViewData>
    {

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChanged<string>(() => Name);
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                NotifyPropertyChanged<string>(() => Description);
            }
        }

        public override void CopyFromViewData(StateViewData viewData)
        {
            Name = viewData.Name;
            Description = viewData.Description;
        }

        public override void CopyToViewData(StateViewData viewData)
        {
            viewData.Name = Name;
            viewData.Description = Description;
        }

        /// <summary>
        /// This override is neccessary for the decision table to compare the actual states to the valid states.
        /// </summary>
        public override bool Equals(object obj)
        {
            var otherState = obj as StateViewModel;
            if (otherState == null)
                return false;

            return (EntityId != null && EntityId.Equals(otherState.EntityId)) &&
                (Name == null || Name.Equals(otherState.Name)) &&
                (Description == null || Description.Equals(otherState.Description));
        }

        public override int GetHashCode()
        {
            return (EntityId == null ? 0 : EntityId.GetHashCode()) +
                (string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode()) +
                (string.IsNullOrEmpty(Description) ? 0 : Description.GetHashCode());
        }

    }
}
