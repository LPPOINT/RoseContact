using UnityEngine.UI;
#if UNITY_EDITOR
using System.Linq;
using AdvancedInspector;
using Assets.Classes.Core;
using Assets.Classes.Implementation;
using UnityEditor;
using UnityEngine;

namespace Assets.Classes.DevTools
{
    [AdvancedInspector(true)]
    public class ChunksPrefabsPostprocess : RoseEntity
    {
        public float Radius;
        public float OffsetX;
        public float OffsetY;

        [Inspect, Method(MethodDisplay.Button)]
        public void RebuildColliders()
        {
            var g = FindObjectOfType<Gameplay>();
            foreach (var p in g.ChunksPrefabs)
            {
                Debug.Log("Rebuilding collider of " + p.gameObject.name);
                var pi = PrefabUtility.InstantiatePrefab(p.gameObject) as GameObject;

                if (pi != null)
                {
                    var colliders = pi.GetComponentsInChildren<CircleCollider2D>();
                    foreach (var c in colliders.Where((d, i) => d.gameObject.CompareTag(Obstacle.Tag)))
                    {
                        c.offset = new Vector2(OffsetX, OffsetY);
                        c.radius = Radius;
                    }
                }

                PrefabUtility.ReplacePrefab(pi, p);
                DestroyImmediate(pi);

            }
        }

        [Inspect, Method(MethodDisplay.Button)]
        public void RemoveCanvases()
        {
            var g = FindObjectOfType<Gameplay>();
            foreach (var p in g.ChunksPrefabs)
            {
                Debug.Log("Remove canvas from " + p.gameObject.name);
                var pi = PrefabUtility.InstantiatePrefab(p.gameObject) as GameObject;

                if (pi != null)
                {
                    var scoreline = pi.GetComponentInChildren<ScoreLine>();
                    if (scoreline != null)
                    {
                        foreach (Transform c in scoreline.transform)
                        {
                            if (c.gameObject.name.Contains("Canvas") &&
                                c.gameObject.GetComponent<RectTransform>() != null)
                            {
                                DestroyImmediate(c.gameObject);
                            }
                        }
                    }
                }

                PrefabUtility.ReplacePrefab(pi, p);
                DestroyImmediate(pi);

            }
        }

        [Inspect, Method(MethodDisplay.Button)]
        public void FixInnerImages()
        {
           // inner.gameObject.transform.localPosition = new Vector3(-0.8f, 0.18f, inner.gameObject.transform.localPosition.z);
            var g = FindObjectOfType<Gameplay>();
            foreach (var p in g.ChunksPrefabs)
            {

                var pi = PrefabUtility.InstantiatePrefab(p.gameObject) as GameObject;

                if (pi != null)
                {
                    var colliders = pi.GetComponentsInChildren<CircleCollider2D>();
                    foreach (var c in colliders.Where((d, i) => d.gameObject.CompareTag(Obstacle.Tag)))
                    {
                        foreach (Transform inner in c.transform)
                        {
                            Debug.Log("Change pos");
                            inner.gameObject.transform.localPosition = new Vector3(-0.09f, 0.18f, inner.gameObject.transform.localPosition.z);
                        }
                    }
                }

                PrefabUtility.ReplacePrefab(pi, p);
                DestroyImmediate(pi);

            }
        }
    }
}
#endif
