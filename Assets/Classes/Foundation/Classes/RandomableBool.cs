namespace Assets.Classes.Foundation.Classes
{
    public enum RandomableBool
    {
        True,
        False,
        Random
    }

    public static class RandomableBoolUtils
    {

        public static RandomableBool Random()
        {
            return UnityEngine.Random.Range(0, 2) == 1 ? RandomableBool.True : RandomableBool.False;
        }

        public static bool RandomBool()
        {
            return Random() == RandomableBool.True;
        }

        public static RandomableBool Normalize(RandomableBool b)
        {
            if (b == RandomableBool.Random)
            {
                return Random();
            }
            return b;
        }

        public static bool ToBool(RandomableBool b)
        {
            if (b == RandomableBool.Random) return RandomBool();
            return b == RandomableBool.True;
        }

        public static bool True(this RandomableBool b)
        {
            return ToBool(b);
        }
        public static bool False(this RandomableBool b)
        {
            return !ToBool(b);
        }
    }

}
