using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectFinal
{
    /// <summary>
    /// Her ligger metoder som håndterer asteroidbelt
    /// </summary>
    public partial class MainClass : Microsoft.Xna.Framework.Game
    {

        List<SpaceObject> asteroids;

        int counter;
        int totalCounter = 0;
        public void addAsteroid(GameTime gt)
        {
            if (this.asteroids == null)
            {
                this.asteroids = new List<SpaceObject>();
            }
            if (totalCounter < 1500)
            {
                if (counter == 5)
                {
                    SpaceObject asteroid = new SpaceObject(this, (float)g.NextDouble() / 50.0f, g.Next(100) + 950, 0, (float)g.NextDouble() + 0.1f, 0, (float)g.NextDouble() / 50f, this.Sol, ref this.view, ref this.projection);
                    Model model = LoadModelWithBoundingSphere(@"models/planet", ref asteroid.matrixBoneTr, ref asteroid.matrixOriginBoneTr);
                    asteroid.load(effect, model, Content.Load<Texture2D>(@"textures-planets/moonmap"));
                    this.Components.Add(asteroid);
                    counter = 0;
                    totalCounter += 1;
                }
                else
                {
                    counter += 1;
                }
            }


        }
    }
}
