using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjectFinal
{
    /// <summary>
    /// Her ligger metoder som beregner partikkeleffekt
    /// </summary>
    public partial class MainClass
    {
        List<ParticleExplosion> explosions = new List<ParticleExplosion>();
        ParticleExplosionSettings particleExplosionSettings = new ParticleExplosionSettings();
        ParticleSettings particleSettings = new ParticleSettings();
        public Texture2D StraalTexture;

        void loadSolaParticles() 
        {
            StraalTexture = Content.Load<Texture2D>(@"textures-planets/sunmap");
        }

        protected void UpdateExplosions(GameTime gameTime)
        {
            
            if (gameTime.TotalGameTime.Milliseconds % 250 == 0) //Bang!!
                this.addExplotion();
            // Loop through and update explosions
            for (int i = 0; i < explosions.Count; ++i)
            {
                explosions[i].Update(gameTime);
                // If explosion is finished, remove it
                if (explosions[i].IsDead)
                {
                    explosions.RemoveAt(i);
                    --i;
                }
            }
        }

        private void addExplotion()
        {
            BoundingSphere s2 = (BoundingSphere)this.Sol.model.Tag;
            s2 = s2.Transform(Matrix.CreateScale(4f));
            float R = s2.Radius;
            float Theta = (float)((g.NextDouble() * Math.PI) - Math.PI / 2);
            float phi = (float)((g.NextDouble() * 2*Math.PI));
            float x = (float)(R * Math.Sin(Theta) * Math.Cos(phi));
            float y = (float)(R * Math.Sin(Theta) * Math.Sin(phi));
            float z = (float)(R * Math.Cos(Theta));
            explosions.Add(new ParticleExplosion(GraphicsDevice,
                new Vector3(x, y, z),
                (
                g.Next(particleExplosionSettings.minLife,
                particleExplosionSettings.maxLife)),
                (
                g.Next(
            particleExplosionSettings.minRoundTime,
            particleExplosionSettings.maxRoundTime)),
            (g.Next(
            particleExplosionSettings.minParticlesPerRound,
            particleExplosionSettings.maxParticlesPerRound)),
            (g.Next(
            particleExplosionSettings.minParticles,
            particleExplosionSettings.maxParticles)),
            new Vector2(StraalTexture.Width,
            StraalTexture.Height),
            particleSettings));

        }
    }
}
