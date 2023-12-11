using System;

using System.Collections;
using System.Collections.Generic;

using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace TreasureHunt
{


    // static Game StaticGame { get; set; }


    public class Game
    {
        // Properties

        public List<Player> playerList;
        public Treasure treasure;
        public bool isTreasureFound;
        public double xSize;
        public double ySize;

        // Constructor
        public Game(double myXSize, double myYSize)
        {
            xSize = myXSize;
            ySize = myYSize;
            isTreasureFound = false;

            // Generate a treasure at a random location
            Random random = new Random();
            double randomX = random.NextDouble() * xSize;
            double randomY = random.NextDouble() * ySize;

            treasure = new Treasure(randomX, randomY);

            playerList = new List<Player>();

        }

        // Methods

        public void UpdatePlayers()
        {

            for (int i = 0; i < playerList.Count; i++)
            {
                // Check distance between player and treasure

                //if (playerList[i].GetLocation().X < treasure.GetLocation().X)
                //        {
                //          playerList[i].lowerBoundX = playerList[i].GetLocation().X;
                //        }
                //
                //
                //        if (playerList[i].GetLocation().X > treasure.GetLocation().X)
                //        {
                //          playerList[i].higherBoundX = playerList[i].GetLocation().X;
                //        }
                //
                //
                //        if (playerList[i].GetLocation().Y < treasure.GetLocation().Y)
                //        {
                //          playerList[i].lowerBoundY = playerList[i].GetLocation().Y;
                //        }
                //
                //
                //        if (playerList[i].GetLocation().Y > treasure.GetLocation().Y)
                //        {
                //          playerList[i].higherBoundY = playerList[i].GetLocation().Y;
                //        }


                double tolerance = 10;
                // Check if player is inside boundaries, if not, make it bounce
                if (playerList[i].GetLocation().X > xSize - tolerance || playerList[i].GetLocation().X < 0 + tolerance)
                {
                    playerList[i].speed.X = -playerList[i].speed.X;
                }
                if (playerList[i].GetLocation().Y > ySize - tolerance || playerList[i].GetLocation().Y < 0 + tolerance)
                {
                    playerList[i].speed.Y = -playerList[i].speed.Y;
                }

                playerList[i].Move();

            }
            // Update movingBall bounds to identify if it is higher or lower.




        }







    }

    public class Treasure
    {
        // Properties
        private Vector3d location;
        // Constructor
        public Treasure(double x, double y)
        {
            location = new Vector3d(x, y, 0);
        }
        // Methods
        public Vector3d GetLocation()
        {
            return location;
        }
    }

    public class Player
    {

        // PROPERTIES
        public Vector3d location;
        public int stepsNumber;
        public Vector3d speed;

        public string statusMessage;
        // Boundaries for the binary search
        public double lowerBoundX;
        public double higherBoundX;
        public double lowerBoundY;
        public double higherBoundY;

        // Constructor
        public Player()
        {
            location = new Vector3d(0, 0, 0);
            lowerBoundX = 0;
            higherBoundX = 0;
            lowerBoundY = 0;
            higherBoundY = 0;
        }

        public Player(double x, double y, double myLowerBoundX, double myLowerBoundY, double myHigherBoundX, double myHigherBoundY)
        {
            location = new Vector3d(x, y, 0);
            speed = new Vector3d(10, 10, 0);
            lowerBoundX = myLowerBoundX;
            lowerBoundY = myLowerBoundY;
            higherBoundX = myHigherBoundX;
            higherBoundY = myHigherBoundY;

        }

        // Methods
        public Vector3d GetLocation()
        {
            return location;
        }

        public virtual void Move()
        {
            location = Vector3d.Add(location, speed);
            stepsNumber++;

            statusMessage = string.Format("number of steps : {0}, lowerBoundX : {1:0.00}, higherBoundX : {2:0.00}", stepsNumber, lowerBoundX, higherBoundX);
        }

    }

    public class CoolPlayer : Player
    {
        public string coolness;

        public CoolPlayer() : base()
        {
 
            coolness = "verycool";
        }


    }

    //public class Lead : Player
    //{
    //    public int directionChangeInterval;
    //    public int directionChangeCountdown;
    //    public Vector3d acceleration;

    //    public Lead(double startX, double startY, double myLowerBoundX, double myLowerBoundY, double myHigherBoundX, double myHigherBoundY, int changeInterval)
    //      : base(startX, startY, myLowerBoundX, myLowerBoundY, myHigherBoundX, myHigherBoundY)
    //    {
    //        directionChangeInterval = changeInterval;
    //        directionChangeCountdown = directionChangeInterval;
    //    }

    //    public override void Move()
    //    {
    //        if (directionChangeCountdown <= 0)
    //        {
    //            // Change direction after countdown reaches zero
    //            ChangeDirection();
    //            directionChangeCountdown = directionChangeInterval;
    //        }
    //        else
    //        {
    //            // Keep moving in the current direction
    //            //MoveInDirection(preferredDirections[0]);
    //            directionChangeCountdown--;
    //        }

    //        speed = Vector3d.Add(speed, acceleration);
    //        location = Vector3d.Add(location, speed);
    //        stepsNumber++;

    //        statusMessage = string.Format("number of steps : {0}, lowerBoundX : {1:0.00}, higherBoundX : {2:0.00}", stepsNumber, lowerBoundX, higherBoundX);


    //    }

    //    private void ChangeDirection()
    //    {
    //        this.speed.X = 1 * 2 - 1;
    //        this.speed.Y = 1 * 2 - 1;

    //        //this.speed.X = RandomProvider.Instance.NextDouble() * 2 - 1;
    //        //this.speed.Y = RandomProvider.Instance.NextDouble() * 2 - 1;
    //    }

    //    //public static class RandomProvider
    //    //{
    //    //    private static readonly Random _random = new Random();

    //    //    public static Random Instance
    //    //    {
    //    //        get { return _random; }
    //    //    }
    //    //}
    //}

    //public class Follower : Player
    //{
    //    public Lead master;
    //    public double randomDirectionChangeProbability;

    //    public Follower(double startX, double startY, double myLowerBoundX, double myLowerBoundY, double myHigherBoundX, double myHigherBoundY, Lead masterAgent, double randomChangeProb)
    //      : base(startX, startY, myLowerBoundX, myLowerBoundY, myHigherBoundX, myHigherBoundY)
    //    {
    //        master = masterAgent;
    //        randomDirectionChangeProbability = randomChangeProb;
    //    }

    //    public override void Move()
    //    {
    //        // Simulate movement of the slave agent
    //        Vector3d directionToMaster = Vector3d.Subtract(master.GetLocation(), location);
    //        double magnitude = directionToMaster.Length;

    //        if (magnitude < 20)
    //        {
    //            double desiredMag = Map(magnitude, 0, 20, 0, 0.75);
    //            directionToMaster.Unitize();
    //            directionToMaster *= desiredMag;

    //        }
    //        else
    //        {
    //            // Follow the master's path with slight deviation
    //            directionToMaster.Unitize();
    //            directionToMaster *= 0.75; // Adjust this value for the desired follow distance
    //        }

    //        speed = directionToMaster; // Set the acceleration towards the master


    //        //speed = Vector3d.Add(speed, acceleration);
    //        location = Vector3d.Add(location, speed);
    //        stepsNumber++;

    //        statusMessage = string.Format("number of steps : {0}, lowerBoundX : {1:0.00}, higherBoundX : {2:0.00}", stepsNumber, lowerBoundX, higherBoundX);


    //        //Move(); // Call the base move method from Player class
    //    }

    //    public static double Map(double value, double fromSource, double toSource, double fromTarget, double toTarget)
    //    {
    //        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    //    }

    //}


}
