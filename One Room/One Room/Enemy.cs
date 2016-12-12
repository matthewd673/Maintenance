using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace One_Room
{
    public class Enemy
    {

        public int x;
        public int y;
        public int health;
        public bool poisoned;
        public int poisonCounter;
        public int attackCounter;

        public Enemy(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.health = 7;
        }

        public void followPlayer()
        {
            //i have no idea how this works
            //and i made it
            bool moveRight = false;
            bool moveLeft = false;
            bool moveUp = false;
            bool moveDown = false;
            for (int k = 0; k < Room.enemyList.Count; k++)
            {
                Enemy tempEnemy = (Enemy)Room.enemyList[k];
                for (int l = 0; l < 1; l++)
                {
                    moveUp = true;
                    for (int i = 0; i < Room.boardSize; i++)
                    {
                        for (int j = 0; j < Room.boardSize; j++)
                        {
                            //probably magic
                            if (Room.board[i, j] == 1)
                                if (Game1.detectCollision(new Rectangle(x, y - 1, Game1.tileSize, Game1.tileSize),
                                    new Rectangle(i * Game1.tileSize, j * Game1.tileSize, Game1.tileSize, Game1.tileSize)) ||
                                    Game1.detectCollision(new Rectangle(x, y - 1, Game1.tileSize, Game1.tileSize),
                                    new Rectangle(tempEnemy.x, tempEnemy.y, Game1.tileSize, Game1.tileSize)))
                                    moveUp = false;
                        }
                    }

                    if (moveUp)
                        y--;
                }
                for (int l = 0; l < 1; l++)
                {
                    moveLeft = true;
                    for (int i = 0; i < Room.boardSize; i++)
                    {
                        for (int j = 0; j < Room.boardSize; j++)
                        {
                            //probably magic
                            if (Room.board[i, j] == 1)
                                if (Game1.detectCollision(new Rectangle(x - 1, y, Game1.tileSize, Game1.tileSize),
                                    new Rectangle(i * Game1.tileSize, j * Game1.tileSize, Game1.tileSize, Game1.tileSize)) ||
                                    Game1.detectCollision(new Rectangle(x - 1, y, Game1.tileSize, Game1.tileSize),
                                    new Rectangle(tempEnemy.x, tempEnemy.y, Game1.tileSize, Game1.tileSize)))
                                    moveLeft = false;
                        }
                    }

                    if (moveLeft)
                        x--;
                }
                for (int l = 0; l < 1; l++)
                {
                    moveDown = true;
                    for (int i = 0; i < Room.boardSize; i++)
                    {
                        for (int j = 0; j < Room.boardSize; j++)
                        {
                            //probably magic
                            if (Room.board[i, j] == 1)
                                if (Game1.detectCollision(new Rectangle(x, y + 1, Game1.tileSize, Game1.tileSize),
                                    new Rectangle(i * Game1.tileSize, j * Game1.tileSize, Game1.tileSize, Game1.tileSize)) ||
                                    Game1.detectCollision(new Rectangle(x, y + 1, Game1.tileSize, Game1.tileSize),
                                    new Rectangle(tempEnemy.x, tempEnemy.y, Game1.tileSize, Game1.tileSize)))
                                    moveDown = false;
                        }
                    }

                    if (moveDown)
                        y++;
                }
                for (int l = 0; l < 1; l++)
                {
                    moveRight = true;
                    for (int i = 0; i < Room.boardSize; i++)
                    {
                        for (int j = 0; j < Room.boardSize; j++)
                        {
                            //probably magic
                            if (Room.board[i, j] == 1)
                                if (Game1.detectCollision(new Rectangle(x + 1, y, Game1.tileSize, Game1.tileSize),
                                    new Rectangle(i * Game1.tileSize, j * Game1.tileSize, Game1.tileSize, Game1.tileSize)) ||
                                    Game1.detectCollision(new Rectangle(x + 1, y, Game1.tileSize, Game1.tileSize),
                                    new Rectangle(tempEnemy.x, tempEnemy.y, Game1.tileSize, Game1.tileSize)))
                                    moveRight = false;
                        }
                    }

                    if (moveRight)
                        x++;
                }
            }

            int detectionRange = 120;
            
            if (Game1.playerX > x && Game1.playerX - x < detectionRange && moveRight)
            {
                x++;
                Room.trailList.Add(new Permanence.slime(x, y));
            }
            if (Game1.playerX < x && x - Game1.playerX < detectionRange && moveLeft)
            {
                x--;
                Room.trailList.Add(new Permanence.slime(x, y));
            }
            if (Game1.playerY < y && y - Game1.playerY < detectionRange && moveUp)
            {
                y--;
                Room.trailList.Add(new Permanence.slime(x, y));
            }
            if (Game1.playerY > y && Game1.playerY - y < detectionRange && moveDown)
            {
                y++;
                Room.trailList.Add(new Permanence.slime(x, y));
            }
        }

        public void combat()
        {
            if (poisoned)
                poisonCounter++;
            else
                poisonCounter = 0;

            if (poisonCounter >= 100)
                health -= 1;

            attackCounter++;

            if(attackCounter >= 70)
            {
                attackCounter = 0;
                if (Game1.detectCollision(new Rectangle(x, y, Game1.tileSize, Game1.tileSize),
                    new Rectangle(Game1.playerX, Game1.playerY, Game1.tileSize, Game1.tileSize)))
                {
                    Game1.playerHealth--;
                    Room.bloodList.Add(new Permanence.blood(Game1.playerX, Game1.playerY));
                }
            }
        }
    }
}
