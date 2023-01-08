using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace GoblinFramework.Physics.Shape
{
    /// <summary>
    /// 形状类型
    /// </summary>
    public enum GShapeType
    {
        /// <summary>
        /// 线
        /// </summary>
        GLine,
        /// <summary>
        /// 圆
        /// </summary>
        GCircle,
        /// <summary>
        /// 多边形
        /// </summary>
        GPolygon
    }

    /// <summary>
    /// 形状
    /// </summary>
    public interface IGShape
    {
        /// <summary>
        /// 形状类型
        /// </summary>
        public GShapeType type { get;}

        /// <summary>
        /// 平移
        /// </summary>
        /// <param name="position">平移值</param>
        /// <returns>平移后的形状</returns>
        public IGShape Translate(TSVector2 position);

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="deg">角度</param>
        /// <returns>旋转后的形状</returns>
        public IGShape Rotate(FP deg);

        /// <summary>
        /// 计算圆形包围盒
        /// </summary>
        /// <returns>圆形包围盒</returns>
        public GCircle CalcCircle();

        /// <summary>
        /// 计算 AABB 包围盒
        /// </summary>
        /// <returns>AABB 包围盒</returns>
        public GPolygon CalcAABB();
    }
}
