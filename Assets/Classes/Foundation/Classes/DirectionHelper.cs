using System.Collections.Generic;
using Assets.Classes.Foundation.Enums;
using UnityEngine;

namespace Assets.Classes.Foundation.Classes
{
    public static class DirectionHelper
    {

        public static Vector2 CalculateDirectionalSize(float width, float height, Direction direction)
        {
            if (direction == Direction.Left || direction == Direction.Right)
                return new Vector2(width, height);
            return new Vector2(height, width);
        }

        public static Vector3 ExtrapolateDirection(Vector3 origin, Direction direction, float value)
        {
            var offset = Vector3.zero;

            switch (direction)
            {
                case Direction.Left:
                    offset = new Vector3(-value, 0);
                    break;
                case Direction.Right:
                    offset = new Vector3(value, 0);
                    break;
                case Direction.Top:
                    offset = new Vector3(0, value);
                    break;
                case Direction.Bottom:
                    offset = new Vector3(0, -value);
                    break;
            }
            return origin + offset;
        }

        public static bool IsHorizontal(Direction d)
        {
            return d == Direction.Left || d == Direction.Right;
        }

        public static bool IsVertical(Direction d)
        {
            return !IsHorizontal(d);
        }

        public static IEnumerable<Direction> EnumerateDirections()
        {
            yield return Direction.Top;
            yield return Direction.Bottom;
            yield return Direction.Left;
            yield return Direction.Right;
        }

        public static HorizontalDirection SwapHorizontalDirection(HorizontalDirection hd)
        {
            return hd == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left;
        }

        public static VerticalDirection SwapVerticalDirection(VerticalDirection vd)
        {
            return vd == VerticalDirection.Top ? VerticalDirection.Bottom : VerticalDirection.Top;
        }
    }
}
