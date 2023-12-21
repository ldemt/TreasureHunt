using System;
using System.Threading;
using TreasureHunt;
namespace TestAppTHunt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("## TreasureHunt v0.0 Demo Program | DBD OOP Course 2023-2024 ## ");
            Console.WriteLine("Initializing...");

            InitializeGame();
            RunGame();

        }

        static void InitializeGame()
        {
            double xSize = 600;
            double ySize = 300;
            double numberOfAgents = 20;


            StaticGame = new Game(xSize, ySize);
            // Generate and initialize the players
            Random random = new Random(7897999);

            double randomX;
            double randomY;

            for (int i = 0; i < numberOfAgents; i++)
            {
                randomX = random.NextDouble() * xSize;
                randomY = random.NextDouble() * ySize;
                // Instantiate a moving ball
                Player player = new Player(randomX, randomY);
                
                // add obstacle
                Obstacle obstacle = new TreasureHunt.Obstacle(20,20,20,20);
                // Add it to the list of balls belonging to the space.
                StaticGame.playerList.Add(player);
            }

            // Declare and instantiate here one (or several ?) of your custom players
            CustomPlayer myCustomPlayer = new CustomPlayer(10,10);
            // Add here your custom player to the player list!
            StaticGame.playerList.Add(myCustomPlayer);

        }

        static void RunGame()
        {

            while (StaticGame.isTreasureFound == false)
            {

                // Simulate some delay
                Thread.Sleep(25);
                StaticGame.UpdatePlayers();
                //Console.SetCursorPosition(0, Console.CursorTop);
                Console.SetCursorPosition(0, 1);
                //Console.Write("\r");
                for (int i = 0; i < StaticGame.playerList.Count; i++)
                {
                    Console.Write(String.Format("PLAYER {0:00} | {1} | Position (X,Y) : {2:0.00}, {3:0.00} \n", i, StaticGame.playerList[i].statusMessage, StaticGame.playerList[i].location.X, StaticGame.playerList[i].location.Y));

                    if (i == StaticGame.playerList.Count)
                    {
                        Console.Write("\n");
                    }

                }
            }
            Console.WriteLine("Game finished, treasure has been found !");

        }

        static Game StaticGame { get; set; }
    }
}
