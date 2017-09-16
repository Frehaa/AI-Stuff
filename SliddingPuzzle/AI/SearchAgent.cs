using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E = SliddingPuzzle.AI.Environment;

namespace SliddingPuzzle.AI
{
    public class SearchAgent
    {
        public delegate void test(int count);
        public event test FrontierCountDEBUGEVENT;
        public enum SearchMethod
        {
            BREATHFIRST, DEPTHFIRST, UNIFORMCOST, DEPTHLIMITED, ITERATIVEDEEPENINGDEPTHFIRST, BIDIRICTIONAL
        }
        private SearchMethod searchMethod;

        public SearchAgent(SearchMethod searchMethod)
        {
            this.searchMethod = searchMethod;
        }

        private Problem<PuzzleBoard, E.Action> problem;

        public Solution<E.Action> Act(Percept percept)
        {
            problem = (Problem<PuzzleBoard, E.Action>) percept.GetAttribute(E.Percepts.PROBLEM);
            
            switch (searchMethod)
            {
                case SearchMethod.BREATHFIRST:
                    return BreathFirstSearch();
                case SearchMethod.DEPTHFIRST:
                    return DepthFirstSearch();
                case SearchMethod.UNIFORMCOST:
                    return UniformCostSearch();
                case SearchMethod.DEPTHLIMITED:
                    return DepthLimitedSearch();
                case SearchMethod.ITERATIVEDEEPENINGDEPTHFIRST:
                    return IterativeDeepeningDepthFirstSearch();
                case SearchMethod.BIDIRICTIONAL:
                    return BidirectionalSearch();
                default:
                    return new Solution<E.Action>();
            }
        }

        private Solution<E.Action> BreathFirstSearch()
        {
            Queue<Node> frontier = new Queue<Node>();
            ISet<PuzzleBoard> explored = new HashSet<PuzzleBoard>();
            Node node = new Node()
            {
                Action = E.Action.INITIAL,
                Parent = null,
                PathCost = 0,
                State = problem.GetInitialState()
            };

            if (problem.GoalTest(node.State)) return CreateSolution(node);

            frontier.Enqueue(node);
            explored.Add(node.State);
            while (frontier.Count != 0)
            {
                FrontierCount_BeforeDequeue_DebugEvent(frontier.Count);
                node = frontier.Dequeue();

                foreach (var action in problem.GetActions())
                {
                    Node child = ChildNode(node, action);
                    if (!explored.Contains(child.State))
                    {
                        if (problem.GoalTest(child.State)) return CreateSolution(child);
                        frontier.Enqueue(child);
                        explored.Add(child.State);
                    }
                }
            }
            
            return new Solution<E.Action>();
        }

        private Solution<E.Action> UniformCostSearch()
        {
            throw new NotImplementedException();
        }

        private Solution<E.Action> DepthFirstSearch()
        {
            throw new NotImplementedException();
        }

        private Solution<E.Action> DepthLimitedSearch()
        {
            throw new NotImplementedException();
        }

        private Solution<E.Action> IterativeDeepeningDepthFirstSearch()
        {
            throw new NotImplementedException();
        }

        private Solution<E.Action> BidirectionalSearch()
        {
            throw new NotImplementedException();
        }

        private Node ChildNode(Node node, E.Action action)
        {
            Node child = new Node()
            {
                Parent = node,
                Action = action,
                PathCost = node.PathCost + problem.StepCost(action),
                State = problem.Transition(node.State, action)
            };
            return child;
        }

        private Solution<E.Action> CreateSolution(Node node)
        {
            if (node.Action == E.Action.INITIAL) return new Solution<E.Action>();

            Solution<E.Action> solution = new Solution<E.Action>();
            while (node.Action != E.Action.INITIAL)
            {
                solution.AddActionFirst(node.Action);
                node = node.Parent;
            }

            return solution;
        }

        private class Node
        {
            public Node Parent { get; set; }
            public PuzzleBoard State { get; set; }
            public double PathCost { get; set; }
            public E.Action Action { get; set; }
        }

        private void FrontierCount_BeforeDequeue_DebugEvent(int count)
        {
            FrontierCountDEBUGEVENT?.Invoke(count);
        }
    }
}
