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
    struct ParticleVertex
    {
        public Vector3 position;
        public Vector2 textureCoordinate;
        public Vector3 normal;

        public ParticleVertex(Vector3 position, Vector2 textureCoordinate, Vector3 _normal)
        {
            this.position = position;
            this.textureCoordinate = textureCoordinate;
            this.normal = _normal;
        }

        /// <summary>
        /// Egendefinert vertex
        /// </summary>
        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3,VertexElementUsage.Position, 0), 
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float) * 5, VertexElementFormat.Vector3, VertexElementUsage.Normal,0)
        );
    }
}
