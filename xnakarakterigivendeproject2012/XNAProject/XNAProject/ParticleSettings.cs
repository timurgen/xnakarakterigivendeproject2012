using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAProject
{
    class ParticleSettings
    {
        // Size of particle
        public int maxSize = 2;
    }

    class ParticleExplosionSettings
    {
        // Life of particles
        public int minLife = 1000;
        public int maxLife = 5000;
        // Particles per round
        public int minParticlesPerRound = 100;
        public int maxParticlesPerRound = 300;
        // Round time
        public int minRoundTime = 16;
        public int maxRoundTime = 50;
        // Number of particles
        public int minParticles = 1000;
        public int maxParticles = 1500;
    }
}
