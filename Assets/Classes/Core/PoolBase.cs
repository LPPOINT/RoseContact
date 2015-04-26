using System;
using Assets.Classes.Foundation.Classes;
using PathologicalGames;

namespace Assets.Classes.Core
{
    public class PoolBase<TMyType, TEntityType> 
        : SingletonEntity<TMyType>
             where TMyType : RoseEntity
             where TEntityType : RoseEntity
    {
        public SpawnPool Pool;
        public string PrefabName;
        public string PoolName;


        public event EventHandler<GenericEventArgs<TEntityType>> EntitySpawned;
        public event EventHandler<GenericEventArgs<TEntityType>> EntityReleased;

        protected virtual void OnEntityReleased(TEntityType e)
        {
            var handler = EntityReleased;
            if (handler != null) handler(this, new GenericEventArgs<TEntityType>(e));
        }
        protected virtual void OnEntitySpawned(TEntityType e)
        {
            var handler = EntitySpawned;
            if (handler != null) handler(this, new GenericEventArgs<TEntityType>(e));
        }


        public virtual TEntityType Spawn()
        {
            var c = Pool.Spawn(PrefabName).GetComponent<TEntityType>();
            OnEntitySpawned(c);
            return c;
        }
        public virtual void Despawn(TEntityType e)
        {
            Pool.Despawn(e.transform);
            OnEntityReleased(e);
        }

    }
}
