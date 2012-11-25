using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAProject
{
    class AsteroidBelt : DrawableGameComponent
    {
        private float frontBorder;
        private float backBorder;
        private int quantity;
        private Vector3 pivote;
        //private SpaceObject[] asteroidBelt;
        private Random generator;

        public AsteroidBelt(float _frontBorder, float _backBorder, int _quantity, Effect _effect, Game _game) : base(_game)
        {
            //this.asteroidBelt = new SpaceObject[_quantity];
            generator = new Random();
            for (int i = 0; i < _quantity; i++)
            {
                SpaceObject asteroid = new SpaceObject(_game, (float)generator.NextDouble(), (float)generator.Next((int)frontBorder, (int)backBorder), 1f, (float)generator.NextDouble() + 0.01f, 1f, (float)generator.NextDouble(), null);
                asteroid.load(_effect, _game.Content.Load<Model>("models/planet"), _game.Content.Load<Texture2D>("textures-planets/moonmap"));
                _game.Components.Add(asteroid);

            }
        }

        
    }
}
