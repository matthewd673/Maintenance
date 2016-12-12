using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace One_Room
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //tiles
        Texture2D floor;
        Texture2D unknown;
        Texture2D barrel;
        Texture2D spill;

        //sprites
        Texture2D player;
        Texture2D enemy;

        //weapons/items
        Texture2D knife;
        Texture2D bomb;
        Texture2D activeBomb;
        Texture2D health;
        Texture2D energyPotion;

        //gamefeel
        Texture2D slimeTrail;
        Texture2D blood1;
        Texture2D blood2;
        Texture2D blood3;
        Texture2D light;
        Texture2D explosion;

        //ui stuff
        Texture2D cursor;
        Texture2D heart;
        Texture2D poisonHeart;
        Texture2D energy;
        Texture2D bombWarning;
        Texture2D intro;
        Texture2D death;
        Texture2D poisonDeath;
        Texture2D inventorySlot1;
        Texture2D inventorySlot2;
        Texture2D inventorySlot3;
        Texture2D inventorySlot4;
        Texture2D inventorySlot5;
        Texture2D inventoryHighlight;

        public static int tileSize = 32;

        public static int playerX = 4 * tileSize;
        public static int playerY = 4 * tileSize;
        public static int playerHealth = 6;
        public static int playerEnergy = 10;
        public static int energyGenCounter = 0;
        public static bool movingRight; //if true, right, if false, left
        int playerSpeed = 3;

        public static bool poisoned;
        public static int poisonedCounter;

        public static bool dead = false;

        public static int score = 0;

        //0 = home
        //1 = game
        public static int currentScreen = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            //set up board
            //Room.fillBoard();
            Inventory.backpack[0] = 1;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            floor = Content.Load<Texture2D>("floor");
            unknown = Content.Load<Texture2D>("unknown");
            barrel = Content.Load<Texture2D>("barrel");
            spill = Content.Load<Texture2D>("spill");

            player = Content.Load<Texture2D>("player");
            enemy = Content.Load<Texture2D>("enemy");

            knife = Content.Load<Texture2D>("knife");
            bomb = Content.Load<Texture2D>("bomb");
            activeBomb = Content.Load<Texture2D>("active-bomb");
            health = Content.Load<Texture2D>("health");
            energyPotion = Content.Load<Texture2D>("energy-boost");

            slimeTrail = Content.Load<Texture2D>("slime-trail");
            blood1 = Content.Load<Texture2D>("blood1");
            blood2 = Content.Load<Texture2D>("blood2");
            blood3 = Content.Load<Texture2D>("blood3");
            light = Content.Load<Texture2D>("light");
            explosion = Content.Load<Texture2D>("explosion");

            cursor = Content.Load<Texture2D>("cursor");
            heart = Content.Load<Texture2D>("heart");
            poisonHeart = Content.Load<Texture2D>("poison-heart");
            energy = Content.Load<Texture2D>("energy");
            bombWarning = Content.Load<Texture2D>("bomb-warning");
            intro = Content.Load<Texture2D>("intro");
            death = Content.Load<Texture2D>("death");
            poisonDeath = Content.Load<Texture2D>("poison-death");
            inventorySlot1 = Content.Load<Texture2D>("inventory-slot1");
            inventorySlot2 = Content.Load<Texture2D>("inventory-slot2");
            inventorySlot3 = Content.Load<Texture2D>("inventory-slot3");
            inventorySlot4 = Content.Load<Texture2D>("inventory-slot4");
            inventorySlot5 = Content.Load<Texture2D>("inventory-slot5");
            inventoryHighlight = Content.Load<Texture2D>("inventory-highlight");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (currentScreen == 1)
            {
                if (!dead)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.D1))
                        Inventory.equippedItem = 0;
                    if (Keyboard.GetState().IsKeyDown(Keys.D2))
                        Inventory.equippedItem = 1;
                    if (Keyboard.GetState().IsKeyDown(Keys.D3))
                        Inventory.equippedItem = 2;
                    if (Keyboard.GetState().IsKeyDown(Keys.D4))
                        Inventory.equippedItem = 3;
                    if (Keyboard.GetState().IsKeyDown(Keys.D5))
                        Inventory.equippedItem = 4;

                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                        for (int k = 0; k < playerSpeed; k++)
                        {
                            bool moveUp = true;
                            for (int i = 0; i < Room.boardSize; i++)
                            {
                                for (int j = 0; j < Room.boardSize; j++)
                                {
                                    //probably magic
                                    if (Room.board[i, j] == 1)
                                        if (detectCollision(new Rectangle(playerX, playerY - 1, tileSize - 2, tileSize - 2),
                                            new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize)))
                                            moveUp = false;
                                }
                            }

                            if (moveUp)
                                playerY--;
                        }

                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                        for (int k = 0; k < playerSpeed; k++)
                        {
                            bool moveLeft = true;
                            for (int i = 0; i < Room.boardSize; i++)
                            {
                                for (int j = 0; j < Room.boardSize; j++)
                                {
                                    //probably magic
                                    if (Room.board[i, j] == 1)
                                        if (detectCollision(new Rectangle(playerX - 1, playerY, tileSize - 2, tileSize - 2),
                                            new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize)))
                                            moveLeft = false;
                                }
                            }

                            if (moveLeft)
                            {
                                playerX--;
                                movingRight = false;
                            }
                        }
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                        for (int k = 0; k < playerSpeed; k++)
                        {
                            bool moveDown = true;
                            for (int i = 0; i < Room.boardSize; i++)
                            {
                                for (int j = 0; j < Room.boardSize; j++)
                                {
                                    //probably magic
                                    if (Room.board[i, j] == 1)
                                        if (detectCollision(new Rectangle(playerX, playerY + 1, tileSize - 2, tileSize - 2),
                                            new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize)))
                                            moveDown = false;
                                }
                            }

                            if (moveDown)
                                playerY++;
                        }
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                        for (int k = 0; k < playerSpeed; k++)
                        {
                            bool moveRight = true;
                            for (int i = 0; i < Room.boardSize; i++)
                            {
                                for (int j = 0; j < Room.boardSize; j++)
                                {
                                    //probably magic
                                    if (Room.board[i, j] == 1)
                                        if (detectCollision(new Rectangle(playerX + 1, playerY, tileSize - 2, tileSize - 2),
                                            new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize)))
                                            moveRight = false;
                                }
                            }

                            if (moveRight)
                            {
                                playerX++;
                                movingRight = true;
                            }
                        }

                    Room.hoverTileX = (int)Mouse.GetState().X / tileSize;
                    Room.hoverTileY = (int)Mouse.GetState().Y / tileSize;

                    if (Room.hoverTileX < 0)
                        Room.hoverTileX = 0;
                    if (Room.hoverTileX > Room.boardSize - 1)
                        Room.hoverTileX = Room.boardSize - 1;
                    if (Room.hoverTileY < 0)
                        Room.hoverTileY = 0;
                    if (Room.hoverTileY > Room.boardSize - 1)
                        Room.hoverTileY = Room.boardSize - 1;

                    Room.playerTileX = (int)playerX / tileSize;
                    Room.playerTileY = (int)playerY / tileSize;

                    if (Room.playerTileX < 0)
                        Room.playerTileX = 0;
                    if (Room.playerTileX > Room.boardSize - 1)
                        Room.playerTileX = Room.boardSize - 1;
                    if (Room.playerTileY < 0)
                        Room.playerTileY = 0;
                    if (Room.playerTileY > Room.boardSize - 1)
                        Room.playerTileY = Room.boardSize - 1;

                    if (playerX < 0)
                        playerX = 0;
                    if (playerY < 0)
                        playerY = 0;
                    if (playerX > (Room.boardSize * tileSize) - tileSize)
                        playerX = (Room.boardSize * tileSize) - tileSize;
                    if (playerY > (Room.boardSize * tileSize) - tileSize)
                        playerY = (Room.boardSize * tileSize) - tileSize;

                    energyGenCounter++;

                    if (energyGenCounter >= 25)
                    {
                        playerEnergy++;
                        energyGenCounter = 0;
                        if (playerEnergy > 20)
                            playerEnergy = 20;
                    }

                    //player attack stuff
                    playerAttack();


                    for (int i = 0; i < Room.enemyList.Count; i++)
                    {
                        Enemy tempEnemy = (Enemy)Room.enemyList[i];
                        tempEnemy.followPlayer();
                        tempEnemy.combat();
                        //save our changes
                        Room.enemyList[i] = tempEnemy;
                        if (tempEnemy.health <= 0)
                            Room.enemyList.Remove(Room.enemyList[i]);
                    }

                    for (int k = 0; k < Room.bombList.Count; k++)
                    {
                        Permanence.tickingBomb tempBomb = (Permanence.tickingBomb)Room.bombList[k];
                        tempBomb = Permanence.bombManager(tempBomb);
                        Room.bombList[k] = tempBomb;
                        if (tempBomb.size >= 32)
                        {
                            //its gonna blow!
                            for (int i = -2; i < 2; i++)
                            {
                                for (int j = -2; j < 2; j++)
                                {
                                    for (int l = 0; l < Room.enemyList.Count; l++)
                                    {
                                        Enemy tempEnemy = (Enemy)Room.enemyList[l];
                                        if (detectCollision(new Rectangle(tempBomb.x + (i * tileSize), tempBomb.y + (j * tileSize), tileSize, tileSize),
                                            new Rectangle(tempEnemy.x, tempEnemy.y, tileSize, tileSize)))
                                        {
                                            //bomb hit an enemy
                                            Room.enemyList.Remove(Room.enemyList[l]);
                                            Room.bloodList.Add(new Permanence.blood(tempEnemy.x, tempEnemy.y));
                                        }
                                    }
                                    for (int l = 0; l < Room.boardSize; l++)
                                    {
                                        for (int m = 0; m < Room.boardSize; m++)
                                        {
                                            if (detectCollision(new Rectangle(tempBomb.x + (i * tileSize), tempBomb.y + (j * tileSize), tileSize, tileSize),
                                                new Rectangle(l * tileSize, m * tileSize, tileSize, tileSize)))
                                            {
                                                if (Room.board[l, m] != 2)
                                                    Room.board[l, m] = 0;
                                            }
                                        }
                                    }

                                    //player
                                    if (detectCollision(new Rectangle(tempBomb.x + (i * tileSize), tempBomb.y + (j * tileSize), tileSize, tileSize),
                                            new Rectangle(playerX, playerY, tileSize, tileSize)))
                                    {
                                        playerHealth -= 2;
                                    }

                                }
                            }
                            Room.bombList.Remove(Room.bombList[k]);
                            Room.explosionList.Add(new Permanence.explosionParticle(tempBomb.x, tempBomb.y));
                        }
                    }

                    for (int i = 0; i < Room.explosionList.Count; i++)
                    {
                        Permanence.explosionParticle tempExplosion = (Permanence.explosionParticle)Room.explosionList[i];
                        tempExplosion = Permanence.explosionManager(tempExplosion);
                        Room.explosionList[i] = tempExplosion;

                        if (tempExplosion.size > 64)
                        {
                            Room.explosionList.Remove(Room.explosionList[i]);
                        }
                    }

                    Room.calculatePlayerSight();
                    Room.regenerateBoard();

                    Room.interactWithTile();

                    if (poisoned)
                    {
                        poisonedCounter++;
                        if (poisonedCounter >= 100)
                        {
                            playerHealth--;
                            poisonedCounter = 0;
                        }
                    }
                    else
                        poisonedCounter = 0;

                    for (int i = 0; i < Room.trailList.Count; i++)
                    {
                        Permanence.slime tempSlime = (Permanence.slime)Room.trailList[i];
                        tempSlime = Permanence.slimeManager(tempSlime);
                        Room.trailList[i] = tempSlime;
                        if (tempSlime.size <= 0)
                            Room.trailList.Remove(Room.trailList[i]);
                    }
                }
                else
                {
                    if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        //reset everything
                        reset();
                        currentScreen = 0;
                        playerX = 4 * tileSize;
                        playerY = 4 * tileSize;
                    }
                }

                if (playerHealth <= 0)
                    dead = true;
                else
                    dead = false;
            }
            else
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    dead = false;
                    currentScreen = 1;
                }
            }

            base.Update(gameTime);

        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            graphics.PreferredBackBufferWidth = Room.boardSize * tileSize;
            graphics.PreferredBackBufferHeight = Room.boardSize * tileSize;
            graphics.ApplyChanges();

            spriteBatch.Begin();

            for(int i = 0; i < Room.boardSize; i++)
            {
                for(int j = 0; j < Room.boardSize; j++)
                {
                    spriteBatch.Draw(floor, new Vector2(i * 32, j * 32), Color.White);
                }
            }

            if (currentScreen == 1)
            {
                //permanence stuff
                for (int i = 0; i < Room.trailList.Count; i++)
                {
                    Permanence.slime tempSlime = (Permanence.slime)Room.trailList[i];
                    spriteBatch.Draw(slimeTrail, new Rectangle(tempSlime.x, tempSlime.y, tempSlime.size, tempSlime.size), Color.White * 0.5f);
                }

                for (int i = 0; i < Room.bloodList.Count; i++)
                {
                    Permanence.blood tempBlood = (Permanence.blood)Room.bloodList[i];
                    switch (tempBlood.type)
                    {
                        case 0:
                            spriteBatch.Draw(blood1, new Rectangle(tempBlood.x, tempBlood.y, tempBlood.size, tempBlood.size), Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(blood2, new Rectangle(tempBlood.x, tempBlood.y, tempBlood.size, tempBlood.size), Color.White);
                            break;
                        case 2:
                            spriteBatch.Draw(blood3, new Rectangle(tempBlood.x, tempBlood.y, tempBlood.size, tempBlood.size), Color.White);
                            break;
                    }
                }

                for (int i = 0; i < Room.boardSize; i++)
                {
                    for (int j = 0; j < Room.boardSize; j++)
                    {
                        switch (Room.board[i, j])
                        {
                            case 1:
                                spriteBatch.Draw(barrel, new Vector2(i * tileSize, j * tileSize), Color.White);
                                break;
                            case 2:
                                spriteBatch.Draw(spill, new Vector2(i * tileSize, j * tileSize), Color.White);
                                break;
                            case 3:
                                spriteBatch.Draw(health, new Vector2(i * tileSize, j * tileSize), Color.White);
                                break;
                            case 4:
                                spriteBatch.Draw(energyPotion, new Vector2(i * tileSize, j * tileSize), Color.White);
                                break;
                            case 5:
                                spriteBatch.Draw(bomb, new Vector2(i * tileSize, j * tileSize), Color.White);
                                break;
                        }
                    }
                }

                for (int i = 0; i < Room.enemyList.Count; i++)
                {
                    Enemy tempEnemy = (Enemy)Room.enemyList[i];
                    spriteBatch.Draw(enemy, new Vector2(tempEnemy.x, tempEnemy.y));
                }

                for (int k = 0; k < Room.bombList.Count; k++)
                {
                    Permanence.tickingBomb tempBomb = (Permanence.tickingBomb)Room.bombList[k];
                    spriteBatch.Draw(activeBomb, new Rectangle(tempBomb.x, tempBomb.y, tempBomb.size, tempBomb.size), Color.White);
                    for (int i = -2; i < 2; i++)
                    {
                        for (int j = -2; j < 2; j++)
                        {
                            spriteBatch.Draw(bombWarning, new Vector2(tempBomb.initialX + (i * tileSize), tempBomb.initialY + (j * tileSize)), Color.White * 0.5f);
                        }
                    }
                }

                for (int i = 0; i < Room.explosionList.Count; i++)
                {
                    Permanence.explosionParticle tempExplosion = (Permanence.explosionParticle)Room.explosionList[i];
                    spriteBatch.Draw(explosion, new Rectangle(tempExplosion.x, tempExplosion.y, tempExplosion.size, tempExplosion.size), Color.White);
                }

                Color playerTint = Color.White;
                if (poisoned)
                    playerTint = Color.YellowGreen;
                else
                    playerTint = Color.White;

                spriteBatch.Draw(player, new Vector2(playerX, playerY), playerTint);
                //draw object player holds
                int adjustment = 0;
                if (movingRight)
                    adjustment = 16;
                switch (Inventory.backpack[Inventory.equippedItem])
                {
                    //case 0: nothing
                    case 1:
                        spriteBatch.Draw(knife, new Vector2(playerX + adjustment, playerY), Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(health, new Vector2(playerX + adjustment, playerY), Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(energyPotion, new Vector2(playerX + adjustment, playerY), Color.White);
                        break;
                    case 4:
                        spriteBatch.Draw(bomb, new Vector2(playerX + adjustment, playerY), Color.White);
                        break;
                }

                //draw unknown over monsters
                for (int i = 0; i < Room.boardSize; i++)
                {
                    for (int j = 0; j < Room.boardSize; j++)
                    {
                        switch (Room.board[i, j])
                        {
                            case -1:
                                spriteBatch.Draw(unknown, new Vector2(i * tileSize, j * tileSize), Color.White);
                                break;
                        }
                    }
                }

                spriteBatch.Draw(light, new Vector2(Room.playerSightX * tileSize, Room.playerSightY * tileSize), Color.White);

                //ui here
                for (int i = 0; i < playerHealth; i++)
                {
                    if (!poisoned)
                        spriteBatch.Draw(heart, new Vector2(i * 16, 0), Color.White);
                    else
                        spriteBatch.Draw(poisonHeart, new Vector2(i * 16, 0), Color.White);
                }

                for (int i = 0; i < (int)playerEnergy / 2; i++)
                {
                    spriteBatch.Draw(energy, new Vector2(i * 16, 20), Color.White);
                }

                for (int i = 0; i < 5; i++)
                {
                    if (i == 0)
                        spriteBatch.Draw(inventorySlot1, new Vector2((i * 40) + 8, (Room.boardSize * tileSize) - 40));
                    if (i == 1)
                        spriteBatch.Draw(inventorySlot2, new Vector2((i * 40) + 8, (Room.boardSize * tileSize) - 40));
                    if (i == 2)
                        spriteBatch.Draw(inventorySlot3, new Vector2((i * 40) + 8, (Room.boardSize * tileSize) - 40));
                    if (i == 3)
                        spriteBatch.Draw(inventorySlot4, new Vector2((i * 40) + 8, (Room.boardSize * tileSize) - 40));
                    if (i == 4)
                        spriteBatch.Draw(inventorySlot5, new Vector2((i * 40) + 8, (Room.boardSize * tileSize) - 40));
                }

                spriteBatch.Draw(inventoryHighlight, new Vector2((Inventory.equippedItem * 40) + 8, (Room.boardSize * tileSize) - 40));

                for (int i = 0; i < Inventory.backpack.Length; i++)
                {
                    switch (Inventory.backpack[i])
                    {
                        //if zero, nothing
                        case 1:
                            //knife
                            spriteBatch.Draw(knife, new Vector2((i * 40) + 16, (Room.boardSize * tileSize) - 32));
                            break;
                        case 2:
                            //health
                            spriteBatch.Draw(health, new Vector2((i * 40) + 16, (Room.boardSize * tileSize) - 32));
                            break;
                        case 3:
                            //energy
                            spriteBatch.Draw(energyPotion, new Vector2((i * 40) + 16, (Room.boardSize * tileSize) - 32));
                            break;
                        case 4:
                            //bomb
                            spriteBatch.Draw(bomb, new Vector2((i * 40) + 16, (Room.boardSize * tileSize) - 32));
                            break;
                    }
                }

                if (dead)
                {
                    if (!poisoned)
                        spriteBatch.Draw(death, new Vector2(192, 192));
                    else
                        spriteBatch.Draw(poisonDeath, new Vector2(192, 192));
                }
            }
            else
            {
                spriteBatch.Draw(intro, new Vector2(128, 128));
            }

            Color cursorTint = Color.White;

            if (Room.board[Room.hoverTileX, Room.hoverTileY] == 0)
            {
                cursorTint = Color.White;
            }
            if (Room.board[Room.hoverTileX, Room.hoverTileY] == 1)
            {
                cursorTint = Color.Red;
            }

            spriteBatch.Draw(cursor, new Vector2(Mouse.GetState().X - 8, Mouse.GetState().Y - 8), cursorTint);

            //NO MORE DRAWING
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static bool detectCollision(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.X < (rect2.X + rect2.Width) &&
                (rect1.X + rect1.Width) > rect2.X &&
                rect1.Y < (rect2.Y + rect2.Height) &&
                (rect1.Height + rect1.Y) > rect2.Y)
                return true;
            else
                return false;
        }

        public static void playerAttack()
        {
            int attackRange = 38;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && playerEnergy > 0)
            {
                switch (Inventory.backpack[Inventory.equippedItem])
                {
                    case 1:
                        //knife
                        if (playerEnergy > 0)
                        {
                            for (int i = 0; i < Room.boardSize; i++)
                            {
                                for (int j = 0; j < Room.boardSize; j++)
                                {
                                    if (detectCollision(new Rectangle(Mouse.GetState().Position, new Point(1, 1)),
                                    new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize)) && 
                                    Math.Abs(playerX - (i * tileSize)) <= attackRange &&
                                    Math.Abs(playerY - (j * tileSize)) <= attackRange &&
                                    Room.board[i, j] == 1)
                                    {
                                        playerEnergy--;
                                        Room.board[i, j] = 0;
                                        //spawn item?
                                        int itemDrop = Room.rng.Next(8);

                                        switch (itemDrop)
                                        {
                                            //sloppy, i know
                                            case 0:
                                                //health
                                                Room.board[i, j] = 3;
                                                break;
                                            case 1:
                                                //health
                                                Room.board[i, j] = 3;
                                                break;
                                            case 2:
                                                //energy
                                                Room.board[i, j] = 4;
                                                break;
                                            case 3:
                                                //energy
                                                Room.board[i, j] = 4;
                                                break;
                                            case 4:
                                                //bomb
                                                Room.board[i, j] = 5;
                                                break;

                                        }
                                    }
                                }
                            }
                            for (int i = 0; i < Room.enemyList.Count; i++)
                            {
                                Enemy tempEnemy = (Enemy)Room.enemyList[i];
                                if (detectCollision(new Rectangle(Mouse.GetState().Position, new Point(1, 1)),
                                    new Rectangle(tempEnemy.x, tempEnemy.y, tileSize, tileSize)) && 
                                    Math.Abs(playerX - tempEnemy.x) <= attackRange &&
                                    Math.Abs(playerY - tempEnemy.y) <= attackRange)
                                {
                                    playerEnergy--;
                                    tempEnemy.health--;
                                    Room.enemyList[i] = tempEnemy;
                                    Room.bloodList.Add(new Permanence.blood(tempEnemy.x, tempEnemy.y));
                                }
                            }

                        }
                        break;

                    case 2:
                        //health
                        if(playerHealth < 7)
                        {
                            playerHealth++;
                            poisoned = false;
                            poisonedCounter = 0;
                            if (playerHealth > 7)
                                playerHealth = 7;
                            Inventory.backpack[Inventory.equippedItem] = 0;
                        }
                        Inventory.equippedItem = 0;
                        break;

                    case 3:
                        //energy
                        if(playerEnergy < 20)
                        {
                            playerEnergy *= 2;
                            if (playerEnergy > 20)
                                playerEnergy = 20;
                            Inventory.backpack[Inventory.equippedItem] = 0;
                        }
                        Inventory.equippedItem = 0;
                        break;

                    case 4:
                        //bomb
                        for (int i = 0; i < Room.boardSize; i++)
                        {
                            for (int j = 0; j < Room.boardSize; j++)
                            {
                                if (detectCollision(new Rectangle(Mouse.GetState().Position, new Point(1, 1)),
                                new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize)) &&
                                Math.Abs(playerX - (i * tileSize)) <= attackRange * 2 &&
                                Math.Abs(playerY - (j * tileSize)) <= attackRange * 2 &&
                                Room.board[i, j] != 1)
                                {
                                    Room.bombList.Add(new Permanence.tickingBomb(i * tileSize, j * tileSize));
                                    Inventory.backpack[Inventory.equippedItem] = 0;
                                    Inventory.equippedItem = 0;
                                }
                            }
                        }
                        break;

                }
                
            }

        }

        public void reset()
        {
            playerX = 4 * tileSize;
            playerY = 4 * tileSize;
            playerHealth = 6;
            playerEnergy = 10;
            energyGenCounter = 0;
            poisoned = false;
            poisonedCounter = 0;
            Room.bloodList.Clear();
            Room.bombList.Clear();
            Room.enemyList.Clear();
            Room.explosionList.Clear();
            Room.trailList.Clear();
            for(int i = 0; i < Room.boardSize; i++)
            {
                for(int j = 0; j < Room.boardSize; j++)
                {
                    Room.board[i, j] = 0;
                }
            }
        }
    }
}
