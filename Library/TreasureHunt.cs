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

            CheckIsTreasureFound();

            if (!isTreasureFound) {

                MovePlayers();
            }


        }
        public void CheckIsTreasureFound()
        {
            double tolerance = 10;

            for (int i = 0; i < playerList.Count; i++)
            {
                // Check if player is near treasure, if yes, put isTreasureFound to True


                if (playerList[i].location.EpsilonEquals(treasure.GetLocation(), tolerance))
                {
                    isTreasureFound = true;

                }


            }

        }
        public void MovePlayers()
        {

            for (int i = 0; i < playerList.Count; i++)
            {

                double tolerance = 10;
                // Check if player is inside boundaries, if not, make it bounce
                if (playerList[i].location.X > xSize - tolerance || playerList[i].location.X < 0 + tolerance)
                {
                    playerList[i].speed.X = -playerList[i].speed.X;
                }

                if (playerList[i].location.Y > ySize - tolerance || playerList[i].location.Y < 0 + tolerance)
                {
                    playerList[i].speed.Y = -playerList[i].speed.Y;
                }

                playerList[i].Move();

            }

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


        // Constructors
        public Player()
        {
            location = new Vector3d(0, 0, 0);
        }

        public Player(double x, double y)
        {
            location = new Vector3d(x, y, 0);

            // Sets a default speed
            speed = new Vector3d(10, 10, 0);


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

            statusMessage = string.Format("number of steps : {0}", stepsNumber);
        }

    }

    public class CustomPlayer : Player
    {
        public string coolness;

        public CustomPlayer(double x, double y) : base(x, y)
        {
            coolness = "very cool";
        }


    }


    public class HayderPlayer : Player
    {

        int limitCount;
        double lowerBoundX;
        double lowerBoundY;
        double higherBoundX;
        double higherBoundY;

        // Flag to toggle the zigzag direction

        private bool zigzagDirection = true;
        private int movesBeforeTurn;

        public HayderPlayer (double x, double y, double myLowerBoundX, double myLowerBoundY, double myHigherBoundX, double myHigherBoundY, Vector3d mySpeed)
          : base (x, y, myLowerBoundX, myLowerBoundY, myHigherBoundX, myHigherBoundY, mySpeed)
        {
            movesBeforeTurn = 0;
        }

        public override void Move()
        {
            movesBeforeTurn++;

            double locTolerance = 10;

            // Check if moving beyond bounds in X-axis and adjust speed to stay within bounds
            if (location.X < lowerBoundX + locTolerance)
            {
                location.X = lowerBoundX + locTolerance + Math.Abs(speed.X);
                speed.X = Math.Abs(speed.X); // Move forward in the X-axis

                // Move up for 50 units
                location.Y += 50;
            }
            else if (location.X > higherBoundX - locTolerance)
            {
                location.X = higherBoundX - locTolerance - Math.Abs(speed.X);
                speed.X = -Math.Abs(speed.X); // Move backward in the X-axis

                // Move up for 50 units
                location.Y += 50;
            }

            // Check if moving beyond bounds in Y-axis and adjust speed to stay within bounds
            if (location.Y > higherBoundY - locTolerance)
            {
                // Move down for 50 units
                location.Y = higherBoundY - locTolerance - Math.Abs(speed.Y);
                speed.Y = -Math.Abs(speed.Y); // Move downward in the Y-axis
            }
            else if (location.Y < lowerBoundY + locTolerance)
            {
                // Move up for 50 units
                location.Y = lowerBoundY + locTolerance + Math.Abs(speed.Y);
                speed.Y = Math.Abs(speed.Y); // Move upward in the Y-axis
            }

            // Zigzag movement
            if (movesBeforeTurn > higherBoundY)
            {
                // Toggle the direction in the Y-axis
                zigzagDirection = !zigzagDirection;

                // Toggle between X and Y movement
                if (zigzagDirection)

                {
                    speed.Y = -speed.Y;

                }

                movesBeforeTurn = 0;
            }

            // Move within boundaries
            location.X += speed.X;
            location.Y += speed.Y;

            stepsNumber++;

            statusMessage = string.Format("number of steps: {0}, lowerBoundX: {1:0.00}, higherBoundX: {2:0.00}", stepsNumber, lowerBoundX, higherBoundX);
        }

    }

}
