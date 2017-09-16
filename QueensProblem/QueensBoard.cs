 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueensProblem
{
    public class QueensBoard
    {
        public int Size { get; private set; }

        public QueensBoard(int size)
        {
            if (size > 4)
            {
                throw new ArgumentException("Board size too small", "size");
            }

            Size = size;
        }
    }
}
