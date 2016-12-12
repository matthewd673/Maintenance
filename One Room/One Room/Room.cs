using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace One_Room
{
    public class Room
    {

        public static int boardSize = 16;

        public static int[,] board = new int[boardSize, boardSize]; //temp dimensions
        public static bool[,] playerSight = new bool[boardSize, boardSize];

        public static Random rng = new Random();

        public static int hoverTileX;
        public static int hoverTileY;

        public static int playerTileX;
        public static int playerTileY;
        public static int playerSightX;
        public static int playerSightY;

        public static int playerSightRange = 9;

        public static ArrayList enemyList = new ArrayList();

        public static ArrayList trailList = new ArrayList();
        public static ArrayList bloodList = new ArrayList();
        public static ArrayList bombList = new ArrayList();
        public static ArrayList explosionList = new ArrayList();

        public static void fillBoard()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    int tileId = rng.Next(52);

                    switch(tileId)
                    {
                        case 1:
                            board[i, j] = 1;
                            break;
                        case 2:
                            board[i, j] = 2;
                            break;
                        case 3:
                            if(enemyList.Count < 7)
                                enemyList.Add(new Enemy(i * Game1.tileSize, j * Game1.tileSize));
                            break;
                    }
                }
            }
        }

        public static void calculatePlayerSight()
        {

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    playerSight[i, j] = false;
                }
            }

            playerSightX = playerTileX - ((int)playerSightRange / 2);
            playerSightY = playerTileY - ((int)playerSightRange / 2);

            if (playerSightX < 0)
                playerSightX = 0;
            if (playerSightX > boardSize - playerSightRange)
                playerSightX = boardSize - playerSightRange;
            if (playerSightY < 0)
                playerSightY = 0;
            if (playerSightY > boardSize - playerSightRange)
                playerSightY = boardSize - playerSightRange;

            for (int i = 0; i < playerSightRange; i++)
            {
                for (int j = 0; j < playerSightRange; j++)
                {
                    playerSight[i + playerSightX, j + playerSightY] = true;
                }
            }

        }

        public static void regenerateBoard()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (!playerSight[i, j])
                    {
                        board[i, j] = -1;
                    }
                    else
                    {
                        if (board[i, j] == -1)
                        {
                            bool spawnObject = true;
                            for(int k = 0; k < Room.enemyList.Count; k++)
                            {
                                Enemy tempEnemy = (Enemy)enemyList[k];
                                if (Game1.detectCollision(new Rectangle(i * Game1.tileSize, j * Game1.tileSize, Game1.tileSize, Game1.tileSize),
                                    new Rectangle(tempEnemy.x, tempEnemy.y, Game1.tileSize, Game1.tileSize)))
                                {
                                    spawnObject = false;
                                }
                            }

                            if (spawnObject)
                            {
                                int tileId = rng.Next(52);
                                if (tileId == 6)
                                {
                                    if (enemyList.Count < 7)
                                        enemyList.Add(new Enemy(i * Game1.tileSize, j * Game1.tileSize));
                                }
                                if (tileId == 1 || tileId == 2 || tileId == 3)
                                    board[i, j] = 1;
                                else if (tileId == 4 || tileId == 5)
                                    board[i, j] = 2;
                                else
                                    board[i, j] = 0;
                            }
                            else
                                board[i, j] = 0;
                        }
                    }
                }
            }
        }

        public static void interactWithTile()
        {
            for(int i = 0; i < boardSize; i++)
            {
                for(int j = 0; j < boardSize; j++)
                {
                    if (Game1.detectCollision(new Rectangle(Game1.playerX, Game1.playerY, Game1.tileSize, Game1.tileSize),
                        new Rectangle(i * Game1.tileSize, j * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
                        {
                        switch (board[i, j])
                        {
                            case 2:
                                //spill
                                Game1.poisoned = true;
                                board[i, j] = 0;
                                break;
                            case 3:
                                //health
                                Game1.playerHealth++;
                                if (Game1.playerHealth > 7)
                                {
                                    Game1.playerHealth = 7;
                                    //place at the first empty inventory space
                                    for (int k = 0; k < 5; k++)
                                    {
                                        if (Inventory.backpack[k] == 0)
                                        {
                                            Inventory.backpack[k] = 2;
                                            break;
                                        }
                                    }
                                }
                                Game1.poisoned = false;
                                board[i, j] = 0;
                                break;
                            case 4:
                                //energy
                                if (Game1.playerEnergy == 20)
                                {
                                    for (int k = 0; k < 5; k++)
                                    {
                                        if (Inventory.backpack[k] == 0)
                                        {
                                            Inventory.backpack[k] = 3;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    Game1.playerEnergy *= 2;
                                    if (Game1.playerEnergy > 20)
                                    {
                                        Game1.playerEnergy = 20;
                                    }
                                }
                                board[i, j] = 0;
                                break;
                            case 5:
                                //bomb pickup
                                for(int k = 0; k < 5; k++)
                                {
                                    if(Inventory.backpack[k] == 0)
                                    {
                                        Inventory.backpack[k] = 4;
                                        break;
                                    }
                                }
                                board[i, j] = 0;
                                break;
                        }
                    }
                }
            }
        }
    }
}