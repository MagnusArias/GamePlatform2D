using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class FloatRect
    {
        float top, bottom, left, right;

        public float Top { get => top; }
        public float Bottom { get => bottom; }
        public float Left { get => left; }
        public float Right { get => right; }

        public FloatRect(float x, float y, float width, float height)
        {
            left = x;
            right = x + width;
            bottom = y + height;
            top = y;
        }

        public bool Intersects(FloatRect f)
        {
            if (right <= f.Left || left >= f.Right || top >= f.Bottom || bottom <= f.Top)
                return false;
            else return true;
        }
    }
}
