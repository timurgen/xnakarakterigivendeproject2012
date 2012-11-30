using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectFinal
{
    /// <summary>
    /// Fra Datamaskingrafikk forelesningsnotater
    /// </summary>
    class ParticleExplosion
    {
        // Array med Particle-objekter, opprettes i konstr.
        Particle[] particles;
        // Kollisjonens posisjon.
        Vector3 position;

        // Hvor mye “liv” som gjenstår før antall partikler begynner å avta
        int lifeLeft;
        // Hvor mange nye partikler som skal slippes for hver “runde”
        int numParticlesPerRound;
        // Maks antall partikler - totalt.
        int maxParticles;

        // Angir tiden mellom hver runde.
        int roundTime;
        // En teller som angir hvor mye tid som er gått siden starten
        // på pågående runde.
        int timeSinceLastRound = 0;

        // Vertex & graphics-info.
        VertexDeclaration vertexDeclaration;
        GraphicsDevice graphicsDevice;
        // Texture
        Vector2 textureSize;
        // Settinger for hver enkelt partikkel – kun størrelse.
        ParticleSettings particleSettings;

        // Følgende to variabler er indekser inn i partikkeltabellen:
        // Markerer slutten på aktive partikler (se fig. Under)
        int endOfLiveParticlesIndex = 100;
        // Markerer starten på aktive partikler (og slutten på ”døde”)
        int endOfDeadParticlesIndex = 1;

        // Bruker et enkelt random-objekt:
        static Random rnd = new Random();


        // Konstruktør:
        public ParticleExplosion(GraphicsDevice graphicsDevice,
        Vector3 position, int lifeLeft, int roundTime,
        int numParticlesPerRound, int maxParticles,
        Vector2 textureSize, ParticleSettings particleSettings)
        {
            this.position = position;
            this.lifeLeft = lifeLeft;
            this.numParticlesPerRound = numParticlesPerRound;
            this.maxParticles = maxParticles;
            this.roundTime = roundTime;
            this.graphicsDevice = graphicsDevice;
            this.textureSize = textureSize;
            this.particleSettings = particleSettings;
            particles = new Particle[maxParticles];
            InitializeParticles();
        }

        public bool IsDead
        {
            get { return endOfDeadParticlesIndex == maxParticles; }
        }

        private void InitializeParticles()
        {
            // Loop until max particles
            for (int i = 0; i < maxParticles; ++i)
            {
                Vector2 txtCoord = new Vector2(rnd.Next(0, (int)textureSize.X) /
                textureSize.X, rnd.Next(0, (int)textureSize.Y) / textureSize.Y);
                Vector3 direction = new Vector3((float)rnd.NextDouble() * 2 - 1, (float)rnd.NextDouble() * 2 - 1, (float)rnd.NextDouble() * 2 - 1);
                direction.Normalize();
                // Multiply by NextDouble to make sure that
                // all particles move at random speeds
                direction *= (float)rnd.NextDouble();
                //Particlesize:
                float size = ((float)rnd.NextDouble() * particleSettings.maxSize) / 10;
                particles[i] = new Particle(position, txtCoord, direction, size);
            }

            //Kopierer til vertekstabell:
            vertexes = new ParticleVertex[particles.Length * 4];
            for (int i = 0; i < particles.Length; i++)
            {
                vertexes[i * 3] = particles[i].pvs[0];
                vertexes[i * 3 + 1] = particles[i].pvs[1];
                vertexes[i * 3 + 2] = particles[i].pvs[2];
            }
        }



        public ParticleVertex[] vertexes { get; set; }

        public void Update(GameTime gameTime)
        {
            // Decrement life left until it's gone
            if (lifeLeft > 0)
            {
                lifeLeft -= gameTime.ElapsedGameTime.Milliseconds;
            }

            // Time for new round?
            timeSinceLastRound += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastRound > roundTime)
            {
                // New round ‐ add and remove particles
                timeSinceLastRound -= roundTime;
                // Increment end of live particles index each
                // round until end of list is reached
                if (endOfLiveParticlesIndex < maxParticles)
                {
                    endOfLiveParticlesIndex += numParticlesPerRound;
                    if (endOfLiveParticlesIndex > maxParticles)
                    {
                        endOfLiveParticlesIndex = maxParticles;
                    }

                }
                if (lifeLeft <= 0)
                {
                    // Increment end of dead particles index each
                    // round until end of list is reached
                    if (endOfDeadParticlesIndex < maxParticles)
                    {
                        endOfDeadParticlesIndex += numParticlesPerRound;
                        if (endOfDeadParticlesIndex > maxParticles)
                        {
                            endOfDeadParticlesIndex = maxParticles;
                        }


                    }


                }
            }
            // Update positions of all live particles
            for (int i = endOfDeadParticlesIndex; i < endOfLiveParticlesIndex; ++i)
            {
                //particles[i].position += particles[i].direction;
                particles[i].UpdatePosition();
                // Assign a random texture coordinate for color
                // to create a flashing effect for each particle
                Vector2 newCoords = new Vector2(
                rnd.Next(0, (int)textureSize.X) / textureSize.X,
                rnd.Next(0, (int)textureSize.Y) / textureSize.Y);
                particles[i].UpdateTextureCoordinate(newCoords);
            }
            //Oppdater verteksene:
            for (int j = 0; j < particles.Length; j++)
            {
                vertexes[j * 3] = particles[j].pvs[0];
                vertexes[j * 3 + 1] = particles[j].pvs[1];
                vertexes[j * 3 + 2] = particles[j].pvs[2];
            }
        }

        public void Draw(Effect effect, ref Matrix _view, ref Matrix _projection, Texture2D _texture)
        {
            // Only draw if there are live particles
            if (endOfLiveParticlesIndex - endOfDeadParticlesIndex > 0)
            {
                // Set HLSL parameters
                effect.Parameters["xEmissiveColor"].SetValue(new Vector4(1f, 1f, 1f, 0f));
                effect.Parameters["isEmissive"].SetValue(true);
                effect.Parameters["xWorld"].SetValue(Matrix.Identity);
                effect.Parameters["xView"].SetValue(_view);
                effect.Parameters["xProjection"].SetValue(_projection);
                effect.Parameters["xTexture"].SetValue(_texture);
                // Draw particles
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                    vertexes, endOfDeadParticlesIndex * 3,
                    endOfLiveParticlesIndex - endOfDeadParticlesIndex,
                    ParticleVertex.VertexDeclaration);
                }
            }
        }
    }
}
