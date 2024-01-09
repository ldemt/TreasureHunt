using System;
using System.Threading;
using TreasureHunt;
using Rhino.Geometry;
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
       

            //NEW Instantiate wormholes and add em to the wormholeList of StaticGame
            int numberOfWormholes = 3;
            StaticGame = new Game(xSize, ySize);


            // Generate and initialize the wormholes
            Random randomWormhole = new Random(8938);
            for (int i = 0; i < numberOfWormholes; i++)
            {

                double wormholeX = randomWormhole.NextDouble() * xSize;
                double wormholeY = randomWormhole.NextDouble() * ySize;
             



                // Instantiate a wormhole
                WormHole wormhole = new WormHole(20, 20);

                // list of wormholes in the space board
                StaticGame.wormholeList.Add(wormhole);
            }


            
            // Generate and initialize the players
            Random random = new Random(7897999);
            // ZigzagPlayer player = new TreasureHunt.ZigzagPlayer();
            double randomX;
            double randomY;

            for (int i = 0; i < numberOfAgents; i++)
            {
                randomX = random.NextDouble() * xSize;
                randomY = random.NextDouble() * ySize;
                
                Player player = new Player(randomX, randomY);
                // Instantiate Hugo agents (one follower for each player)
                HugoFollower follower = new HugoFollower(player);
                // Add them to the list of players belonging to the space.
                
               
                
                // Add it to the list of balls belonging to the space.
                StaticGame.playerList.Add(player);
                StaticGame.playerList.Add(follower);
            }

            // add obstacle
            Obstacle obstacle = new Obstacle(20, 20, 20, 20);
            StaticGame.obstacles.Add(obstacle);
            // Declare and instantiate here one (or several ?) of your custom players
            double x = 20;
            double y = 20;
            ZigzagPlayer myCustomPlayer = new ZigzagPlayer(x, y, 0, 0, 600, 300);
            SpiralMovement myCustomPlayer2 = new SpiralMovement(20, 50);
            //CustomPlayer myCustomPlayer = new CustomPlayer(10,10);
            AlonsoPlayer alonsoPlayer = new AlonsoPlayer(new Vector3d(10,10,0), new Vector3d(1,1,0));
            VictorSierraPlayer vsPlayer = new VictorSierraPlayer(20, 20, 5, 5, 2, 2);
            // Add here your custom player to the player list!
            StaticGame.playerList.Add(myCustomPlayer);
            StaticGame.playerList.Add(myCustomPlayer2);

            // Adding Tin player classes
            TinLeader tinLeader = new TinLeader(10,10);
            TinFollower tinFollower = new TinFollower(10, 10, xSize, ySize);
            Team team = new Team(tinLeader, tinFollower);

            StaticGame.playerList.Add(tinLeader);
            StaticGame.playerList.Add(tinFollower);
            StaticGame.teamList.Add(team);

            //StaticGame.playerList.Add(myCustomPlayer);
            StaticGame.playerList.Add(alonsoPlayer);

        }

        static void RunGame()
        {

            while (StaticGame.isTreasureFound == false)
            {

                // Simulate some delay
                Thread.Sleep(100);
                StaticGame.UpdatePlayers();
                //Console.SetCursorPosition(0, Console.CursorTop);
                Console.SetCursorPosition(0, 1);
                //Console.Write("\r");

                for (int i = 0; i < StaticGame.wormholeList.Count; i++)
                {
                    Console.Write(StaticGame.wormholeList[i].statusMessage);
                    Console.Write("\n");

                }

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
