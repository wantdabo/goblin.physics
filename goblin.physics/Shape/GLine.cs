using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace GoblinFramework.Physics.Shape
{
    /// <summary>
    /// 线段
    /// </summary>
    public struct GLine : IGShape
    {
        /// <summary>
        /// 起点
        /// </summary>
        public TSVector2 p0 { get; private set; }

        /// <summary>
        /// 终点
        /// </summary>
        public TSVector2 p1 { get; private set; }

        /// <summary>
        /// 请使用有参的构造函数构造, GLine(TSVector2 p0, TSVector2 p1)
        /// </summary>
        /// <exception cref="Exception">请使用有参的构造函数构造, GLine(TSVector2 p0, TSVector2 p1)</exception>
        public GLine()
        {
            throw new Exception("请使用有参的构造函数构造, GLine(TSVector2 p0, TSVector2 p1)");
        }

        /// <summary>
        /// 线段构造
        /// </summary>
        /// <param name="p0">起点</param>
        /// <param name="p1">终点</param>
        public GLine(TSVector2 p0, TSVector2 p1)
        {
            this.p0 = p0;
            this.p1 = p1;
        }

        /// <summary>
        /// 获得法线
        /// </summary>
        /// <param name="normalize">是否归一化/标准化向量</param>
        /// <returns>法线</returns>
        public TSVector2 GetNormal(bool normalize = false)
        {
            var dire = p1 - p0;
            var normal = new TSVector2(-dire.y, dire.x);

            // 需要归一化
            if (normalize) return normal.normalized;

            return normal;
        }

        /// <summary>
        /// 获得平面
        /// </summary>
        /// <returns>平面</returns>
        public TSVector GetPlane()
        {
            var normal = GetNormal(true);

            return new TSVector(normal.x, normal.y, TSVector2.Dot(normal, p0));
        }

        public GCircle CalcCircle(TSVector2 position, FP deg)
        {
            return GHelper.CalcCircle(position, this, deg);
        }

        public GPolygon CalcAABB(TSVector2 position, FP deg)
        {
            return GHelper.CalcAABB(position, this, deg);
        }
    }
}
