using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliddingPuzzle.AI
{
    public class Environment
    {
        private PuzzleBoard board;

        public enum Action
        {
            UP, DOWN, LEFT, RIGHT, INITIAL
        }

        public enum Percepts
        {
            PROBLEM
        }

        public Environment(PuzzleBoard board)
        {
            this.board = board;
        }

        public void Step()
        {
            SearchAgent agent = new SearchAgent(SearchAgent.SearchMethod.BREATHFIRST);
            Percept percept = new Percept();
            percept.AddAttribute(Percepts.PROBLEM, new SliddingPuzzleProblem(board));


            Solution<Action> solution = agent.Act(percept);
        }
        
        public class SliddingPuzzleProblem : Problem<PuzzleBoard, Action>
        {
            private PuzzleBoard initialBoard;

            public SliddingPuzzleProblem(PuzzleBoard board)
            {
                initialBoard = board;
            }

            public override ICollection<Action> GetActions()
            {
                IList<Action> list = new List<Action>
                {
                    Action.UP,
                    Action.DOWN,
                    Action.LEFT,
                    Action.RIGHT
                };
                return list;
            }

            public override PuzzleBoard GetInitialState()
            {
                return initialBoard;
            }            

            public override bool GoalTest(PuzzleBoard state)
            {
                return state.IsGoalState();
            }

            public override double PathCost()
            {
                throw new NotImplementedException();
            }

            public override double StepCost(Action action)
            {
                return 1;
            }

            public override PuzzleBoard Transition(PuzzleBoard state, Action action)
            {
                PuzzleBoard transitionedBoard = new PuzzleBoard(state);

                switch (action)
                {
                    case Action.UP:
                        transitionedBoard.MoveUp();
                        break;
                    case Action.DOWN:
                        transitionedBoard.MoveDown();
                        break;
                    case Action.LEFT:
                        transitionedBoard.MoveLeft();
                        break;
                    case Action.RIGHT:
                        transitionedBoard.MoveRight();
                        break;
                    default:
                        throw new ArgumentException("Illigal Action", "action");
                }

                return transitionedBoard;
            }
        }
    }
}
