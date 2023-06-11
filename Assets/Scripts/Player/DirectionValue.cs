using System;
using UnityEngine;

namespace Player
{
    public class Direction
    {
        
        public Direction()
        {
            _value = DirectionValue.Bottom;
        }

        public Direction(DirectionValue value)
        {
            _value = value;
        }

        private DirectionValue _value;

        public void SetValue(DirectionValue value)
        {
            _value = value;
        }

        public void SetValue(Vector2 vectorValue)
        {
            var normalizedInput = vectorValue.normalized;
            if (normalizedInput == new Vector2(0, 0))
            {
                return;
            }

            if (normalizedInput == new Vector2(0, 1))
            {
                _value = DirectionValue.Top;
                return;
            }

            if (normalizedInput == new Vector2(-1, 0))
            {
                _value = DirectionValue.Left;
                return;
            }

            if (normalizedInput == new Vector2(1, 0))
            {
                _value = DirectionValue.Right;
                return;
            }

            if (normalizedInput == new Vector2(0, -1))
            {
                _value = DirectionValue.Bottom;
            }
        }


        public DirectionValue GetDirectionValue()
        {
            return _value;
        }

        public Vector2 GetDirectionVector()
        {
            switch (_value)
            {
                case DirectionValue.Top:
                    return new Vector2(0, 1);
                case DirectionValue.Left:
                    return new Vector2(-1, 0);
                case DirectionValue.Right:
                    return new Vector2(1, 0);
                case DirectionValue.Bottom:
                    return new Vector2(0, -1);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum DirectionValue
    {
        Top,
        Left,
        Right,
        Bottom,
    }
}