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

    public class SpiralMovement : Player
    {
        // Additional properties for bounds and speed
        double mySpeedX;
        double mySpeedY;
        public double SpiralRadius { get; set; } = 10.0;

        // Constructor
        public SpiralMovement(double x, double y)
            : base(x, y)
        {
            mySpeedX = 0.2;
            mySpeedY = 0.2;
        }


        // Override the Move method to implement the spiral movement
        public override void Move()
        {
            // Call the base class Move method to update the location based on the speed
            base.Move();

            // Implement the spiral movement logic here
            double radius = SpiralRadius;
            double angularSpeed = 0.6;

            double angle = stepsNumber * angularSpeed;
            double spiralX = (radius + (stepsNumber / 5)) * Math.Cos(angle);
            double spiralY = (radius + (stepsNumber / 5)) * Math.Sin(angle);

            // Update the location based on the spiral movement
            location = new Vector3d(location.X + spiralX, location.Y + spiralY, 0);
            double increase = stepsNumber / 2;
            increase++;

        }

    }
    public class ZigzagPlayer : Player
    {

        // Properties
        int limitCount;
        double lowerBoundX;
        double lowerBoundY;
        double higherBoundX;
        double higherBoundY;

        // Declare the missing properties (lowerboundY, etc..)


        // Constructor(s)

        public ZigzagPlayer(double x, double y, double myLowerBoundX, double myLowerBoundY, double myHigherBoundX, double myHigherBoundY)
        {
            location = new Vector3d(x, y, 0);
            lowerBoundX = myLowerBoundX;
            lowerBoundY = myLowerBoundY;
            higherBoundX = 10;
            higherBoundY = myHigherBoundY;
            speed = GenerateRandomVector();

        }

        // Methods
        public override void Move()
        {

            Vector3d newPosition = location + speed * 5;
            double tolerance = 10;
            // check and adjust for X boundaries
            if (newPosition.X < lowerBoundX + tolerance)
            {
                newPosition.X = lowerBoundX + tolerance;
                speed.X = -speed.X;//reverse X speed to bounce back
            }
            else if (newPosition.X > higherBoundX - tolerance)
            {
                newPosition.X = higherBoundX - tolerance;
                speed.X = -speed.X;//reverse X speed to bounce back
            }
            // check and adjust for Y boundaries
            if (newPosition.Y < lowerBoundY + tolerance)
            {
                newPosition.Y = lowerBoundY + tolerance;
                speed.Y = -speed.Y;// Reverse Y speed to bounce back

            }
            else if (newPosition.Y > higherBoundY - tolerance)
            {
                newPosition.Y = higherBoundY - tolerance;
                speed.Y = -speed.Y;
            }

            location = newPosition;
            stepsNumber++;
            limitCount++;

            if (higherBoundX < 600)
            {
                if (limitCount > 50)
                {
                    higherBoundX = higherBoundX + 20;
                    limitCount = 0;
                }
            }
            statusMessage = string.Format("number of steps : {0}, lowerBoundX : {1}, higherBoundX : {2:0.00}", stepsNumber, lowerBoundX, higherBoundX, location);

        }

        public Vector3d GenerateRandomVector()
        {
            Random random = new Random();
            double randomX = random.NextDouble() * 5 - 1;
            double randomY = random.NextDouble() * 5 - 1;

            return new Vector3d(randomX, randomY, 0);
        }

    }

    public class HugoFollower : Player
    {
        // PROPERTIES
        Player leader;
        double rotationAngle;

        // CONSTRUCTOR
        public HugoFollower(Player player)
        {
            leader = player;
        }

        // METHODS
        public override void Move()
        {
            double radius = 15; // Set the radius of rotation
            rotationAngle += 1.0; // Set the rotation speed

            location.X = leader.GetLocation().X + radius * Math.Cos(rotationAngle);
            location.Y = leader.GetLocation().Y + radius * Math.Sin(rotationAngle);
        }

    }

        
    }


