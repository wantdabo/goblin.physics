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

        private List<IGShape>? mShapes = null;
        /// <summary>
        /// 形状列表
        /// </summary>
        public List<IGShape>? shapes
        {
            get { return mShapes; }
            set
            {
                mShapes = value;
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
                SetDirty();
            }
        }

        private FP mDeg;
        /// <summary>
        /// 角度（非弧度）
        /// </summary>
        public FP deg
        {
            get { return mDeg; }
            set
            {
                mDeg = value;
                SetDirty();
            }
        }

        private GWorld? mWorld = null;
        public GWorld? world { get { return mWorld; } set { mWorld = value; } }

        /// <summary>
        /// 构建实体
        /// </summary>
        /// <param name="position">坐标</param>
        /// <param name="deg">角度</param>
        /// <param name="shapes">形状列表</param>
        public GEntity(List<IGShape> shapes, TSVector2 position, FP deg)
        {
            SetEntity(shapes, position, deg);
        }

        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="position">坐标</param>
        /// <param name="deg">角度</param>
        /// <param name="shapes">形状列表</param>
        public void SetEntity(List<IGShape> shapes, TSVector2 position, FP deg)
        {
            this.position = position;
            this.deg = deg;
            this.shapes = shapes;
            SetDirty();
        }

        /// <summary>
        /// 设置为脏，重新计算碰撞树
        /// </summary>
        public void SetDirty() 
        {
            world?.SetDirty(this);
        }
    }
}
