using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNAProject
{
    public partial class MainClass : Microsoft.Xna.Framework.Game
    {
        protected void UpdateExplosions(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.B)) //Bang!!
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
            explosions.Add(new ParticleExplosion(GraphicsDevice,
                new Vector3(0.0f, 0.0f, 0.0f),
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
            new Vector2(explosionTexture.Width,
            explosionTexture.Height),
            particleSettings));

        }
    }
}
