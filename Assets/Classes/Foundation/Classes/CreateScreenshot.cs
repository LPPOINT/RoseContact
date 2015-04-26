using System;
using System.Collections;
using UnityEngine;

namespace Assets.Classes.Foundation.Classes
{
    public class CreateScreenshot : MonoBehaviour
    {

        public void Create()
        {

            StartCoroutine(WaitAndCreate());


        }

        public int Power = 1;

        private IEnumerator WaitAndCreate()
        {

            yield return new WaitForEndOfFrame();

            Application.CaptureScreenshot(@"C:\Users\Sasha\Documents\ProjectX\scr" + Guid.NewGuid().ToString("N") + ".png", Power);

            //// Create a texture the size of the screen, RGB24 format
            //var width = Screen.width;
            //var height = Screen.height;
            //var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            //// Read screen contents into the texture
            //tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            //tex.Apply();

            //// Encode texture into PNG
            //var bytes = tex.EncodeToPNG();

            //using (var f = File.Create(@"C:\Users\Sasha\Documents\OBox\Assets\Editor\Screenshots\Scr.png"))
            //{
            //    f.Write(bytes, 0, bytes.Length);
            //}
        }
    }
}
