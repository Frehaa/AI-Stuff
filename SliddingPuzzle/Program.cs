using SliddingPuzzle.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Env = SliddingPuzzle.AI.Environment;

namespace SliddingPuzzle
{
    class Program
    {
        private static PuzzleBoard board;
        static void Main(string[] args)
        {
            board = new PuzzleBoard(3);
            PrintBoard();

            while (true)
            {
                string input = Console.ReadLine();
                switch (input)
                {
                    case "w":
                        board.MoveUp();
                        break;
                    case "s":
                        board.MoveDown();
                        break;
                    case "a":
                        board.MoveLeft();
                        break;
                    case "d":
                        board.MoveRight();
                        break;
                    case "q":
                        return;
                    case "r":
                        board.Shuffle();
                        Console.WriteLine("Is solveable: " + board.IsSolveable());
                        break;
                    case "bfs":
                        Search(SearchAgent.SearchMethod.BREATHFIRST);
                        break;
                    case "dfs":
                        Search(SearchAgent.SearchMethod.DEPTHFIRST);
                        break;
                    case "dfsl":
                        Search(SearchAgent.SearchMethod.DEPTHLIMITED);
                        break;
                    case "iddf":
                        Search(SearchAgent.SearchMethod.ITERATIVEDEEPENINGDEPTHFIRST);
                        break;
                    case "uc":
                        Search(SearchAgent.SearchMethod.UNIFORMCOST);
                        break;
                    case "bi":
                        Search(SearchAgent.SearchMethod.BIDIRICTIONAL);
                        break;
                    default:
                        break;
                }
                PrintBoard();
            }
        }

        private static void Search(SearchAgent.SearchMethod searchMethod)
        {
            SearchAgent agent = new SearchAgent(searchMethod);
            agent.FrontierCountDEBUGEVENT += FrontierCount;
            Percept percept = CreatePercept();
            Solution<Env.Action> solution = agent.Act(percept);

            PrintSolution(solution);
            Console.WriteLine("AI Finished");
            Console.WriteLine();
        }

        private static void FrontierCount(int count)
        {
            Console.WriteLine("Frontier Count: " + count);
        }

        private static Percept CreatePercept()
        {
            Percept percept = new Percept();
            percept.AddAttribute(Env.Percepts.PROBLEM, new Env.SliddingPuzzleProblem(board));
            return percept;
        }

        private static void PrintSolution(Solution<Env.Action> solution)
        {
            Console.WriteLine("Solution Length: " + solution.Length);
            foreach (var action in solution)
            {
                switch (action)
                {
                    case Env.Action.UP:
                        board.MoveUp();
                        break;
                    case Env.Action.DOWN:
                        board.MoveDown();
                        break;
                    case Env.Action.LEFT:
                        board.MoveLeft();
                        break;
                    case Env.Action.RIGHT:
                        board.MoveRight();
                        break;
                    default:
                        break;
                }

                PrintBoard();
                Console.WriteLine();
                System.Threading.Thread.Sleep(500);

            }
        }

        private static void PrintBoard()
        {
            string s = board.ToString();
            Console.WriteLine(board.ToString());
        }
    }
}
