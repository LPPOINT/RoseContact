using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedInspector;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Foundation.Extensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

namespace Assets.Classes.Implementation
{

    [AdvancedInspector(true)]
    public class ScoreLine : RoseEntity
    {

        public const string Tag = "ScoreLine";

        private Chunk chunk;


        private static readonly List<ScoreLine> allLines = new List<ScoreLine>();

        public static ScoreLine GetScoreLineByLineCollider(Collider2D c)
        {
            return allLines.FirstOrDefault(scoreLine => scoreLine.gameObject.name == c.gameObject.name);
        }

        #region Inspector varialbes

        [Inspect("IsLinear")]
        public Transform From;

        [Inspect("IsLinear")]
        public Transform To;

        [Inspect("IsCircle")]
        public Transform Center;

        [Inspect("IsCircle")]
        public float Radius;

        public Material Material;
        public Color Color;
        public float Width;

        public enum Type
        {
            Linear,
            Circle
        }

        public Type LineType;


        public bool IsLinear()
        {
            return LineType == Type.Linear;
        }

        public bool IsCircle()
        {
            return LineType == Type.Circle;
        }

        #endregion

        #region Line canvas

        private static bool isLineCanvasPrepared;
        private static void PrepareLineCanvas()
        {
            isLineCanvasPrepared = true;

            //VectorLine.SetCanvasCamera(GameCamera.Instance.Camera);
            //VectorLine.canvas.sortingOrder = 0;
            //VectorLine.canvas.planeDistance = 50;

            VectorLine.canvas3D.sortingOrder = -1;

        }
        #endregion

        #region Line create/destroy

        private VectorLine line;
        private void CreateLine()
        {
            if (LineType == Type.Linear)
            {
                var endPoints = new Vector3[2];

                endPoints[0] = From.transform.position;
                endPoints[1] = To.transform.position;

                line = new VectorLine("Line" + Id, endPoints, Material, Width);
                line.capLength = 8;
                line.textureScale = 1f;
            }
            else
            {
                line = VectorLine.SetLine(Color, new Vector3[90]);
                line.MakeCircle(Center.position, Radius);
            }



            line.color = ColorThemes.Instance.CurrentColorTheme.OverlayForegroundColor;
            line.SetWidth(14);
            //line.material = Gameplay.Instance.ScoreLineMaterial;

            if (Gameplay.Instance.CurrentMode == Gameplay.GameplayMode.Play)
            {
                line.collider = true;
                line.trigger = true;

                var rb = line.rectTransform.gameObject.AddComponent<Rigidbody2D>();

                rb.isKinematic = true;

            }
            line.Draw3D();

        }
        private void DestroyLine()
        {
            if (line != null)
                VectorLine.Destroy(ref line);
        }
        #endregion

        #region Dispose

        public bool IsDisposing { get; private set; }

        public float AlphaTime = 0.3f;

        public Image UIPlusOne;

        public void Dispose(Collider2D c, bool withPlusOneSign, bool isInitiator)
        {
            if(IsDisposing)
                return;
            IsDisposing = true;
            if(isInitiator)
                chunk.InvalidateScoreLines(c, this);
            DOTween.To(() => line.color, value => line.color = value, new Color(Color.green.r, Color.green.g, Color.green.b, 0), AlphaTime)
                .OnComplete(() =>
                            {
                               // IsDisposing = false;
                                Destroy(gameObject, 1);
                            });

            if (UIPlusOne != null && withPlusOneSign)
            {
                
                UIPlusOne.gameObject.SetActive(true);
                UIPlusOne.color = new Color(UIPlusOne.color.r, UIPlusOne.color.g, UIPlusOne.color.b, 1);
                var distance = 4f;
                var time = 0.86f;
                var offsetX = 0.5f;
                var offsetY = 0.5f;

                UIPlusOne.gameObject.transform.position = new Vector3(Benjamin.Instance.transform.position.x + offsetX, Benjamin.Instance.transform.position.y + offsetY, Benjamin.Instance.transform.position.z);
                UIPlusOne.transform.DOMoveY(UIPlusOne.transform.position.y - distance, time)
                    .SetEase(Ease.InBack);
                UIPlusOne.DOColor(new Color(1, 1, 1, 0), 1f)
                        .SetDelay(0.3f);
                var s = UIPlusOne.transform.localScale;
                UIPlusOne.transform.localScale = Vector3.zero;
                UIPlusOne.transform.DOScale(s, 0.4f)
                    .SetEase(Ease.OutBack);
            }

        }

        #endregion

        #region Unity callbacks

        public string Id { get; private set; }

        public string GetIdByGameObjectName()
        {
            return gameObject.name.Substring(4);
        }

        private void OnColorThemeChanged()
        {
            if(IsDisposing)
                return;
            var nextColor = ColorThemes.Instance.CurrentColorTheme.OverlayForegroundColor;
            DOTween.To(() => line.color, value => line.color = value, nextColor, 0.4f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (LineType == Type.Linear)
            {
                if (From != null && To != null)
                {

                    Gizmos.DrawLine(From.transform.position, To.transform.position);

                }
            }
            else if(Center != null)
            {
                Gizmos.DrawWireSphere(Center.transform.position, Radius);
            }
        }

        private void Start()
        {
            chunk = GetComponentInParent<Chunk>();
            if (!isLineCanvasPrepared)
                PrepareLineCanvas();

            GameMessenger.AddListener(ColorThemes.ColorThemeChangedEventName, OnColorThemeChanged);


            if (chunk.IsMirroredByXAxis)
            {
                transform.SetScaleX(transform.GetScaleX() * -1);
            }

            if (UIPlusOne == null)
            {
                UIPlusOne = GetComponentInChildren<Image>();
            }

            if (UIPlusOne != null)
            {
                var m = 2f;
                UIPlusOne.transform.localScale = new Vector3(UIPlusOne.transform.localScale.x * m,
                    UIPlusOne.transform.localScale.y * m, UIPlusOne.transform.localScale.z * m);
                UIPlusOne.gameObject.SetActive(false);
            }



            Id = Guid.NewGuid().ToString("N");
            CreateLine();
            allLines.Add(this);


            gameObject.name = "Line" + Id;
        }

        private void Update()
        {


            if (LineType == Type.Linear && chunk.IsInViewport)
            {
                var f = From.transform.position;
                var t = To.transform.position;

                if (line.points3[0] != f || line.points3[1] != t)
                {
                    line.points3[0] = f;
                    line.points3[1] = t;

                    line.Draw3D();
                }
                else if (IsDisposing)
                {
                    // line.Draw3D();
                }
            }

        }

        private void OnDisable()
        {
            DestroyLine();
            allLines.Remove(this);
        }

        private void OnDestroy()
        {
            GameMessenger.RemoveListener(ColorThemes.ColorThemeChangedEventName, OnColorThemeChanged);
            DestroyLine();
            allLines.Remove(this);
        }

        #endregion

    }
}
