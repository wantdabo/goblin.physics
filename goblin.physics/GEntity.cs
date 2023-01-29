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
                shape.Translate(mPosition);
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
        /// <summary>
        /// 碰撞更新的事件
        /// </summary>
        public event Action onCollision;

        /// <summary>
        /// 构建实体
        /// </summary>
        /// <param name="shape"></param>
        public GEntity(IGShape shape)
        {
            SetEntity(shape, TSVector2.zero, FP.Zero);
        }

        /// <summary>
        /// 构建实体
        /// </summary>
        /// <param name="shape">形状</param>
        /// <param name="position">坐标</param>
        /// <param name="deg">角度</param>
        public GEntity(IGShape shape, TSVector2 position, FP deg)
        {
            SetEntity(shape, position, deg);
        }

        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="shape">形状</param>
        /// <param name="position">坐标</param>
        /// <param name="deg">角度</param>
        public void SetEntity(IGShape shape, TSVector2 position, FP deg)
        {
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
    }
}
