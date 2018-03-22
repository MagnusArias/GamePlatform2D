using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePlatform2D
{
    public class Animation
    {
        #region Variables
        private Texture2D image;
        private string text;
        private SpriteFont font;
        private Color color, drawColor;
        private Rectangle sourceRect;
        private float rotation, scale, alpha;
        private Vector2 origin, position, frames, currentFrame;
        private ContentManager content;
        private bool isActive;
        protected int numFrames, delay;
        protected int timesPlayed;

        #endregion

        #region Properties

        /// <summary>
        /// Kolor do rysowania, domyślnie używany Color.White
        /// </summary>
        public Color DrawColor
        {
            set { drawColor = value; }
        }

        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        /// <summary>
        /// Pozycja animacji
        /// </summary>
        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }
        
        /// <summary>
        /// Czy animacja jest aktywna, w przypadku 'false' nie jest rysowana ani przewijana
        /// </summary>
        public bool IsActive
        {
            set { isActive = value; }
            get { return isActive; }
        }

        /// <summary>
        /// Ilość klatek animacji
        /// </summary>
        public int NumFrames
        {
            get { return numFrames; }
            set { numFrames = value; }
        }
        
        /// <summary>
        /// Przezroczystosc animacji
        /// </summary>
        /// 
        public virtual float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }
       
        /// <summary>
        /// Skala animacji, jednowymiarowa
        /// </summary>
        public virtual float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        
        /// <summary>
        /// Czcionka
        /// </summary>
        public virtual SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        /// <summary>
        /// Ilość klatek animacji (łączna wartość)
        /// </summary>
        public Vector2 Frames
        {
            set { frames = value; }
            get { return frames; }
        }

        /// <summary>
        /// Aktualnie wyświetlana klatka
        /// </summary>
        public Vector2 CurrentFrame
        {
            set { currentFrame = value; }
            get { return currentFrame; }
        }
        
        /// <summary>
        /// Szerokośc klatki
        /// </summary>
        public int FrameWidth
        {
            get { return image.Width / (int)Frames.X; }
        }
       
        /// <summary>
        /// Szerokość klatki
        /// </summary>
        public int FrameHeight
        {
            get { return image.Height / (int)Frames.Y; }
        }

        /// <summary>
        /// Klatka źródłowa
        /// </summary>
        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        /// <summary>
        /// Grafika, z której korzystamy
        /// </summary>
        public Texture2D Image
        {
            get { return image; }
        }
        #endregion

        #region Public Methods
        public void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");

            this.image = image;
            this.text = text;
            this.position = position;

            if (text != String.Empty)
            {
                font = this.content.Load<SpriteFont>("AnimationFont");
                color = new Color(114, 77, 233);
            }

            rotation = 0.0f;
            scale = 1.0f;
            alpha = 1.0f;
            timesPlayed = 0;
            isActive = false;
            drawColor = Color.White;

            //numFrames = 0;
            currentFrame = new Vector2(0, 0);
            if (image != null && frames != Vector2.Zero)
                sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
            else
                sourceRect = new Rectangle(0, 0, image.Width, image.Height);
        }

        public void UnloadContent()
        {
            content.Unload();
            text = String.Empty;
            position = Vector2.Zero;
            sourceRect = Rectangle.Empty;
            image = null;
        }

        public virtual void Update(GameTime gameTime, ref Animation a)
        {

        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects se)
        {
            if (image != null)
            {
                origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
                spriteBatch.Draw(
                    texture: image, 
                    position: position + origin, 
                    sourceRectangle: sourceRect, 
                    color: drawColor * alpha, 
                    rotation: rotation, 
                    origin: origin,
                    scale: scale, 
                    effects: se, 
                    layerDepth: 0.0f);
            }

            if (text != String.Empty)
            {
                origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
                spriteBatch.DrawString(font, text, position + origin, color * alpha, rotation, origin, scale, SpriteEffects.None, 0.0f);
            }
        }

        public bool HasPlayedOnce()
        {
            return timesPlayed > 0;
        }

        public bool HasPlayed(int i)
        {
            return timesPlayed == i;
        }
        #endregion
    }
}
