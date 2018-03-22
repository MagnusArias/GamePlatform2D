using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GamePlatform2D
{
    public class Player : Entity
    {
        List<Bullet> bullets;
        Texture2D bulletImage;
        int flinchCount;
        bool skill_doubleJump;
        private static Texture2D collisionRect;

        public override void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input, Layer l)
        {
            base.LoadContent(content, attributes, contents, input , l);
            string[] saveAttribute = { "PlayerPosition" };
            string[] saveContent = { position.X.ToString() + ',' + position.Y.ToString() };

            fileManager = new FileManager();
            fileManager.SaveContent("Load/Maps/Map1.mma", saveAttribute, saveContent, "");

            bullets = new List<Bullet>();
            bulletImage = content.Load<Texture2D>("bullet");

            speed.move = 0.5f;
            speed.max = 2.8f;
            speed.stop = 0.1f;

            speed.jump = -3.5f;
            speed.stopJump = 0.3f;
            speed.doubleJump = -5.0f;
            speed.fall = 0.1f;
            speed.maxFall = 3.0f;
            skill_doubleJump = false;

            alreadyDoubleJump = false;
            collisionBox.X = (int)(moveAnimation.FrameWidth-10);
            collisionBox.Y = moveAnimation.FrameHeight;

        }

        private void DrawRectangle(Rectangle coords, Color color, SpriteBatch spriteBatch)
        {
            if (collisionRect == null)
            {
                collisionRect = new Texture2D(ScreenManager.Instance.GraphicsDevice, 1, 1);
                collisionRect.SetData(new[] { Color.White });
            }
            spriteBatch.Draw(collisionRect, coords, color);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            moveAnimation.DrawColor = Color.White;

            GetNextPosition();
            CheckMapCollision();
            SetPosition(temp);

            if (velocity.X == 0) position.X = (int)position.X;

            /*

            if (input.KeyDown(Keys.Z) && CheckDirection(localState) != 0)
            {
                bullets.Add(new Bullet(
                    new Vector2(
                        position.X + moveAnimation.FrameWidth / 2,
                        position.Y + moveAnimation.FrameHeight / 2),
                    CheckDirection(localState),
                    400.0f,
                    bulletImage));
            }*/

            foreach (Bullet b in bullets) b.Update(gameTime);

            if (state.flinching)
            {
                flinchCount++;
                if (flinchCount >= 120)
                {
                    state.flinching = false;
                }
            }

            position += velocity;
            CheckAnimation(state);
            moveAnimation.Position = position;
            ssAnimation.Update(gameTime, ref moveAnimation);

            Console.WriteLine(velocity.X.ToString() + "\t" + velocity.Y.ToString() + "\t" + state.left.ToString() + " " + state.right.ToString() + " " + state.jumping.ToString() + " " + state.falling.ToString());
            Camera.Instance.SetFocalPoint(new Vector2(position.X, position.Y));
        }

        public void Stop()
        {
            state.left = state.right =
                state.jumping =
                state.flinching =
                state.dashing =
                state.squat =
                state.highAttack = state.lowAttack = state.normalAttack = false;
        }

        public override void OnCollision(Entity e)
        {
            if (state.flinching) return;

            Type type = e.GetType();
            if (type == typeof(Enemy))
            {
                Stop();
                health--;
                if (health < 0) health = 0;
                state.flinching = true;
                flinchCount = 0;
                moveAnimation.DrawColor = Color.Red;

                if (direction == 1) velocity.X = -1;
                else velocity.X = 1;
                velocity.Y = -3;
                state.knockback = true;
                state.falling = true;
                state.jumping = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets) b.Draw(spriteBatch);
            /*if (localState.flinching && !localState.knockback)
            {
                if (flinchCount % 10 < 5) return;
            }
            if (CheckDirection(localState) == 1) moveAnimation.Draw(spriteBatch, SpriteEffects.None);
            else if (CheckDirection(localState) == -1) moveAnimation.Draw(spriteBatch, SpriteEffects.FlipHorizontally);
            */
            moveAnimation.Draw(spriteBatch, SpriteEffects.None);
            DrawRectangle(new Rectangle((int)position.X, (int)position.Y, (int)collisionBox.X, (int)collisionBox.Y), Color.White, spriteBatch);
        }

        private void GetNextPosition()
        {
            if (state.knockback)
            {
                velocity.Y += speed.fall * 2;
                if (!state.falling) state.knockback = false;
                return;
            }

            if (state.left)
            {
                velocity.X -= speed.move;
                if (velocity.X < -speed.max) velocity.X = -speed.max;
            }
            else if (state.right)
            {
                velocity.X += speed.move;
                if (velocity.X > speed.max) velocity.X = speed.max;
            }
            else
            {
                if (velocity.X > 0)
                {
                    velocity.X -= speed.stop;
                    if (velocity.X < 0) velocity.X = 0;
                }
                else if (velocity.X < 0)
                {
                    velocity.X += speed.stop;
                    if (velocity.X > 0) velocity.X = 0;
                }
            }

            if ((state.normalAttack || state.highAttack ||
                state.lowAttack || state.dashing)
                && !(state.jumping || state.falling)) velocity.X = 0;

            if (state.jumping && !state.falling)
            {
                velocity.Y = speed.jump;
                state.falling = true;
            }

            if (state.dashing)
            {
                //dashCooldown = 0;
                //dashTimer++;
                if (direction == 1)
                {
                    velocity.X = speed.move * (float)(10 - /*dashTimer * */ 0.4);
                }
                else if (direction == -1)
                {
                    velocity.X = -speed.move * (float)(10 - /*dashTimer * */ 0.4);
                }
            }

            if (state.doubleJump && skill_doubleJump)
            {
                velocity.Y = speed.jump;
                alreadyDoubleJump = true;
                state.doubleJump = false;
            }

            if (!state.falling) alreadyDoubleJump = false;

            if (state.falling)
            {
                velocity.Y += speed.fall;
                if (velocity.Y < 0 && !state.jumping) velocity.Y += speed.stopJump;
                if (velocity.Y > speed.maxFall) velocity.Y = speed.maxFall;
            }

        }


        private void CheckAnimation(States states)
        {
            /*
             * teleport
             * knockback
             * health = 0
             * hiAttaack
             * attack
             * lowAttack
             * jumping
             * falling
             * dashing
             * left || right
             * squat
             * standing
             */
            if (states.teleporting)
            {
                // animacja teleportowania
            }
            else if (states.knockback)
            {
                // animacja knockback'u
            }
            else if (states.highAttack)
            {
                // atak w powietrzu
            }
            else if (states.normalAttack)
            {
                // atak na stojaka
            }
            else if (states.lowAttack)
            {
                // atak z przykuca
            }
            else if (states.jumping)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                ssAnimation.NumFrames = 1;
                ssAnimation.Delay = 1;
            }
            else if (states.falling)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);
                ssAnimation.NumFrames = 1;
                ssAnimation.Delay = 1;
            }
            else if (states.dashing)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 3);
                ssAnimation.NumFrames = 8;
                ssAnimation.Delay = 4;
            }
            else if (states.right || states.left)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 3);
                ssAnimation.NumFrames = 8;
                ssAnimation.Delay = 4;
            }
            else if (states.squat)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 7);
                ssAnimation.NumFrames = 1;
                ssAnimation.Delay = 1;
            }
            else if (states.standing)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                ssAnimation.NumFrames = 1;
                ssAnimation.Delay = 11;
            }
        }
    }
}
