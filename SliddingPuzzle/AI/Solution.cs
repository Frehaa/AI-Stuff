using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliddingPuzzle.AI
{
    public class Solution<Action> : IEnumerable<Action>
    {
        private LinkedList<Action> actions = new LinkedList<Action>();

        public bool IsEmpty { get { return actions.Count == 0; } }
        public int Length { get { return actions.Count; } }

        public void AddAction(Action action)
        {
            actions.AddLast(action);
        }

        public void AddActionFirst(Action action)
        {
            actions.AddFirst(action);
        }

        public IEnumerator<Action> GetEnumerator()
        {
            return actions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return actions.GetEnumerator();
        }
    }
}
