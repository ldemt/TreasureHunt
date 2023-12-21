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
        public List<Team> teamList;
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
            double modif = 100;
            treasure = new Treasure(randomX, randomY);

            playerList = new List<Player>();
            teamList = new List<Team>();

        }

        // Methods
        public void UpdateTeams()
        {

            for (int i = 0; i < teamList.Count; i++)
            {
                teamList[i].MoveTeam();
            }
        }
        public void UpdatePlayers()
        {

            CheckIsTreasureFound();

            if (!isTreasureFound) {

                MovePlayers();
                if (teamList.Count > 0)
                {
                    UpdateTeams();
                }
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

    public class TinLeader : Player
    {
        public TinLeader(double x, double y)
        : base(x, y) { }
    }

    public class TinFollower : Player
    {
        public double lowerBoundX;
        public double lowerBoundY;
        public double higherBoundX;
        public double higherBoundY;

        public TinFollower(double x, double y,double myHigherBoundX, double myHigherBoundY)
        : base(x, y) {

            lowerBoundX = 0;
            lowerBoundY = 0;
            higherBoundY = myHigherBoundY;
            higherBoundX = myHigherBoundX;
        }
        public override void Move()
        {
            Vector3d randomVector = new Vector3d();

            do
            {
                Random random = new Random();
                randomVector.X = (random.NextDouble() * 2 - 1) * 20;
            }
            while (location.X + randomVector.X < lowerBoundX || location.X + randomVector.X > higherBoundX);
            do
            {
                Random random = new Random();
                randomVector.Y = (random.NextDouble() * 2 - 1) * 20;
            }
            while (location.Y + randomVector.Y < lowerBoundY || location.Y + randomVector.Y > higherBoundY);

            location.X += randomVector.X;
            location.Y += randomVector.Y;
            location.Z += randomVector.Z;
        }
    }
    public class Team
    {
        public TinLeader leader;
        public TinFollower follower;
        public Team(TinLeader myLeader, TinFollower myFollower)
        {
            leader = myLeader;
            follower = myFollower;

        }
        public void MoveTeam()
        {

            leader.Move();
            Point3d myLocation = new Point3d();
            myLocation = (Point3d)leader.location;
            follower.location.X = myLocation.X;
            follower.location.Y = myLocation.Y;
            follower.location.Z = myLocation.Z;

            follower.Move();
        }

    }
}
