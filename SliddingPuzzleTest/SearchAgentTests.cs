using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliddingPuzzle.AI;
using SliddingPuzzle;
using E = SliddingPuzzle.AI.Environment;
using System.Collections.Generic;

namespace SliddingPuzzleTest
{
    [TestClass]
    public class SearchAgentTests
    {
        private PuzzleBoard board;
        private SearchAgent agent;
        private Percept percept;

        [TestInitialize]
        public void Initialize()
        {
            board = new PuzzleBoard(3);

            percept = new Percept();
        }

        [TestMethod]
        public void BFS_InitialConfiguration_ReturnNullSolution()
        {
            agent = new SearchAgent(SearchAgent.SearchMethod.BREATHFIRST);
            percept.AddAttribute(E.Percepts.PROBLEM, new E.SliddingPuzzleProblem(board));
            
            Solution<E.Action> solution = agent.Act(percept);

            Assert.IsTrue(solution.IsEmpty);
        }

        [TestMethod]
        public void BFS_SingleMoveUp_ReturnSolutionOfSingleMoveDown()
        {
            agent = new SearchAgent(SearchAgent.SearchMethod.BREATHFIRST);

            board.MoveUp();
            percept.AddAttribute(E.Percepts.PROBLEM, new E.SliddingPuzzleProblem(board));

            Solution<E.Action> solution = agent.Act(percept);

            IEnumerator<E.Action> enumerator = solution.GetEnumerator();

            enumerator.MoveNext();

            Assert.AreEqual(E.Action.DOWN, enumerator.Current);
        }

        [TestMethod]
        public void BFS_MoveUpMoveLeft_ReturnSolutionOfMoveRightMoveDown()
        {
            agent = new SearchAgent(SearchAgent.SearchMethod.BREATHFIRST);

            board.MoveUp();
            board.MoveLeft();
            percept.AddAttribute(E.Percepts.PROBLEM, new E.SliddingPuzzleProblem(board));

            Solution<E.Action> solution = agent.Act(percept);

            IEnumerator<E.Action> enumerator = solution.GetEnumerator();

            enumerator.MoveNext();

            Assert.AreEqual(E.Action.RIGHT, enumerator.Current);

            enumerator.MoveNext();

            Assert.AreEqual(E.Action.DOWN, enumerator.Current);
        }

        [TestMethod]
        public void BFS_UpLeftLeftDownRight_ReturnSolutionOfLeftUpRightRightDown()
        {
            agent = new SearchAgent(SearchAgent.SearchMethod.BREATHFIRST);

            board.MoveUp();
            board.MoveLeft();
            board.MoveLeft();
            board.MoveDown();
            board.MoveRight();
            percept.AddAttribute(E.Percepts.PROBLEM, new E.SliddingPuzzleProblem(board));

            Solution<E.Action> solution = agent.Act(percept);

            IEnumerator<E.Action> enumerator = solution.GetEnumerator();

            enumerator.MoveNext();

            Assert.AreEqual(E.Action.LEFT, enumerator.Current);

            enumerator.MoveNext();

            Assert.AreEqual(E.Action.UP, enumerator.Current);

            enumerator.MoveNext();

            Assert.AreEqual(E.Action.RIGHT, enumerator.Current);

            enumerator.MoveNext();

            Assert.AreEqual(E.Action.RIGHT, enumerator.Current);

            enumerator.MoveNext();

            Assert.AreEqual(E.Action.DOWN, enumerator.Current);
        }
    }
}
