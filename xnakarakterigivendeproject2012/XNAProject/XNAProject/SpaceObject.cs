using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAProject
{
    /// <summary>
    /// Klassen representerer et objekt i solar systemet slik som Sola, planetter og satelitter (samt kunstige satellitter)
    /// 
    /// </summary>
    class SpaceObject : DrawableGameComponent
    {
        /// <summary>
        /// Nødvendige variabler
        /// </summary>
        private SpaceObject parent;
        private Model thisPlanet;
        private Matrix world, view, projection;
        private float size;
        private float orbitRadius;
        private float akseAngle;
        private float orbitalSpeed;
        private float orbitAngle;
        private float rotSpeed;
        private Texture2D texture;
        private Effect effect;
        private Game game;

        /// <summary>
        /// Brukes til å lage nytt objekt
        /// </summary>
        /// <param name="size">Størrelse av planet</param>
        /// <param name="orbitRadius">planetens baneradius</param>
        /// <param name="akseAngle"> vinkelen til rotasjonsakse</param>
        /// <param name="orbitalSpeed">banehastiget</param>
        /// <param name="orbitAngle">banevinkel i forhold til xz plane</param>
        /// <param name="parent">parent planet</param>
        public SpaceObject(Game _game, float _size, float _orbitRadius, float _akseAngle, float _orbitalSpeed, float _orbitAngle, float _rotSpeed, SpaceObject _parent): base(_game)
        {
            this.size = _size;
            this.orbitRadius = _orbitRadius;
            this.akseAngle = _akseAngle;
            this.orbitalSpeed = _orbitalSpeed;
            this.orbitAngle = _orbitAngle;
            this.rotSpeed = _rotSpeed;
            this.game = _game;
            if (_parent != null)
            {
                this.parent = _parent;
            }
        }


        /// <summary>
        ///  Metode skal kalles i load metode av main class
        /// </summary>
        /// <param name="game">reference til spill, objekt tilhører</param>
        public void load(Effect _effect)
        {
            
        }//end of load


        float RotY, orbRotY;
        /// <summary>
        /// metode skal kalles i update metode av main class
        /// </summary>
        public void update(GameTime gt)
        {
            Matrix matIdentity, matTrans, matRotateY, matRotateX, matScale, matOrbT, matOrbR;
            
            matIdentity = Matrix.Identity;
            
            matScale = Matrix.CreateScale(this.size);
            
            //rotasjon om y akse
            RotY += this.rotSpeed * (float)gt.ElapsedGameTime.Milliseconds / 1000f;
            RotY = RotY % (float)(2 * Math.PI);
            matRotateY = Matrix.CreateRotationY(RotY);

            //vinkel til jordakse
            matRotateX = Matrix.CreateRotationX(this.akseAngle);

            //orbital rotasjon
            matOrbT = Matrix.CreateTranslation(this.orbitRadius, 0, this.orbitRadius);
            orbRotY += this.orbitalSpeed * (float)gt.ElapsedGameTime.Milliseconds / 5000.0f;
            orbRotY = orbRotY % (float)(2 * Math.PI);
            matOrbR = Matrix.CreateRotationY((orbRotY));

            matTrans = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            
            //Kumulativ world‐matrise;
            if (parent == null)
            {
                world = matIdentity * matScale * matRotateY * matRotateX * matOrbT * matOrbR * matTrans;
            }
            else
            {
                world = matIdentity * matScale * matRotateY * matRotateX * matOrbT * matOrbR * matTrans * parent.world;
            }

        }//end of update


        /// <summary>
        /// Tegner model, den metoden kalles fra draw i main class
        /// </summary>
        public void draw()
        {

            foreach (ModelMesh mesh in thisPlanet.Meshes)
            {
                foreach (Effect e in mesh.Effects)
                {

                }
                
                mesh.Draw();
            }

        }//end of draw
    }
}
