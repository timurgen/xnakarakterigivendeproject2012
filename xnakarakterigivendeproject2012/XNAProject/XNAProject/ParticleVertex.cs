using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAProject
{
    struct ParticleVertex
    {
        public Vector3 position;
        public Vector2 textureCoordinate;

        public ParticleVertex(Vector3 position, Vector2 textureCoordinate)
        {
            this.position = position;
            this.textureCoordinate = textureCoordinate;
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3,
            VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2,
            VertexElementUsage.TextureCoordinate, 0)
        );
    }
}
