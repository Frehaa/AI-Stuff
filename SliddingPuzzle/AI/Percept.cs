using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliddingPuzzle.AI
{
    public class Percept
    {
        private IDictionary<object, object> attributes = new Dictionary<object, object>();

        public object GetAttribute(object key)
        {
            return attributes[key];
        }

        public void AddAttribute(object key, object attribute)
        {
            if (attributes.Keys.Contains(key))
            {
                throw new ArgumentException("Key already associated with attribute", "key");
            }

            attributes[key] = attribute;
        }

    }
}
