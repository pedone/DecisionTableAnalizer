using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;

namespace DTCore
{

    public delegate void ViewModelUpdatedEventHandler(ViewModel viewModel);

    public abstract class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public EntityId EntityId { get; private set; }
        internal ViewModelState State { get; set; }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    NotifyPropertyChanged<bool>(() => IsSelected);
                }
            }
        }

        private int _ValidationErrorCount;
        private int ValidationErrorCount
        {
            get { return _ValidationErrorCount; }
            set
            {
                _ValidationErrorCount = value;
                if (_ValidationErrorCount < 0)
                    _ValidationErrorCount = 0;

                HasValidationError = _ValidationErrorCount > 0;
            }
        }

        private bool _ValidationError;
        public bool HasValidationError
        {
            get { return _ValidationError; }
            set
            {
                if (_ValidationError != value)
                {
                    _ValidationError = value;
                    NotifyPropertyChanged<bool>(() => HasValidationError);
                }
            }
        }

        public ViewModel()
        {
            State = ViewModelState.New;
        }

        public void Init(EntityId entityId)
        {
            if (entityId == null)
                throw new ArgumentNullException("entityId", "entityId is null.");

            EntityId = entityId;
            State = ViewModelState.Synchronized;
        }

        protected void NotifyPropertyChanged<TValue>(Expression<Func<TValue>> property)
        {
            if (PropertyChanged != null)
            {
                var memberExpr = property.Body as MemberExpression;
                if (memberExpr != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(memberExpr.Member.Name));
            }
        }

        protected VType CreateViewData<VType>(ViewModel viewModel)
            where VType : ViewData
        {
            var viewData = Activator.CreateInstance<VType>();
            if (viewModel != null)
                viewModel.CopyToViewData(viewData);

            return viewData;
        }

        protected ViewModelType CopyViewModelFromViewData<ViewDataType, ViewModelType>(ViewDataType viewData)
            where ViewDataType : ViewData
            where ViewModelType : ViewModel
        {
            if (viewData == null)
                return null;

            var newViewModel = Activator.CreateInstance<ViewModelType>();
            newViewModel.Init(viewData.EntityId);
            newViewModel.CopyFromViewData(viewData);
            ViewModelService.Instance.AddViewModel(newViewModel);

            return newViewModel;
        }

        protected List<ViewModelType> CopyViewModelsFromViewDatas<ViewDataType, ViewModelType>(IEnumerable<ViewDataType> viewDatas)
            where ViewDataType : ViewData
            where ViewModelType : ViewModel
        {
            if (viewDatas == null)
                return null;

            var result = new List<ViewModelType>();
            foreach (var viewData in viewDatas)
            {
                var newViewModel = Activator.CreateInstance<ViewModelType>();
                newViewModel.Init(viewData.EntityId);
                newViewModel.CopyFromViewData(viewData);
                ViewModelService.Instance.AddViewModel(newViewModel);

                result.Add(newViewModel);
            }

            return result;
        }

        virtual internal void CopyToViewData(ViewData viewData)
        { }
        virtual internal void CopyFromViewData(ViewData viewData)
        { }


        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string error = Validate(columnName);
                if (string.IsNullOrEmpty(error))
                    ValidationErrorCount--;
                else
                    ValidationErrorCount++;

                return error;
            }
        }

        virtual public string Validate(string propertyName)
        {
            return string.Empty;
        }

    }

    public abstract class ViewModel<ViewDataType> : ViewModel
        where ViewDataType : ViewData
    {
        public event ViewModelUpdatedEventHandler Updated;

        internal override void CopyFromViewData(ViewData viewData)
        {
            if (viewData == null)
                throw new ArgumentNullException("viewData", "viewData is null.");
            if (!(viewData is ViewDataType))
                throw new ArgumentException("viewData", "Invalid viewData type.");

            CopyFromViewData((ViewDataType)viewData);
            if (Updated != null)
                Updated(this);
        }

        internal override void CopyToViewData(ViewData viewData)
        {
            if (viewData == null)
                throw new ArgumentNullException("viewData", "viewData is null.");
            if (!(viewData is ViewDataType))
                throw new ArgumentException("viewData", "Invalid viewData type.");

            viewData.EntityId = EntityId;
            CopyToViewData((ViewDataType)viewData);
        }

        abstract public void CopyFromViewData(ViewDataType viewData);
        abstract public void CopyToViewData(ViewDataType viewData);
    }

}
