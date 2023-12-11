using System;
using TreasureHunt;
namespace TestAppTHunt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            CoolPlayer player = new CoolPlayer();

            string checkCoolness = player.coolness;
        }
    }
}
