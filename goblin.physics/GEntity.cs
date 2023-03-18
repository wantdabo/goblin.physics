using GoblinFramework.Physics.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace GoblinFramework.Physics.Entity
{
    /// <summary>
    /// 实体
    /// </summary>
    public class GEntity
    {
        /// <summary>
        /// 实体 ID
        /// </summary>
        public uint entityId;

        private IGShape mShape = null;
        /// <summary>
        /// 形状
        /// </summary>
        public IGShape shape
        {
            get { return mShape; }
            set
            {
                mShape = value;
                SetDirty();
            }
        }

        private TSVector2 mPosition;
        /// <summary>
        /// 坐标
        /// </summary>
        public TSVector2 position
        {
            get { return mPosition; }
            set
            {
                mPosition = value;
                shape = shape.Translate(mPosition);
                SetDirty();
            }
        }

        private FP mDeg;
        /// <summary>
        /// 角度
        /// </summary>
        public FP deg
        {
            get { return mDeg; }
            set
            {
                mDeg = value;
                shape.Rotate(mDeg);
                SetDirty();
            }
        }

        private GWorld mWorld = null;
        /// <summary>
        /// 该 Entity 属于的 World
        /// </summary>
        public GWorld world { get { return mWorld; } set { mWorld = value; } }
        /// <summary>
        /// 上一次的碰撞列表
        /// </summary>
        public List<uint> lastCollisions = new List<uint>();
        /// <summary>
        /// 最新的碰撞列表
        /// </summary>
        public List<uint> collisions = new List<uint>();
        private List<uint> mAddCollisions = new List<uint>();
        /// <summary>
        /// 碰撞列表中，相对上一次的新增
        /// </summary>
        public List<uint> addCollisions {
            get
            {
                CalcDiffCollisions();

                return mAddCollisions;
            }
            private set { }
        }

        private List<uint> mDecCollisions = new List<uint>();
        /// <summary>
        /// 碰撞列表中，相对上一次的减少
        /// </summary>
        public List<uint> decCollisions {
            get
            {
                CalcDiffCollisions();

                return mDecCollisions;
            }
            private set { }
        }

        private List<uint> mDiffCollisions = new List<uint>();
        /// <summary>
        /// 碰撞列表的差集
        /// </summary>
        public List<uint> diffCollisions {
            get
            {
                CalcDiffCollisions();

                return mDiffCollisions;
            }
            private set { }
        }

        private List<uint> mUnionCollisions = new List<uint>();
        /// <summary>
        /// 碰撞列表的并集
        /// </summary>
        public List<uint> unionCollisions {
            get
            {
                mUnionCollisions.Clear();
                foreach (var entityId in collisions)
                    if (lastCollisions.Contains(entityId)) unionCollisions.Add(entityId);

                return mUnionCollisions;
            }

            private set { }
        }

        /// <summary>
        /// 计算碰撞列表的差集
        /// </summary>
        private void CalcDiffCollisions()
        {
            mAddCollisions.Clear();
            mDecCollisions.Clear();
            mDiffCollisions.Clear();
            foreach (var entityId in collisions)
            {
                if (false == unionCollisions.Contains(entityId)) diffCollisions.Add(entityId);
            }

            foreach (var entityId in lastCollisions)
            {
                if (false == unionCollisions.Contains(entityId)) diffCollisions.Add(entityId);
            }

            foreach (var entityId in mDiffCollisions)
            {
                if (collisions.Contains(entityId) && false == lastCollisions.Contains(entityId)) mAddCollisions.Add(entityId);
                if (false == collisions.Contains(entityId) && lastCollisions.Contains(entityId)) mDecCollisions.Add(entityId);
            }
        }

        /// <summary>
        /// 通知碰撞检测事件，监听就通知，不论是否变化
        /// </summary>
        public event Action onCollision;
        /// <summary>
        /// 碰撞事件更新就会通知，不论是否变化
        /// </summary>
        public event Action onCollisionUpdate;

        /// <summary>
        /// 构建实体
        /// </summary>
        /// <param name="world">世界</param>
        /// <param name="shape"></param>
        public GEntity(GWorld world, IGShape shape)
        {
            SetEntity(world, shape, TSVector2.zero, FP.Zero);
        }

        /// <summary>
        /// 构建实体
        /// </summary>
        /// <param name="world">世界</param>
        /// <param name="shape">形状</param>
        /// <param name="position">坐标</param>
        /// <param name="deg">角度</param>
        public GEntity(GWorld world, IGShape shape, TSVector2 position, FP deg)
        {
            SetEntity(world, shape, position, deg);
        }

        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="world">世界</param>
        /// <param name="shape">形状</param>
        /// <param name="position">坐标</param>
        /// <param name="deg">角度</param>
        public void SetEntity(GWorld world, IGShape shape, TSVector2 position, FP deg)
        {
            this.world = world;
            this.shape = shape;
            this.position = position;
            this.deg = deg;

            SetDirty();
        }

        /// <summary>
        /// 设置为脏，重新计算碰撞树
        /// </summary>
        public void SetDirty()
        {
            world.SetDirty(this);
        }

        /// <summary>
        /// 通知碰撞检测事件，监听就通知，不论是否变化
        /// </summary>
        public void NotifyCollision()
        {
            onCollision?.Invoke();
        }

        /// <summary>
        /// 碰撞事件更新就会通知，不论是否变化
        /// </summary>
        public void NotifyCollisionUpdate()
        {
            onCollisionUpdate?.Invoke();
        }
    }
}
