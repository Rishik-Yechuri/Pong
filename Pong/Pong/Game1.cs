using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Texture2D sprite;
        Rectangle backgroundRct, ballRect, whiteRct;
        int ballSpeedX;
        int ballSpeedY;
        int ballSpeedXTemp;

        int leftPaddleY = 185;
        int rightPaddleY = 185;

        Rectangle bottom, left, right, top;

        Rectangle leftPaddle;
        Rectangle rightPaddle;

        int playerOneScore = 0;
        int playerTwoScore = 0;
        int playerOneGamesWon = 0;
        int playerTwoGamesWon = 0;

        int ballX = 400;
        int ballY = 230;

        int spin = 3;
        bool gameOver = false;
        SpriteFont font1;
        SpriteFont bigFont;
        KeyboardState oldKb = Keyboard.GetState();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            int screenWidth = graphics.GraphicsDevice.Viewport.Width;
            int screenHeight = graphics.GraphicsDevice.Viewport.Height;
            bottom = new Rectangle(0, screenHeight, screenWidth, 20);
            top = new Rectangle(0, 0, screenWidth, 0);
            left = new Rectangle(0, 0, 0, screenHeight);
            right = new Rectangle(screenWidth, 0, 20, screenHeight);
            backgroundRct = new Rectangle(0, 0, screenWidth, screenHeight);
            ballRect = new Rectangle(ballX, ballY, 20, 20);
            leftPaddle = new Rectangle(15, leftPaddleY, 30, 90);
            rightPaddle = new Rectangle(760,leftPaddleY,30,90);
            ballSpeedX = 2;
            ballSpeedXTemp = 2;
            ballSpeedY = 3;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            sprite = Content.Load<Texture2D>("Pong Sprite Sheet");
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            bigFont = Content.Load<SpriteFont>("SpriteFont2");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            if (ballRect.Intersects(bottom) || ballRect.Intersects(top))
                ballSpeedY *= -1;
                ballSpeedXTemp = ballSpeedX + spin;
                reduceSpin();
            if (ballRect.Intersects(leftPaddle) || ballRect.Intersects(rightPaddle))
                ballSpeedX *= -1;
                ballSpeedXTemp = ballSpeedX + spin;
                //reduceSpin();
            if (ballRect.Intersects(right)) {
                if (++playerOneScore >= 11 && playerOneScore-playerTwoScore >= 2) {
                    playerOneGamesWon++;
                    playerOneScore = 0;
                    playerTwoScore = 0;
                }
                resetBall();
            }
            if (ballRect.Intersects(left)) {
                if (++playerTwoScore >= 11 && playerTwoScore-playerOneScore >= 2)
                {
                    playerTwoGamesWon++;
                    playerOneScore = 0;
                    playerTwoScore = 0;
                }
                resetBall();
            }
            if (kb.IsKeyDown(Keys.W)) {
                leftPaddleY -= 3;
            }
            if (kb.IsKeyDown(Keys.S)) {
                leftPaddleY += 3;
            }
            if (kb.IsKeyDown(Keys.Up)) {
                rightPaddleY -= 3;
            }
            if (kb.IsKeyDown(Keys.Down)) {
                rightPaddleY += 3;
            }
            leftPaddle = new Rectangle(15, leftPaddleY, 30, 90);
            rightPaddle = new Rectangle(760, rightPaddleY, 30, 90);
            ballX += ballSpeedXTemp;
            ballY += ballSpeedY;
            ballRect = new Rectangle(ballX, ballY, 20, 20);
            oldKb = kb;
            base.Update(gameTime);
        }
        public void resetBall() {
            Random random = new Random();
            ballSpeedX = random.Next(2, 3);
            ballSpeedY = random.Next(2, 4);
            if (random.Next(0, 2) == 1) { ballSpeedX *= -1; }
            if (random.Next(0, 2) == 1) { ballSpeedY *= -1; }
            ballX = 400;
            ballY = 230;
            spin = random.Next(-3,4);
            ballSpeedXTemp = ballSpeedX + spin;
        }
        public void reduceSpin() {
            if (spin > 0) { spin--; }
            else if (spin < 0) { spin++; }
           // spin += Math.Sign(spin);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, backgroundRct, new Rectangle(0, 0, 800, 480), Color.White);
            for (int i = 0; i < GraphicsDevice.Viewport.Height; i++)
                spriteBatch.Draw(sprite, new Rectangle((GraphicsDevice.Viewport.Width / 2) - 8, i * 25, 16, 16), new Rectangle(867, 714, 16, 16), Color.White);
            spriteBatch.Draw(sprite, ballRect, new Rectangle(801, 0, 713, 713), Color.White);
            spriteBatch.Draw(sprite,leftPaddle,new Rectangle(801,700,32,150),Color.White);
            spriteBatch.Draw(sprite,rightPaddle,new Rectangle(833,700,32,150),Color.White);
            spriteBatch.DrawString(font1,playerOneScore.ToString(),new Vector2(360,440),Color.LightBlue);
            spriteBatch.DrawString(font1, playerTwoScore.ToString(), new Vector2(425, 440), Color.OrangeRed);
            spriteBatch.DrawString(font1,playerOneGamesWon.ToString(),new Vector2(360,20),Color.LightBlue);
            spriteBatch.DrawString(font1, playerTwoGamesWon.ToString(), new Vector2(425, 20), Color.OrangeRed);
            if (gameOver) {
                spriteBatch.DrawString(bigFont,"Game Over",new Vector2(400,300),Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
