﻿using System;
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
        /// 计算圆形包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="polygon">多边形</param>
        /// <param name="deg">角度</param>
        /// <returns>圆形包围盒</returns>
        public GCircle CalcCircle(TSVector2 position, FP deg, FP scale);

        /// <summary>
        /// 计算 AABB 包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="deg">角度</param>
        /// <returns>AABB 包围盒</returns>
        public GPolygon CalcAABB(TSVector2 position, FP deg, FP scale);
    }
}
