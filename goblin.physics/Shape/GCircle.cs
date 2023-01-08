using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace GoblinFramework.Physics.Shape
{
    /// <summary>
    /// 圆形
    /// </summary>
    public struct GCircle : IGShape
    {
        /// <summary>
        /// 圆心
        /// </summary>
        public TSVector2 center { get; private set; }

        /// <summary>
        /// 半径
        /// </summary>
        public FP radius { get; private set; }

        /// <summary>
        /// 请使用有参的构造函数构造, GCircle(TSVector2 center, FP radius)
        /// </summary>
        /// <exception cref="Exception">请使用有参的构造函数构造, GCircle(TSVector2 center, FP radius)</exception>
        public GCircle()
        {
            throw new Exception("请使用有参的构造函数构造, GCircle(TSVector2 center, FP radius)");
        }

        /// <summary>
        /// 圆形构造
        /// </summary>
        /// <param name="center">圆心</param>
        /// <param name="radius">半径</param>
        public GCircle(TSVector2 center, FP radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public GShapeType type { get { return GShapeType.GCircle; } }

        public IGShape Translate(TSVector2 position)
        {
            return GHelper.Translate(this, position);
        }

        public IGShape Rotate(FP deg)
        {
            return this;
        }

        public GCircle CalcCircle()
        {
            return GHelper.CalcCircle(this);
        }

        public GPolygon CalcAABB()
        {
            return GHelper.CalcAABB(this);
        }
    }
}
