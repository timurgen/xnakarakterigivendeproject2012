using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjectFinal
{
    public partial class MainClass : Microsoft.Xna.Framework.Game
    {
        private Model LoadModelWithBoundingSphere(String modelName, ref Matrix[] matrix, ref Matrix[] originalTransforms)
        {
            Model model = Content.Load<Model>(modelName);
            matrix = new Matrix[model.Bones.Count];
            //Legger komplett transformasjonsmatrise for hver
            //ModelMesh i matrisetabellen:
            model.CopyAbsoluteBoneTransformsTo(matrix);
            //Finner BoundingSphere for hele modellen:
            BoundingSphere completeBoundingSphere = new BoundingSphere();
            foreach (ModelMesh mesh in model.Meshes)
            {
                //Henter ut BoundigSphere for aktuell ModelMesh:
                BoundingSphere origMeshSphere = mesh.BoundingSphere;
                //Denne transformeres i forhold til sitt Bone:
                origMeshSphere = TransformBoundingSphere(origMeshSphere, matrix[mesh.ParentBone.Index]);
                //Slår sammen:
                completeBoundingSphere =
                BoundingSphere.CreateMerged(completeBoundingSphere,
                origMeshSphere);
            }
            model.Tag = completeBoundingSphere;
            return model;
        }

        public BoundingSphere TransformBoundingSphere(BoundingSphere originalBoundingSphere, Matrix transformationMatrix)
        {
            Vector3 scaling;
            Quaternion rot;
            Vector3 trans;
            transformationMatrix.Decompose(out scaling, out rot, out trans);
            float maxScale = scaling.X;
            if (maxScale < scaling.Y)
                maxScale = scaling.Y;
            if (maxScale < scaling.Z)
                maxScale = scaling.Z;
            //Skalerer radius til opprinnelig sfære (originalBoundingSphere):
            float transformedSphereRadius = originalBoundingSphere.Radius * maxScale;
            //Transformerer sfærens sentrum:
            Vector3 transformedSphereCenter = Vector3.Transform(originalBoundingSphere.Center, transformationMatrix);
            //Oppretter en ny og transformert sfære:
            BoundingSphere transformedBoundingSphere = new BoundingSphere(transformedSphereCenter, transformedSphereRadius);
            return transformedBoundingSphere;
        }
    }
}
