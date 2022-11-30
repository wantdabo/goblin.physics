using GoblinFramework.Physics.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将圆平移
        /// </summary>
        /// <param name="circle">圆</param>
        /// <param name="offset">平移值</param>
        /// <returns>平移后的线</returns>
        public static GCircle Translate(GCircle circle, TSVector2 offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将多边形平移
        /// </summary>
        /// <param name="plygon">多边形</param>
        /// <param name="offset">平移值</param>
        /// <returns>平移后的线</returns>
        public static GPolygon Translate(GPolygon polygon, TSVector2 offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将线旋转
        /// </summary>
        /// <param name="line">线</param>
        /// <param name="deg">角度</param>
        /// <returns>旋转后的线</returns>
        public static GLine Rotate(GLine line, FP deg)
        {
            var p0 = TSVector2.Rotate(line.p0, deg);
            var p1 = TSVector2.Rotate(line.p1, deg);

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
        /// 将线缩放
        /// </summary>
        /// <param name="line">线</param>
        /// <param name="scale">缩放值</param>
        /// <returns>缩放后的线</returns>
        public static GLine Scale(GLine line, FP scale) { throw new NotImplementedException(); }

        /// <summary>
        /// 将圆缩放
        /// </summary>
        /// <param name="circle">圆</param>
        /// <param name="scale">缩放值</param>
        /// <returns>缩放后的圆</returns>
        public static GCircle Scale(GCircle circle, FP scale) { throw new NotImplementedException(); }

        /// <summary>
        /// 将多边形缩放
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <param name="scale">缩放值</param>
        /// <returns>缩放后的多边形</returns>
        public static GCircle Scale(GPolygon polygon, FP scale) { throw new NotImplementedException(); }

        /// <summary>
        /// 计算线的圆形包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="line">线</param>
        /// <param name="deg">角度</param>
        /// <returns>圆形包围盒</returns>
        public static GCircle CalcCircle(TSVector2 position, GLine line, FP deg)
        {
            var tmp = Rotate(line, deg);
            var dire = tmp.p1 - tmp.p0;

            var radius = dire.magnitude * FP.Half;
            var center = dire.normalized * (dire.magnitude * radius);

            return new GCircle(position + center, radius);
        }

        /// <summary>
        /// 计算圆形的圆形包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="circle">圆形</param>
        /// <param name="deg">角度</param>
        /// <returns>圆形包围盒</returns>
        public static GCircle CalcCircle(TSVector2 position, GCircle circle, FP deg)
        {
            return new GCircle(position + circle.center, circle.radius);
        }

        /// <summary>
        /// 计算多边形的圆形包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="polygon">多边形</param>
        /// <param name="deg">角度</param>
        /// <returns>圆形包围盒</returns>
        public static GCircle CalcCircle(TSVector2 position, GPolygon polygon, FP deg)
        {
            var maxX = FP.MinValue;
            var maxY = FP.MinValue;
            foreach (var vertex in Rotate(polygon, deg).vertexes)
            {
                maxX = TSMath.Max(vertex.x, maxX);
                maxY = TSMath.Max(vertex.y, maxY);
            }

            var center = new TSVector2(maxX * FP.Half, maxY * FP.Half);

            return new GCircle(position + center, TSMath.Max(center.x, center.y));
        }

        /// <summary>
        /// 计算线的 AABB 包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="line">线</param>
        /// <param name="deg">角度</param>
        /// <returns>AABB 包围盒</returns>
        public static GPolygon CalcAABB(TSVector2 position, GLine line, FP deg)
        {
            var tmp = Rotate(line, deg);
            var minX = TSMath.Min(tmp.p0.x, tmp.p1.x);
            var minY = TSMath.Min(tmp.p0.y, tmp.p1.y);
            var maxX = TSMath.Max(tmp.p0.x, tmp.p1.x);
            var maxY = TSMath.Max(tmp.p0.y, tmp.p1.y);

            return new GPolygon(new List<TSVector2>
            {
                new TSVector2(minX, maxY) + position,
                new TSVector2(maxX, maxY) + position,
                new TSVector2(maxX, minY) + position,
                new TSVector2(minX, minY) + position
            });
        }

        /// <summary>
        /// 计算圆形的 AABB 包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="circle">圆形</param>
        /// <param name="deg">角度</param>
        /// <returns>AABB 包围盒</returns>
        public static GPolygon CalcAABB(TSVector2 position, GCircle circle, FP deg)
        {
            return new GPolygon(new List<TSVector2>
            {
                new TSVector2(-circle.radius, circle.radius) + position + circle.center,
                new TSVector2(circle.radius, circle.radius) + position + circle.center,
                new TSVector2(circle.radius, -circle.radius) + position + circle.center,
                new TSVector2(-circle.radius, -circle.radius) + position + circle.center,
            });
        }

        /// <summary>
        /// 计算多边形的 AABB 包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="polygon">多边形</param>
        /// <param name="deg">角度</param>
        /// <returns>AABB 包围盒</returns>
        public static GPolygon CalcAABB(TSVector2 position, GPolygon polygon, FP deg)
        {
            var minX = FP.MaxValue;
            var minY = FP.MaxValue;
            var maxX = FP.MinValue;
            var maxY = FP.MinValue;
            foreach (var vertex in Rotate(polygon, deg).vertexes)
            {
                minX = TSMath.Min(vertex.x, minX);
                minY = TSMath.Min(vertex.y, minY);
                maxX = TSMath.Max(vertex.x, maxX);
                maxY = TSMath.Max(vertex.y, maxY);
            }

            return new GPolygon(new List<TSVector2>
            {
                new TSVector2(minX, maxY) + position,
                new TSVector2(maxX, maxY) + position,
                new TSVector2(maxX, minY) + position,
                new TSVector2(minX, minY) + position
            });
        }
    }
}
