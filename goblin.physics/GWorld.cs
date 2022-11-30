using GoblinFramework.Physics.Entity;
using GoblinFramework.Physics.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace GoblinFramework.Physics
{
    public class GWorld
    {
        public uint incrementId { get; private set; }

        private List<GEntity> entities = new List<GEntity>();
        private Dictionary<uint, GEntity> entityDict = new Dictionary<uint, GEntity>();
        private List<GEntity> dirtyEntities = new List<GEntity>();

        public GEntity? GetEntity(uint entityId)
        {
            if (entityDict.TryGetValue(entityId, out var entity)) return entity;

            return null;
        }

        public void AddEntity(GEntity entity)
        {
            entity.entityId = ++incrementId;
            entities.Add(entity);
            entityDict.Add(entity.entityId, entity);
        }

        public void RmvEntity(uint entityId)
        {
            if (entityDict.TryGetValue(entityId, out var entity)) RmvEntity(entity.entityId);
        }

        public void RmvEntity(GEntity entity)
        {
            entities.Remove(entity);
            entityDict.Remove(entity.entityId);
        }

        public void SetDirty(GEntity entity)
        {
            if (dirtyEntities.Contains(entity)) return;
            dirtyEntities.Add(entity);
        }
    }
}
