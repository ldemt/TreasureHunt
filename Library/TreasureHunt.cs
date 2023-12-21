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

    public class CustomPlayer : Player
    {
        public string coolness;

        public CustomPlayer(double x, double y) : base(x,y)
        {
            coolness = "very cool";
        }


    }

    // New class for obstacles
    class Obstacle
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public double Rotation { get; private set; }
        public double RotationSpeed { get; private set; }

        // Constructor with four arguments
        public Obstacle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Rotation = 15; // Initial rotation angle
            RotationSpeed = 40; // Initial rotation speed
        }

        // Method to rotate the obstacle
        public void Rotate()
        {

            Rotation += RotationSpeed;

            // I can add logic here to handle rotation of the obstacle
            // For example, updating the angle or performing rotation calculations
            // based on the RotationSpeed
        }
    }
}
