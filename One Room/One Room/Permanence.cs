using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_Room
{
    public class Permanence
    {

        public struct slime
        {
            public int x;
            public int y;
            public int size;
            public int age;

            public slime(int x, int y)
            {
                this.x = x;
                this.y = y;
                this.size = 32;
                this.age = 0;
            }
        }

        public static slime slimeManager(slime s)
        {
            s.age++;
            if (s.age >= 140)
            {
                s.size--;
                s.x++;
                s.y++;
            }
            return s;
        }

        public struct blood
        {
            public int x;
            public int y;
            public int size;
            public int age;
            public int type;

            public blood(int x, int y)
            {
                this.x = x;
                this.y = y;
                this.size = 16;
                this.age = 0;
                this.type = new Random().Next(3);
            }
        }

        public struct tickingBomb
        {
            public int x;
            public int y;
            public int initialX;
            public int initialY;
            public int size;
            public int age;

            public tickingBomb(int x, int y)
            {
                this.x = x;
                this.y = y;
                this.initialX = x;
                this.initialY = y;
                this.size = 8;
                this.age = 0;
            }
        }

        public static tickingBomb bombManager(tickingBomb b)
        {
            b.age++;
            if(b.age >= 50)
            {
                b.size += 2;
                b.x--;
                b.y--;
            }
            return b;
        }

        public struct explosionParticle
        {
            public int x;
            public int y;
            public int size;

            public explosionParticle(int x, int y)
            {
                this.x = x;
                this.y = y;
                this.size = 0;
            }
        }

        public static explosionParticle explosionManager(explosionParticle e)
        {
            e.size += 2;
            e.x--;
            e.y--;
            return e;
        }

    }
}
