using System;
using System.Linq;
using DG.Tweening;
using PathologicalGames;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Core
{
  
    public abstract class RoseEntity : MonoBehaviour
    {

        #region Components caching
        [HideInInspector, NonSerialized]
        private Animation _animation;

        public new Animation animation { get { return _animation ? _animation : (_animation = GetComponent<Animation>()); } }

        [HideInInspector, NonSerialized]
        private Canvas _canvas;
        public Canvas canvas { get { return _canvas ?? (_canvas = GetComponent<Canvas>()); } }

        [HideInInspector, NonSerialized]
        private Graphic _graphic;
        public Graphic graphic { get { return _graphic ?? (_graphic = GetComponent<Graphic>()); } }

        [HideInInspector, NonSerialized]
        private AudioSource _audio;


        public new AudioSource audio { get { return _audio ? _audio : (_audio = GetComponent<AudioSource>()); } }

        [HideInInspector, NonSerialized]
        private Camera _camera;


        public new Camera camera { get { return _camera ? _camera : (_camera = GetComponent<Camera>()); } }

        [HideInInspector, NonSerialized]
        private Collider _collider;


        public new Collider collider { get { return _collider ? _collider : (_collider = GetComponent<Collider>()); } }

        [HideInInspector, NonSerialized]
        private Collider2D _collider2D;


        public new Collider2D collider2D { get { return _collider2D ? _collider2D : (_collider2D = GetComponent<Collider2D>()); } }

        [HideInInspector, NonSerialized]
        private BoxCollider2D _boxCollider2D;


        public  BoxCollider2D boxCollider2D { get { return _boxCollider2D ? _boxCollider2D : (_boxCollider2D = GetComponent<BoxCollider2D>()); } }

        [HideInInspector, NonSerialized]
        private ConstantForce _constantForce;


        public new ConstantForce constantForce { get { return _constantForce ? _constantForce : (_constantForce = GetComponent<ConstantForce>()); } }

        [HideInInspector, NonSerialized]
        private GUIText _guiText;


        public new GUIText guiText { get { return _guiText ? _guiText : (_guiText = GetComponent<GUIText>()); } }

        [HideInInspector, NonSerialized]
        private GUITexture _guiTexture;


        public new GUITexture guiTexture { get { return _guiTexture ? _guiTexture : (_guiTexture = GetComponent<GUITexture>()); } }

        [HideInInspector, NonSerialized]
        private HingeJoint _hingeJoint;


        public new HingeJoint hingeJoint { get { return _hingeJoint ? _hingeJoint : (_hingeJoint = GetComponent<HingeJoint>()); } }

        [HideInInspector, NonSerialized]
        private Light _light;

        public new Light light { get { return _light ? _light : (_light = GetComponent<Light>()); } }

        [HideInInspector, NonSerialized]
        private NetworkView _networkView;


        public new NetworkView networkView { get { return _networkView ? _networkView : (_networkView = GetComponent<NetworkView>()); } }

        [HideInInspector, NonSerialized]
        private ParticleEmitter _particleEmitter;


        public new ParticleEmitter particleEmitter { get { return _particleEmitter ? _particleEmitter : (_particleEmitter = GetComponent<ParticleEmitter>()); } }

        [HideInInspector, NonSerialized]
        private ParticleSystem _particleSystem;


        public new ParticleSystem particleSystem { get { return _particleSystem ? _particleSystem : (_particleSystem = GetComponent<ParticleSystem>()); } }

        [HideInInspector, NonSerialized]
        private SpriteRenderer _spriteRenderer;


        public SpriteRenderer spriteRenderer { get { return _spriteRenderer ? _spriteRenderer : (_spriteRenderer = GetComponent<SpriteRenderer>()); } }


        [HideInInspector, NonSerialized]
        private Renderer _renderer;


        public new Renderer renderer { get { return _renderer ? _renderer : (_renderer = GetComponent<Renderer>()); } }


        [HideInInspector, NonSerialized]
        private Rigidbody _rigidbody;


        public new Rigidbody rigidbody { get { return _rigidbody ? _rigidbody : (_rigidbody = GetComponent<Rigidbody>()); } }

        [HideInInspector, NonSerialized]
        private Animator _animator;


        public new Animator animator { get { return _animator ? _animator : (_animator = GetComponent<Animator>()); } }

        [HideInInspector, NonSerialized]
        private Rigidbody2D _rigidbody2D;

        public new Rigidbody2D rigidbody2D { get { return _rigidbody2D ? _rigidbody2D : (_rigidbody2D = GetComponent<Rigidbody2D>()); } }

        [HideInInspector, NonSerialized]
        private Transform _transform;

        public new Transform transform { get { return _transform ? _transform : (_transform = GetComponent<Transform>()); } }

        #endregion

        #region Logging
        public void Log(string message)
        {
            Debug.Log(message, this);
        }

        public void Log(string format, params object[] args)
        {
            Debug.Log(string.Format(format, args), this);
        }

        public void ProcessError(string message)
        {
            ProcessError(message, Logs.LogOutputFlags.ConsoleLog);

        }

        public void ProcessError(string message, Logs.LogOutputFlags displayFlags)
        {
            Logs.Instance.ProcessLog(Logs.LogNode.Create(message, this, Logs.LogNodeType.Error), displayFlags);
        }

        public void ProcessFatalError(string message)
        {
            ProcessFatalError(message, Logs.LogOutputFlags.ConsoleLog);

        }

        public void ProcessFatalError(string message, Logs.LogOutputFlags displayFlags)
        {
            Logs.Instance.ProcessLog(Logs.LogNode.CreateFatalError(message, this), displayFlags);
        }


        public object MakeUniqueId(string id)
        {
            return GetInstanceID() +  id;
        }

        #endregion


        protected virtual void Awake()
        {

        }

    }
}
