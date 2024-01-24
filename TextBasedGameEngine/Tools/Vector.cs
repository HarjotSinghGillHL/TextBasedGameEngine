using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Tools
{
    public class Vector3
    {
        public float x; public float y; public float z;
        public Vector3() { 
           x = 0; y = 0; z = 0;
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static Vector3 Zero()
        {
            return new Vector3(0, 0, 0);
        }


    }

    public class Vector2
    {
        public float x; public float y;

        public Vector2()
        {
            x = 0; y = 0; 
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 Zero()
        {
            return new Vector2(0, 0);
        }
    }
}
