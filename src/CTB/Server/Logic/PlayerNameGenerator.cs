using System;

namespace CTB.Server.Logic;

public static class PlayerNameGenerator
{
    public static string CreateName()
    {
        var first = new string[]
        {
            "Cool", "Super", "Mighty", "Winning", "Champion", "King",
            "Queen", "Mighthy", "Amazing", "Incredible", "Unbeatable",
            "Angry", "General", "Wimpy", "Red", "Blue", "Yellow",
            "Brave", "Big", "Jolly", "Silly", "Magnificient",
            "Scary", "Tiny", "Lazy", "Clumsy", "Muscular",
            "Gigantic", "Good", "Anxious", "Arrogant",
            "Dangerous", "Fragile", "Gifted", "Impossible",
            "Kind", "Powerful", "Proud", "Precious",
            "Perfect", "Sleepy", "Stormy", "Strange",
            "Super", "Victorious", "Wicked", "Wild"
        };
        var second = new string[]
        {
            "Mouse", "Dog", "Cat", "Horse", "House", "Car", "Road",
            "Rat", "Diamond", "Parrot", "Tiger", "Armadillo",
            "Penguin", "Foxhound", "Hornet", "Elephant",
            "Dolphin", "Albatross", "Leopard", "Fox",
            "Terrier", "Spaniel", "Wolf", "Baboon", "Camel",
            "Fish", "Owl", "Marlin", "Bear", "Frog",
            "Lizard", "Chimpanzee", "Cheetah", "Chihuahua",
            "Hawk", "Cougar", "Duck", "Deer", "Dingo",
            "Crocodile", "Dragon", "Eagle", "Gorilla",
            "Seal", "Falcon", "Whale", "Flamingo", "Squirrel",
            "Gecho", "Tortoise", "Worm", "Pig", "Hamster",
            "Hedgehog", "Bee", "Crab", "Hyena", "Bird",
            "Beetle", "Hippopotamus", "Iguana", "Impala",
            "Jaguar", "Jellyfish", "Kangaroo", "Cobra",
            "Koala", "Lynx", "Lemur", "Llama", "Mole",
            "Mink", "Mule", "Monkey", "Octopus", "Opossum",
            "Oyster", "Panther", "Parrot", "Pelican",
            "Piranha", "PoisonFrog", "Poodle", "Puma",
            "Rabbit", "Raccoon", "Reindeer", "Turtle",
            "Rottweiler", "Salmon", "Toad", "Squid",
            "Swan", "TasmanianDevil", "Bat", "Walrus",
            "Woodpecker", "Wolverine", "Wombat"
        };

        var random = new Random();
        return $"{first[random.Next(first.Length)]} {second[random.Next(second.Length)]}";
    }
}
