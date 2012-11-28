using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAProject
{
    class AsteroidBelt : SpaceObject
    {
        /// <summary>
        /// max antall objekter i asteroidbeltet
        /// </summary>
        public const int MAX_ASTEROIDS = 200;
        Model[] objects;
        List<Vector3> asteroidsPos;
        Texture2D texture;

        /// <summary>
        /// Konstruktør
        /// </summary>
        /// <param name="_game"></param>
        /// <param name="_size">Størrelse på asteroider</param>
        /// <param name="_orbitRadius">baneradius av asteroidbelt</param>
        /// <param name="_akseAngle">Yakse vinkelen</param>
        /// <param name="_orbitalSpeed">Banehastighet</param>
        /// <param name="_orbitAngle">banevinkel</param>
        /// <param name="_rotSpeed">rotasjonshastighet</param>
        /// <param name="_parent">Parent objekt</param>
        public AsteroidBelt(Game _game, float _size, float _orbitRadius, float _akseAngle, float _orbitalSpeed, float _orbitAngle, float _rotSpeed, SpaceObject _parent): base(_game,  _size,  _orbitRadius,  _akseAngle,  _orbitalSpeed,  _orbitAngle,  _rotSpeed,  _parent)
        {
            objects = new Model[MAX_ASTEROIDS];
            asteroidsPos = new List<Vector3>(MAX_ASTEROIDS);
        }

        /// <summary>
        /// Utføres innefra Load metode i MainClass
        /// </summary>
        /// <param name="_effect"></param>
        /// <param name="_model"></param>
        /// <param name="_texture"></param>
        public override void load(Effect _effect, Model _model, Texture2D _texture)
        {
            texture = _texture;
            base.effect = _effect;
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i] = _model;
                foreach (ModelMesh mesh in objects[i].Meshes)
                {
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = _effect.Clone();
                    }
                }
            }
            
        } //end of load

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public override void Draw(GameTime gt)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                foreach (ModelMesh mesh in objects[i].Meshes)
                {
                    foreach (Effect e in mesh.Effects)
                    {
                        e.CurrentTechnique = e.Techniques["Textured"];
                        e.Parameters["xWorld"].SetValue(base.world);
                        e.Parameters["xView"].SetValue(base.view);
                        e.Parameters["xTexture"].SetValue(this.texture);
                        e.Parameters["xEmissiveColor"].SetValue(new Vector4(1f, 1f, 1f, 0f));
                        e.Parameters["isEmissive"].SetValue(isEmissive);
                        e.Parameters["xLightsWorldViewProjection"].SetValue(this.world * base.effect.Parameters["xView"].GetValueMatrix() * base.effect.Parameters["xProjection"].GetValueMatrix());
                    }

                    mesh.Draw();
                }
            }

            //base.Draw(gt);
        }
    }

}
