using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliddingPuzzle.AI
{
    public abstract class Problem<State, Action>
    {
        public abstract State GetInitialState();
        public abstract ICollection<Action> GetActions();
        public abstract State Transition(State state, Action action);
        public abstract bool GoalTest(State state);
        public abstract double PathCost();
        public abstract double StepCost(Action action);
    }
}
