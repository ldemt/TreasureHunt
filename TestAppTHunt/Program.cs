using System;
using System.Threading;
using TreasureHunt;
using Rhino.Geometry;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TestAppTHunt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("## TreasureHunt v0.0 Demo Program | DBD OOP Course 2023-2024 ## ");
            Console.WriteLine("Initializing...");


            // Base code to run game in CLI and see variables.
            //InitializeGame();
            //RunGame();

            // Code to run game in benchmark mode
            Benchmarker benchmarker = new Benchmarker();

            // Write benchmark results to .csv file
            var csv = new StringBuilder();

            for (int i = 0; i < 2500; i++) {
                List<int> numberOfSteps = benchmarker.RunBenchmark();
                Console.Write(String.Format("Benchmark {0:000} Finished. Steps : {1} \n", i, numberOfSteps[0]));

                var newLine = string.Format("{0}", numberOfSteps[0]);
                csv.AppendLine(newLine);
            }

            string filePath = "C:/Users/Leo/source/repos/TreasureHunt/BenchmarkResults/testBenchmark.csv";
            File.WriteAllText(filePath, csv.ToString());


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
            //ZigzagPlayer myCustomPlayer = new ZigzagPlayer(x, y, 0, 0, 600, 300);
            //SpiralMovement myCustomPlayer2 = new SpiralMovement(20, 50);
            //CustomPlayer myCustomPlayer = new CustomPlayer(10,10);
            AlonsoPlayer alonsoPlayer = new AlonsoPlayer(new Vector3d(10, 10, 0), new Vector3d(1, 1, 0));
            //AlonsoPlayer alonsoPlayer = new AlonsoPlayer(new Vector3d(10,10,0), new Vector3d(1,1,0));
            VictorSierraPlayer vsPlayer = new VictorSierraPlayer(20, 20, 5, 5, 2, 2,0,0,600,300);
            HayderPlayer myCustomPlayer = new HayderPlayer(10,10,0,0,600,300,new Rhino.Geometry.Vector3d(10,10,0));
            // Add here your custom player to the player list!
            StaticGame.playerList.Add(myCustomPlayer);
            //StaticGame.playerList.Add(myCustomPlayer);
            //StaticGame.playerList.Add(myCustomPlayer2);

            // Adding Tin player classes
            TinLeader tinLeader = new TinLeader(10, 10);
            TinFollower tinFollower = new TinFollower(10, 10, xSize, ySize);
            Team team = new Team(tinLeader, tinFollower);

            // StaticGame.playerList.Add(tinLeader);
            //StaticGame.playerList.Add(tinFollower);
            //StaticGame.teamList.Add(team);

            //StaticGame.playerList.Add(myCustomPlayer);
            //StaticGame.playerList.Add(alonsoPlayer);

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

    public class Benchmarker
    {
        Game game;
        Random random;
        double xSize;
        double ySize;
        int numberOfAgents;
        int numberOfWormholes;
        int numberOfObstacles;

        public Benchmarker()
        {
            numberOfAgents = 1;
            // Init game
            xSize = 600;
            ySize = 300;

            game = new Game(xSize, ySize);

            random = new Random();

            InitializeGame();
        }

        void InitializeGame()
        {

            double randomX;
            double randomY;

            game.playerList.Clear();

            randomX = RandomProvider.Instance.NextDouble() * xSize;
            randomY = RandomProvider.Instance.NextDouble() * ySize;
            
            game.treasure = new Treasure(randomX, randomY);
            game.isTreasureFound = false;

            for (int i = 0; i < numberOfAgents; i++)
            {
                randomX = RandomProvider.Instance.NextDouble() * xSize;
                randomY = RandomProvider.Instance.NextDouble() * ySize;

                Player player = new Player(randomX, randomY);
                // Add them to the list of players belonging to the space.
                // Add it to the list of balls belonging to the space.
                game.playerList.Add(player);

            }

        }
        public List<int> RunBenchmark()
        {
            List<int> stepsCount = new List<int>();

            while (game.isTreasureFound == false)
            {
                game.UpdatePlayers();

            }

            

            foreach (Player player in game.playerList)
            {
                stepsCount.Add(player.stepsNumber);
            }
            
            // Reset the game
            InitializeGame();
            return stepsCount;
        
        }

    }


}
