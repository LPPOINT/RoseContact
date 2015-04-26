using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedInspector;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Extensions;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    [AdvancedInspector]
    public class ColorThemes : SingletonEntity<ColorThemes>
    {
        [Serializable]
        public class ColorTheme
        {
            public Color CameraBackgroundColor;
            public Color OverlayBackgroundColor;
            public Color OverlayForegroundColor;

            protected bool Equals(ColorTheme other)
            {
                return CameraBackgroundColor.Equals(other.CameraBackgroundColor) && OverlayBackgroundColor.Equals(other.OverlayBackgroundColor) && OverlayForegroundColor.Equals(other.OverlayForegroundColor);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((ColorTheme)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = CameraBackgroundColor.GetHashCode();
                    hashCode = (hashCode * 397) ^ OverlayBackgroundColor.GetHashCode();
                    hashCode = (hashCode * 397) ^ OverlayForegroundColor.GetHashCode();
                    return hashCode;
                }
            }
        }

        [Inspect]
        public List<ColorTheme> Themes;

        public const string ColorThemeChangedEventName = "ColorThemeChanged";

        public void SelectNextColorTheme()
        {
            nextColorTheme = null;
        }

        private ColorTheme nextColorTheme;
        public ColorTheme NextColorTheme
        {
            get { return nextColorTheme ?? (nextColorTheme = Themes.NonRecurringRandom()); }
        }
        public ColorTheme CurrentColorTheme { get; private set; }
        public ColorTheme GetColorThemeByCameraBackgroundColor(Color color)
        {
            return Themes.FirstOrDefault(theme => theme.CameraBackgroundColor.Equals(color));
        }


        [Inspect, Method(MethodDisplay.Button)]
        public void ChangeColorTheme()
        {
            var theme = NextColorTheme;
            CurrentColorTheme = theme;
            SelectNextColorTheme();
            GameMessenger.Broadcast(ColorThemeChangedEventName);
        }

        public void ValidateColors(bool force)
        {
            const float fgOverlayDarkness = 0.2f;
            const float bgOverlayDarkness = 0.4f;
            const float bgOverlayAlpha = 0.7f;

            foreach (var t in Themes)
            {
                if (t.OverlayBackgroundColor == Color.clear || force)
                {
                    t.OverlayBackgroundColor = new Color(t.CameraBackgroundColor.r - fgOverlayDarkness, t.CameraBackgroundColor.g - fgOverlayDarkness, t.CameraBackgroundColor.b - fgOverlayDarkness, bgOverlayAlpha);
                }
                if (t.OverlayBackgroundColor.a != bgOverlayAlpha)
                {
                    t.OverlayBackgroundColor = new Color(t.OverlayBackgroundColor.r, t.OverlayBackgroundColor.g, t.OverlayBackgroundColor.b, bgOverlayAlpha);
                }
                if (t.OverlayForegroundColor == Color.clear || force)
                {
                    t.OverlayForegroundColor = new Color(t.CameraBackgroundColor.r - bgOverlayDarkness, t.CameraBackgroundColor.g - bgOverlayDarkness, t.CameraBackgroundColor.b - bgOverlayDarkness, 1);
                }
            }

        }

        [Inspect, Method(MethodDisplay.Button)]
        public void ValidateColors()
        {
            ValidateColors(false);
        }
        [Inspect, Method(MethodDisplay.Button)]
        public void ForceValidateColors()
        {
            ValidateColors(true);
        }


        protected  void Start()
        {
            var theme = GetColorThemeByCameraBackgroundColor(GameCamera.Instance.Camera.backgroundColor);
            if (theme != null)
            {
                CurrentColorTheme = theme;
            }
            else
            {
               ChangeColorTheme();
            }
            Camera.main.backgroundColor = CurrentColorTheme.CameraBackgroundColor;
        }


    }
}
