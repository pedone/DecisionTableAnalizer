using ViewModels.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ViewModels;
using System.Collections.Generic;
using DTCore;
using System.Reflection;
using System.Linq;

namespace DecisionTableAnalizerTests
{
    
    
    /// <summary>
    ///This is a test class for DecisionTableViewModelUtilsTest and is intended
    ///to contain all DecisionTableViewModelUtilsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DecisionTableViewModelUtilsTest
    {


        private TestContext testContextInstance;
        private EntityId ProjectId { get; set; }
        private DecisionTableViewModel DecisionTable { get; set; }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            ViewModelService.Instance.Init();
            Assembly.Load(new AssemblyName("DTServices"));
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            var projectDialog = new ProjectDialogModel
            {
                Name = "Test Project"
            };

            string serviceId = "DTServices.ProjectServices";
            string operationId = "CreateProject";
            ProjectId = ViewModelService.Instance.ExecuteOperation<EntityId>(serviceId, operationId, projectDialog);

            string serviceId2 = "DTServices.CommonServices";
            string operationId2 = "GetDecisionTableManager";
            var decisionTableManagerViewModel = ViewModelService.Instance.ExecuteOperation<DecisionTableManagerViewModel>(serviceId2, operationId2, ProjectId);
            var decisionTableDialog = new DecisionTableDialogModel
            {
                Name = "Test Table",
                DecisionTableManagerId = decisionTableManagerViewModel.EntityId
            };
            ViewModelService.Instance.InsertViewModel(decisionTableDialog, decisionTableManagerViewModel.EntityId, "DecisionTables");
            DecisionTable = ViewModelService.Instance.QueryViewModel<DecisionTableViewModel>(decisionTableManagerViewModel.DecisionTables[0].EntityId);
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            string serviceId = "DTServices.CommonServices";
            string operationId = "UnloadEntities";
            ViewModelService.Instance.ExecuteOperation(serviceId, operationId);

            ProjectId = null;
            DecisionTable = null;
        }
        
        #endregion


        private StateViewModel GetActionEmptyState(DecisionTableViewModel decisionTable)
        {
            string serviceId = "DTServices.CommonServices";
            string operationId = "GetActionEmptyState";
            return ViewModelService.Instance.ExecuteOperation<StateViewModel>(serviceId, operationId, decisionTable.DecisionTableManagerId);
        }

        private StateViewModel GetConditionNoPreferenceState(DecisionTableViewModel decisionTable)
        {
            string serviceId = "DTServices.CommonServices";
            string operationId = "GetConditionNoPreferenceState";
            return ViewModelService.Instance.ExecuteOperation<StateViewModel>(serviceId, operationId, decisionTable.DecisionTableManagerId);
        }

        /// <summary>
        ///A test for BuildRules
        ///</summary>
        [TestMethod()]
        public void BuildRulesTest()
        {
            var f = new StateViewModel { Name = "false" };
            var t = new StateViewModel { Name = "true" };
            var validStates = new List<StateViewModel>(new[] { t, f });

            for (int i = 0; i < 3; i++)
                DecisionTable.Conditions.Add(new ConditionViewModel { Name = string.Format("Condition {0}", i), ValidStates = validStates });
            for (int i = 0; i < 2; i++)
                DecisionTable.Actions.Add(new ActionViewModel { Name = string.Format("Action {0}", i) });

            var actual = DecisionTableViewModelUtils.Instance.BuildRules(DecisionTable);
            Assert.IsTrue(actual.Count == 8);

            var expectedStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, t, t, t, f, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { t, t, f, f, t, t, f, f }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { t, f, t, f, t, f, t, f }.ToList() },
            		            };

            for (int ruleIndex = 0; ruleIndex < actual.Count; ruleIndex++)
            {
                var actualRule = actual[ruleIndex];
                foreach (var actualPair in actualRule.ConditionStates)
                {
                    var condition = actualPair.Key;
                    var actualState = actualPair.Value;
                    var expectedState = expectedStates[condition];

                    Assert.AreEqual(expectedState[ruleIndex], actualState);
                }
            }
        }

        /// <summary>
        ///A test for CalculateRuleCount
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ViewModels.dll")]
        public void CalculateRuleCountTest()
        {
            var f = new StateViewModel { Name = "false" };
            var t = new StateViewModel { Name = "true" };
            var validStates = new List<StateViewModel>(new[] { t, f });

            for (int i = 0; i < 3; i++)
                DecisionTable.Conditions.Add(new ConditionViewModel { Name = string.Format("Condition {0}", i), ValidStates = validStates });
            for (int i = 0; i < 2; i++)
                DecisionTable.Actions.Add(new ActionViewModel { Name = string.Format("Action {0}", i) });

            DecisionTableViewModelUtils_Accessor target = new DecisionTableViewModelUtils_Accessor();
            var actual = target.CalculateRuleCount(DecisionTable.Conditions);
            int expected = 8;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CheckForCompletenessTestCase1()
        {
            var f = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "false" });
            var t = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "true" });
            var e = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "Empty" });
            var a = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "ActionYes" });
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionDialogModel>();
            var actions = new List<ActionDialogModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Condition {0}", i),
                });
            for (int i = 0; i < 2; i++)
                actions.Add(new ActionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Action {0}", i)
                });

            ViewModelService.Instance.InsertViewModels(conditions, DecisionTable.EntityId, "Conditions");
            ViewModelService.Instance.InsertViewModels(actions, DecisionTable.EntityId, "Actions");

            foreach (var condition in DecisionTable.Conditions)
                ViewModelService.Instance.InsertViewModels(validConditionStates, condition.EntityId, "ValidStates");

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
                                    //Last two rules are doubles
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, t, t, t, f, f, f, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { t, t, f, f, t, t, f, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { t, f, t, f, t, f, t, f, t, f }.ToList() },
            		            };
            var actualActionStates = new Dictionary<ActionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Actions[0], new StateViewModel[] { a, e, e, e, e, a, e, e, e, e }.ToList() },
            		                { DecisionTable.Actions[1], new StateViewModel[] { a, a, e, a, a, e, a, e, a, e }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                var actionStates = new Dictionary<ActionViewModel, StateViewModel>();
                foreach (var pair in actualActionStates)
                    actionStates.Add(pair.Key, pair.Value[ruleIndex]);

                DecisionTable.Rules.Add(new RuleViewModel
                {
                    Index = ruleIndex,
                    ConditionStates = conditionStates,
                    ActionStates = actionStates
                });
            }

            DecisionTable.Rules.RemoveAt(4);
            DecisionTable.Rules.RemoveAt(3);
            DecisionTable.Rules.RemoveAt(1);

            var expectedMissingRules = new[]
                {
                    new RuleViewModel
                    {
                        Index = 1,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], t },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], f },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 3,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], t },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], f },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 4,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], t },
                        }
                    }
                }.ToList();

            List<RuleViewModel> actualMissingRules = null;
            var isComplete = DecisionTableViewModelUtils.Instance.CheckForCompleteness(DecisionTable, out actualMissingRules);

            Assert.IsFalse(isComplete);
            Assert.IsTrue(actualMissingRules.Count == expectedMissingRules.Count, "Wrong missing element count.");
            for (int i = 0; i < actualMissingRules.Count; i++)
            {
                Assert.IsTrue(CompareConditions(expectedMissingRules[i], actualMissingRules[i]));
            }
        }

        [TestMethod()]
        public void CheckForCompletenessTestCase2()
        {
            var f = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "false" });
            var t = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "true" });
            var e = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "Empty" });
            var a = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "ActionYes" });
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionDialogModel>();
            var actions = new List<ActionDialogModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Condition {0}", i),
                });
            for (int i = 0; i < 2; i++)
                actions.Add(new ActionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Action {0}", i)
                });

            ViewModelService.Instance.InsertViewModels(conditions, DecisionTable.EntityId, "Conditions");
            ViewModelService.Instance.InsertViewModels(actions, DecisionTable.EntityId, "Actions");

            foreach (var condition in DecisionTable.Conditions)
                ViewModelService.Instance.InsertViewModels(validConditionStates, condition.EntityId, "ValidStates");

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, t, t, t, f, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { t, t, f, f, t, t, f, f }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { t, f, t, f, t, f, t, f }.ToList() },
            		            };
            var actualActionStates = new Dictionary<ActionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Actions[0], new StateViewModel[] { a, e, e, e, e, a, e, e }.ToList() },
            		                { DecisionTable.Actions[1], new StateViewModel[] { a, a, e, a, a, e, a, e }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                var actionStates = new Dictionary<ActionViewModel, StateViewModel>();
                foreach (var pair in actualActionStates)
                    actionStates.Add(pair.Key, pair.Value[ruleIndex]);

                DecisionTable.Rules.Add(new RuleViewModel
                {
                    Index = ruleIndex,
                    ConditionStates = conditionStates,
                    ActionStates = actionStates
                });
            }

            DecisionTable.Rules.RemoveAt(4);
            DecisionTable.Rules.RemoveAt(3);
            DecisionTable.Rules.RemoveAt(1);

            var expectedMissingRules = new[]
                {
                    new RuleViewModel
                    {
                        Index = 1,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], t },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], f },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 3,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], t },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], f },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 4,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], t },
                        }
                    }
                }.ToList();

            List<RuleViewModel> actualMissingRules = null;
            var isComplete = DecisionTableViewModelUtils.Instance.CheckForCompleteness(DecisionTable, out actualMissingRules);

            Assert.IsFalse(isComplete);
            Assert.IsTrue(actualMissingRules.Count == expectedMissingRules.Count, "Wrong missing element count.");
            for (int i = 0; i < actualMissingRules.Count; i++)
            {
                Assert.IsTrue(CompareConditions(expectedMissingRules[i], actualMissingRules[i]));
            }
        }

        [TestMethod()]
        public void CheckForCompletenessTestCase3()
        {
            var f = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "false" });
            var t = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "true" });
            var e = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "Empty" });
            var a = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "ActionYes" });
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionDialogModel>();
            var actions = new List<ActionDialogModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Condition {0}", i),
                });
            for (int i = 0; i < 2; i++)
                actions.Add(new ActionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Action {0}", i)
                });

            ViewModelService.Instance.InsertViewModels(conditions, DecisionTable.EntityId, "Conditions");
            ViewModelService.Instance.InsertViewModels(actions, DecisionTable.EntityId, "Actions");

            foreach (var condition in DecisionTable.Conditions)
                ViewModelService.Instance.InsertViewModels(validConditionStates, condition.EntityId, "ValidStates");

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
            		                // more than one rule covered using empty
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, t, t, t, t, f, f, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { t, t, f, f, f, t, t, f, f, e }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { t, f, t, e, f, t, f, t, f, e }.ToList() },
            		            };
            var actualActionStates = new Dictionary<ActionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Actions[0], new StateViewModel[] { a, e, e, e, e, a, e, e, e, e }.ToList() },
            		                { DecisionTable.Actions[1], new StateViewModel[] { a, a, e, a, a, e, a, e, a, e }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                var actionStates = new Dictionary<ActionViewModel, StateViewModel>();
                foreach (var pair in actualActionStates)
                    actionStates.Add(pair.Key, pair.Value[ruleIndex]);

                DecisionTable.Rules.Add(new RuleViewModel
                {
                    Index = ruleIndex,
                    ConditionStates = conditionStates,
                    ActionStates = actionStates
                });
            }

            DecisionTable.Rules.RemoveAt(8);
            DecisionTable.Rules.RemoveAt(7);
            DecisionTable.Rules.RemoveAt(6);
            DecisionTable.Rules.RemoveAt(5);
            DecisionTable.Rules.RemoveAt(2);

            var expectedMissingRules = new[]
                {
                    new RuleViewModel
                    {
                        Index = 2,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], t },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], t },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 5,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], t },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 6,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], f },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 7,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], t },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 8,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], f },
                        }
                    }
                }.ToList();

            List<RuleViewModel> actualMissingRules = null;
            var isComplete = DecisionTableViewModelUtils.Instance.CheckForCompleteness(DecisionTable, out actualMissingRules);

            Assert.IsFalse(isComplete);
            Assert.IsTrue(actualMissingRules.Count == expectedMissingRules.Count, "Wrong missing element count.");
            for (int i = 0; i < actualMissingRules.Count; i++)
            {
                Assert.IsTrue(CompareConditions(expectedMissingRules[i], actualMissingRules[i]));
            }
        }

        private bool CompareConditions(RuleViewModel ruleA, RuleViewModel ruleB)
        {
            foreach (var conditionPairA in ruleA.ConditionStates)
            {
                var curConditionId = conditionPairA.Key.EntityId;
                var stateA = conditionPairA.Value;
                var matchingPairB = ruleB.ConditionStates.First(cur => cur.Key.EntityId.Equals(curConditionId));
                var stateB = matchingPairB.Value;
                if (!stateA.EntityId.Equals(stateB.EntityId))
                    return false;
            }

            return true;
        }

        [TestMethod()]
        public void CheckForRedundancyTestCase1()
        {
            var f = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "false" });
            var t = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "true" });
            var a = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "ActionYes" });
            var e = GetActionEmptyState(DecisionTable);
            var n = GetConditionNoPreferenceState(DecisionTable);
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionDialogModel>();
            var actions = new List<ActionDialogModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Condition {0}", i),
                });
            for (int i = 0; i < 2; i++)
                actions.Add(new ActionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Action {0}", i)
                });

            ViewModelService.Instance.InsertViewModels(conditions, DecisionTable.EntityId, "Conditions");
            ViewModelService.Instance.InsertViewModels(actions, DecisionTable.EntityId, "Actions");

            foreach (var condition in DecisionTable.Conditions)
                ViewModelService.Instance.InsertViewModels(validConditionStates, condition.EntityId, "ValidStates");

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, t, t, t, f, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { t, t, f, f, t, t, f, f }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { t, f, t, f, t, f, t, f }.ToList() },
            		            };
            var actualActionStates = new Dictionary<ActionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Actions[0], new StateViewModel[] { t, e, e, e, e, t, e, e }.ToList() },
            		                { DecisionTable.Actions[1], new StateViewModel[] { t, t, e, t, t, e, t, e }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                var actionStates = new Dictionary<ActionViewModel, StateViewModel>();
                foreach (var pair in actualActionStates)
                    actionStates.Add(pair.Key, pair.Value[ruleIndex]);

                DecisionTable.Rules.Add(new RuleViewModel
                {
                    Index = ruleIndex,
                    ConditionStates = conditionStates,
                    ActionStates = actionStates
                });
            }

            List<RuleViewModel> actualRedundantRules = null;
            var hasRedundantRules = DecisionTableViewModelUtils.Instance.CheckForRedundancy(DecisionTable, out actualRedundantRules);

            Assert.IsFalse(hasRedundantRules);
            Assert.AreEqual(0, actualRedundantRules.Count);
        }

        [TestMethod()]
        public void CheckForRedundancyTestCase2()
        {
            var f = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "false" });
            var t = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "true" });
            var a = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "ActionYes" });
            var e = GetActionEmptyState(DecisionTable);
            var n = GetConditionNoPreferenceState(DecisionTable);
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionDialogModel>();
            var actions = new List<ActionDialogModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Condition {0}", i),
                });
            for (int i = 0; i < 2; i++)
                actions.Add(new ActionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Action {0}", i)
                });

            ViewModelService.Instance.InsertViewModels(conditions, DecisionTable.EntityId, "Conditions");
            ViewModelService.Instance.InsertViewModels(actions, DecisionTable.EntityId, "Actions");

            foreach (var condition in DecisionTable.Conditions)
                ViewModelService.Instance.InsertViewModels(validConditionStates, condition.EntityId, "ValidStates");

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
                                    //redundant: 2 (same as 1), 7 (4)
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, t, t, t, t, f, f, t, f, f }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { t, t, t, f, f, t, t, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { t, f, f, t, f, t, f, f, t, f }.ToList() },
            		            };
            var actualActionStates = new Dictionary<ActionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Actions[0], new StateViewModel[] { t, e, e, e, e, e, t, e, e, e }.ToList() },
            		                { DecisionTable.Actions[1], new StateViewModel[] { t, t, t, e, t, t, e, t, t, e }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                var actionStates = new Dictionary<ActionViewModel, StateViewModel>();
                foreach (var pair in actualActionStates)
                    actionStates.Add(pair.Key, pair.Value[ruleIndex]);

                DecisionTable.Rules.Add(new RuleViewModel
                {
                    Index = ruleIndex,
                    ConditionStates = conditionStates,
                    ActionStates = actionStates
                });
            }

            var expectedRedundantRules = new[] { 2, 7 };

            List<RuleViewModel> actualRedundantRules = null;
            var hasRedundantRules = DecisionTableViewModelUtils.Instance.CheckForRedundancy(DecisionTable, out actualRedundantRules);

            Assert.IsTrue(hasRedundantRules);
            Assert.IsTrue(actualRedundantRules.Count == expectedRedundantRules.Count(), "Wrong redundant rules count.");
            foreach (var redundantRule in actualRedundantRules)
            {
                bool isExpected = expectedRedundantRules.Any(cur => cur == redundantRule.Index);
                Assert.IsTrue(isExpected);
            }
        }

        [TestMethod()]
        public void CheckForRedundancyTestCase3()
        {
            var f = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "false" });
            var t = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "true" });
            var a = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "ActionYes" });
            var e = GetActionEmptyState(DecisionTable);
            var n = GetConditionNoPreferenceState(DecisionTable);
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionDialogModel>();
            var actions = new List<ActionDialogModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Condition {0}", i),
                });
            for (int i = 0; i < 2; i++)
                actions.Add(new ActionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Action {0}", i)
                });

            ViewModelService.Instance.InsertViewModels(conditions, DecisionTable.EntityId, "Conditions");
            ViewModelService.Instance.InsertViewModels(actions, DecisionTable.EntityId, "Actions");

            foreach (var condition in DecisionTable.Conditions)
                ViewModelService.Instance.InsertViewModels(validConditionStates, condition.EntityId, "ValidStates");

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
                                    // more than one rule covered using empty
                                    // 3 (covers 2), 9 (5, 6, 7, 8)
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, t, t, t, t, f, f, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { t, t, f, f, f, t, t, f, n, n }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { t, f, t, n, f, t, f, n, f, n }.ToList() },
            		            };
            var actualActionStates = new Dictionary<ActionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Actions[0], new StateViewModel[] { a, e, e, e, e, e, e, e, e, e }.ToList() },
            		                { DecisionTable.Actions[1], new StateViewModel[] { a, e, e, e, a, a, a, a, a, a }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                var actionStates = new Dictionary<ActionViewModel, StateViewModel>();
                foreach (var pair in actualActionStates)
                    actionStates.Add(pair.Key, pair.Value[ruleIndex]);

                DecisionTable.Rules.Add(new RuleViewModel
                {
                    Index = ruleIndex,
                    ConditionStates = conditionStates,
                    ActionStates = actionStates
                });
            }
            DecisionTable = ViewModelService.Instance.CommitViewModelAndQueryResult<DecisionTableViewModel>(DecisionTable);

            var expectedRedundantRules = new[] { 2, 5, 6, 7, 8 };

            List<RuleViewModel> actualRedundantRules = null;
            var hasRedundantRules = DecisionTableViewModelUtils.Instance.CheckForRedundancy(DecisionTable, out actualRedundantRules);

            Assert.IsTrue(hasRedundantRules);
            Assert.IsTrue(actualRedundantRules.Count == expectedRedundantRules.Count(), "Wrong redundant rules count.");
            foreach (var redundantRule in actualRedundantRules)
            {
                bool isExpected = expectedRedundantRules.Any(cur => cur == redundantRule.Index);
                Assert.IsTrue(isExpected);
            }
        }

        [TestMethod()]
        public void CheckForContradictionTestCase1()
        {
            var f = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "false" });
            var t = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "true" });
            var a = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "ActionYes" });
            var e = GetActionEmptyState(DecisionTable);
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionDialogModel>();
            var actions = new List<ActionDialogModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Condition {0}", i),
                });
            for (int i = 0; i < 2; i++)
                actions.Add(new ActionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Action {0}", i)
                });

            ViewModelService.Instance.InsertViewModels(conditions, DecisionTable.EntityId, "Conditions");
            ViewModelService.Instance.InsertViewModels(actions, DecisionTable.EntityId, "Actions");

            foreach (var condition in DecisionTable.Conditions)
                ViewModelService.Instance.InsertViewModels(validConditionStates, condition.EntityId, "ValidStates");

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
                                    //contradicted: 0,1 / 2,4 / 3,6,7
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, t, t, t, t, f, t, t, f, f }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { t, t, t, f, t, t, f, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { t, t, f, t, f, t, t, t, t, f }.ToList() },
            		            };
            var actualActionStates = new Dictionary<ActionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Actions[0], new StateViewModel[] { a, e, e, e, a, e, a, e, e, e }.ToList() },
            		                { DecisionTable.Actions[1], new StateViewModel[] { a, a, a, e, a, a, e, a, a, e }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                var actionStates = new Dictionary<ActionViewModel, StateViewModel>();
                foreach (var pair in actualActionStates)
                    actionStates.Add(pair.Key, pair.Value[ruleIndex]);

                DecisionTable.Rules.Add(new RuleViewModel
                {
                    Index = ruleIndex,
                    ConditionStates = conditionStates,
                    ActionStates = actionStates
                });
            }

            var expectedContradictedRules = new[]
            {
                new { IndexA = 0, IndexB = 1 },
                new { IndexA = 2, IndexB = 4 },
                new { IndexA = 3, IndexB = 6 },
                new { IndexA = 3, IndexB = 7 },
                new { IndexA = 6, IndexB = 7 },
            };

            List<ContradictionGroup> actualContradictedRules = null;
            var isContradicted = DecisionTableViewModelUtils.Instance.CheckForContradiction(DecisionTable, out actualContradictedRules);

            Assert.IsTrue(isContradicted);
            Assert.IsTrue(actualContradictedRules.Count == expectedContradictedRules.Count(), "Wrong contradicted rules count.");
            foreach (var actualContradictedPair in actualContradictedRules)
            {
                var indexA = actualContradictedPair.RuleA.Index;
                var indexB = actualContradictedPair.RuleB.Index;

                bool isExpected = expectedContradictedRules.Any(cur =>
                    {
                        return (cur.IndexA == indexA && cur.IndexB == indexB) ||
                            cur.IndexA == indexB && cur.IndexB == indexA;
                    });

                Assert.IsTrue(isExpected);
            }
        }

        [TestMethod()]
        public void CheckForContradictionTestCase2()
        {
            var f = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "false" });
            var t = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "true" });
            var a = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "ActionYes" });
            var e = GetActionEmptyState(DecisionTable);
            var n = GetConditionNoPreferenceState(DecisionTable);
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionDialogModel>();
            var actions = new List<ActionDialogModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Condition {0}", i),
                });
            for (int i = 0; i < 2; i++)
                actions.Add(new ActionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Action {0}", i)
                });

            ViewModelService.Instance.InsertViewModels(conditions, DecisionTable.EntityId, "Conditions");
            ViewModelService.Instance.InsertViewModels(actions, DecisionTable.EntityId, "Actions");

            foreach (var condition in DecisionTable.Conditions)
                ViewModelService.Instance.InsertViewModels(validConditionStates, condition.EntityId, "ValidStates");

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
                                    //contradicted: 0,1
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, t }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { f, f }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { f, n }.ToList() },
            		            };                                                        
            var actualActionStates = new Dictionary<ActionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Actions[0], new StateViewModel[] { a, a }.ToList() },
            		                { DecisionTable.Actions[1], new StateViewModel[] { e, a }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                var actionStates = new Dictionary<ActionViewModel, StateViewModel>();
                foreach (var pair in actualActionStates)
                    actionStates.Add(pair.Key, pair.Value[ruleIndex]);

                DecisionTable.Rules.Add(new RuleViewModel
                {
                    Index = ruleIndex,
                    ConditionStates = conditionStates,
                    ActionStates = actionStates
                });
            }

            var expectedContradictedRules = new[]
            {
                new { IndexA = 0, IndexB = 1 },
            };

            List<ContradictionGroup> actualContradictedRules = null;
            var isContradicted = DecisionTableViewModelUtils.Instance.CheckForContradiction(DecisionTable, out actualContradictedRules);

            Assert.IsTrue(isContradicted);
            Assert.IsTrue(actualContradictedRules.Count == expectedContradictedRules.Count(), "Wrong contradicted rules count.");
            foreach (var actualContradictedPair in actualContradictedRules)
            {
                var indexA = actualContradictedPair.RuleA.Index;
                var indexB = actualContradictedPair.RuleB.Index;

                bool isExpected = expectedContradictedRules.Any(cur =>
                {
                    return (cur.IndexA == indexA && cur.IndexB == indexB) ||
                        cur.IndexA == indexB && cur.IndexB == indexA;
                });

                Assert.IsTrue(isExpected);
            }
        }

        [TestMethod()]
        public void ExtendRulesTestCase1()
        {
            var f = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "false" });
            var t = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "true" });
            var a = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "ActionYes" });
            var e = GetActionEmptyState(DecisionTable);
            var n = GetConditionNoPreferenceState(DecisionTable);
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionDialogModel>();
            var actions = new List<ActionDialogModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Condition {0}", i),
                });
            for (int i = 0; i < 2; i++)
                actions.Add(new ActionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Action {0}", i)
                });
            
            ViewModelService.Instance.InsertViewModels(conditions, DecisionTable.EntityId, "Conditions");
            ViewModelService.Instance.InsertViewModels(actions, DecisionTable.EntityId, "Actions");

            foreach (var condition in DecisionTable.Conditions)
                ViewModelService.Instance.InsertViewModels(validConditionStates, condition.EntityId, "ValidStates");

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { n, t, f, f }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { n, n, t, f }.ToList() },
            		            };
            var actualActionStates = new Dictionary<ActionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Actions[0], new StateViewModel[] { a, a, a, e }.ToList() },
            		                { DecisionTable.Actions[1], new StateViewModel[] { a, e, e, a }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                var actionStates = new Dictionary<ActionViewModel, StateViewModel>();
                foreach (var pair in actualActionStates)
                    actionStates.Add(pair.Key, pair.Value[ruleIndex]);

                DecisionTable.Rules.Add(new RuleViewModel
                {
                    Index = ruleIndex,
                    ConditionStates = conditionStates,
                    ActionStates = actionStates
                });
            }

            var expectedExtendedRules = new[]
                {
                    new RuleViewModel
                    {
                        Index = 0,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], t },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], t },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 1,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], t },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], f },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 2,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], t },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], t },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 3,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], t },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], f },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 4,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], t },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 5,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], f },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 6,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], t },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 7,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], f },
                        }
                    }
                }.ToList();

            List<RuleViewModel> actualExtendedRules = null;
            var isExtended = DecisionTableViewModelUtils.Instance.ExtendRules(DecisionTable, out actualExtendedRules);
            actualExtendedRules = DecisionTableViewModelUtils.Instance.ReindexRules(actualExtendedRules.OrderBy(cur => cur.Index));

            Assert.IsTrue(isExtended);
            Assert.IsTrue(actualExtendedRules.Count == expectedExtendedRules.Count, "Wrong extended rules count.");
            for (int i = 0; i < actualExtendedRules.Count; i++)
            {
                Assert.IsTrue(CompareConditions(expectedExtendedRules[i], actualExtendedRules[i]));
            }
        }

        /// <summary>
        ///A test for ReindexRules
        ///</summary>
        [TestMethod()]
        public void ReindexRulesTest()
        {
            var f = new StateViewModel { Name = "false" };
            var t = new StateViewModel { Name = "true" };
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionViewModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionViewModel
                {
                    Name = string.Format("Condition {0}", i),
                });

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
            		                { conditions[0], new StateViewModel[] { t, t, t, t, f, f, f, f }.ToList() },
            		                { conditions[1], new StateViewModel[] { t, t, f, f, t, t, f, f }.ToList() },
            		                { conditions[2], new StateViewModel[] { t, f, t, f, t, f, t, f }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            var expectedOrder = new List<RuleViewModel>();
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                RuleViewModel newRule = new RuleViewModel
                                {
                                    Index = ruleIndex * 2,
                                    ConditionStates = conditionStates,
                                };
                DecisionTable.Rules.Add(newRule);
                expectedOrder.Add(newRule);
            }

            var actual = DecisionTableViewModelUtils.Instance.ReindexRules(DecisionTable.Rules.Reverse<RuleViewModel>());
            CollectionAssert.AreEqual(expectedOrder, actual);
            for (int i = 0; i < actual.Count; i++)
                Assert.AreEqual(i, actual[i].Index);
        }

        [TestMethod()]
        public void SimplifyTestCase1()
        {
            var f = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "false" });
            var t = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "true" });
            var a = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "ActionYes" });
            var e = GetActionEmptyState(DecisionTable);
            var n = GetConditionNoPreferenceState(DecisionTable);
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionDialogModel>();
            var actions = new List<ActionDialogModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Condition {0}", i),
                });
            for (int i = 0; i < 2; i++)
                actions.Add(new ActionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Action {0}", i)
                });

            ViewModelService.Instance.InsertViewModels(conditions, DecisionTable.EntityId, "Conditions");
            ViewModelService.Instance.InsertViewModels(actions, DecisionTable.EntityId, "Actions");

            foreach (var condition in DecisionTable.Conditions)
                ViewModelService.Instance.InsertViewModels(validConditionStates, condition.EntityId, "ValidStates");

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, t, t, t, f, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { t, t, f, f, t, t, f, f }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { t, f, t, f, t, f, t, f }.ToList() },
            		            };
            var actualActionStates = new Dictionary<ActionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Actions[0], new StateViewModel[] { a, a, a, a, a, a, a, e }.ToList() },
            		                { DecisionTable.Actions[1], new StateViewModel[] { e, e, e, e, e, e, e, a }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                var actionStates = new Dictionary<ActionViewModel, StateViewModel>();
                foreach (var pair in actualActionStates)
                    actionStates.Add(pair.Key, pair.Value[ruleIndex]);

                DecisionTable.Rules.Add(new RuleViewModel
                {
                    Index = ruleIndex,
                    ConditionStates = conditionStates,
                    ActionStates = actionStates
                });
            }

            var expectedSimplifiedRules = new[]
                {
                    new RuleViewModel
                    {
                        Index = 0,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], t },
                            { DecisionTable.Conditions[1], n },
                            { DecisionTable.Conditions[2], n },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 1,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], n },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 2,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], t },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 3,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], f },
                        }
                    }
                }.ToList();

            List<RuleViewModel> actualSimplifiedRules = null;
            var isSimplified = DecisionTableViewModelUtils.Instance.Simplify(DecisionTable, out actualSimplifiedRules);
            actualSimplifiedRules = DecisionTableViewModelUtils.Instance.ReindexRules(actualSimplifiedRules.OrderBy(cur => cur.Index));

            Assert.IsTrue(isSimplified);
            Assert.IsTrue(actualSimplifiedRules.Count == expectedSimplifiedRules.Count, "Wrong simplified rules count.");
            for (int i = 0; i < actualSimplifiedRules.Count; i++)
            {
                Assert.IsTrue(CompareConditions(expectedSimplifiedRules[i], actualSimplifiedRules[i]));
            }
        }

        [TestMethod()]
        public void SimplifyTestCase2()
        {
            var f = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "false" });
            var t = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "true" });
            var a = ViewModelService.Instance.InsertViewModelAndQueryResult<StateViewModel>(new StateViewModel { Name = "ActionYes" });
            var e = GetActionEmptyState(DecisionTable);
            var n = GetConditionNoPreferenceState(DecisionTable);
            var validConditionStates = new List<StateViewModel>(new[] { t, f });

            var conditions = new List<ConditionDialogModel>();
            var actions = new List<ActionDialogModel>();
            for (int i = 0; i < 3; i++)
                conditions.Add(new ConditionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Condition {0}", i),
                });
            for (int i = 0; i < 2; i++)
                actions.Add(new ActionDialogModel
                {
                    DecisionTableId = DecisionTable.EntityId,
                    Name = string.Format("Action {0}", i)
                });

            ViewModelService.Instance.InsertViewModels(conditions, DecisionTable.EntityId, "Conditions");
            ViewModelService.Instance.InsertViewModels(actions, DecisionTable.EntityId, "Actions");

            foreach (var condition in DecisionTable.Conditions)
                ViewModelService.Instance.InsertViewModels(validConditionStates, condition.EntityId, "ValidStates");

            var actualConditionStates = new Dictionary<ConditionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Conditions[0], new StateViewModel[] { t, f, f, f, f }.ToList() },
            		                { DecisionTable.Conditions[1], new StateViewModel[] { n, t, t, f, f }.ToList() },
            		                { DecisionTable.Conditions[2], new StateViewModel[] { n, t, f, t, f }.ToList() },
            		            };
            var actualActionStates = new Dictionary<ActionViewModel, List<StateViewModel>>
            		            {
            		                { DecisionTable.Actions[0], new StateViewModel[] { a, e, a, e, e }.ToList() },
            		                { DecisionTable.Actions[1], new StateViewModel[] { e, a, a, a, a }.ToList() },
            		            };

            var ruleCount = actualConditionStates.First().Value.Count;
            for (int ruleIndex = 0; ruleIndex < ruleCount; ruleIndex++)
            {
                var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>();
                foreach (var pair in actualConditionStates)
                    conditionStates.Add(pair.Key, pair.Value[ruleIndex]);

                var actionStates = new Dictionary<ActionViewModel, StateViewModel>();
                foreach (var pair in actualActionStates)
                    actionStates.Add(pair.Key, pair.Value[ruleIndex]);

                DecisionTable.Rules.Add(new RuleViewModel
                {
                    Index = ruleIndex,
                    ConditionStates = conditionStates,
                    ActionStates = actionStates
                });
            }

            var expectedSimplifiedRules = new[]
                {
                    new RuleViewModel
                    {
                        Index = 0,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], t },
                            { DecisionTable.Conditions[1], n },
                            { DecisionTable.Conditions[2], n },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 1,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], t },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 2,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], t },
                            { DecisionTable.Conditions[2], f },
                        }
                    },
                    new RuleViewModel
                    {
                        Index = 3,
                        ConditionStates = new Dictionary<ConditionViewModel,StateViewModel>
                        {
                            { DecisionTable.Conditions[0], f },
                            { DecisionTable.Conditions[1], f },
                            { DecisionTable.Conditions[2], n },
                        }
                    }
                }.ToList();

            List<RuleViewModel> actualSimplifiedRules = null;
            var isSimplified = DecisionTableViewModelUtils.Instance.Simplify(DecisionTable, out actualSimplifiedRules);
            actualSimplifiedRules = DecisionTableViewModelUtils.Instance.ReindexRules(actualSimplifiedRules.OrderBy(cur => cur.Index));

            Assert.IsTrue(isSimplified);
            Assert.IsTrue(actualSimplifiedRules.Count == expectedSimplifiedRules.Count, "Wrong simplified rules count.");
            for (int i = 0; i < actualSimplifiedRules.Count; i++)
            {
                Assert.IsTrue(CompareConditions(expectedSimplifiedRules[i], actualSimplifiedRules[i]));
            }
        }

    }
}
