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
        #region Properties

        /// <summary>
        /// 实体 ID
        /// </summary>
        public uint entityId;

        private TSVector2 mPosition;
        /// <summary>
        /// 坐标
        /// </summary>
        public TSVector2 position { get { return mPosition; } set { mPosition = value; } }

        private FP mDegress;
        /// <summary>
        /// 角度（非弧度）
        /// </summary>
        public FP degress { get { return mDegress; } set { mDegress = value; } }

        private List<IGShape>? mShapes = null;
        /// <summary>
        /// 形状列表
        /// </summary>
        public List<IGShape>? shapes { get { return mShapes; } private set { mShapes = value; } }

        #endregion

        private GWorld? mWorld = null;
        public GWorld? world { get { return mWorld; } set { mWorld = value; } }

        #region 构造实体
        /// <summary>
        /// 构建实体
        /// </summary>
        /// <param name="position">坐标</param>
        /// <param name="shapes">形状列表</param>
        public GEntity(TSVector2 position, List<IGShape> shapes)
        {
            SetEntity(position, shapes);
        }

        /// <summary>
        /// 构建实体
        /// </summary>
        /// <param name="position">坐标</param>
        /// <param name="degress">角度</param>
        /// <param name="shapes">形状列表</param>
        public GEntity(TSVector2 position, FP degress, List<IGShape> shapes)
        {
            SetEntity(position, degress, shapes);
        }

        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="position">坐标</param>
        /// <param name="shapes">形状列表</param>
        /// <param name="entityType">实体类型</param>
        public void SetEntity(TSVector2 position, List<IGShape> shapes)
        {
            SetEntity(position, FP.Zero, shapes);
        }

        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="position">坐标</param>
        /// <param name="degress">角度</param>
        /// <param name="shapes">形状列表</param>
        public void SetEntity(TSVector2 position, FP degress, List<IGShape> shapes)
        {
            this.mPosition = position;
            this.mDegress = degress;
            this.mShapes = shapes;
        }
        #endregion
    }
}
