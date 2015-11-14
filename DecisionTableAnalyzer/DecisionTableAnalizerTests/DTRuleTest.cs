using DecisionTableAnalyzer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTableAnalizerTests
{
    
    
    /// <summary>
    ///This is a test class for DTRuleTest and is intended
    ///to contain all DTRuleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DTRuleTest
    {

        private List<DTRuleElement> ConditionRuleElements { get; set; }
        private List<DTRuleElement> ActionRuleElements { get; set; }

        private TestContext testContextInstance;

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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
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
            ConditionRuleElements = new List<DTRuleElement>(InitRuleElements(5));
            ActionRuleElements = new List<DTRuleElement>(InitRuleElements(3));
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private IEnumerable<DTRuleElement> InitRuleElements(int count)
        {
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
                yield return new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = rnd.Next(1) == 0 ? DTState.No : DTState.Yes };
        }

        /// <summary>
        ///A test for DTRule Constructor
        ///</summary>
        [TestMethod()]
        public void DTRuleConstructorTest()
        {
            int ruleIndex = 5;
            IEnumerable<DTRuleElement> conditions = ConditionRuleElements;
            IEnumerable<DTRuleElement> actions = ActionRuleElements;
            DTRule rule = new DTRule(ruleIndex, conditions, actions);

            Assert.AreEqual(rule.Index, ruleIndex);
            Assert.AreEqual(conditions.Count(), rule.ConditionRuleElements.Count);
            Assert.AreEqual(actions.Count(), rule.ActionRuleElements.Count);
        }

        /// <summary>
        ///A test for CompareElements
        ///</summary>
        [TestMethod()]
        [DeploymentItem("DecisionTableAnalyzer.exe")]
        public void CompareElementsTest()
        {
            List<DTRuleElement> testConditions = new List<DTRuleElement>
                (new DTRuleElement[] {
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.Yes },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.No },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.No },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.Yes },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.No },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.Yes }});
            List<DTRuleElement> testActions = new List<DTRuleElement>
                (new DTRuleElement[] {
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.No },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.No },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.Yes },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.Yes }});

            List<DTRuleElement> changedConditions = new List<DTRuleElement>
                (new DTRuleElement[] {
                    new DTRuleElement { ElementId = testConditions[0].ElementId, State = testConditions[0].State },
                    new DTRuleElement { ElementId = testConditions[1].ElementId, State = DTState.Yes }, //Changed
                    new DTRuleElement { ElementId = testConditions[2].ElementId, State = DTState.Yes }, //Changed
                    new DTRuleElement { ElementId = testConditions[3].ElementId, State = testConditions[3].State },
                    new DTRuleElement { ElementId = testConditions[4].ElementId, State = DTState.Yes }, //Changed
                    new DTRuleElement { ElementId = testConditions[5].ElementId, State = DTState.No }}); //Changed

            List<DTRuleElement> changedActions = new List<DTRuleElement>
                (new DTRuleElement[] {
                    new DTRuleElement { ElementId = testActions[0].ElementId, State = testActions[0].State },
                    new DTRuleElement { ElementId = testActions[1].ElementId, State = DTState.Yes }, //Changed
                    new DTRuleElement { ElementId = testActions[2].ElementId, State = DTState.No }, //Changed
                    new DTRuleElement { ElementId = testActions[3].ElementId, State = testActions[3].State }});

            DTRule_Accessor target = new DTRule_Accessor(1, changedConditions, changedActions);
            DTRule rule = new DTRule(0, testConditions, testActions);
            
            List<DTRuleElement> allElements = new List<DTRuleElement>(rule.ConditionRuleElements);
            allElements.AddRange(rule.ActionRuleElements);
            IEnumerable<DTRuleElement> elements = allElements;

            RuleComparison ruleComparison = RuleComparison.DifferentStates;
            List<string> expected = new List<string>
                (new string[] {
                    testConditions[1].ElementId,
                    testConditions[2].ElementId,
                    testConditions[4].ElementId,
                    testConditions[5].ElementId,
                    testActions[1].ElementId,
                    testActions[2].ElementId});

            IEnumerable<string> actual = target.CompareElements(rule, elements, ruleComparison);
            CollectionAssert.AreEqual(expected, actual.ToList());

            ruleComparison = RuleComparison.MatchingStates;
            actual = target.CompareElements(rule, elements, ruleComparison);
            expected = expected = new List<string>
                (new string[] {
                    testConditions[0].ElementId,
                    testConditions[3].ElementId,
                    testActions[0].ElementId,
                    testActions[3].ElementId});
            CollectionAssert.AreEqual(expected, actual.ToList());
        }

        /// <summary>
        ///A test for Merge
        ///</summary>
        [TestMethod()]
        public void MergeTest()
        {
            List<DTRuleElement> testConditions = new List<DTRuleElement>
                (new DTRuleElement[] {
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.Yes },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.No },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.No },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.Yes },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.No },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.Yes }});
            List<DTRuleElement> testActions = new List<DTRuleElement>
                (new DTRuleElement[] {
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.No },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.No },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.Yes },
                    new DTRuleElement { ElementId = Guid.NewGuid().ToString(), State = DTState.Yes }});

            List<DTRuleElement> changedConditions = new List<DTRuleElement>
                (new DTRuleElement[] {
                    new DTRuleElement { ElementId = testConditions[0].ElementId, State = testConditions[0].State },
                    new DTRuleElement { ElementId = testConditions[1].ElementId, State = DTState.Yes }, //Changed
                    new DTRuleElement { ElementId = testConditions[2].ElementId, State = DTState.Yes }, //Changed
                    new DTRuleElement { ElementId = testConditions[3].ElementId, State = testConditions[3].State },
                    new DTRuleElement { ElementId = testConditions[4].ElementId, State = DTState.Yes }, //Changed
                    new DTRuleElement { ElementId = testConditions[5].ElementId, State = DTState.No }}); //Changed

            List<DTRuleElement> changedActions = new List<DTRuleElement>
                (new DTRuleElement[] {
                    new DTRuleElement { ElementId = testActions[0].ElementId, State = testActions[0].State },
                    new DTRuleElement { ElementId = testActions[1].ElementId, State = DTState.Yes }, //Changed
                    new DTRuleElement { ElementId = testActions[2].ElementId, State = DTState.No }, //Changed
                    new DTRuleElement { ElementId = testActions[3].ElementId, State = testActions[3].State }});

            int ruleIndex = 0;
            DTRule target = new DTRule(ruleIndex, testConditions, testActions);
            DTRule mergeRule = new DTRule(1, changedConditions, changedActions);
            target.Merge(mergeRule);

            //Conditions should be merged
            Assert.AreEqual(target.GetState(testConditions[0].ElementId), testConditions[0].State);
            Assert.AreEqual(target.GetState(testConditions[1].ElementId), DTState.Empty);
            Assert.AreEqual(target.GetState(testConditions[2].ElementId), DTState.Empty);
            Assert.AreEqual(target.GetState(testConditions[3].ElementId), testConditions[3].State);
            Assert.AreEqual(target.GetState(testConditions[4].ElementId), DTState.Empty);
            Assert.AreEqual(target.GetState(testConditions[5].ElementId), DTState.Empty);

            //Actions should not be merged
            Assert.AreEqual(target.GetState(changedActions[0].ElementId), testActions[0].State);
            Assert.AreEqual(target.GetState(changedActions[1].ElementId), testActions[1].State);
            Assert.AreEqual(target.GetState(changedActions[2].ElementId), testActions[2].State);
            Assert.AreEqual(target.GetState(changedActions[3].ElementId), testActions[3].State);
        }

        /// <summary>
        ///A test for GetState
        ///</summary>
        [TestMethod()]
        public void GetStateTest()
        {
            int ruleIndex = 0;
            IEnumerable<DTRuleElement> conditions = ConditionRuleElements;
            IEnumerable<DTRuleElement> actions = ActionRuleElements;
            DTRule target = new DTRule(ruleIndex, conditions, actions);

            foreach (var condition in ConditionRuleElements)
                Assert.AreEqual(target.GetState(condition.ElementId), condition.State);
            foreach (var action in ActionRuleElements)
                Assert.AreEqual(target.GetState(action.ElementId), action.State);
        }
    }
}
