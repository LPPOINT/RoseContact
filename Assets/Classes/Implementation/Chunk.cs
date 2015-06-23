using System.Collections.Generic;
using System.Linq;
using AdvancedInspector;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Foundation.Extensions;
using Assets.Classes.Implementation.Randomization;
using UnityEngine;
using Vectrosity;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Classes.Implementation
{
    [AdvancedInspector]
    public class Chunk : RoseEntity
    {

        [Inspect]public int Number;
  
        [Inspect, Range(0, 10*5)]
        public float Height = 4f;

        [Inspect]
        public bool CanBeMirroredByXAxis = true;

        [Inspect] 
        public bool OnlyOneScoreLine = true;

        public bool IsMirroredByXAxis
        {
            get { return transform.localScale.x < 0; }
        }

        public bool IsInViewport
        {
            get
            {
                return gameObject.activeSelf &&
                       GameCamera.Instance.DetectContainmentType(
                           new Rect(GameCamera.Instance.Camera.transform.position.x, TopMiddle.y, 10, Height)) ==
                       GameCameraBase.ViewportContainmentType.Inside;
            }
        }

        public Vector3 BottomMiddle
        {
            get { return transform.position; }
        }
        public Vector3 TopMiddle
        {
            get
            {
                return new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);
            }
        }
        public Vector3 LeftCenter
        {
            get
            {
                var halfWidth = GameCamera.Instance.Viewport.width / 2;
                return new Vector3(transform.position.x - halfWidth, transform.position.y + Height / 2, transform.position.z);
            }
        }
        public Vector3 RightCenter
        {
            get
            {
               // GameCamera.Instance.InvalidateViewport();
                var halfWidth = GameCamera.Instance.Viewport.width / 2;
                return new Vector3(transform.position.x + halfWidth, transform.position.y + Height / 2, transform.position.z);
            }
        }

        public void Positionate(float yMinPosition)
        {
            //GameCamera.Instance.InvalidateViewport();
            transform.position = new Vector3(GameCamera.Instance.Viewport.xMin + GameCamera.Instance.Viewport.width / 2f, yMinPosition, transform.position.z);
        }

        public void Randomize()
        {
            if (CanBeMirroredByXAxis && RandomableBoolUtils.RandomBool())
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            var randomable = GetComponentsInChildren<ChunkRandomable>();
            foreach (var chunkRandomable in randomable)
            {
                chunkRandomable.Randomize();
            }
        }

        private List<ScoreLine> lines; 

        private void InitializeScoreLines()
        {
            lines = new List<ScoreLine>(GetComponentsInChildren<ScoreLine>());

            if (lines.Count > 1 && OnlyOneScoreLine)
            {
                var scoreLine = lines.Random();

                foreach (var line in lines)
                {
                    if (line != scoreLine)
                    {
                        Destroy(line.gameObject);
                    }
                }

                lines.Clear();
                lines.Add(scoreLine);

            }

        }

        public void InvalidateScoreLines(Collider2D c, ScoreLine line)
        {
            if (line.GetComponentInParent<Chunk>().Equals(this))
            {

                foreach (var scoreLine in lines)
                {
                    if (!scoreLine.Equals(line))
                    {
                        scoreLine.Dispose(c, false, false);
                    }
                }
                lines.Clear();
            }

        }

        protected override void Awake()
        {
            InitializeScoreLines();      
            Randomize();
            base.Awake();
        }

        private void Start()
        {
            SetupDebugGizmos();
        }

        private void Update()
        {
            UpdateDebugGizmos();
        }

        private bool isWithDebugGizmos;

        private VectorLine bottomLine;

        private void SetupDebugGizmos()
        {
            isWithDebugGizmos = false;
            return;
            isWithDebugGizmos = true;
            bottomLine = VectorLine.SetLine(Color.blue, new Vector3(GameCamera.Instance.Viewport.xMin, BottomMiddle.y, -20.3f),
                new Vector3(GameCamera.Instance.Viewport.xMax, BottomMiddle.y, -20.3f));

            bottomLine.name = gameObject.name + "Bottom";
        }

        private void DestroyDebugGizmos()
        {
            if (isWithDebugGizmos)
            {
                VectorLine.Destroy(ref bottomLine);
            }
        }

        private void UpdateDebugGizmos()
        {
            if (isWithDebugGizmos)
            {
                bottomLine.Draw3D();
            }
        }

        private void OnGUI()
        {
            if (isWithDebugGizmos)
            {
                
            }
        }

  

        private void OnDestroy()
        {
            DestroyDebugGizmos();
        }

        #region Physics


        public List<Rigidbody2D> childRigidbodies;

        private void InitializePhysics()
        {
            childRigidbodies = new List<Rigidbody2D>(GetComponentsInChildren<Rigidbody2D>());
        }

        public void EnablePhysics()
        {
            foreach (var childRigidbody in childRigidbodies)
            {
                childRigidbody.Sleep();
            }
        }

        public void DisablePhysics()
        {
            
        }

        #endregion

        #region UnityEditor stuff
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //GameCamera.Instance.InvalidateViewport();
            var halfWidth = GameCamera.Instance.Viewport.width/2;

            Gizmos.DrawLine(new Vector3(transform.position.x - halfWidth, transform.position.y, transform.position.z), new Vector3(transform.position.x + halfWidth, transform.position.y, transform.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x - halfWidth, transform.position.y + Height, transform.position.z), new Vector3(transform.position.x + halfWidth, transform.position.y + Height, transform.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x - halfWidth, transform.position.y, transform.position.z), new Vector3(transform.position.x - halfWidth, transform.position.y + Height, transform.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x + halfWidth, transform.position.y, transform.position.z), new Vector3(transform.position.x + halfWidth, transform.position.y + Height, transform.position.z));
        }

        [Inspect, Method(MethodDisplay.Button)]
        public void FitScreenByHeight()
        {
            GameCamera.Instance.InvalidateViewport();
            Height = GameCamera.Instance.Viewport.height;
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif

        }

        [Inspect, Method(MethodDisplay.Button)]
        public void AlignCameraToChunk()
        {
            GameCamera.Instance.InvalidateViewport();
            GameCamera.ImplementationInstance.AlignTo(new Vector3(BottomMiddle.x, BottomMiddle.y + GameCamera.Instance.Viewport.height/2, GameCamera.Instance.transform.position.z));
        }

        #endregion

    }
}
