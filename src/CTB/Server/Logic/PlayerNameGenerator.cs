using System;

namespace CTB.Server.Logic
{
    public static class PlayerNameGenerator
    {
        public static string CreateName()
        {
            var first = new string[]
            {
                "Cool", "Super", "Mighty", "Winning", "Champion"
            };
            var second = new string[]
            {
                "Mouse", "Dog", "Cat", "Horse", "House", "Car", "Road"
            };

            var random = new Random();

            return $"{first[random.Next(first.Length)]}{second[random.Next(second.Length)]}{random.Next(10, 100)}";
        }
    }
}
