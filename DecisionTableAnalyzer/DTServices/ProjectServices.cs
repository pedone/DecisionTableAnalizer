using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewModels;
using Entities;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using ExtensionLibrary;
using System.Collections;
using HelperLibrary;

namespace DTServices
{

    public class ProjectServices : DTService
    {

        private class EntityPropertyLoadData
        {
            public PropertyInfo PropertyInfo { get; set; }
            public Entity Instance { get; set; }
            public string ValueEntityId { get; set; }
        }

        public override object ExecuteOperation(string operationId, params object[] args)
        {
            if (operationId == "CreateProject")
                return CreateProject(args);
            else if (operationId == "SaveProject")
                return SaveProject(args);
            else if (operationId == "LoadProject")
                return LoadProject(args);
            else if (operationId == "HasProjectFilename")
                return HasProjectFilename(args);
            else
                throw new ArgumentException(string.Format("No operation found with id '{0}'.", operationId));
        }

        private bool HasProjectFilename(params object[] args)
        {
            EntityId projectId = args.FirstOrDefault() as EntityId;
            if (projectId == null || projectId.EntityType != typeof(DTProject))
                throw new ArgumentException("projectId", "projectId is invalid.");

            DTProject project = EntityService.GetEntity<DTProject>(projectId);
            if (project == null)
                throw new ArgumentException("projectId", "No project found with projectId.");

            return !project.Filename.IsEmpty();
        }

        private EntityId CreateProject(params object[] args)
        {
            var projectDialogModel = args.FirstOrDefault() as ProjectDialogModel;
            if (projectDialogModel == null)
                throw new ArgumentNullException("projectDialogModel", "projectDialogModel is null.");

            var result = ViewModelService.Instance.InsertViewModelAndQueryResult<ProjectDialogModel>(projectDialogModel);

            //Init project
            var project = EntityService.GetEntity<DTProject>(result.EntityId);
            var requirementManager = EntityService.CreateNew<RequirementManager>();
            requirementManager.Project = project;
            project.RequirementManager = requirementManager;

            var decisionTableManager = EntityService.CreateNew<DecisionTableManager>();
            decisionTableManager.Project = project;
            project.DecisionTableManager = decisionTableManager;

            requirementManager.CommitChanges();
            decisionTableManager.CommitChanges();
            project.CommitChanges();

            return result.EntityId;
        }

        private EntityId LoadProject(params object[] args)
        {
            string projectFile = args.FirstOrDefault() as string;
            if (string.IsNullOrEmpty(projectFile) || !File.Exists(projectFile))
                throw new ArgumentException("projectFile", "Invalid project file.");

            XElement projectRoot = null;
            using (FileStream fs = new FileStream(projectFile, FileMode.Open))
                projectRoot = XElement.Load(fs);

            var tables = projectRoot.Elements("Table");
            const string entityAssemblyName = "Entities";
            Assembly entityAssembly = Assembly.Load(new AssemblyName(entityAssemblyName));

            const string enumAssemblyName = "DTEnums";
            Assembly enumAssembly = Assembly.Load(new AssemblyName(enumAssemblyName));
            var enumTypeList = enumAssembly.GetTypes();

            EntityService.Unload();
            List<EntityPropertyLoadData> entityPropertyInfos = new List<EntityPropertyLoadData>();
            var propertyBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            foreach (var table in tables)
            {
                Type tableElementType = entityAssembly.GetType(table.GetAttributeValue("Type"));
                foreach (var row in table.Elements())
                {
                    var entity = Activator.CreateInstance(tableElementType) as Entity;
                    entityPropertyInfos.AddRange(from attribute in row.Attributes()
                                                 let propertyInfo = tableElementType.GetProperty(attribute.Name.ToString(), propertyBindingFlags)
                                                 where typeof(Entity).IsAssignableFrom(propertyInfo.PropertyType)
                                                 select new EntityPropertyLoadData
                                                 {
                                                     PropertyInfo = propertyInfo,
                                                     ValueEntityId = attribute.Value,
                                                     Instance = entity
                                                 });

                    var entityData = row.Attributes().ToDictionary(cur => cur.Name.ToString(), cur => (object)cur.Value);
                    entity.InitPropertyValues(entityData, enumTypeList);

                    EntityService.Insert(entity);
                }
            }

            foreach (var entityPropertyInfo in entityPropertyInfos)
            {
                var entity = EntityService.GetEntity(entityPropertyInfo.ValueEntityId);
                entityPropertyInfo.PropertyInfo.SetValue(entityPropertyInfo.Instance, entity, null);
            }

            LoadSubEntityTable(projectRoot);
            LoadSubItemTable(projectRoot);
            LoadSubDictionaryTable(projectRoot);

            EntityService.CommitChanges();
            return EntityService.GetEntity(new Predicate<Entity>(cur => cur.GetType() == typeof(DTProject))).EntityId;
        }

        private void LoadSubDictionaryTable(XElement projectRoot)
        {
            var propertyBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var subEntityTable = projectRoot.Element("SubDictionaryTable");
            foreach (var entry in subEntityTable.Elements())
            {
                string subKeyEntityId = entry.GetAttributeValue("KeyEntityId");
                string subValueEntityId = entry.GetAttributeValue("ValueEntityId");
                string subKey = entry.GetAttributeValue("Key");
                string subValue = entry.GetAttributeValue("Value");
                string ownerId = entry.GetAttributeValue("OwnerId");
                string ownerCollection = entry.GetAttributeValue("OwnerCollection");

                var ownerEntity = EntityService.GetEntity(ownerId);
                var ownerCollectionProperty = ownerEntity.GetType().GetProperty(ownerCollection, propertyBindingFlags);

                object key = null;
                object value = null;
                if (!string.IsNullOrEmpty(subKeyEntityId))
                    key = EntityService.GetEntity(subKeyEntityId);
                else
                    key = subKey;

                if (!string.IsNullOrEmpty(subValueEntityId))
                    value = EntityService.GetEntity(subValueEntityId);
                else
                {
                    if (string.IsNullOrEmpty(subValue as string))
                    {
                        var valueType = ownerCollectionProperty.PropertyType.GetGenericArguments().ElementAtOrDefault(1);
                        value = valueType.IsValueType ? Activator.CreateInstance(valueType) : null;
                    }
                    else
                        value = subValue;
                }

                if (ownerEntity == null)
                    throw new ArgumentNullException("ownerEntity", "ownerEntity is null");
                if (key == null)
                    throw new ArgumentNullException("key", "key is null");

                var ownerCollectionPropertyValue = (IDictionary)ownerCollectionProperty.GetValue(ownerEntity, null);
                ownerCollectionPropertyValue.Add(key, value);
                
            }
        }

        private void LoadSubItemTable(XElement projectRoot)
        {
            var propertyBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var subEntityTable = projectRoot.Element("SubItemTable");
            foreach (var entry in subEntityTable.Elements())
            {
                string value = entry.GetAttributeValue("Value");
                string ownerId = entry.GetAttributeValue("OwnerId");
                string ownerCollection = entry.GetAttributeValue("OwnerCollection");

                var ownerEntity = EntityService.GetEntity(ownerId);

                if (ownerEntity == null)
                    throw new ArgumentNullException("ownerEntity", "ownerEntity is null");

                var ownerCollectionProperty = ownerEntity.GetType().GetProperty(ownerCollection, propertyBindingFlags).GetValue(ownerEntity, null) as IList;
                ownerCollectionProperty.Add(value);
            }
        }

        private void LoadSubEntityTable(XElement projectRoot)
        {
            var propertyBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var subEntityTable = projectRoot.Element("SubEntityTable");
            foreach (var entry in subEntityTable.Elements())
            {
                string subEntityId = entry.GetAttributeValue("EntityId");
                string ownerId = entry.GetAttributeValue("OwnerId");
                string ownerCollection = entry.GetAttributeValue("OwnerCollection");

                var ownerEntity = EntityService.GetEntity(ownerId);
                var subEntity = EntityService.GetEntity(subEntityId);

                if (ownerEntity == null)
                    throw new ArgumentNullException("ownerEntity", "ownerEntity is null");
                if (subEntity == null)
                    throw new ArgumentNullException("subEntity", "subEntity is null");

                var ownerCollectionProperty = ownerEntity.GetType().GetProperty(ownerCollection, propertyBindingFlags).GetValue(ownerEntity, null) as IList;
                ownerCollectionProperty.Add(subEntity);
            }
        }

        private object SaveProject(params object[] args)
        {
            EntityId projectId = args.FirstOrDefault() as EntityId;
            if (projectId == null || projectId.EntityType != typeof(DTProject))
                throw new ArgumentException("projectId", "projectId is invalid.");

            DTProject project = EntityService.GetEntity<DTProject>(projectId);
            if (project == null)
                throw new ArgumentException("projectId", "No project found with projectId.");

            string filename = args.ElementAtOrDefault(1) as string;
            if (string.IsNullOrEmpty(filename))
            {
                if (string.IsNullOrEmpty(project.Filename))
                    throw new ArgumentNullException("filename", "No filename found for project.");

                filename = project.Filename;
            }

            project.Filename = filename;
            project.CommitChanges();

            //Save project entities to tables and to the file
            XElement subElementTable = new XElement("SubEntityTable");
            XElement subItemTable = new XElement("SubItemTable");
            XElement subdictionaryTable = new XElement("SubDictionaryTable");

            var projectEntitiesByType = RetrieveAllEntities(project).GroupBy(cur => cur.GetType());
            XElement projectElement = new XElement("DTData",
                from entityGroup in projectEntitiesByType
                select SaveToTable(entityGroup, subElementTable, subItemTable, subdictionaryTable),
                subElementTable,
                subItemTable,
                subdictionaryTable);

            projectElement.Save(project.Filename);
            return null;
        }

        private XElement SaveToTable(IGrouping<Type, Entity> entityTypeGroup, XElement subEntityTable, XElement subItemTable, XElement subDictionaryTable)
        {
            string entityTypeName = entityTypeGroup.Key.FullName;
            return new XElement("Table", new XAttribute("Type", entityTypeName),
                from entity in entityTypeGroup
                select BuildRow(entity, subEntityTable, subItemTable, subDictionaryTable));
        }

        private XElement BuildRow(Entity entity, XElement subEntityTable, XElement subItemTable, XElement subDictionaryTable)
        {
            var entityPropertyValueEntities = entity.GetPropertyValues().Where(cur => cur.Value is Entity);
            var entityPropertyValuesWithoutEntitiesOrLists = entity.GetPropertyValues().Where(cur => !(cur.Value is Entity || cur.Value is IEnumerable<object> || cur.Value is IDictionary));
            var subEntityListPropertyValues = entity.GetPropertyValues().Where(cur => cur.Value is IEnumerable<Entity>);
            var subItemListPropertyValues = entity.GetPropertyValues().Where(cur => cur.Value is IEnumerable<object> && !(cur.Value is IEnumerable<Entity>));
            var subDictionaryPropertyValues = entity.GetPropertyValues().Where(cur => cur.Value is IDictionary);

            subDictionaryTable.Add(subDictionaryPropertyValues.SelectMany(dictPropertyValue =>
            {
                var result = new List<XElement>();

                var dict = (IDictionary)dictPropertyValue.Value;
                var keys = dict.Keys.Cast<object>();
                var values = dict.Values.Cast<object>();
                for (int i = 0; i < dict.Count; i++)
                {
                    var key = keys.ElementAt(i);
                    var value = values.ElementAt(i);

                    XAttribute keyAttribute = null;
                    XAttribute valueAttribute = null;

                    if (key is Entity)
                        keyAttribute = new XAttribute("KeyEntityId", ((Entity)key).EntityId.Id);
                    else
                        keyAttribute = new XAttribute("Key", key ?? string.Empty);

                    if (value is Entity)
                        valueAttribute = new XAttribute("ValueEntityId", ((Entity)value).EntityId.Id);
                    else
                        valueAttribute = new XAttribute("Value", value ?? string.Empty);

                    result.Add(new XElement("Entry",
                        new XAttribute("OwnerId", entity.EntityId.Id),
                        new XAttribute("OwnerCollection", dictPropertyValue.Key),
                        keyAttribute,
                        valueAttribute));
                }

                return result;
            }));

            subEntityTable.Add(subEntityListPropertyValues.SelectMany(curSubEntityListPropertyValue =>
                from subEntity in (IEnumerable<Entity>)curSubEntityListPropertyValue.Value
                select new XElement("Entry",
                    new XAttribute("OwnerId", entity.EntityId.Id),
                    new XAttribute("OwnerCollection", curSubEntityListPropertyValue.Key),
                    new XAttribute("EntityId", subEntity.EntityId.Id))));

            subItemTable.Add(subItemListPropertyValues.SelectMany(curSubItemListPropertyValue =>
                from subItem in (IEnumerable<object>)curSubItemListPropertyValue.Value
                select new XElement("Entry",
                    new XAttribute("OwnerId", entity.EntityId.Id),
                    new XAttribute("OwnerCollection", curSubItemListPropertyValue.Key),
                    new XAttribute("Value", subItem.ToString()))));

            return new XElement("Entry",
                new XAttribute("EntityId", entity.EntityId.Id),
                from propertyValue in entityPropertyValueEntities
                select new XAttribute(propertyValue.Key, ((Entity)propertyValue.Value).EntityId.Id),
                from propertyValue in entityPropertyValuesWithoutEntitiesOrLists
                select new XAttribute(propertyValue.Key, propertyValue.Value ?? ""));
        }

        private IEnumerable<Entity> RetrieveAllEntities(DTProject project)
        {
            List<Entity> projectEntities = new List<Entity>();
            projectEntities.Add(project);
            var subEntities = GetSubEntities(project, projectEntities);
            if (subEntities.Count() > 0)
                projectEntities.AddRange(subEntities);

            return projectEntities.Distinct();
        }

        private IEnumerable<Entity> GetSubEntities(Entity entity, IEnumerable<Entity> except)
        {
            List<Entity> thisSubEntities = new List<Entity>();
            Dictionary<string, object> entityPropertyValues = entity.GetPropertyValues();
            var entityPropertyValueEntities = (from propertyValue in entityPropertyValues
                                               where propertyValue.Value is Entity
                                               select (Entity)propertyValue.Value).ToList();
            var subEntityListPropertyValues = entityPropertyValues
                .Where(cur => cur.Value is IEnumerable<Entity>)
                .SelectMany(cur => (IEnumerable<Entity>)cur.Value).ToList();
            var subDictionaryPropertyValues = entityPropertyValues
                .Where(cur => cur.Value is IDictionary)
                .SelectMany(cur =>
                    {
                        var dictionary = (IDictionary)cur.Value;
                        return dictionary.Keys.OfType<Entity>().Concat(dictionary.Values.OfType<Entity>());
                    }).ToList();

            thisSubEntities.AddRange(entityPropertyValueEntities);
            thisSubEntities.AddRange(subEntityListPropertyValues);
            thisSubEntities.AddRange(subDictionaryPropertyValues);
            foreach (var subEntity in entityPropertyValueEntities.Concat(subEntityListPropertyValues).Except(except))
            {
                var subEntities = GetSubEntities(subEntity, except.Concat(thisSubEntities));
                if (subEntities.Count() > 0)
                    thisSubEntities.AddRange(subEntities);
            }

            return thisSubEntities;
        }

    }
}
