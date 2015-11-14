using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections;

namespace DTCore
{
    public class ViewModelService
    {

        public static ViewModelService Instance { get; private set; }

        private List<WeakReference> _InternalViewModels;
        private List<Type> _InternalViewModelCommands;
        private List<Type> _InternalServices;

        internal IEnumerable<ViewModel> ViewModels
        {
            get
            {
                return from viewModelReference in _InternalViewModels
                       where viewModelReference.IsAlive
                       select (ViewModel)viewModelReference.Target;
            }
        }

        internal IEnumerable<ViewModel> SynchronizedViewModels
        {
            get { return ViewModels.Where(cur => cur.State == ViewModelState.Synchronized); }
        }

        static ViewModelService()
        {
            Instance = new ViewModelService();
        }

        private ViewModelService()
        {
            _InternalViewModels = new List<WeakReference>();
            _InternalViewModelCommands = new List<Type>();
            _InternalServices = new List<Type>();
        }

        public void Init()
        {
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
        }

        public void DeleteViewModel(ViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel", "viewModel is null.");

            EntityService.Instance.Delete(viewModel.EntityId);
        }

        public object ExecuteOperation(string serviceId, string operationId, params object[] args)
        {
            var serviceType = _InternalServices.FirstOrDefault(cur => cur.FullName == serviceId);
            if (serviceType != null)
            {
                var serviceInstance = Activator.CreateInstance(serviceType) as DTService;
                return serviceInstance.ExecuteOperation(operationId, args);
            }

            return null;
        }

        public T ExecuteOperation<T>(string serviceId, string operationId, params object[] args)
        {
            return (T)ExecuteOperation(serviceId, operationId, args);
        }

        private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            LoadViewModelCommands(args);
            LoadServices(args);
        }

        private void LoadServices(AssemblyLoadEventArgs args)
        {
            var services = args.LoadedAssembly.GetTypes().Where(cur => typeof(DTService).IsAssignableFrom(cur));
            if (services.Count() > 0)
            {
                var servicesWithoutDefaultConstructor = services.Where(cur => cur.GetConstructor(Type.EmptyTypes) == null);
                foreach (var svc in servicesWithoutDefaultConstructor)
                    Debug.WriteLine(string.Format("No default constructor found for service '{0}'", svc.FullName));

                _InternalServices.AddRange(services.Except(servicesWithoutDefaultConstructor));
            }
        }

        private void LoadViewModelCommands(AssemblyLoadEventArgs args)
        {
            var viewModelCommands = args.LoadedAssembly.GetTypes().Where(cur => typeof(ViewModelCommand).IsAssignableFrom(cur));
            if (viewModelCommands.Count() > 0)
            {
                var commandsWithoutDefaultConstructor = viewModelCommands.Where(cur => cur.GetConstructor(Type.EmptyTypes) == null);
                foreach (var cmd in commandsWithoutDefaultConstructor)
                    Debug.WriteLine(string.Format("No default constructor found for command '{0}'", cmd.FullName));

                _InternalViewModelCommands.AddRange(viewModelCommands.Except(commandsWithoutDefaultConstructor));
            }
        }

        internal ViewModelCommand GetViewModelCommand(string commandFullName)
        {
            var commandType = _InternalViewModelCommands.FirstOrDefault(cur => cur.FullName == commandFullName);
            if (commandType != null)
                return Activator.CreateInstance(commandType) as ViewModelCommand;

            return null;
        }

        internal void AddViewModel(ViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel", "viewModel is null.");

            _InternalViewModels.Add(new WeakReference(viewModel, false));
        }

        public ViewModelType QueryViewModel<ViewModelType>(EntityId entityId)
            where ViewModelType : ViewModel
        {
            if (entityId == null)
                throw new ArgumentNullException("entityId", "entityId is null.");

            //Get entity and copy to view data
            var existingEntity = EntityService.Instance.GetEntity(entityId);
            if (existingEntity == null)
                return null;

            var newViewData = CreateViewData(typeof(ViewModelType));
            newViewData.CopyFromEntity(existingEntity);

            //Create new viewModel and copy data
            var newViewModel = Activator.CreateInstance<ViewModelType>();
            newViewModel.Init(entityId);
            newViewModel.CopyFromViewData(newViewData);
            AddViewModel(newViewModel);

            return newViewModel;
        }

        public void CommitViewModel(ViewModel viewModel)
        {
            if (viewModel.State == ViewModelState.New || viewModel.EntityId == null)
                throw new ArgumentException("Invalid view model state. The view model is not synchronized with any entity");

            var newViewData = CreateViewData(viewModel.GetType());
            viewModel.CopyToViewData(newViewData);

            var entity = EntityService.Instance.GetEntity(viewModel.EntityId);
            if (entity == null)
                throw new EntityNotFoundException();

            //Commit changes
            newViewData.CopyToEntity(entity);
            entity.CommitChanges();
        }

        internal void UpdateAllViewModelsWithEntityId(EntityId entityId)
        {
            var entity = EntityService.Instance.GetEntity(entityId);
            UpdateAllViewModelsWithEntity(entity);
        }

        internal void UpdateAllViewModelsWithEntity(Entity entity)
        {
            if (entity == null)
                return;

            foreach (var subViewModel in SynchronizedViewModels.Where(cur => cur.EntityId.Equals(entity.EntityId)).ToList())
            {
                var subViewData = CreateViewData(subViewModel.GetType());
                subViewData.CopyFromEntity(entity);
                subViewModel.CopyFromViewData(subViewData);
            }
        }

        public ViewModelType CommitViewModelAndQueryResult<ViewModelType>(ViewModelType viewModel)
            where ViewModelType : ViewModel, new()
        {
            CommitViewModel(viewModel);
            return QueryViewModel<ViewModelType>(viewModel.EntityId);
        }

        public void InsertViewModel(ViewModel viewModel, EntityId ownerId, string ownerCollection)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel", "viewModel is null.");
            if (ownerId == null)
                throw new ArgumentNullException("ownerId", "ownerId is null.");
            if (String.IsNullOrEmpty(ownerCollection))
                throw new ArgumentException("ownerCollection", "ownerCollection is null or empty.");

            var ownerEntity = EntityService.Instance.GetEntity(ownerId);
            if (ownerEntity == null)
                throw new ArgumentException("ownerId", "ownerId not found in entities.");

            var ownerCollectionProperty = ownerEntity.GetType().GetProperty(ownerCollection);
            if (ownerCollectionProperty == null)
                throw new ArgumentException("ownerCollection", "ownerCollection was not found on owner.");
            if (!typeof(IList).IsAssignableFrom(ownerCollectionProperty.PropertyType))
                throw new ArgumentException("ownerCollection", "ownerCollection does not inherit from IList.");

            var newViewData = CreateViewData(viewModel.GetType());
            viewModel.CopyToViewData(newViewData);

            var entity = viewModel.EntityId != null ? EntityService.Instance.GetEntity(viewModel.EntityId) ?? CreateEntity(newViewData.GetType()) : CreateEntity(newViewData.GetType());
            newViewData.CopyToEntity(entity);
            entity.CommitChanges();

            //entity could have been deleted because the validation failed
            if (entity.State != EntityState.Deleted)
            {
                var entityOwnerCollection = ownerCollectionProperty.GetValue(ownerEntity, null) as IList;
                if (!entityOwnerCollection.Contains(entity))
                {
                    //copy collection and insert new item
                    var newOwnerCollection = Activator.CreateInstance(ownerCollectionProperty.PropertyType) as IList;
                    foreach (var oldItem in entityOwnerCollection)
                        newOwnerCollection.Add(oldItem);

                    newOwnerCollection.Add(entity);
                    ownerCollectionProperty.SetValue(ownerEntity, newOwnerCollection, null);
                    ownerEntity.CommitChanges();
                }
            }
        }

        public void InsertViewModels(IEnumerable<ViewModel> viewModels, EntityId ownerId, string ownerCollection)
        {
            if (viewModels == null)
                throw new ArgumentNullException("viewModel", "viewModel is null.");
            if (ownerId == null)
                throw new ArgumentNullException("ownerId", "ownerId is null.");
            if (String.IsNullOrEmpty(ownerCollection))
                throw new ArgumentException("ownerCollection", "ownerCollection is null or empty.");

            if (viewModels.Count() == 0)
                return;

            var ownerEntity = EntityService.Instance.GetEntity(ownerId);
            if (ownerEntity == null)
                throw new ArgumentException("ownerId", "ownerId not found in entities.");

            var ownerCollectionProperty = ownerEntity.GetType().GetProperty(ownerCollection);
            if (ownerCollectionProperty == null)
                throw new ArgumentException("ownerCollection", "ownerCollection was not found on owner.");
            if (!typeof(IList).IsAssignableFrom(ownerCollectionProperty.PropertyType))
                throw new ArgumentException("ownerCollection", "ownerCollection does not inherit from IList.");

            //copy owner collection
            var entityOwnerCollection = ownerCollectionProperty.GetValue(ownerEntity, null) as IList;
            var newOwnerCollection = Activator.CreateInstance(ownerCollectionProperty.PropertyType) as IList;
            foreach (var oldItem in entityOwnerCollection)
                newOwnerCollection.Add(oldItem);

            foreach (var viewModel in viewModels)
            {
                var newViewData = CreateViewData(viewModel.GetType());
                viewModel.CopyToViewData(newViewData);

                var entity = viewModel.EntityId != null ? EntityService.Instance.GetEntity(viewModel.EntityId) ?? CreateEntity(newViewData.GetType()) : CreateEntity(newViewData.GetType());
                newViewData.CopyToEntity(entity);
                entity.CommitChanges();

                if (entity.State != EntityState.Deleted && !newOwnerCollection.Contains(entity))
                    newOwnerCollection.Add(entity);
            }

            ownerCollectionProperty.SetValue(ownerEntity, newOwnerCollection, null);
            ownerEntity.CommitChanges();
        }

        public void InsertViewModel(ViewModel viewModel)
        {
            EntityId entityId = null;
            InsertViewModel(viewModel, out entityId);
        }

        private void InsertViewModel(ViewModel viewModel, out EntityId entityId)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel", "viewModel is null.");
            if (viewModel.State == ViewModelState.Synchronized || viewModel.EntityId != null)
                throw new ArgumentException("Invalid view model state. The view model is already synchronized with a entity");

            var newViewData = CreateViewData(viewModel.GetType());
            viewModel.CopyToViewData(newViewData);

            var newEntity = CreateEntity(newViewData.GetType());
            newViewData.CopyToEntity(newEntity);
            newEntity.CommitChanges();

            if (newEntity.State != EntityState.Deleted)
                entityId = newEntity.EntityId;
            else
                entityId = null;
        }

        public T InsertViewModelAndQueryResult<T>(ViewModel viewModel)
            where T : ViewModel, new()
        {
            EntityId entityId = null;
            InsertViewModel(viewModel, out entityId);
            return QueryViewModel<T>(entityId);
        }

        private ViewData CreateViewData(Type viewModelType)
        {
            if (viewModelType == null)
                throw new ArgumentNullException("viewModelType", "viewModelType is null.");
            if (!(typeof(ViewModel).IsAssignableFrom(viewModelType)))
                throw new ArgumentException("viewModelType", "viewModelType does not inherit from ViewModel.");

            var viewDataTypeInformation = viewModelType.GetCustomAttributes(typeof(ViewDataTypeAttribute), false).FirstOrDefault() as ViewDataTypeAttribute;
            if (viewDataTypeInformation == null)
                throw new ArgumentException("No view data type information found on view model.");

            var viewDataType = viewDataTypeInformation.ViewDataType;
            var newViewData = viewDataType.GetConstructor(Type.EmptyTypes).Invoke(null) as ViewData;
            if (newViewData == null)
                throw new ArgumentNullException("Invalid view data.");

            return newViewData;
        }

        /// <summary>
        /// Unloads all ViewModels and Entities
        /// </summary>
        public void Unload()
        {
            _InternalViewModels.Clear();
            EntityService.Instance.Unload();
        }

        internal Entity CreateEntity(Type viewDataType)
        {
            if (viewDataType == null)
                throw new ArgumentNullException("viewDataType", "viewDataType is null.");
            if (!(typeof(ViewData).IsAssignableFrom(viewDataType)))
                throw new ArgumentException("viewDataType", "viewDataType does not inherit from ViewData.");

            var entityTypeInformation = viewDataType.GetCustomAttributes(typeof(EntityTypeAttribute), false).FirstOrDefault() as EntityTypeAttribute;
            if (entityTypeInformation == null)
                throw new ArgumentException("No entity type information found on view data.");

            var entityType = entityTypeInformation.EntityType;
            return EntityService.Instance.CreateNew(entityType);
        }

    }
}
