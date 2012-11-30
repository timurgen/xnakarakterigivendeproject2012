using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectFinal
{
    class ParticleSettings
    {
        // Size of particle
        public int maxSize = 20;
    }

    class ParticleExplosionSettings
    {
        // Life of particles
        public int minLife = 5000;
        public int maxLife = 6000;
        // Particles per round
        public int minParticlesPerRound = 20;
        public int maxParticlesPerRound = 50;
        // Round time
        public int minRoundTime = 50;
        public int maxRoundTime = 100;
        // Number of particles
        public int minParticles = 1000;
        public int maxParticles = 1200;
    }
}
