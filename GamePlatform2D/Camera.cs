﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace GamePlatform2D
{
    public class Camera
    {
        private static Camera instance;
        Vector2 position;
        Matrix viewMatrix;

        public static Camera Instance
        {
            get
            {
                if (instance == null)
                    instance = new Camera();
                return instance;
            }
        }

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        public void SetFocalPoint(Vector2 focalPosition)
        {
            position = new Vector2(
                focalPosition.X - ScreenManager.Instance.Dimensions.X / ScreenManager.Instance.Scale.X / 2,
                focalPosition.Y - ScreenManager.Instance.Dimensions.Y / ScreenManager.Instance.Scale.Y / 2);

            if (position.X < 0) position.X = 0;
            if (position.Y < 0) position.Y = 0;
        }

        public void Update()
        {
            //viewMatrix.Scale = new Vector3(ScreenManager.Instance.Scale, ScreenManager.Instance.Scale, 1);
            viewMatrix = Matrix.CreateTranslation(new Vector3(-position* ScreenManager.Instance.Scale, 0));
            viewMatrix.Scale = new Vector3(ScreenManager.Instance.Scale.X, ScreenManager.Instance.Scale.Y, 1);
        }
    }
}
