using System;
using Assets.Classes.Core;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Assets.Editor
{
    [InitializeOnLoad]
    public static class VersionIncrement
    {


        static VersionIncrement()
        {
            EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
        }


        private delegate Version ChangeVersionDelegate(Version currentVersion);
        private static void ChangeVersion(ChangeVersionDelegate cvd)
        {
            var currentVersion = new Version(PlayerSettings.bundleVersion);
            var newVersion = cvd(currentVersion);
            PlayerSettings.bundleVersion = newVersion.ToString();
        }


        private static void PlaymodeStateChanged()
        {

            if (!EditorApplication.isPlayingOrWillChangePlaymode 
                && !EditorApplication.isPlaying
                && !EditorApplication.isPaused 
                && !EditorApplication.isCompiling)
            {
                ChangeVersion(oldVersion =>  new Version(oldVersion.Major, oldVersion.Minor, oldVersion.Build, oldVersion.Revision+1));
            }
        }

        [DidReloadScripts]
        private static void IncrementBuildVersion()
        {

            if(!EditorApplication.isPlayingOrWillChangePlaymode) 
            {
                ChangeVersion(oldVersion => new Version(oldVersion.Major, oldVersion.Minor, oldVersion.Build+1, oldVersion.Revision));
            }

        }

        [PostProcessBuild]
        private static void IncrementRevisionVersion(BuildTarget target, string pathToBuiltProject)
        {
            ChangeVersion(oldVersion => new Version(oldVersion.Major+1, oldVersion.Minor, oldVersion.Build, oldVersion.Revision));
        }

    }
}
