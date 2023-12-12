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

            if (!isTreasureFound)
            {

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
        
        public Vector3d location;
        public int stepsNumber;
        public Vector3d speed;
        
        public string statusMessage;
        public bool treasureFound = false;


        public Player() { }

        public Player(double x, double y)
        {
            location = new Vector3d(x, y, 0);
            speed = new Vector3d(10, 10, 0);

        }

        public Player(double x, double y, double speedX, double speedY)
        {
            location = new Vector3d(x, y, 0);
            speed = new Vector3d(speedX, speedY, 0);
            
        }

        public Vector3d GetLocation()
        {
            return location;
        }

        public virtual void Move()
        {
            location = speed + location;
            stepsNumber++;

            statusMessage = string.Format("number of steps : {0}", stepsNumber);
            // statusMessage = "V-S Search";
        }

        public virtual void SetTreasureFound()
        {
            treasureFound = true;
        }
    }

    public class VictorSierraPlayer : Player
    {
        private double turnAngle = 120.0; // Angle to turn in degrees, this is specified by V-S standards but I am experimenting with changing this
        private int triangleSize = 20; // Distance to move in the current step
        private int stepsSinceTurn = 0; // Number of steps taken since the last turn
        private int stepsSinceSkip = 0;
        private bool turnClockwise = true; // Direction of the turn
        public Vector3d drift;
        protected bool moveRight = true;

        public VictorSierraPlayer(double x, double y, double speedX, double speedY, double driftX, double driftY)
          : base(x, y, speedX, speedY)
        {

            drift = new Vector3d(driftX, driftY, 0);

        }

        public override void Move()
        {

            location.X += moveRight ? (speed.X + drift.X) : (-speed.X + drift.X);
            location.Y += speed.Y + drift.Y;

            stepsNumber++;
            stepsSinceTurn++;
            stepsSinceSkip++;

            // Check if it's time to turn

            if (stepsSinceTurn >= triangleSize)
            {
                if (stepsSinceSkip <= triangleSize * 2)
                {
                    // Turn
                    Turn();
                }

                // Reset steps since turn

                stepsSinceTurn = 0;
            }

            if (stepsSinceSkip >= triangleSize * 3)
            {

                // Turn

                stepsSinceSkip = 0;
            }


            

            if (treasureFound) statusMessage = "Treasure found!!";
            else statusMessage = "V-S";

        }

        public void TeleportAtBoundary(double lowerBoundX, double higherBoundX, double lowerBoundY, double higherBoundY)

        {

            // Wrap around the bounds
            // Here simply if it reaches upper bound the y value returns to lower bound
            // And the same with the X on the sides

            if (location.X < lowerBoundX) location.X = higherBoundX;
            else if (location.X > higherBoundX) location.X = lowerBoundX;

            if (location.Y < lowerBoundY) location.Y = higherBoundY;
            else if (location.Y > higherBoundY) location.Y = lowerBoundY;


        }

        private void Turn()
        {
            // Change direction based on the turn

            turnClockwise = turnClockwise;

            // Update distance for the next step

            // Rotate the speed vector based on the turn angle

            double radians = turnAngle * (Math.PI / 180.0);
            double newX = speed.X * Math.Cos(radians) - (turnClockwise ? 1 : -1) * speed.Y * Math.Sin(radians);
            double newY = (turnClockwise ? 1 : -1) * speed.X * Math.Sin(radians) + speed.Y * Math.Cos(radians);

            speed.X = newX;
            speed.Y = newY;
        }

        private int CalculateStepsForDistance(double distance)
        {
            // Adjust this factor to control the smoothness of the turns

            double factor = 0.1;
            return (int)Math.Ceiling(distance / factor);
        }
    }

}