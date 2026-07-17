using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reclaimers.UpdateSystem
{
    public class UpdateSystem : MonoBehaviour
    {
        public static UpdateSystem Instance { get; private set; }

        private readonly CustomList<IUpdateable> _updateableObjects = new CustomList<IUpdateable>();
        private readonly CustomList<IFixedUpdateable> _fixedUpdateableObjects = new CustomList<IFixedUpdateable>();
        private readonly CustomList<ILateUpdateable> _lateUpdateablesObjects = new CustomList<ILateUpdateable>();
        private readonly CustomList<IPauseable> _pauseableObjects = new CustomList<IPauseable>();

        public bool isPaused = false;
        private bool togglePause = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);

                SceneManager.sceneUnloaded += OnSceneUnloaded;
            }
            else Destroy(this);
        }

        private void OnSceneUnloaded(Scene current)
        {
            _updateableObjects.Clear();
            _fixedUpdateableObjects.Clear();
            _lateUpdateablesObjects.Clear();
        }

        public void TogglePause() => togglePause = true;

        private void Update()
        {
            if (togglePause)
            {
                togglePause = false;
                isPaused = !isPaused;
                foreach (IPauseable pauseable in _pauseableObjects)
                {
                    pauseable.ManagedPause(isPaused);
                }
            }

            foreach (IUpdateable updatable in _updateableObjects)
            {
                updatable.ManagedUpdate();
            }
        }

        private void FixedUpdate()
        {
            foreach (IFixedUpdateable fixedUpdateable in _fixedUpdateableObjects)
            {
                fixedUpdateable.ManagedFixedUpdate();
            }
        }

        private void LateUpdate()
        {
            foreach (ILateUpdateable lateUpdateable in _lateUpdateablesObjects)
            {
                lateUpdateable.ManagedLateUpdate();
            }
        }


        public void Register(IManagedObject obj)
        {
            if (obj is IUpdateable updateable && !_updateableObjects.Contains(updateable))
            {
                _updateableObjects.Add(updateable);
            }
            if (obj is IFixedUpdateable fixedUpdateable && !_fixedUpdateableObjects.Contains(fixedUpdateable))
            {
                _fixedUpdateableObjects.Add(fixedUpdateable);
            }
            if (obj is ILateUpdateable lateUpdateable && !_lateUpdateablesObjects.Contains(lateUpdateable))
            {
                _lateUpdateablesObjects.Add(lateUpdateable);
            }
            if (obj is IPauseable pauseable && !_pauseableObjects.Contains(pauseable))
            {
                _pauseableObjects.Add(pauseable);
            }
        }

        public void Unregister(IManagedObject obj)
        {
            if (obj is IUpdateable updateable && _updateableObjects.Contains(updateable))
            {
                _updateableObjects.Remove(updateable);
            }
            if (obj is IFixedUpdateable fixedUpdateable && _fixedUpdateableObjects.Contains(fixedUpdateable))
            {
                _fixedUpdateableObjects.Remove(fixedUpdateable);
            }
            if (obj is ILateUpdateable lateUpdateable && _lateUpdateablesObjects.Contains(lateUpdateable))
            {
                _lateUpdateablesObjects.Remove(lateUpdateable);
            }
            if (obj is IPauseable pauseable && _pauseableObjects.Contains(pauseable))
            {
                _pauseableObjects.Remove(pauseable);
            }
        }
    }
}

