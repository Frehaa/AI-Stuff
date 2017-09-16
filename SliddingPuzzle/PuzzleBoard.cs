using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliddingPuzzle
{
    /**
     * A sliding board, with a dynamic size >= 2, values from 0 to size * size-1 where 0 represents the empty field
     * 
     * Coordinates as follows:
     * [0, 0][1, 0][2, 0]
     * [0, 1][1, 1][2, 1]
     * [0, 2][1, 2][2, 2]
     * 
     * Initial configuration for a 3 by 3 board, this is a goal state:
     * 0 1 2
     * 3 4 5
     * 6 7 8
     */
    public class PuzzleBoard
    {
        private int[,] board;
        private Point zeroCoordinates = new Point(0, 0);

        public int Size { get; private set; }

        public PuzzleBoard(int size)
        {
            if (size < 2)
            {
                throw new ArgumentException("Size too small");
            }

            Size = size;
            board = new int[Size, Size];

            int counter = 0;
            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    board[x, y] = counter++;
                }
            }
        }

        public PuzzleBoard(PuzzleBoard puzzleBoard)
        {
            Size = puzzleBoard.Size;
            zeroCoordinates = puzzleBoard.zeroCoordinates;
            board = new int[Size, Size];

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    board[x, y] = puzzleBoard.GetValue(x, y);
                }
            }
        }

        public int GetValue(int x, int y)
        {
            if (OutOfBounds(new Point(x, y)))
            {
                throw new ArgumentOutOfRangeException();
            }
            return board[x, y];
        }

        // Fisher–Yates shuffle
        public void Shuffle(bool forceSolveable)
        {
            Random random = new Random();
            for (int i = Size * Size - 1; i > 0; --i)
            {
                int j = random.Next(0, i + 1);

                Point either = new Point(i % Size, i / Size);
                Point other = new Point(j % Size, j / Size);

                Swap(either, other);
            }

            if (forceSolveable && !IsSolveable())
                Shuffle(forceSolveable);
        }

        public void Shuffle()
        {
            Shuffle(false);
        }

        public int CalculateInversions()
        {
            int inversions = 0;
            ICollection<int> previousValues = new List<int>();

            for (int i = Size * Size -1; i >= 0; --i)
            {
                int x = i % Size;
                int y = i / Size;

                int value = GetValue(x, y);

                if (value == 0)
                {
                    continue;
                }

                foreach (var previousValue in previousValues)
                {
                    if (previousValue < value)
                    {
                        inversions++;
                    }
                }
                previousValues.Add(value);
            }

            return inversions;
        }
        /*  If the grid width is odd, then the number of inversions in a solvable situation is even.
            If the grid width is even, and the blank is on an even row counting from the bottom (second-last, fourth-last etc), then the number of inversions in a solvable situation is odd.
            If the grid width is even, and the blank is on an odd row counting from the bottom (last, third-last, fifth-last etc) then the number of inversions in a solvable situation is even. */

        public bool IsSolveable()
        {
            int inversion = CalculateInversions();
            
            if (Size % 2 == 1)
            {
                return inversion % 2 == 0;
            }
            else
            {
                if (zeroCoordinates.y % 2 == 1)
                {
                    return inversion % 2 == 1;  
                }
                else 
                {
                    return inversion % 2 == 0;
                }
            }
        }

        public bool IsGoalState()
        {
            int counter = 1;
            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    int value = GetValue(x, y);
                    if (value == 0)
                        continue;

                    if (value != counter)
                    {
                        return false;
                    }
                    ++counter;
                }
            }

            return true;
        }

        public void MoveUp()
        {
            Point other = new Point(zeroCoordinates.x, zeroCoordinates.y + 1);
            if (OutOfBounds(other)) return;
            Swap(zeroCoordinates, other);
        }

        public void MoveDown()
        {
            Point other = new Point(zeroCoordinates.x, zeroCoordinates.y - 1);
            if (OutOfBounds(other)) return;
            Swap(zeroCoordinates, other);
        }

        public void MoveRight()
        {
            Point other = new Point(zeroCoordinates.x - 1, zeroCoordinates.y);
            if (OutOfBounds(other)) return;
            Swap(zeroCoordinates, other);
        }

        public void MoveLeft()
        {
            Point other = new Point(zeroCoordinates.x + 1, zeroCoordinates.y);
            if (OutOfBounds(other)) return;
            Swap(zeroCoordinates, other);
        }

        private void Swap(Point either, Point other)
        {
            SwapZeroCoordinatesIfNeeded(either, other);

            int tmp = board[either.x, either.y];
            board[either.x, either.y] = board[other.x, other.y];
            board[other.x, other.y] = tmp;
        }

        private bool OutOfBounds(Point point)
        {
            if (point.x < 0 || point.x >= Size || point.y < 0 || point.y >= Size)
            {
                return true;
            }
            return false;
        }

        private void SwapZeroCoordinatesIfNeeded(Point either, Point other)
        {
            if (either.Equals(zeroCoordinates))
            {
                zeroCoordinates = other;
            }
            else if (other.Equals(zeroCoordinates))
            {
                zeroCoordinates = either;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            PuzzleBoard other = (PuzzleBoard)obj;

            if (Size != other.Size)
                return false;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (GetValue(x, y) != other.GetValue(x, y)) return false;
                }
            }

            return true;
        }
        
        public override int GetHashCode()
        { 
            int hash = 17;
            hash = hash * 23 + Size.GetHashCode();
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    hash = hash * 23 + GetValue(x, y);
                }
            }

            return hash;
        }

        public override string ToString()
        {
            StringBuilder boardBuilder = new StringBuilder();

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    boardBuilder.Append(GetValue(x, y));
                    if (x != Size - 1)
                        boardBuilder.Append(" ");
                }

                if (y != Size - 1)
                    boardBuilder.AppendLine();
            }

            return boardBuilder.ToString();
        }

        private struct Point
        {
            public int x;
            public int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            
            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                Point other = (Point)obj;
                
                return x == other.x && y == other.y;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash += hash * 23 + x.GetHashCode();
                    hash += hash * 23 + y.GetHashCode();

                    return hash;
                }
            }
        }

    }
}
