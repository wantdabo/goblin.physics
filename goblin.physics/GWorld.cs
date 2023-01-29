using GoblinFramework.Physics.Collision;
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
        /// <summary>
        /// 自增的唯一 ID
        /// </summary>
        public uint incrementId { get; private set; }

        /// <summary>
        /// 实体列表（ALL）
        /// </summary>
        private List<GEntity> entities = new List<GEntity>();
        /// <summary>
        /// 实体字典（ALL）
        /// </summary>
        private Dictionary<uint, GEntity> entityDict = new Dictionary<uint, GEntity>();
        /// <summary>
        /// 脏的实体，需要重新计算状态
        /// </summary>
        private List<GEntity> dirtyEntities = new List<GEntity>();
        /// <summary>
        /// 空闲的 Entity 对象池
        /// </summary>
        private Queue<GEntity> idleEntities = new Queue<GEntity>();

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="detailTime">tick</param>
        /// <returns></returns>
        public void Update(FP detailTime)
        {
            foreach (var e0 in dirtyEntities)
            {
                var temp = e0.lastCollisions;
                e0.lastCollisions = e0.collisions;
                e0.collisions = temp;
                e0.collisions.Clear();

                foreach (var e1 in dirtyEntities)
                {
                    if (e0 == e1) continue;
                    if (false == GCollisionTesting.Test(e0, e1)) continue;

                    e0.collisions.Add(e1.entityId);
                }
            }
        }

        /// <summary>
        /// 构造一个实体
        /// </summary>
        /// <param name="shape">形状</param>
        /// <returns>实体</returns>
        public GEntity BornEntity(IGShape shape)
        {
            return BornEntity(shape, TSVector2.zero, FP.Zero);
        }

        /// <summary>
        /// 构造一个实体
        /// </summary>
        /// <param name="shape">形状</param>
        /// <param name="position">坐标</param>
        /// <param name="deg">角度</param>
        /// <returns>实体</returns>
        public GEntity BornEntity(IGShape shape, TSVector2 position, FP deg)
        {
            if (idleEntities.Count > 0)
            {
                var entity = idleEntities.Dequeue();
                entity.SetEntity(shape, position, deg);
                Add2World(entity);

                return entity;
            }

            return Add2World(new GEntity(shape, position, deg));
        }

        /// <summary>
        /// 销毁一个实体
        /// </summary>
        /// <param name="entity">实体</param>
        public void DeadEntity(GEntity entity)
        {
            idleEntities.Enqueue(entity);
            Rmv4World(entity);
        }

        /// <summary>
        /// 标记实体为脏
        /// </summary>
        /// <param name="entity"></param>
        public void SetDirty(GEntity entity)
        {
            if (dirtyEntities.Contains(entity)) return;
            dirtyEntities.Add(entity);
        }

        /// <summary>
        /// 添加到世界
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        private GEntity Add2World(GEntity entity)
        {
            entity.entityId = ++incrementId;
            entities.Add(entity);
            entityDict.Add(entity.entityId, entity);

            return entity;
        }

        /// <summary>
        /// 从世界移除
        /// </summary>
        /// <param name="entity">实体</param>
        private void Rmv4World(GEntity entity)
        {
            entities.Remove(entity);
            entityDict.Remove(entity.entityId);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="entityId">实体 ID</param>
        /// <returns>实体</returns>
        public GEntity GetEntity(uint entityId)
        {
            if (entityDict.TryGetValue(entityId, out var entity)) return entity;

            return null;
        }
    }
}
