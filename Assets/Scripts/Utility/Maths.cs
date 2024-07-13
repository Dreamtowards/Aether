namespace Aether
{
    public static class Maths
    {
        public static int Floor16(int i)
        {
            return i & (~15);
        }

        public static int Mod16(int i)
        {
            return i & 15;
        }
    }
}