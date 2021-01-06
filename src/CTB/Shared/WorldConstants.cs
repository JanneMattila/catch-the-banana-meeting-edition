using CTB.Shared.Interfaces;

namespace CTB.Shared
{
    public static class WorldConstants
    {
        public const int BorderRadius = 10;

        public static readonly Size Screen = new(640, 480);
        public static readonly Size Banana = new(7, 12);
        public static readonly Size Monkey = new(16, 16);

        // Note: Significantly smaller to require large overlap to be able to catch the monkey
        public static readonly Size Shark = new(30, 8);
    }
}
