using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAProject
{
    class AsteroidBelt 
    {
        private int width = MainClass.WIDTH;
        private int height = MainClass.HEIGHT;
        Texture2D asteroidTexture;

        List<Asteroid> asteroidList;

        public AsteroidBelt(MainClass _game)
        {
            this.asteroidList = new List<Asteroid>();
            this.asteroidTexture = _game.Content.Load<Texture2D>(@"/textures-planets/ASTEROIDS");
        }

    }

    struct Asteroid
    {
        public Vector3 position;
        public Quaternion rotation;
    }
}
