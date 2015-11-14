using DecisionTableAnalyzer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace DecisionTableAnalizerTests
{
    
    
    /// <summary>
    ///This is a test class for DTRuleSetTest and is intended
    ///to contain all DTRuleSetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DTRuleSetTest
    {


        private TestContext testContextInstance;
        private Dictionary<DTElement, List<DTState>> ElementData { get; set; }

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
            ElementData = new Dictionary<DTElement, List<DTState>>();
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                DTElement element = new DTElement
                {
                    Name = string.Format("Element {0}", i),
                    Kind = rnd.Next(1) == 0 ? DTElementKind.Condition : DTElementKind.Action
                };
                List<DTState> states = new List<DTState>();
                for (int stateIndex = 0; stateIndex < 10; stateIndex++)
                    states.Add(rnd.Next(1) == 0 ? DTState.Yes : DTState.No);

                ElementData.Add(element, states);
            }
        }

        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for InitializeRules
        ///</summary>
        [TestMethod()]
        public void InitializeRulesTest()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var actual = DTRuleSet.InitializeRules(elements);
            Assert.IsTrue(actual.Rules.Count == 8);

            var f = DTState.No;
            var t = DTState.Yes;

            var firstConditionStates = new DTState[] { t, t, t, t, f, f, f, f };
            var secondConditionStates = new DTState[] { t, t, f, f, t, t, f, f };
            var thirdConditionStates = new DTState[] { t, f, t, f, t, f, t, f };
            var actionStates = Enumerable.Repeat(DTState.Empty, 8).ToList();

            CollectionAssert.AreEqual(actual.GetStates(elements[0]).ToList(), firstConditionStates);
            CollectionAssert.AreEqual(actual.GetStates(elements[1]).ToList(), secondConditionStates);
            CollectionAssert.AreEqual(actual.GetStates(elements[2]).ToList(), thirdConditionStates);

            CollectionAssert.AreEqual(actual.GetStates(elements[3]).ToList(), actionStates);
            CollectionAssert.AreEqual(actual.GetStates(elements[4]).ToList(), actionStates);
        }

        /// <summary>
        ///A test for GetStates
        ///</summary>
        [TestMethod()]
        public void GetStatesTest()
        {
            DTRuleSet_Accessor target = new DTRuleSet_Accessor(ElementData);
            foreach (var dataPair in ElementData)
            {
                CollectionAssert.AreEqual(dataPair.Value, target.GetStates(dataPair.Key).ToList());
            }
        }

        /// <summary>
        ///A test for InitRules
        ///</summary>
        [TestMethod()]
        [DeploymentItem("DecisionTableAnalyzer.exe")]
        public void InitRulesTest()
        {
            DTRuleSet_Accessor target = new DTRuleSet_Accessor(new Dictionary<DTElement, List<DTState>>());
            Dictionary<DTElement, List<DTState>> elementData = ElementData;
            target.SetupRules(elementData);

            for (int ruleIndex = 0; ruleIndex < target.Rules.Count; ruleIndex++)
            {
                DTRule curRule = target.Rules[ruleIndex];
                foreach (var element in elementData.Keys)
                    Assert.AreEqual(curRule.GetState(element.Id), elementData[element][ruleIndex]);
            }
        }

        /// <summary>
        ///A test for Simplify
        ///</summary>
        [TestMethod()]
        public void SimplifyTest()
        {
            SimplifyTestCase1();
            SimplifyTestCase2();
        }

        internal void SimplifyTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var actual = DTRuleSet.InitializeRules(elements);
            Assert.IsTrue(actual.Rules.Count == 8);

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            //Conditions
            var states0 = new DTState[] { t, t, t, t, f, f, f, f };
            var states1 = new DTState[] { t, t, f, f, t, t, f, f };
            var states2 = new DTState[] { t, f, t, f, t, f, t, f };
            //Actions
            var states3 = new DTState[] { a, a, a, a, a, a, a, e };
            var states4 = new DTState[] { e, e, e, e, e, e, e, a };

            elements[0].SetStates(states0);
            elements[1].SetStates(states1);
            elements[2].SetStates(states2);
            elements[3].SetStates(states3);
            elements[4].SetStates(states4);

            DTRuleSet target = DTRuleSet.Simplify(elements);

            //Conditions
            var expectedStates0 = new DTState[] { t, f, f, f };
            var expectedStates1 = new DTState[] { e, t, f, f };
            var expectedStates2 = new DTState[] { e, e, t, f };
            //Actions
            var expectedStates3 = new DTState[] { a, a, a, e };
            var expectedStates4 = new DTState[] { e, e, e, a };

            CollectionAssert.AreEqual(target.GetStates(elements[0]).ToList(), expectedStates0.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[1]).ToList(), expectedStates1.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[2]).ToList(), expectedStates2.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[3]).ToList(), expectedStates3.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[4]).ToList(), expectedStates4.ToList());
        }

        internal void SimplifyTestCase2()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var actual = DTRuleSet.InitializeRules(elements);
            Assert.IsTrue(actual.Rules.Count == 8);

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            //Conditions
            var states0 = new DTState[] { t, f, f, f, f };
            var states1 = new DTState[] { e, t, t, f, f };
            var states2 = new DTState[] { e, t, f, t, f };
            //Actions
            var states3 = new DTState[] { a, e, a, e, e };
            var states4 = new DTState[] { e, a, a, a, a };

            elements[0].SetStates(states0);
            elements[1].SetStates(states1);
            elements[2].SetStates(states2);
            elements[3].SetStates(states3);
            elements[4].SetStates(states4);

            DTRuleSet target = DTRuleSet.Simplify(elements);

            //Conditions
            var expectedStates0 = new DTState[] { t, f, f, f };
            var expectedStates1 = new DTState[] { e, t, t, f };
            var expectedStates2 = new DTState[] { e, t, f, e };
            //Actions
            var expectedStates3 = new DTState[] { a, e, a, e };
            var expectedStates4 = new DTState[] { e, a, a, a };

            CollectionAssert.AreEqual(target.GetStates(elements[0]).ToList(), expectedStates0.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[1]).ToList(), expectedStates1.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[2]).ToList(), expectedStates2.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[3]).ToList(), expectedStates3.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[4]).ToList(), expectedStates4.ToList());
        }

        /// <summary>
        ///A test for Extend
        ///</summary>
        [TestMethod()]
        public void ExtendTest()
        {
            ExtendTestCase1();
        }

        internal void ExtendTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var actual = DTRuleSet.InitializeRules(elements);
            Assert.IsTrue(actual.Rules.Count == 8);

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            //Conditions
            var states0 = new DTState[] { t, f, f, f };
            var states1 = new DTState[] { e, t, f, f };
            var states2 = new DTState[] { e, e, t, f };
            //Actions
            var states3 = new DTState[] { a, a, a, e };
            var states4 = new DTState[] { a, e, e, a };

            elements[0].SetStates(states0);
            elements[1].SetStates(states1);
            elements[2].SetStates(states2);
            elements[3].SetStates(states3);
            elements[4].SetStates(states4);

            DTRuleSet target = DTRuleSet.Extend(elements);

            //Conditions
            var expectedStates0 = new DTState[] { t, t, t, t, f, f, f, f };
            var expectedStates1 = new DTState[] { t, t, f, f, t, t, f, f };
            var expectedStates2 = new DTState[] { t, f, t, f, t, f, t, f };
            //Actions
            var expectedStates3 = new DTState[] { a, a, a, a, a, a, a, e };
            var expectedStates4 = new DTState[] { a, a, a, a, e, e, e, a };

            CollectionAssert.AreEqual(target.GetStates(elements[0]).ToList(), expectedStates0.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[1]).ToList(), expectedStates1.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[2]).ToList(), expectedStates2.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[3]).ToList(), expectedStates3.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[4]).ToList(), expectedStates4.ToList());
        }


        /// <summary>
        ///A test for CalculateRuleCount
        ///</summary>
        [TestMethod()]
        [DeploymentItem("DecisionTableAnalyzer.exe")]
        public void CalculateRuleCountTest()
        {
            CalculateRuleCountTestCase1();
            CalculateRuleCountTestCase2();
            CalculateRuleCountTestCase3();
        }

        private void CalculateRuleCountTestCase3()
        {
            DTState stateA = new DTState { Name = "A" };
            DTState stateB = new DTState { Name = "B" };

            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
            {
                DTElement newDTElement = new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition };
                elements.Add(newDTElement);
            }
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            elements[0].AddValidState(stateA);
            elements[0].AddValidState(stateB);

            elements[1].AddValidState(stateA);

            int expected = 24;
            int actual = DTRuleSet_Accessor.CalculateRuleCount(elements);
            Assert.AreEqual(expected, actual);
        }

        private void CalculateRuleCountTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            int expected = 8;
            int actual = DTRuleSet_Accessor.CalculateRuleCount(elements);
            Assert.AreEqual(expected, actual);
        }

        private void CalculateRuleCountTestCase2()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 6; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            int expected = 64;
            int actual = DTRuleSet_Accessor.CalculateRuleCount(elements);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SortRules
        ///</summary>
        [TestMethod()]
        public void SortRulesTest()
        {
            SortRulesTestCase1();
            SortRulesTestCase2();
        }

        private void SortRulesTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;
            
            var notOrdered = new List<List<DTState>>
            {
                new DTState[] { t, f, f, t, f, f, t, t }.ToList(),
                new DTState[] { f, t, f, t, t, f, t, f }.ToList(),
                new DTState[] { f, t, f, t, f, t, f, t }.ToList(),
                new DTState[] { e, e, e, a, a, e, e, e }.ToList(),
                new DTState[] { a, a, e, a, e, a, a, e }.ToList()
            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(notOrdered[i]);

            var ordered = new List<List<DTState>>
            {
                new DTState[] { t, t, t, t, f, f, f, f }.ToList(),
                new DTState[] { t, t, f, f, t, t, f, f }.ToList(),
                new DTState[] { t, f, t, f, t, f, t, f }.ToList(),
                new DTState[] { a, e, e, e, e, a, e, e }.ToList(),
                new DTState[] { a, a, e, a, a, e, a, e }.ToList()
            };

            DTRuleSet_Accessor actual = new DTRuleSet_Accessor(elements);
            for (int i = 0; i < elements.Count; i++)
            {
                CollectionAssert.AreNotEqual(ordered[i], actual.GetStates(elements[i]).ToList());
            }

            actual.SortRules();
            for (int i = 0; i < elements.Count; i++)
            {
                CollectionAssert.AreEqual(ordered[i], actual.GetStates(elements[i]).ToList());
            }

            DTRuleSet actual1 = DTRuleSet.SortRules(elements);
            for (int i = 0; i < elements.Count; i++)
            {
                CollectionAssert.AreEqual(ordered[i], actual1.GetStates(elements[i]).ToList());
            }
        }

        private void SortRulesTestCase2()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            var notOrdered = new List<List<DTState>>
            {
                new DTState[] { t, f, f, t, f, f, t, t, t, f }.ToList(),
                new DTState[] { f, t, f, t, t, f, t, f, t, f }.ToList(),
                new DTState[] { f, t, f, t, f, t, f, t, f, f }.ToList(),
                new DTState[] { e, e, e, a, a, e, e, e, e, e }.ToList(),
                new DTState[] { a, a, e, a, e, a, a, e, a, e }.ToList()
            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(notOrdered[i]);

            var ordered = new List<List<DTState>>
            {
                new DTState[] { t, t, t, t, t, f, f, f, f, f }.ToList(),
                new DTState[] { t, t, t, f, f, t, t, f, f, f }.ToList(),
                new DTState[] { t, f, f, t, f, t, f, t, f, f }.ToList(),
                new DTState[] { a, e, e, e, e, e, a, e, e, e }.ToList(),
                new DTState[] { a, a, a, e, a, a, e, a, e, e }.ToList()
            };

            DTRuleSet_Accessor actual = new DTRuleSet_Accessor(elements);
            for (int i = 0; i < elements.Count; i++)
            {
                CollectionAssert.AreNotEqual(ordered[i], actual.GetStates(elements[i]).ToList());
            }

            actual.SortRules();
            for (int i = 0; i < elements.Count; i++)
            {
                CollectionAssert.AreEqual(ordered[i], actual.GetStates(elements[i]).ToList());
            }

            DTRuleSet actual1 = DTRuleSet.SortRules(elements);
            for (int i = 0; i < elements.Count; i++)
            {
                CollectionAssert.AreEqual(ordered[i], actual1.GetStates(elements[i]).ToList());
            }
        }

        /// <summary>
        ///A test for CalculateConditionStates
        ///</summary>
        [TestMethod()]
        [DeploymentItem("DecisionTableAnalyzer.exe")]
        public void CalculateConditionStatesTest()
        {
            CalculateConditionStatesTestCase1();
            CalculateConditionStatesTestCase2();
        }

        private void CalculateConditionStatesTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var expected = new Dictionary<DTElement, List<DTState>>
            {
            { elements[0], new DTState[] { t, t, t, t, f, f, f, f }.ToList() },
            { elements[1], new DTState[] { t, t, f, f, t, t, f, f }.ToList() },
            { elements[2], new DTState[] { t, f, t, f, t, f, t, f }.ToList() },
            };

            Dictionary<DTElement, List<DTState>> actual = DTRuleSet_Accessor.CalculateConditionStates(elements);
            foreach (var pair in expected)
            {
                CollectionAssert.AreEqual(pair.Value, actual[pair.Key]);
            }
        }

        private void CalculateConditionStatesTestCase2()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 5; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var expected = new Dictionary<DTElement, List<DTState>>
            {
            { elements[0], new DTState[] { t, t, t, t, t, t, t, t, t, t, t, t, t, t, t, t, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, }.ToList() },
            { elements[1], new DTState[] { t, t, t, t, t, t, t, t, f, f, f, f, f, f, f, f, t, t, t, t, t, t, t, t, f, f, f, f, f, f, f, f, }.ToList() },
            { elements[2], new DTState[] { t, t, t, t, f, f, f, f, t, t, t, t, f, f, f, f, t, t, t, t, f, f, f, f, t, t, t, t, f, f, f, f, }.ToList() },
            { elements[3], new DTState[] { t, t, f, f, t, t, f, f, t, t, f, f, t, t, f, f, t, t, f, f, t, t, f, f, t, t, f, f, t, t, f, f, }.ToList() },
            { elements[4], new DTState[] { t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f, }.ToList() },
            };

            var actual = DTRuleSet_Accessor.CalculateConditionStates(elements);
            foreach (var pair in expected)
            {
                CollectionAssert.AreEqual(pair.Value, actual[pair.Key]);
            }
        }

        /// <summary>
        ///A test for CalculateActionStatesBasedOn
        ///</summary>
        [TestMethod()]
        [DeploymentItem("DecisionTableAnalyzer.exe")]
        public void CalculateActionStatesBasedOnTest()
        {
            CalculateActionStatesBasedOnTestCase1();
            CalculateActionStatesBasedOnTestCase2();
        }

        private static void CalculateActionStatesBasedOnTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            var ordered = new List<List<DTState>>
            		            {
            		                new DTState[] { t, t, t, t, f, f, f, f }.ToList(),
            		                new DTState[] { t, t, f, f, t, t, f, f }.ToList(),
            		                new DTState[] { t, f, t, f, t, f, t, f }.ToList(),
            		                new DTState[] { a, e, e, e, e, a, e, e }.ToList(),
            		                new DTState[] { a, a, e, a, a, e, a, e }.ToList()
            		            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(ordered[i]);

            DTRuleSet basedOn = DTRuleSet.FromElements(elements);
            elements.Insert(3, new DTElement { Name = "Condition 3", Kind = DTElementKind.Condition });

            var expected = new Dictionary<DTElement, List<DTState>>
            		            {
                                //{ elements[0], new DTState[] { t, t, t, t, t, t, t, t, f, f, f, f, f, f, f, f }.ToList() },
                                //{ elements[1], new DTState[] { t, t, t, t, f, f, f, f, t, t, t, t, f, f, f, f }.ToList() },
                                //{ elements[2], new DTState[] { t, t, f, f, t, t, f, f, t, t, f, f, t, t, f, f }.ToList() },
                                //{ elements[3], new DTState[] { t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f }.ToList() },
            		            { elements[4], new DTState[] { a, a, e, e, e, e, e, e, e, e, a, a, e, e, e, e }.ToList() },
            		            { elements[5], new DTState[] { a, a, a, a, e, e, a, a, a, a, e, e, a, a, e, e }.ToList() },
            		            };

            var actual = DTRuleSet_Accessor.CalculateActionStatesBasedOn(elements, basedOn);
            foreach (var pair in expected)
            {
                CollectionAssert.AreEqual(pair.Value, actual[pair.Key]);
            }
        }

        private static void CalculateActionStatesBasedOnTestCase2()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            var notOrdered = new List<List<DTState>>
            {
                new DTState[] { f, t, f, t }.ToList(),
                new DTState[] { f, f, t, t }.ToList(),
                new DTState[] { e, e, e, a }.ToList(),
                new DTState[] { a, a, e, a }.ToList()
            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(notOrdered[i]);

            DTRuleSet basedOn = DTRuleSet.FromElements(elements);
            elements.Insert(2, new DTElement { Name = "Condition 2", Kind = DTElementKind.Condition });
            elements.Insert(3, new DTElement { Name = "Condition 3", Kind = DTElementKind.Condition });

            var expected = new Dictionary<DTElement, List<DTState>>
            		            {
                                //{ elements[0], new DTState[] { f, f, f, f, t, t, t, t, f, f, f, f, t, t, t, t }.ToList() },
                                //{ elements[1], new DTState[] { f, f, f, f, f, f, f, f, t, t, t, t, t, t, t, t }.ToList() },
                                //{ elements[2], new DTState[] { t, t, f, f, t, t, f, f, t, t, f, f, t, t, f, f }.ToList() },
                                //{ elements[3], new DTState[] { t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f }.ToList() },
            		            { elements[4], new DTState[] { e, e, e, e, e, e, e, e, e, e, e, e, a, a, a, a }.ToList() },
            		            { elements[5], new DTState[] { a, a, a, a, a, a, a, a, e, e, e, e, a, a, a, a }.ToList() },
            		            };

            var actual = DTRuleSet_Accessor.CalculateActionStatesBasedOn(elements, basedOn);
            foreach (var pair in expected)
            {
                CollectionAssert.AreEqual(pair.Value, actual[pair.Key]);
            }
        }

        /// <summary>
        ///A test for InitializeRules(baseOn)
        ///</summary>
        [TestMethod()]
        public void InitializeRulesBasedOnTest()
        {
            InitializeRulesBasedOnTestCase1();
            InitializeRulesBasedOnTestCase2();
        }

        private static void InitializeRulesBasedOnTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            var ordered = new List<List<DTState>>
            		            {
            		                new DTState[] { t, t, t, t, f, f, f, f }.ToList(),
            		                new DTState[] { t, t, f, f, t, t, f, f }.ToList(),
            		                new DTState[] { t, f, t, f, t, f, t, f }.ToList(),
            		                new DTState[] { a, e, e, e, e, a, e, e }.ToList(),
            		                new DTState[] { a, a, e, a, a, e, a, e }.ToList()
            		            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(ordered[i]);

            DTRuleSet basedOn = DTRuleSet.FromElements(elements);
            elements.Insert(3, new DTElement { Name = "Condition 3", Kind = DTElementKind.Condition });

            var expected = new Dictionary<DTElement, List<DTState>>
            		            {
                                { elements[0], new DTState[] { t, t, t, t, t, t, t, t, f, f, f, f, f, f, f, f }.ToList() },
                                { elements[1], new DTState[] { t, t, t, t, f, f, f, f, t, t, t, t, f, f, f, f }.ToList() },
                                { elements[2], new DTState[] { t, t, f, f, t, t, f, f, t, t, f, f, t, t, f, f }.ToList() },
                                { elements[3], new DTState[] { t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f }.ToList() },
                                { elements[4], new DTState[] { a, a, e, e, e, e, e, e, e, e, a, a, e, e, e, e }.ToList() },
                                { elements[5], new DTState[] { a, a, a, a, e, e, a, a, a, a, e, e, a, a, e, e }.ToList() },
            		            };

            var actual = DTRuleSet_Accessor.InitializeRules(elements, basedOn);
            foreach (var pair in expected)
            {
                CollectionAssert.AreEqual(pair.Value, actual.GetStates(pair.Key).ToList());
            }
        }

        private static void InitializeRulesBasedOnTestCase2()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            var notOrdered = new List<List<DTState>>
            {
                new DTState[] { f, t, f, t }.ToList(),
                new DTState[] { f, f, t, t }.ToList(),
                new DTState[] { e, e, e, a }.ToList(),
                new DTState[] { a, a, e, a }.ToList()
            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(notOrdered[i]);

            DTRuleSet basedOn = DTRuleSet.FromElements(elements);
            elements.Insert(2, new DTElement { Name = "Condition 2", Kind = DTElementKind.Condition });
            elements.Insert(3, new DTElement { Name = "Condition 3", Kind = DTElementKind.Condition });

            var expected = new Dictionary<DTElement, List<DTState>>
            		            {
                                { elements[0], new DTState[] { f, f, f, f, t, t, t, t, f, f, f, f, t, t, t, t }.ToList() },
                                { elements[1], new DTState[] { f, f, f, f, f, f, f, f, t, t, t, t, t, t, t, t }.ToList() },
                                { elements[2], new DTState[] { t, t, f, f, t, t, f, f, t, t, f, f, t, t, f, f }.ToList() },
                                { elements[3], new DTState[] { t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f }.ToList() },
                                { elements[4], new DTState[] { e, e, e, e, e, e, e, e, e, e, e, e, a, a, a, a }.ToList() },
                                { elements[5], new DTState[] { a, a, a, a, a, a, a, a, e, e, e, e, a, a, a, a }.ToList() },
            		            };

            var actual = DTRuleSet_Accessor.InitializeRules(elements, basedOn);
            foreach (var pair in expected)
            {
                System.Collections.ICollection actualGetStatesToList = actual.GetStates(pair.Key).ToList();
                CollectionAssert.AreEqual(pair.Value, actualGetStatesToList);
            }
        }

        /// <summary>
        ///A test for CalculateConditionStatesBasedOn
        ///</summary>
        [TestMethod()]
        [DeploymentItem("DecisionTableAnalyzer.exe")]
        public void CalculateConditionStatesBasedOnTest()
        {
            CalculateConditionStatesBasedOnTestCase1();
            CalculateConditionStatesBasedOnTestCase2();
        }

        private static void CalculateConditionStatesBasedOnTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;

            var ordered = new List<List<DTState>>
            		            {
            		                new DTState[] { t, t, t, t, f, f, f, f }.ToList(),
            		                new DTState[] { t, t, f, f, t, t, f, f }.ToList(),
            		                new DTState[] { t, f, t, f, t, f, t, f }.ToList(),
            		                new DTState[] { t, e, e, e, e, t, e, e }.ToList(),
            		                new DTState[] { t, t, e, t, t, e, t, e }.ToList()
            		            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(ordered[i]);

            DTRuleSet basedOn = DTRuleSet.FromElements(elements);
            elements.Insert(3, new DTElement { Name = "Condition 3", Kind = DTElementKind.Condition });

            var expected = new Dictionary<DTElement, List<DTState>>
            		            {
                                { elements[0], new DTState[] { t, t, t, t, t, t, t, t, f, f, f, f, f, f, f, f }.ToList() },
                                { elements[1], new DTState[] { t, t, t, t, f, f, f, f, t, t, t, t, f, f, f, f }.ToList() },
                                { elements[2], new DTState[] { t, t, f, f, t, t, f, f, t, t, f, f, t, t, f, f }.ToList() },
                                { elements[3], new DTState[] { t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f }.ToList() },
                                //{ elements[4], new DTState[] { t, t, e, e, e, e, e, e, e, e, t, t, e, e, e, e }.ToList() },
                                //{ elements[5], new DTState[] { t, t, t, t, e, e, t, t, t, t, e, e, t, t, e, e }.ToList() },
            		            };

            var actual = DTRuleSet_Accessor.CalculateConditionStatesBasedOn(elements, basedOn);
            foreach (var pair in expected)
            {
                CollectionAssert.AreEqual(pair.Value, actual[pair.Key]);
            }
        }

        private static void CalculateConditionStatesBasedOnTestCase2()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;

            var notOrdered = new List<List<DTState>>
            {
                new DTState[] { f, t, f, t }.ToList(),
                new DTState[] { f, f, t, t }.ToList(),
                new DTState[] { e, e, e, t }.ToList(),
                new DTState[] { t, t, e, t }.ToList()
            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(notOrdered[i]);

            DTRuleSet basedOn = DTRuleSet.FromElements(elements);
            elements.Insert(2, new DTElement { Name = "Condition 2", Kind = DTElementKind.Condition });
            elements.Insert(3, new DTElement { Name = "Condition 3", Kind = DTElementKind.Condition });

            var expected = new Dictionary<DTElement, List<DTState>>
            		            {
                                { elements[0], new DTState[] { f, f, f, f, t, t, t, t, f, f, f, f, t, t, t, t }.ToList() },
                                { elements[1], new DTState[] { f, f, f, f, f, f, f, f, t, t, t, t, t, t, t, t }.ToList() },
                                { elements[2], new DTState[] { t, t, f, f, t, t, f, f, t, t, f, f, t, t, f, f }.ToList() },
                                { elements[3], new DTState[] { t, f, t, f, t, f, t, f, t, f, t, f, t, f, t, f }.ToList() },
                                //{ elements[4], new DTState[] { e, e, e, e, e, e, e, e, e, e, e, e, t, t, t, t }.ToList() },
                                //{ elements[5], new DTState[] { t, t, t, t, t, t, t, t, e, e, e, e, t, t, t, t }.ToList() },
            		            };

            var actual = DTRuleSet_Accessor.CalculateConditionStatesBasedOn(elements, basedOn);
            foreach (var pair in expected)
            {
                CollectionAssert.AreEqual(pair.Value, actual[pair.Key]);
            }
        }


        /// <summary>
        ///A test for CheckForRedundancy
        ///</summary>
        [TestMethod()]
        public void CheckForRedundancyTest()
        {
            CheckForRedundancyTestCase1();
            CheckForRedundancyTestCase2();
            CheckForRedundancyTestCase3();
        }

        private static void CheckForRedundancyTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;

            var states = new List<List<DTState>>
            		            {
            		                new DTState[] { t, t, t, t, f, f, f, f }.ToList(),
            		                new DTState[] { t, t, f, f, t, t, f, f }.ToList(),
            		                new DTState[] { t, f, t, f, t, f, t, f }.ToList(),
            		                new DTState[] { t, e, e, e, e, t, e, e }.ToList(),
            		                new DTState[] { t, t, e, t, t, e, t, e }.ToList()
            		            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(states[i]);

            var ruleSet = DTRuleSet_Accessor.FromElements(elements);

            IEnumerable<DTRule> redundantRules;
            bool hasRedundantRules = ruleSet.CheckForRedundancy(out redundantRules);

            Assert.IsFalse(hasRedundantRules);
            Assert.IsTrue(redundantRules.Count() == 0);
        }

        private static void CheckForRedundancyTestCase2()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;

            var states = new List<List<DTState>>
            		            {   //redundant: 2 (same as 1), 7 (4)
            		                new DTState[] { t, t, t, t, t, f, f, t, f, f }.ToList(),
            		                new DTState[] { t, t, t, f, f, t, t, f, f, f }.ToList(),
            		                new DTState[] { t, f, f, t, f, t, f, f, t, f }.ToList(),
            		                new DTState[] { t, e, e, e, e, e, t, e, e, e }.ToList(),
            		                new DTState[] { t, t, t, e, t, t, e, t, t, e }.ToList()
            		            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(states[i]);

            var expectedRedundantRules = new[]
                {
                    new DTRule(2, new[] {
                                         new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[2] },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[2] },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[2] },
                                        },                                                                           
                                  new[] {                                                                            
                                         new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[2] },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[2] },
                                        }),                                                                           
                    new DTRule(7, new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[7] },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[7] },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[7] },
                                        },                                                                            
                                  new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[7] },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[7] },
                                        })
                };

            var ruleSet = DTRuleSet_Accessor.FromElements(elements);

            IEnumerable<DTRule> redundantRules;
            bool hasRedundantRules = ruleSet.CheckForRedundancy(out redundantRules);
            redundantRules = redundantRules.OrderBy(cur => cur.Index);

            Assert.IsTrue(hasRedundantRules);
            Assert.IsTrue(expectedRedundantRules.Count() == redundantRules.Count(), "Wrong redundand rules count.");
            for (int i = 0; i < redundantRules.Count(); i++)
            {
                Assert.IsTrue(AreEqual(expectedRedundantRules[i], redundantRules.ElementAt(i)));
            }
        }

        private static void CheckForRedundancyTestCase3()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            var states = new List<List<DTState>>
            		            {   // more than one rule covered using empty
                                    // 3 (covers 1, 2), 9 (5, 6, 7, 8)
            		                new DTState[] { t, t, t, t, t, f, f, f, f, f }.ToList(),
            		                new DTState[] { t, t, f, f, f, t, t, f, e, e }.ToList(),
            		                new DTState[] { t, f, t, e, f, t, f, e, f, e }.ToList(),
            		                new DTState[] { a, e, e, e, e, e, e, e, e, e }.ToList(),
            		                new DTState[] { a, e, e, e, a, a, a, a, a, a }.ToList()
            		            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(states[i]);

            var expectedRedundantRules = new[]
                {
                    //new DTRule(1, new[] {
                    //                     new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[1] },
                    //                     new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[1] },
                    //                     new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[1] },
                    //                    },                                                                           
                    //              new[] {                                                                            
                    //                     new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[1] },
                    //                     new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[1] },
                    //                    }),                                                                           
                    new DTRule(2, new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[2] },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[2] },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[2] },
                                        },                                                                            
                                  new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[2] },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[2] },
                                        }),
                    new DTRule(5, new[] {
                                         new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[5] },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[5] },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[5] },
                                        },                                                                           
                                  new[] {                                                                            
                                         new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[5] },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[5] },
                                        }),                                                                           
                    new DTRule(6, new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[6] },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[6] },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[6] },
                                        },                                                                            
                                  new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[6] },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[6] },
                                        }),
                    new DTRule(7, new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[7] },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[7] },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[7] },
                                        },                                                                            
                                  new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[7] },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[7] },
                                        }),
                    new DTRule(8, new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[8] },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[8] },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[8] },
                                        },                                                                            
                                  new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[8] },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[8] },
                                        })
                };

            var ruleSet = DTRuleSet_Accessor.FromElements(elements);

            IEnumerable<DTRule> redundantRules;
            bool hasRedundantRules = ruleSet.CheckForRedundancy(out redundantRules);
            redundantRules = redundantRules.OrderBy(cur => cur.Index);

            Assert.IsTrue(hasRedundantRules);
            Assert.IsTrue(redundantRules.Count() == expectedRedundantRules.Count(), "Wrong redundand rules count.");
            for (int i = 0; i < redundantRules.Count(); i++)
            {
                Assert.IsTrue(AreEqual(expectedRedundantRules[i], redundantRules.ElementAt(i)));
            }
        }

        /// <summary>
        ///A test for CheckForCompleteness
        ///</summary>
        [TestMethod()]
        public void CheckForCompletenessTest()
        {
            CheckForCompletenessTestCase1();
            CheckForCompletenessTestCase2();
            CheckForCompletenessTestCase3();
        }
           
        private static void CheckForCompletenessTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            var states = new List<List<DTState>>
            		            {
            		                new DTState[] { t, /*t,*/ t, /*t,*/ /*f,*/ f, f, f }.ToList(),
            		                new DTState[] { t, /*t,*/ f, /*f,*/ /*t,*/ t, f, f }.ToList(),
            		                new DTState[] { t, /*f,*/ t, /*f,*/ /*t,*/ f, t, f }.ToList(),
            		                new DTState[] { a, /*e,*/ e, /*e,*/ /*e,*/ a, e, e }.ToList(),
            		                new DTState[] { a, /*a,*/ e, /*a,*/ /*a,*/ e, a, e }.ToList()
            		            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(states[i]);

            var expectedMissingRules = new[]
                {
                    new DTRule(1, new[] {
                                         new DTRuleElement{ ElementId = elements[0].Id, State = t },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = t },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = f },
                                        },                                                                            
                                  new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[3].Id, State = DTState.Empty },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = DTState.Empty },
                                        }),                                                                           
                    new DTRule(3, new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[0].Id, State = t },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = f },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = f },
                                        },                                                                            
                                  new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[3].Id, State = DTState.Empty },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = DTState.Empty },
                                        }),                                                                           
                    new DTRule(4, new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[0].Id, State = f },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = t },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = t },
                                        },                                                                            
                                  new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[3].Id, State = DTState.Empty },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = DTState.Empty },
                                        }),
                };


            var ruleSet = DTRuleSet_Accessor.FromElements(elements);

            IEnumerable<DTRule> missingRules;
            bool isComplete = ruleSet.CheckForCompleteness(out missingRules);
            missingRules.OrderBy(cur => cur.Index);

            Assert.IsFalse(isComplete);
            Assert.IsTrue(missingRules.Count() == expectedMissingRules.Count(), "Wrong missing element count.");
            for (int i = 0; i < missingRules.Count(); i++)
            {
                Assert.IsTrue(AreEqual(expectedMissingRules[i], missingRules.ElementAt(i)));
            }
        }

        private static void CheckForCompletenessTestCase2()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            var states = new List<List<DTState>>
            		            {
                                    //Last two rules are doubles
            		                new DTState[] { t, /*t,*/ t, /*t,*/ /*f,*/ f, f, f, t, f }.ToList(),
            		                new DTState[] { t, /*t,*/ f, /*f,*/ /*t,*/ t, f, f, f, f }.ToList(),
            		                new DTState[] { t, /*f,*/ t, /*f,*/ /*t,*/ f, t, f, t, t }.ToList(),
            		                new DTState[] { a, /*e,*/ e, /*e,*/ /*e,*/ a, e, e, e, e }.ToList(),
            		                new DTState[] { a, /*a,*/ e, /*a,*/ /*a,*/ e, a, e, e, a }.ToList()
            		            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(states[i]);

            var expectedMissingRules = new[]
                {
                    new DTRule(1, new[] {
                                         new DTRuleElement{ ElementId = elements[0].Id, State = t },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = t },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = f },
                                        },                                                                            
                                  new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[3].Id, State = DTState.Empty },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = DTState.Empty },
                                        }),                                                                           
                    new DTRule(3, new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[0].Id, State = t },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = f },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = f },
                                        },                                                                            
                                  new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[3].Id, State = DTState.Empty },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = DTState.Empty },
                                        }),                                                                           
                    new DTRule(4, new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[0].Id, State = f },
                                         new DTRuleElement{ ElementId = elements[1].Id, State = t },
                                         new DTRuleElement{ ElementId = elements[2].Id, State = t },
                                        },                                                                            
                                  new[] {                                                                             
                                         new DTRuleElement{ ElementId = elements[3].Id, State = DTState.Empty },
                                         new DTRuleElement{ ElementId = elements[4].Id, State = DTState.Empty },
                                        }),
                };

            var ruleSet = DTRuleSet_Accessor.FromElements(elements);

            IEnumerable<DTRule> missingRules;
            bool isComplete = ruleSet.CheckForCompleteness(out missingRules);
            missingRules.OrderBy(cur => cur.Index);

            Assert.IsFalse(isComplete);
            Assert.IsTrue(missingRules.Count() == expectedMissingRules.Count(), "Wrong missing element count.");
            for (int i = 0; i < missingRules.Count(); i++)
            {
                Assert.IsTrue(AreEqual(expectedMissingRules[i], missingRules.ElementAt(i)));
            }
        }

        private static void CheckForCompletenessTestCase3()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            var states = new List<List<DTState>>
            		            {   // more than one rule covered using empty
            		                new DTState[] { t, t, /*t,*/ t, t, /*f, f, f, f*/ f }.ToList(),
            		                new DTState[] { t, t, /*f,*/ f, f, /*t, t, f, f*/ e }.ToList(),
            		                new DTState[] { t, f, /*t,*/ e, f, /*t, f, t, f*/ e }.ToList(),
            		                new DTState[] { a, e, /*e,*/ e, e, /*e, a, e, e*/ e }.ToList(),
            		                new DTState[] { a, a, /*e,*/ e, a, /*a, e, a, e*/ e }.ToList()
            		            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(states[i]);

            var ruleSet = DTRuleSet_Accessor.FromElements(elements);

            IEnumerable<DTRule> missingRules;
            bool isComplete = ruleSet.CheckForCompleteness(out missingRules);

            Assert.IsTrue(isComplete);
            Assert.IsTrue(missingRules.Count() == 0, "Wrong missing element count.");
        }

        /// <summary>
        /// Compares the conditions and actions of both rules.
        /// </summary>
        private static bool AreEqual(DTRule a, DTRule b)
        {
            if (a.ConditionRuleElements.Count != b.ConditionRuleElements.Count ||
                a.ActionRuleElements.Count != b.ActionRuleElements.Count)
                return false;

            for (int i = 0; i < a.ConditionRuleElements.Count; i++)
            {
                if (a.ConditionRuleElements[i].ElementId != b.ConditionRuleElements[i].ElementId ||
                    a.ConditionRuleElements[i].State != b.ConditionRuleElements[i].State)
                    return false;
            }

            for (int i = 0; i < a.ActionRuleElements.Count; i++)
            {
                if (a.ActionRuleElements[i].ElementId != b.ActionRuleElements[i].ElementId ||
                    a.ActionRuleElements[i].State != b.ActionRuleElements[i].State)
                    return false;
            }

            return true;
        }


        /// <summary>
        ///A test for CalculateRules
        ///</summary>
        [TestMethod()]
        [DeploymentItem("DecisionTableAnalyzer.exe")]
        public void CalculateRulesTest()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;

            var expected = new List<DTRule>
            {
                new DTRule(0,
                    new []
                    {
                        new DTRuleElement { ElementId = elements[0].Id, State = t },
                        new DTRuleElement { ElementId = elements[1].Id, State = t },
                        new DTRuleElement { ElementId = elements[2].Id, State = t },
                    },                                                            
                    new []                                                        
                    {                                                             
                        new DTRuleElement { ElementId = elements[3].Id, State = e },
                        new DTRuleElement { ElementId = elements[4].Id, State = e },
                    }),
                new DTRule(1,
                    new []
                    {
                        new DTRuleElement { ElementId = elements[0].Id, State = t },
                        new DTRuleElement { ElementId = elements[1].Id, State = t },
                        new DTRuleElement { ElementId = elements[2].Id, State = f },
                    },                                                            
                    new []                                                        
                    {                                                             
                        new DTRuleElement { ElementId = elements[3].Id, State = e },
                        new DTRuleElement { ElementId = elements[4].Id, State = e },
                    }),
                new DTRule(2,
                    new []
                    {
                        new DTRuleElement { ElementId = elements[0].Id, State = t },
                        new DTRuleElement { ElementId = elements[1].Id, State = f },
                        new DTRuleElement { ElementId = elements[2].Id, State = t },
                    },                                                            
                    new []                                                        
                    {                                                             
                        new DTRuleElement { ElementId = elements[3].Id, State = e },
                        new DTRuleElement { ElementId = elements[4].Id, State = e },
                    }),
                new DTRule(3,
                    new []
                    {
                        new DTRuleElement { ElementId = elements[0].Id, State = t },
                        new DTRuleElement { ElementId = elements[1].Id, State = f },
                        new DTRuleElement { ElementId = elements[2].Id, State = f },
                    },                                                            
                    new []                                                        
                    {                                                             
                        new DTRuleElement { ElementId = elements[3].Id, State = e },
                        new DTRuleElement { ElementId = elements[4].Id, State = e },
                    }),
                new DTRule(4,
                    new []
                    {
                        new DTRuleElement { ElementId = elements[0].Id, State = f },
                        new DTRuleElement { ElementId = elements[1].Id, State = t },
                        new DTRuleElement { ElementId = elements[2].Id, State = t },
                    },                                                            
                    new []                                                        
                    {                                                             
                        new DTRuleElement { ElementId = elements[3].Id, State = e },
                        new DTRuleElement { ElementId = elements[4].Id, State = e },
                    }),
                new DTRule(5,
                    new []
                    {
                        new DTRuleElement { ElementId = elements[0].Id, State = f },
                        new DTRuleElement { ElementId = elements[1].Id, State = t },
                        new DTRuleElement { ElementId = elements[2].Id, State = f },
                    },                                                            
                    new []                                                        
                    {                                                             
                        new DTRuleElement { ElementId = elements[3].Id, State = e },
                        new DTRuleElement { ElementId = elements[4].Id, State = e },
                    }),
                new DTRule(6,
                    new []
                    {
                        new DTRuleElement { ElementId = elements[0].Id, State = f },
                        new DTRuleElement { ElementId = elements[1].Id, State = f },
                        new DTRuleElement { ElementId = elements[2].Id, State = t },
                    },                                                            
                    new []                                                        
                    {                                                             
                        new DTRuleElement { ElementId = elements[3].Id, State = e },
                        new DTRuleElement { ElementId = elements[4].Id, State = e },
                    }),
                new DTRule(7,
                    new []
                    {
                        new DTRuleElement { ElementId = elements[0].Id, State = f },
                        new DTRuleElement { ElementId = elements[1].Id, State = f },
                        new DTRuleElement { ElementId = elements[2].Id, State = f },
                    },                                                            
                    new []                                                        
                    {                                                             
                        new DTRuleElement { ElementId = elements[3].Id, State = e },
                        new DTRuleElement { ElementId = elements[4].Id, State = e },
                    })
            };

            var actual = DTRuleSet_Accessor.CalculateRules(elements);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.IsTrue(AreEqual(expected[i], actual.ElementAtOrDefault(i)));
            }
        }


        /// <summary>
        ///A test for CheckForContradiction
        ///</summary>
        [TestMethod()]
        public void CheckForContradictionTest()
        {
            CheckForContradictionTestCase1();
            CheckForContradictionTestCase2();   
        }

        private static void CheckForContradictionTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            var states = new List<List<DTState>>
            		            {   //contradicted: 0,1 / 2,4 / 3,6,7
            		                new DTState[] { t, t, t, t, t, f, t, t, f, f }.ToList(),
            		                new DTState[] { t, t, t, f, t, t, f, f, f, f }.ToList(),
            		                new DTState[] { t, t, f, t, f, t, t, t, t, f }.ToList(),
            		                new DTState[] { a, e, e, e, a, e, a, e, e, e }.ToList(),
            		                new DTState[] { a, a, a, e, a, a, e, a, a, e }.ToList()
            		            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(states[i]);

            var expectedContradictedRules = new[]
                {
                    new[]
                    {
                        new DTRule(0, new[]
                        {
                            new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[0] },
                            new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[0] },
                            new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[0] },
                        },                                                                           
                        new[]
                        {                                                                            
                            new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[0] },
                            new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[0] },
                        }),                                                                           
                        new DTRule(1, new[]
                        {                                                                             
                            new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[1] },
                            new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[1] },
                            new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[1] },
                        },                                                                            
                        new[] {                                                                             
                            new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[1] },
                            new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[1] },
                        })
                    },
                    new[]
                    {
                        new DTRule(2, new[]
                        {
                            new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[2] },
                            new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[2] },
                            new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[2] },
                        },                                                                           
                        new[]
                        {                                                                            
                            new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[2] },
                            new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[2] },
                        }),                                                                           
                        new DTRule(4, new[]
                        {                                                                             
                            new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[4] },
                            new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[4] },
                            new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[4] },
                        },                                                                            
                        new[] {                                                                             
                            new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[4] },
                            new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[4] },
                        })
                    },
                    new[]
                    {
                        new DTRule(3, new[]
                        {
                            new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[3] },
                            new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[3] },
                            new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[3] },
                        },                                                                           
                        new[]
                        {                                                                            
                            new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[3] },
                            new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[3] },
                        }),                                                                           
                        new DTRule(6, new[]
                        {                                                                             
                            new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[6] },
                            new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[6] },
                            new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[6] },
                        },                                                                            
                        new[] {                                                                             
                            new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[6] },
                            new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[6] },
                        }),                                                                           
                        new DTRule(7, new[]
                        {                                                                             
                            new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[7] },
                            new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[7] },
                            new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[7] },
                        },                                                                            
                        new[] {                                                                             
                            new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[7] },
                            new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[7] },
                        })
                    }
                };

            var ruleSet = DTRuleSet_Accessor.FromElements(elements);

            IEnumerable<IEnumerable<DTRule>> contradictedRules;
            bool hasContradictedRules = ruleSet.CheckForContradiction(out contradictedRules);
            contradictedRules = contradictedRules.OrderBy(cur => cur.Count());

            Assert.IsTrue(hasContradictedRules);
            Assert.IsTrue(expectedContradictedRules.Count() == contradictedRules.Count(), "Wrong contradicted rules count.");
            for (int i = 0; i < contradictedRules.Count(); i++)
            {
                var curActualList = contradictedRules.ElementAt(i);
                var curExpectedList = expectedContradictedRules.ElementAt(i);

                Assert.AreEqual(curExpectedList.Count(), curExpectedList.Count());
                for (int j = 0; j < curActualList.Count(); j++)
                {
                    var curActualRule = curActualList.ElementAt(j);
                    Assert.IsTrue(curExpectedList.Any(curExpectedRule => AreEqual(curExpectedRule, curActualRule)));
                }
            }
        }

        private static void CheckForContradictionTestCase2()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            var states = new List<List<DTState>>
            		            {   //contradicted: 0,1
            		                new DTState[] { t, t }.ToList(),
            		                new DTState[] { f, f }.ToList(),
            		                new DTState[] { f, e }.ToList(),
            		                new DTState[] { a, a }.ToList(),
            		                new DTState[] { e, a }.ToList()
            		            };
            for (int i = 0; i < elements.Count; i++)
                elements[i].SetStates(states[i]);

            var expectedContradictedRules = new[]
                {
                    new[]
                    {
                        new DTRule(0, new[]
                        {
                            new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[0] },
                            new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[0] },
                            new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[0] },
                        },                                                                           
                        new[]
                        {                                                                            
                            new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[0] },
                            new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[0] },
                        }),                                                                           
                        new DTRule(1, new[]
                        {                                                                             
                            new DTRuleElement{ ElementId = elements[0].Id, State = elements[0].States[1] },
                            new DTRuleElement{ ElementId = elements[1].Id, State = elements[1].States[1] },
                            new DTRuleElement{ ElementId = elements[2].Id, State = elements[2].States[1] },
                        },                                                                            
                        new[] {                                                                             
                            new DTRuleElement{ ElementId = elements[3].Id, State = elements[3].States[1] },
                            new DTRuleElement{ ElementId = elements[4].Id, State = elements[4].States[1] },
                        })
                    }
                };

            var ruleSet = DTRuleSet_Accessor.FromElements(elements);

            IEnumerable<IEnumerable<DTRule>> contradictedRules;
            bool hasContradictedRules = ruleSet.CheckForContradiction(out contradictedRules);
            contradictedRules = contradictedRules.OrderBy(cur => cur.Count());

            Assert.IsTrue(hasContradictedRules);
            Assert.IsTrue(expectedContradictedRules.Count() == contradictedRules.Count(), "Wrong contradicted rules count.");
            for (int i = 0; i < contradictedRules.Count(); i++)
            {
                var curActualList = contradictedRules.ElementAt(i);
                var curExpectedList = expectedContradictedRules.ElementAt(i);

                Assert.AreEqual(curExpectedList.Count(), curExpectedList.Count());
                for (int j = 0; j < curActualList.Count(); j++)
                {
                    var curActualRule = curActualList.ElementAt(j);
                    Assert.IsTrue(curExpectedList.Any(curExpectedRule => AreEqual(curExpectedRule, curActualRule)));
                }
            }
        }

        /// <summary>
        ///A test for RemoveRedundantRules
        ///</summary>
        [TestMethod()]
        public void RemoveRedundantRulesTest()
        {
            RemoveRedundantRulesTestCase1();
        }

        internal void RemoveRedundantRulesTestCase1()
        {
            List<DTElement> elements = new List<DTElement>();
            for (int i = 0; i < 3; i++)
                elements.Add(new DTElement { Name = string.Format("Condition {0}", i), Kind = DTElementKind.Condition });
            for (int i = 0; i < 2; i++)
                elements.Add(new DTElement { Name = string.Format("Action {0}", i), Kind = DTElementKind.Action });

            var actual = DTRuleSet.InitializeRules(elements);
            Assert.IsTrue(actual.Rules.Count == 8);

            var f = DTState.No;
            var t = DTState.Yes;
            var e = DTState.Empty;
            var a = DTState.ActionYes;

            //Conditions
            var states0 = new DTState[] { t, t, t, t, t, t, f, f, f, f, f };
            var states1 = new DTState[] { e, t, t, f, t, f, t, t, f, f, t };
            var states2 = new DTState[] { t, f, f, t, f, f, t, f, t, f, f };
            //Actions
            var states3 = new DTState[] { a, a, a, a, a, a, a, a, a, e, a };
            var states4 = new DTState[] { e, e, e, e, e, e, e, e, e, a, e };

            elements[0].SetStates(states0);
            elements[1].SetStates(states1);
            elements[2].SetStates(states2);
            elements[3].SetStates(states3);
            elements[4].SetStates(states4);

            DTRuleSet target = DTRuleSet.RemoveRedundantRules(elements);

            //Conditions
            var expectedStates0 = new DTState[] { t, t, t, f, f, f, f };
            var expectedStates1 = new DTState[] { e, t, f, t, t, f, f };
            var expectedStates2 = new DTState[] { t, f, f, t, f, t, f };
            //Actions
            var expectedStates3 = new DTState[] { a, a, a, a, a, a, e };
            var expectedStates4 = new DTState[] { e, e, e, e, e, e, a };

            CollectionAssert.AreEqual(target.GetStates(elements[0]).ToList(), expectedStates0.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[1]).ToList(), expectedStates1.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[2]).ToList(), expectedStates2.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[3]).ToList(), expectedStates3.ToList());
            CollectionAssert.AreEqual(target.GetStates(elements[4]).ToList(), expectedStates4.ToList());
        }

    }
}
