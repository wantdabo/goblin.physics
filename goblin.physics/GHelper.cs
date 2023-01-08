using GoblinFramework.Physics.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace GoblinFramework.Physics
{
    public class GHelper
    {
        /// <summary>
        /// 将线平移
        /// </summary>
        /// <param name="line">线</param>
        /// <param name="offset">平移值</param>
        /// <returns>平移后的线</returns>
        public static GLine Translate(GLine line, TSVector2 offset)
        {
            return new GLine(line.begin + offset, line.end + offset);
        }

        /// <summary>
        /// 将圆平移
        /// </summary>
        /// <param name="circle">圆</param>
        /// <param name="offset">平移值</param>
        /// <returns>平移后的圆</returns>
        public static GCircle Translate(GCircle circle, TSVector2 offset)
        {
            return new GCircle(circle.center + offset, circle.radius);
        }

        /// <summary>
        /// 将多边形平移
        /// </summary>
        /// <param name="plygon">多边形</param>
        /// <param name="offset">平移值</param>
        /// <returns>平移后的多边形</returns>
        public static GPolygon Translate(GPolygon polygon, TSVector2 offset)
        {
            List<TSVector2> vertexes = new List<TSVector2>();
            foreach (var vertex in polygon.vertexes) vertexes.Add(vertex + offset);

            return new GPolygon(vertexes);
        }

        /// <summary>
        /// 将线旋转
        /// </summary>
        /// <param name="line">线</param>
        /// <param name="deg">角度</param>
        /// <returns>旋转后的线</returns>
        public static GLine Rotate(GLine line, FP deg)
        {
            var p0 = TSVector2.Rotate(line.begin, deg);
            var p1 = TSVector2.Rotate(line.end, deg);

            return new GLine(p0, p1);
        }

        /// <summary>
        /// 将多边形旋转
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <param name="deg">角度</param>
        /// <returns>旋转后的多边形</returns>
        public static GPolygon Rotate(GPolygon polygon, FP deg)
        {
            List<TSVector2> vertexes = new List<TSVector2>();
            foreach (var vertex in polygon.vertexes) vertexes.Add(TSVector2.Rotate(vertex, deg));

            return new GPolygon(vertexes);
        }

        /// <summary>
        /// 计算线的圆形包围盒
        /// </summary>
        /// <param name="line">线</param>
        /// <returns>圆形包围盒</returns>
        public static GCircle CalcCircle(GLine line)
        {
            var center = (line.begin + line.end) * FP.Half;
            var radius = TSVector2.Distance(line.end, line.end) * FP.Half;

            return new GCircle(center, radius);
        }

        /// <summary>
        /// 计算圆形的圆形包围盒
        /// </summary>
        /// <param name="circle">圆形</param>
        /// <returns>圆形包围盒</returns>
        public static GCircle CalcCircle(GCircle circle)
        {
            return circle;
        }

        /// <summary>
        /// 计算多边形的圆形包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <returns>圆形包围盒</returns>
        public static GCircle CalcCircle(GPolygon polygon)
        {
            var minX = FP.MaxValue;
            var minY = FP.MaxValue;
            var maxX = FP.MinValue;
            var maxY = FP.MinValue;
            foreach (var vertex in polygon.vertexes)
            {
                minX = TSMath.Min(vertex.x, minX);
                minY = TSMath.Min(vertex.y, minY);
                maxX = TSMath.Max(vertex.x, maxX);
                maxY = TSMath.Max(vertex.y, maxY);
            }

            var point0 = new TSVector2(minX, maxY);
            TSVector2 center = new TSVector2((minX + maxX) * FP.Half, (minY + maxY) * FP.Half);

            return new GCircle(center, TSVector2.Distance(center, point0));
        }

        /// <summary>
        /// 计算线的 AABB 包围盒
        /// </summary>
        /// <param name="line">线</param>
        /// <returns>AABB 包围盒</returns>
        public static GPolygon CalcAABB(GLine line)
        {
            var minX = TSMath.Min(line.begin.x, line.end.x);
            var minY = TSMath.Min(line.begin.y, line.end.y);
            var maxX = TSMath.Max(line.begin.x, line.end.x);
            var maxY = TSMath.Max(line.begin.y, line.end.y);

            return new GPolygon(new List<TSVector2>
            {
                new TSVector2(minX, maxY),
                new TSVector2(maxX, maxY),
                new TSVector2(maxX, minY),
                new TSVector2(minX, minY)
            });
        }

        /// <summary>
        /// 计算圆形的 AABB 包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="deg">角度</param>
        /// <param name="circle">圆形</param>
        /// <returns>AABB 包围盒</returns>
        public static GPolygon CalcAABB(GCircle circle)
        {
            return new GPolygon(new List<TSVector2>
            {
                new TSVector2(-circle.radius, circle.radius) + circle.center,
                new TSVector2(circle.radius, circle.radius) + circle.center,
                new TSVector2(circle.radius, -circle.radius) + circle.center,
                new TSVector2(-circle.radius, -circle.radius) + circle.center,
            });
        }

        /// <summary>
        /// 计算多边形的 AABB 包围盒
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <returns>AABB 包围盒</returns>
        public static GPolygon CalcAABB(GPolygon polygon)
        {
            var minX = FP.MaxValue;
            var minY = FP.MaxValue;
            var maxX = FP.MinValue;
            var maxY = FP.MinValue;
            foreach (var vertex in polygon.vertexes)
            {
                minX = TSMath.Min(vertex.x, minX);
                minY = TSMath.Min(vertex.y, minY);
                maxX = TSMath.Max(vertex.x, maxX);
                maxY = TSMath.Max(vertex.y, maxY);
            }

            return new GPolygon(new List<TSVector2>
            {
                new TSVector2(minX, maxY),
                new TSVector2(maxX, maxY),
                new TSVector2(maxX, minY),
                new TSVector2(minX, minY)
            });
        }
    }
}