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
        FileManager fileManager;
        List<Bullet> bullets;
        Texture2D bulletImage;
        States localState;
        Speeds localSpeeds;

        public override void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input)
        {
            base.LoadContent(content, attributes, contents, input);
            string[] saveAttribute = { "PlayerPosition" };
            string[] saveContent = { position.X.ToString() + ',' + position.Y.ToString() };

            fileManager = new FileManager();
            fileManager.SaveContent("Load/Maps/Map1.mma", saveAttribute, saveContent, "");

            bullets = new List<Bullet>();
            bulletImage = content.Load<Texture2D>("bullet");

            localSpeeds.jump = 5.0f;
            localSpeeds.move = 5.0f;
            localSpeeds.maxFall = 10.0f;
            localSpeeds.stop = 2.0f;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            base.Update(gameTime, input, col, layer);

            moveAnimation.DrawColor = Color.White;
            GetNextPosition(velocity);

            if (input.KeyDown(Keys.Right, Keys.D))
            {
                localState.left = false;
                localState.right = true;
                
            }
            else if (input.KeyDown(Keys.Left, Keys.A))
            {
                localState.right = false;
                localState.left = true;
            }
            else localState.right = localState.left = false;

            if (input.KeyDown(Keys.Up, Keys.W)) localState.jumping = true;

            if (input.KeyDown(Keys.Down, Keys.S)) localState.squat = true;
            else localState.squat = false;


            if (input.KeyDown(Keys.Z) && CheckDirection(localState) != 0)
            {
                bullets.Add(new Bullet(
                    new Vector2(
                        position.X + moveAnimation.FrameWidth / 2, 
                        position.Y + moveAnimation.FrameHeight / 2), 
                    CheckDirection(localState), 
                    400.0f, 
                    bulletImage));
            }

            foreach (Bullet b in bullets) b.Update(gameTime);

            
            position += velocity;
            CheckAnimation(localState);
            moveAnimation.Position = position;
            ssAnimation.Update(gameTime, ref moveAnimation);

            Camera.Instance.SetFocalPoint(new Vector2(position.X, position.Y));
        }

        public override void OnCollision(Entity e)
        {
            Type type = e.GetType();
            if (type == typeof(Enemy))
            {
                health--;
                moveAnimation.DrawColor = Color.Red;

                fileManager = new FileManager();
                fileManager.LoadContent("Load/Maps/Map1.mma", "");
                for (int i = 0; i < fileManager.Attributes.Count; i++)
                {
                    for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                    {
                        switch (fileManager.Attributes[i][j])
                        {
                            case "PlayerPosition":
                                string[] split = fileManager.Contents[i][j].Split(',');
                                position = new Vector2(int.Parse(split[0]), int.Parse(split[1]));
                                break;
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets) b.Draw(spriteBatch);
            if (CheckDirection(localState) == 1) moveAnimation.Draw(spriteBatch, SpriteEffects.None);
            else if (CheckDirection(localState) == -1) moveAnimation.Draw(spriteBatch, SpriteEffects.FlipHorizontally);

            spriteBatch.DrawString(spriteFont, 
                velocity.X.ToString() + "\n" + 
                velocity.Y.ToString() + "\n" + 
                localState.left.ToString() +"\n" +
                localState.right.ToString() + "\n" +
                localState.standing.ToString() + "\n" +
                localState.jumping.ToString() , new Vector2(100, 100), Color.White);
        }

        private void GetNextPosition(Vector2 vel)
        {
            if (localState.knockback)
            {
                velocity.Y += localSpeeds.fall * 2;
                if (!localState.falling) localState.knockback = false;
                return;
            }

            if (localState.left)
            {
                velocity.X -= localSpeeds.move;
                if (velocity.X < -localSpeeds.max) velocity.X = -localSpeeds.max;
            }
            else if (localState.right)
            {
                velocity.X += localSpeeds.move;
                if (velocity.X > localSpeeds.max) velocity.X = localSpeeds.max;
            }
            else
            {
                if (velocity.X > 0)
                {
                    velocity.X -= localSpeeds.stop;
                    if (velocity.X < 0) velocity.X = 0;
                }
                else if (velocity.X < 0)
                {
                    velocity.X += localSpeeds.stop;
                    if (velocity.X > 0) velocity.X = 0;
                }
            }

            if ((localState.normalAttack || localState.highAttack || 
                localState.lowAttack || localState.dashing) 
                && !(localState.jumping || localState.falling)) velocity.X = 0;

            if (localState.jumping && !localState.falling)
            {
                velocity.Y = localSpeeds.jump;
                localState.falling = true;
            }

            if (localState.dashing)
            {
                //dashCooldown = 0;
                //dashTimer++;
                if (direction == 1)
                {
                    velocity.X = localSpeeds.move * (float)(10 - /*dashTimer * */ 0.4);
                }
                else if (direction == -1)
                {
                    velocity.X = -localSpeeds.move * (float)(10 - /*dashTimer * */ 0.4);
                }
            }

            if (localState.doubleJump /*&& skill_doubleJump*/)
            {
                velocity.Y = localSpeeds.jump;
                //alreadyDoubleJump = true;
                localState.doubleJump = false;
            }

            //if (!localState.falling) alreadyDoubleJump = false;

            if (localState.falling)
            {
                velocity.Y += localSpeeds.fall;
                if (velocity.Y < 0 && !localState.jumping) velocity.Y += localSpeeds.stopJump;
                if (velocity.Y > localSpeeds.maxFall) velocity.Y = localSpeeds.maxFall;
            }

           // this.velocity = velocity;
        }

        private int CheckDirection(States states)
        {
            int dir;
            if (states.right) dir = 1;
            else if (states.left) dir = -1;
            else dir = 0;

            return dir;
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
                ssAnimation.Delay = -1;
            }
            else if (states.falling)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);
                ssAnimation.NumFrames = 1;
                ssAnimation.Delay = -1;
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
                ssAnimation.Delay = -1;
            }
            else if (states.standing)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                ssAnimation.NumFrames = 1;
                ssAnimation.Delay = -1;
            }
        }
    }
}
