namespace Toast.Gamebase.Internal
{
    using UnityEngine;
    
    public class GamebaseColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public GamebaseColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
        
        public GamebaseColor(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 1f;
        }
        
        public GamebaseColor(Color color)
        {
            this.r = color.r;
            this.g = color.g;
            this.b = color.b;
            this.a = color.a;
        }
        
        public static implicit operator Vector4(GamebaseColor c) => new Vector4(c.r, c.g, c.b, c.a);
        public static implicit operator GamebaseColor(Vector4 v) => new GamebaseColor(v.x, v.y, v.z, v.w);
        public static implicit operator Color(GamebaseColor c) => new Color(c.r, c.g, c.b, c.a);
        public static implicit operator GamebaseColor(Color c) => new GamebaseColor(c.r, c.g, c.b, c.a);
        
        public static GamebaseColor RGB255(int r, int g, int b)
        {
            return new GamebaseColor(r / 255.0f, g / 255.0f, b / 255.0f, 1);
        }
        
        public static GamebaseColor RGB255(int r, int g, int b, int a)
        {
            return new GamebaseColor(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }
    }
}