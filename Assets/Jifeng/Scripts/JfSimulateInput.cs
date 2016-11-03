using UnityEngine;
using System.Collections;

namespace Jifeng.SoftVrEngine
{
    public class SpaceData
    {
        public Vector3 Position { get; private set; }
        public Quaternion Orientation { get; protected set; }
        public Matrix4x4 Matrix { get; protected set; }

        public SpaceData()
        {
            Position = Vector3.zero;
            Orientation = Quaternion.identity;
            Matrix = Matrix4x4.identity;
        }
        public SpaceData(Vector3 position,Quaternion orientation)
        {
            Set(position, orientation);
        }
        public SpaceData(Matrix4x4 matrix)
        {
            Set(matrix);
        }
        public void Set(Vector3 position,Quaternion orientation)
        {
            Position = position;
            Orientation = orientation;
            Matrix = Matrix4x4.TRS(position, orientation, Vector3.one);
        }
        public void Set(Matrix4x4 matrix)
        {
            Matrix = matrix;
            Position = matrix.GetColumn(3);
            Orientation = Quaternion.LookRotation(matrix.GetColumn(2)
                                                        , matrix.GetColumn(1));
        }
    }
    public class JfSimulateInput
    {
        private float mouseX = 0;
        private float mouseY = 0;
        private float mouseZ = 0;
        public SpaceData headPose = new SpaceData();
        // simulate neck model in the editor mode.
        private static readonly Vector3 neckOffset = 
                                            new Vector3(0, 0.075f, -0.08f);
        private float neckModelScale = 0.0f;
        public  void    UpdateState()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            Quaternion rot;
            bool rolled = false;
            if(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                mouseX += Input.GetAxis("Mouse X") * 5;
                if(mouseX <= -180)
                {
                    mouseX += 360;
                }
                else if(mouseX > 180)
                {
                    mouseX -= 360;
                }
                mouseY -= Input.GetAxis("Mouse Y") * 2.4f;
                mouseY = Mathf.Clamp(mouseY, -85, 85);
            }
            else if(Input.GetKey(KeyCode.LeftControl) 
                            || Input.GetKey(KeyCode.RightControl))
            {
                rolled = true;
                mouseZ += Input.GetAxis("Mouse X") * 5;
                mouseZ = Mathf.Clamp(mouseZ, -85, 85);
            }
            if(!rolled)
            {
                mouseZ = Mathf.Lerp(mouseZ,0,Time.deltaTime / 
                                        (Time.deltaTime + 0.1f));
            }
            rot = Quaternion.Euler(mouseY, mouseX, mouseZ);
            var neck = (rot * neckOffset - neckOffset.y * Vector3.up)
                                        * neckModelScale;
            headPose.Set(neck, rot);
#endif
        }
    }
    
}