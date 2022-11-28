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
        public uint incrementEntityId;

        private List<GEntity> entities = new List<GEntity>();
        private Dictionary<uint, GEntity> entityDict = new Dictionary<uint, GEntity>();

        public GEntity? GetEntity(uint entityId)
        {
            if (entityDict.TryGetValue(entityId, out var entity)) return entity;

            return null;
        }

        public void AddEntity(GEntity entity)
        {
            entity.entityId = ++incrementEntityId;
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
    }
}
