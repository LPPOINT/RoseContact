using System;
using System.Collections;
using System.IO;
using AdvancedInspector;
using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Foundation.Classes
{

    
    public class ScreenshotMaker : RoseEntity
    {

        public int Power = 1;
        public string Path = @"C:\RoseContactCommon\ScreenshotsSource\";
        public string CustomName;

        public KeyCode Hotkey = KeyCode.F12;

        public string GenerateScreenshotPathWithCurrentDate()
        {
            var path = CustomName == string.Empty ? string.Format("{2}{0}x{1}_{3}.png", (Screen.width * Power), (Screen.height * Power), Path, DateTime.Now.GetHashCode()) :  string.Format("{0}{1}.png", Path, CustomName);
            Debug.Log(path);
            return path;
        }

       
        public void CreateScreenshot()
        {

            StartCoroutine(CreateScreenshotDelayed());
        }

        public IEnumerator CreateScreenshotDelayed()
        {

            yield return new WaitForEndOfFrame();

            Application.CaptureScreenshot(GenerateScreenshotPathWithCurrentDate(), Power);

        }

        private void Update()
        {
            if (Input.GetKeyDown(Hotkey))
            {
                CreateScreenshot();
            }
        }
    }
}
