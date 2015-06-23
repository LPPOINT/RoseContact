using System.Collections;
using Assets.Classes.Core;
using SmartLocalization;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class Social : SingletonEntity<Social>
    {


        public const string AppStoreLink = "https://itunes.apple.com/us/app/cyrcle/id982772237";

        #region Intarnal functions (very dirty code)
        private IEnumerator GameOverFacebookInternal(int score)
        {
            yield return new WaitForEndOfFrame();

            var texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

            IOSSocialManager.instance.FacebookPost(string.Format(LanguageManager.Instance.GetTextValue("Go.Facebook"), score, AppStoreLink), texture);

        }
        private IEnumerator GameOverTwitterInternal(int score)
        {
            yield return new WaitForEndOfFrame();

            var texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

            IOSSocialManager.instance.TwitterPost(string.Format(LanguageManager.Instance.GetTextValue("Go.Twitter"), score, AppStoreLink), texture);

        }
        private IEnumerator GameOverShareInternal(int score)
        {
            yield return new WaitForEndOfFrame();

            var texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

            IOSSocialManager.instance.ShareMedia(string.Format(LanguageManager.Instance.GetTextValue("Go.Twitter"), score, AppStoreLink), texture);

        }
        private IEnumerator MainMenuShareInternal()
        {
            yield return new WaitForEndOfFrame();

            var texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

            IOSSocialManager.instance.ShareMedia(string.Format(LanguageManager.Instance.GetTextValue("MainMenu.ShareMessage"), GameExternals.AppStoreURL), texture);

        }
        private IEnumerator MainMenuTwitterInternal()
        {
            yield return new WaitForEndOfFrame();

            var texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

            IOSSocialManager.instance.TwitterPost(string.Format(LanguageManager.Instance.GetTextValue("MainMenu.Twitter")), texture);

        }

        #endregion

        public void GameOverFacebook(int score)
        {
            StartCoroutine(GameOverFacebookInternal(score));
        }
        public void GameOverTwitter(int score)
        {
            StartCoroutine(GameOverTwitterInternal(score));
            
        }
        public void GameOverShare(int score)
        {
            StartCoroutine(GameOverShareInternal(score));
        }

        public void MainMenuShare()
        {
            StartCoroutine(MainMenuShareInternal());
        }
        public void MainMenuTwitter()
        {
            StartCoroutine(MainMenuTwitterInternal());
        }

        public void Rate()
        {
            Application.OpenURL(GameExternals.AppStoreURL);
        }


    }
}
