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
                

                if (playerList[i].location.EpsilonEquals(treasure.GetLocation(),tolerance))
                { 
                isTreasureFound=true;
                
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

    class SpiralMovement : Player
    {
        // Constructor
        public SpiralMovement(double x, double y, double myLowerBoundX, double myLowerBoundY, double myHigherBoundX, double myHigherBoundY, double mySpeedX, double mySpeedY)
            : base(x, y, myLowerBoundX, myLowerBoundY, myHigherBoundX, myHigherBoundY, mySpeedX, mySpeedY)
        {

        }

        public double SpiralRadius { get; set; } = 15.0;

        // Override the Move method to implement the spiral movement
        public override void Move()
        {
            // Call the base class Move method to update the location based on the speed
            base.Move();


            // Implement the spiral movement logic here
            double radius = 15; // You can adjust the radius of the spiral
            double angularSpeed = 0.4; // You can adjust the angular speed of the spiral

            double angle = stepsNumber * angularSpeed;
            double spiralX = (radius + (stepsNumber / 80)) * Math.Cos(angle);
            double spiralY = (radius + (stepsNumber / 80)) * Math.Sin(angle);

            // Update the location based on the spiral movement
            location = new Vector3d(location.X + spiralX + mySpeedX, location.Y + spiralY + mySpeedY, 0);
            stepsNumber++;

            // Update the bounds based on the new location
            lowerBoundX = location.X - SpiralRadius;
            higherBoundX = location.X + SpiralRadius;
            lowerBoundY = location.Y - SpiralRadius;
            higherBoundY = location.Y + SpiralRadius;

        }

    }


}
