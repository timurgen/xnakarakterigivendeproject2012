using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticlesTraining
{
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
        int endOfLiveParticlesIndex = 0;
        // Markerer starten på aktive partikler (og slutten på ”døde”)
        int endOfDeadParticlesIndex = 0;

        // Bruker et enkelt random-objekt:
        static Random rnd = new Random();

        private ParticleVertex[] vertexes;




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

        private void InitializeParticles()
        {
            // Loop until max particles
            for (int i = 0; i < maxParticles; ++i)
            {
                Vector2 txtCoord = new Vector2(rnd.Next(0, (int)textureSize.X) /
                textureSize.X, rnd.Next(0, (int)textureSize.Y) / textureSize.Y);
                Vector3 direction = new Vector3((float)rnd.NextDouble() * 2 - 1, (float)rnd.NextDouble() * 2 - 2, (float)rnd.NextDouble() * 2 - 1);
                direction.Normalize();
                // Multiply by NextDouble to make sure that
                // all particles move at random speeds
                direction *= (float)rnd.NextDouble();
                //Particlesize:
                float size = ((float)rnd.NextDouble() * particleSettings.maxSize) / 10;
                particles[i] = new Particle(position, txtCoord, direction, size);
            }

            //Kopierer til vertekstabell:
            vertexes = new ParticleVertex[particles.Length * 3];
            for (int i = 0; i < particles.Length; i++)
            {
                vertexes[i * 3] = particles[i].pvs[0];
                vertexes[i * 3 + 1] = particles[i].pvs[1];
                vertexes[i * 3 + 2] = particles[i].pvs[2];
            }

        }


    }


}
