using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class GameSound : SingletonEntity<GameSound>
    {

        public const string SoundMutedDbKey = "SoundMuted";

        public bool IsMuted { get; private set; }

        public const string MuteStateChangedEventName = "MuteStateChanged";

        private void FireMuteStateChangedEvent()
        {
            GameMessenger.Broadcast(MuteStateChangedEventName);
        }

        public void Mute()
        {
            BackgroundSource.Stop();
            SfxSource.Stop();
            IsMuted = true;
            FireMuteStateChangedEvent();
            Game.Instance.Database.SetBool(SoundMutedDbKey, true);
        }

        public void ToggleMute()
        {
            if (IsMuted) UnMute();
            else Mute();
        }

        public void UnMute()
        {
            IsMuted = false;
            FireMuteStateChangedEvent();
            Game.Instance.Database.SetBool(SoundMutedDbKey, false);

            if (!IsMuted && BackgroundClip != null && BackgroundSource != null)
            {
                BackgroundSource.clip = BackgroundClip;
                BackgroundSource.Play();
            }
        }


        public AudioSource BackgroundSource;
        public AudioSource SfxSource;


        public enum ClipType
        {
            Sfx,
            Background
        }

        public AudioSource GetSourceByClipType(ClipType ct)
        {
            return ct == ClipType.Background ? BackgroundSource : SfxSource;
        }

        public void PlaySingle(AudioClip clip, ClipType type)
        {
            if(clip == null)
                return;
            //return;
            if (IsMuted)
                return;
            //var s = GetSourceByClipType(type);
            //s.clip = clip;
            //s.Play();
            AudioSource.PlayClipAtPoint(clip, GameCamera.Instance.Camera.transform.position, 0.2f);
        }

        public AudioClip TeleportationClip;
        public AudioClip BackgroundClip;
        public AudioClip CrashClip;
        public AudioClip GameoverOpenClip;
        public AudioClip GameoverCloseClip;
        public AudioClip MainMenuCloseClip;
        public AudioClip ScoreLineCrossedClip;
        public AudioClip DirectionChangedClip;

        public void PlayLineCrossed()
        {
            PlaySingle(ScoreLineCrossedClip, ClipType.Sfx);
        }

        public void PlayTeleportation()
        {
            PlaySingle(TeleportationClip, ClipType.Sfx);
        }

        public void PlayCrash()
        {
            PlaySingle(CrashClip, ClipType.Sfx);
        }

        public void PlayDirectionChanged()
        {
            PlaySingle(DirectionChangedClip, ClipType.Sfx);
        }

        protected override void Awake()
        {
            if (Game.Instance.Database.GetBool(SoundMutedDbKey))
            {
                Mute();
            }
            base.Awake();
        }

        private void Start()
        {
            if (!IsMuted && BackgroundClip != null && BackgroundSource != null)
            {
                BackgroundSource.clip = BackgroundClip;
                BackgroundSource.Play();
            }
        }
    }
}