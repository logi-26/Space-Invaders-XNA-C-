using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{
    // This is the SpriteManager class. This class is used for loading, drawing and updating all of the sprites in the game.
    // Created by Louis Gilmartin
    // Last edited: 25/2/2014

    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        //Game states
        enum GameState { GameStart, GamePlay, GameOver, GameWon };
        GameState currentGameState = GameState.GameStart;

        // Button down variables
        private Boolean triggerDown = false;
        private Boolean enterDown = false;

        // Timer variables
        private float rumbleTime;

        // Score variable
        private Boolean finalScoreCalculated = false;

        // Random number
        private Random rnd = new Random();

        // Font sprites
        private SpriteFont gameFont;

        // Background sprites
        private BackgroundSprite startBackground;
        private BackgroundSprite endBackground;
        private BackgroundSprite wonBackground;

        // Barrier sprites
        private Barrier[] barrierArray = new Barrier[3];

        // Game timer component
        GameTimer gameTimer;

        // Player sprite
        private PlayerSprite playerSprite;

        // Player character
        private Player player = new Player();

        // Player projectile sprite
        private PlayerProjectile playerProjectile;

        // 2D array of enemy sprites
        private Sprite[,] enemySpriteArray = new Sprite[3, 8];

        // 2D array of enemy projectile sprites
        private EnemyProjectile[,] enemyProjectileArray = new EnemyProjectile[3, 8];

        public SpriteManager(Game game)
            : base(game)
        {
        }


        public override void Initialize()
        {
            // Initialises the game timer component
            gameTimer = new GameTimer(Game);
            gameTimer.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // Loads the fonts
            gameFont = Game.Content.Load<SpriteFont>(@"Font/gameFont");

            // Loads and initialises the barriers
            barrierArray[0] = new Barrier(Game, 100.0f, 610.0f);
            barrierArray[0].Initialize();
            barrierArray[1] = new Barrier(Game, 450.0f, 610.0f);
            barrierArray[1].Initialize();
            barrierArray[2] = new Barrier(Game, 800.0f, 610.0f);
            barrierArray[2].Initialize();

            // Loads the background sprites
            startBackground = new BackgroundSprite(
                Game.Content.Load<Texture2D>(@"Backgrounds/Start_Screen"),
                Vector2.Zero, new Point(1024, 768), new Point(0, 0),
                new Point(2, 1), new Vector2(1, 0), 400, 1, true);

            endBackground = new BackgroundSprite(
                Game.Content.Load<Texture2D>(@"Backgrounds/GameEnd_Screen"),
                Vector2.Zero, new Point(1024, 768), new Point(0, 0),
                new Point(2, 1), new Vector2(1, 0), 400, 1, true);

            wonBackground = new BackgroundSprite(
                Game.Content.Load<Texture2D>(@"Backgrounds/GameWon_Screen"),
                Vector2.Zero, new Point(1024, 768), new Point(0, 0),
                new Point(2, 1), new Vector2(1, 0), 400, 1, true);

            // Loads the player sprite
            playerSprite = new PlayerSprite(
                Game.Content.Load<Texture2D>(@"Player/Spaceship_Centre"),
                new Vector2(490, 680), new Point(39, 43), new Point(0, 0),
                new Point(2, 1), new Vector2(4, 0), 30, 2, true);

            // Loads the player projectile sprite
            playerProjectile = new PlayerProjectile(
                Game.Content.Load<Texture2D>(@"Player/Spaceship_Projectile"),
                new Vector2(0, 0), new Point(4, 14), new Point(0, 0),
                new Point(2, 1), new Vector2(4, 0), 30, 2, false);

            // Loads the enemy sprites into the 2D array
            Vector2 enemyLocation;
            Point frameSize;
            Texture2D spriteImage;

            for (int i = 0; i <= 2; i++)
            {
                // Loads the row 1 enemy sprites
                if (i == 0)
                {
                    enemyLocation = new Vector2(50, 100);
                    frameSize = new Point(48, 48);
                    spriteImage = Game.Content.Load<Texture2D>(@"Enemies/Alien_3");
                }
                // Loads the row 2 enemy sprites
                else if (i == 1)
                {
                    enemyLocation = new Vector2(40, 170);
                    frameSize = new Point(66, 48);
                    spriteImage = Game.Content.Load<Texture2D>(@"Enemies/Alien_1");
                }
                else
                // Loads the row 3 enemy sprites
                {
                    enemyLocation = new Vector2(34, 240);
                    frameSize = new Point(72, 48);
                    spriteImage = Game.Content.Load<Texture2D>(@"Enemies/Alien_2");
                }
                for (int j = 0; j <= 7; j++)
                {
                    // Generates a new random number 
                    int randomX = rnd.Next(1, 1024);

                    enemySpriteArray[i, j] = new EnemySprite(spriteImage, enemyLocation, frameSize, new Point(0, 0),
                    new Point(2, 1), new Vector2(2, 0), 400, 1, true, true, new Vector2(randomX, enemyLocation.Y));

                    enemyLocation += new Vector2(100, 0);
                }
            }

            // Loads the enemy projectiles into a 2D array
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    enemyProjectileArray[i, j] = new EnemyProjectile(
                        Game.Content.Load<Texture2D>(@"Player/Spaceship_Projectile"),
                        new Vector2(-10, 0), new Point(4, 14), new Point(0, 0),
                        new Point(2, 1), new Vector2(4, 0), 30, 2, false);
                }
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Update depending on current gamestate
            switch (currentGameState)
            {
                case GameState.GameStart:
                    // Updates the start screen background sprite
                    startBackground.Update(gameTime, Game.Window.ClientBounds);

                    // If the enter key or start buttons are clicked, the game will begin
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && enterDown != true ||
                        GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && enterDown != true)
                    {
                        currentGameState = GameState.GamePlay;
                        NewGame();
                        gameTimer.counter = 1;
                        gameTimer.countDuration = 1f;
                        gameTimer.currentTime = 0f;
                        finalScoreCalculated = false;
                    } 
                    break;
                case GameState.GamePlay:
                    // Updates the player sprite
                    playerSprite.Update(gameTime, Game.Window.ClientBounds);

                    // Updates the player projectile
                    if (playerProjectile.spriteActive != false) playerProjectile.Update(gameTime, Game.Window.ClientBounds);

                    // If the space bar or right trigger button are pressed, a projectile is created
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && playerProjectile.spriteActive != true && triggerDown != true ||
                        GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.5 && playerProjectile.spriteActive != true && triggerDown != true)
                    {
                        playerProjectile = new PlayerProjectile(
                        Game.Content.Load<Texture2D>(@"Player/Spaceship_Projectile"),
                        playerSprite.spritePosition + new Vector2(35,-30), new Point(4, 14), new Point(0, 0),
                        new Point(2, 1), new Vector2(4, 0), 30, 2, true);

                        playerProjectile.spriteActive = true;
                    }

                    // This records the previous state of the right trigger button and space bar key
                    if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.5 || Keyboard.GetState().IsKeyDown(Keys.Space)) triggerDown = true;
                    else triggerDown = false;
                       
                    // If the projectile reaches the upper screen bounds it is made inactive
                    if (playerProjectile.spritePosition.Y < 50) playerProjectile.spriteActive = false;

                    // Updates all of the enemy sprites in the array
                    for (int i = 0; i <= 2; i++)
                    {
                        for (int j = 0; j <= 7; j++)
                        {
                            enemySpriteArray[i, j].Update(gameTime, Game.Window.ClientBounds);

                            // If any of the enemy sprites reach the right bounds of the screen
                            if (enemySpriteArray[i, j].spriteActive != false && enemySpriteArray[i, j].spritePosition.X == 1024 - 66)
                            {
                                // loops through the entire enemy array
                                for (int a = 0; a <= 2; a++)
                                {
                                    for (int b = 0; b <= 7; b++)
                                    {
                                        // All of the sprites in the array have their position Y incremented                                  
                                        enemySpriteArray[a, b].spritePosition += new Vector2(0, 50);

                                        // The moving foward variable is set false
                                        enemySpriteArray[a, b].movingFoward = false;

                                        // Generates a new random number 
                                        int randomX = rnd.Next(1, 1024);

                                        // Sets the random number as the enemy sprites firePosition X 
                                        // This is used to give each enemy sprite a new random fire position 
                                        enemySpriteArray[a, b].firePosition = new Vector2(randomX, enemySpriteArray[a, b].spritePosition.Y);
                                    }
                                }
                            }
                            // If any of the enemy sprites reach the left bounds of the screen
                            else if (enemySpriteArray[i, j].spriteActive != false && enemySpriteArray[i, j].spritePosition.X < 0)
                            {
                                // loops through the entire enemy array
                                for (int a = 0; a <= 2; a++)
                                {
                                    for (int b = 0; b <= 7; b++)
                                    {
                                        // All of the sprites in the array have their position Y incremented
                                        enemySpriteArray[a, b].spritePosition += new Vector2(0, 50);

                                        // The moving foward variable is set true
                                        enemySpriteArray[a, b].movingFoward = true;

                                        // Generates a new random number 
                                        int randomX = rnd.Next(1, 1024);

                                        // Sets the random number as the enemy sprites firePosition X 
                                        // This is used to give each enemy sprite a new random fire position 
                                        enemySpriteArray[a, b].firePosition = new Vector2(randomX, enemySpriteArray[a, b].spritePosition.Y);
                                    }
                                }
                            }
                        }
                    }

                    // Loops through the enemy sprite array
                    for (int i = 0; i <= 2; i++)
                    {
                        for (int j = 0; j <= 7; j++)
                        {
                            // If the enemy sprites position is equal to the enemy sprites random fire posistion
                            if (enemySpriteArray[i, j].spritePosition == enemySpriteArray[i, j].firePosition && enemySpriteArray[i, j].spriteActive != false)
                            {
                                // A new enemy projectile is created
                                enemyProjectileArray[i, j] = new EnemyProjectile(
                                Game.Content.Load<Texture2D>(@"Enemies/Alien_Projectile"),
                                enemySpriteArray[i, j].spritePosition, new Point(4, 14), new Point(0, 0),
                                new Point(2, 1), new Vector2(1, 0), 30, 2, true);

                                enemyProjectileArray[i, j].spriteActive = true;
                            }

                            // If the projectile is active, it is updated
                            if (enemyProjectileArray[i, j].spriteActive != false)
                                enemyProjectileArray[i, j].Update(gameTime, Game.Window.ClientBounds);
                          
                            // Once the projectile goes out of the lower screen bounds, the sprite is made in-active
                            if (enemyProjectileArray[i, j].spritePosition.Y > 768)
                                enemyProjectileArray[i, j].spriteActive = false;
                        }
                    }

                    // Checks for collisions between the enemy sprites, the player sprite, the barriers or the projectile sprites
                    for (int i = 0; i <= 2; i++)
                    {
                        for (int j = 0; j <= 7; j++)
                        {
                            // If the enemy sprites collide with the player or barrier sprites, the game is over
                            if (enemySpriteArray[i, j].collisionRect.Intersects(playerSprite.collisionRect) && enemySpriteArray[i, j].spriteActive != false ||
                                enemySpriteArray[i, j].collisionRect.Intersects(barrierArray[0].collisionRect) &&
                                enemySpriteArray[i, j].spriteActive != false && barrierArray[0].spriteActive != false ||
                                enemySpriteArray[i, j].collisionRect.Intersects(barrierArray[1].collisionRect) &&
                                enemySpriteArray[i, j].spriteActive != false && barrierArray[1].spriteActive != false ||
                                enemySpriteArray[i, j].collisionRect.Intersects(barrierArray[2].collisionRect) &&
                                enemySpriteArray[i, j].spriteActive != false && barrierArray[2].spriteActive != false)
                                currentGameState = GameState.GameOver;

                            // If the players projectile hits an enemy sprite, the enemy sprite is removed and the players score is incremented
                            if (enemySpriteArray[i, j].collisionRect.Intersects(playerProjectile.collisionRect) && playerProjectile.spriteActive != false)
                            {
                                if (enemySpriteArray[i, j].spriteActive != false)
                                {
                                    enemySpriteArray[i, j].spriteActive = false;
                                    playerProjectile.spriteActive = false;
                                    player.playerScore += 10;
                                }
                            }

                            // If the enemies projectile hits the player sprite, the players lives are decremented
                            if (enemyProjectileArray[i, j].collisionRect.Intersects(playerSprite.collisionRect) && enemyProjectileArray[i, j].spriteActive != false)
                            {
                                player.playerLives -= 1;
                                enemyProjectileArray[i, j].spriteActive = false;

                                GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                                rumbleTime = gameTimer.currentTime + 0.3f;
                            }
                        }
                    }

                    // Makes the Xbox 360 controller rumble for the specified period of time
                    if (rumbleTime < gameTimer.currentTime) GamePad.SetVibration(PlayerIndex.One, 0f, 0f);

                    // Prevents the player from shooting through the barrier sprites
                    if (playerProjectile.collisionRect.Intersects(barrierArray[0].collisionRect) && barrierArray[0].spriteActive != false ||
                        playerProjectile.collisionRect.Intersects(barrierArray[1].collisionRect) && barrierArray[1].spriteActive != false ||
                        playerProjectile.collisionRect.Intersects(barrierArray[2].collisionRect) && barrierArray[2].spriteActive != false) playerProjectile.spriteActive = false;

                    // Detects if an enemy projectile has collided with a barrier
                    for (int i = 0; i <= 2; i++)
                    {
                        for (int j = 0; j <= 7; j++)
                        {
                            if (enemyProjectileArray[i, j].collisionRect.Intersects(barrierArray[0].collisionRect) &&
                                barrierArray[0].spriteActive != false && enemyProjectileArray[i, j].spriteActive != false)
                            {
                                barrierArray[0].barrierHits += 1;
                                enemyProjectileArray[i, j].spriteActive = false;
                            }
                            else if (enemyProjectileArray[i, j].collisionRect.Intersects(barrierArray[1].collisionRect) &&
                                barrierArray[1].spriteActive != false && enemyProjectileArray[i, j].spriteActive != false)
                            {
                                barrierArray[1].barrierHits += 1;
                                enemyProjectileArray[i, j].spriteActive = false;
                            }
                            else if (enemyProjectileArray[i, j].collisionRect.Intersects(barrierArray[2].collisionRect) &&
                                barrierArray[2].spriteActive != false && enemyProjectileArray[i, j].spriteActive != false)
                            {
                                barrierArray[2].barrierHits += 1;
                                enemyProjectileArray[i, j].spriteActive = false;
                            }
                        }
                    }

                    // Updates the barrier sprites
                    barrierArray[0].Update(gameTime);
                    barrierArray[1].Update(gameTime);
                    barrierArray[2].Update(gameTime);
                     
                    // When the player has no lives left, the game is over
                    if (player.playerLives == 0) currentGameState = GameState.GameOver;

                    // Variable to hold the number of in-active enemies
                    int wonCheck = 0;

                    // This loops through the enemy array counting the number of in-active sprites
                    for (int i = 0; i <= 2; i++)
                    {
                        for (int j = 0; j <= 7; j++)
                        {
                            if (enemySpriteArray[i, j].spriteActive != true) wonCheck += 1;
                        }
                    }

                    // If all 24 enemy sprites are in-active, the gamestate is set to game won
                    if (wonCheck == 24) currentGameState = GameState.GameWon;

                    break;
                case GameState.GameOver:
                    // Updates the end screen background sprite
                    endBackground.Update(gameTime, Game.Window.ClientBounds);

                    GamePad.SetVibration(PlayerIndex.One, 0f, 0f);

                    // If the enter key or start button are pressed, the gamestate is set to game start
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed) currentGameState = GameState.GameStart;

                    break;
                case GameState.GameWon:
                    // Updates the won screen background sprite
                    wonBackground.Update(gameTime, Game.Window.ClientBounds);

                    GamePad.SetVibration(PlayerIndex.One, 0f, 0f);

                    // Calculates the final score
                    if (finalScoreCalculated != true) CalculateFinalScore();

                    // If the enter key or start button are pressed, the gamestate is set to game start
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed) currentGameState = GameState.GameStart;

                    break;
            }

            // Records the previous state of the enter key and start button
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter)) enterDown = true;
            else enterDown = false;

            // Updates the game timer
            gameTimer.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            // Draw depending on current gamestate
            switch (currentGameState)
            {
                case GameState.GameStart:

                    // Draws the start screen background sprite
                    startBackground.Draw(gameTime, spriteBatch);
                    break;
                case GameState.GamePlay:

                    // Draws the barrier sprites
                    barrierArray[0].Draw(gameTime);
                    barrierArray[1].Draw(gameTime);
                    barrierArray[2].Draw(gameTime);

                    // Draws the player sprite
                    playerSprite.Draw(gameTime, spriteBatch);

                    // Draws the player projectile
                    if (playerProjectile.spriteActive != false) playerProjectile.Draw(gameTime, spriteBatch);

                    // Draws the enemy sprites and the enemy projectiles
                    for (int i = 0; i <= 2; i++)
                    {
                        for (int j = 0; j <= 7; j++)
                        {
                            if (enemySpriteArray[i, j].spriteActive != false) enemySpriteArray[i, j].Draw(gameTime, spriteBatch);
                            if (enemyProjectileArray[i, j].spriteActive != false) enemyProjectileArray[i, j].Draw(gameTime, spriteBatch); 
                        }
                    }

                    // Draws the timer font
                    gameTimer.Draw(gameTime);

                    // Draws the game font
                    spriteBatch.DrawString(gameFont, "Score:", new Vector2(30, 5), Color.White);

                    // Draws the player score font
                    spriteBatch.DrawString(gameFont, player.playerScore.ToString(), new Vector2(125, 5), Color.White);

                    // Draws the player lives font
                    spriteBatch.DrawString(gameFont, "Lives: ", new Vector2(880, 10), Color.White);

                    // Draws the player lives number
                    spriteBatch.DrawString(gameFont, player.playerLives.ToString(), new Vector2(965, 10), Color.White);

                    break;
                case GameState.GameOver:
                    // Draws the end screen background sprite
                    endBackground.Draw(gameTime, spriteBatch);

                    break;
                case GameState.GameWon:
                    // Draws the end screen background sprite
                    wonBackground.Draw(gameTime, spriteBatch);

                    // Draws the game font
                    spriteBatch.DrawString(gameFont, "Your final score is:", new Vector2(350, 500), Color.White);

                    // Draws the player score font
                    spriteBatch.DrawString(gameFont, player.playerFinalScore.ToString(), new Vector2(585, 500), Color.White);

                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void NewGame()
        {
            // Resets the values so a new game can begin
            player.playerLives = 3;
            gameTimer.currentTime = 0f;
            gameTimer.counter = 0;
            player.playerScore = 0;
            player.playerFinalScore = 0;
            barrierArray[0].barrierHits = 0;
            barrierArray[1].barrierHits = 0;
            barrierArray[2].barrierHits = 0;
            barrierArray[0].Initialize();
            LoadContent();
        }

        public void CalculateFinalScore()
        {
            // This calculates the players final score
            // The final score is determined by the players lives, the time taken to win and the condition of the barrier sprites
            player.playerFinalScore = player.playerScore;

            // Adds points to the final score depending on the amount of lives the player had remaining
            player.playerFinalScore += player.playerLives * 100;

            // Deducts the time taken to complete the game from the players final score
            player.playerFinalScore -= gameTimer.counter;

            // Deducts points from the final score depending on the condition of the 3 barrier sprites
            player.playerFinalScore -= barrierArray[0].barrierHits * 10;
            player.playerFinalScore -= barrierArray[1].barrierHits * 10;
            player.playerFinalScore -= barrierArray[2].barrierHits * 10;

            finalScoreCalculated = true;
        }
    }
}
