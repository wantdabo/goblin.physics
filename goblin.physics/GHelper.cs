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
            return new GLine(line.begin + offset, line.end);
        }

        /// <summary>
        /// 将圆平移
        /// </summary>
        /// <param name="circle">圆</param>
        /// <param name="offset">平移值</param>
        /// <returns>平移后的线</returns>
        public static GCircle Translate(GCircle circle, TSVector2 offset)
        {
            return new GCircle(circle.center + offset, circle.radius);
        }

        /// <summary>
        /// 将多边形平移
        /// </summary>
        /// <param name="plygon">多边形</param>
        /// <param name="offset">平移值</param>
        /// <returns>平移后的线</returns>
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
        /// 将线缩放
        /// </summary>
        /// <param name="line">线</param>
        /// <param name="scale">缩放值</param>
        /// <returns>缩放后的线</returns>
        public static GLine Scale(GLine line, FP scale)
        {
            var halfDis = TSVector2.Distance(line.begin, line.end) * FP.Half;
            var center = (line.begin + line.end) * FP.Half;

            return new GLine((line.begin - center).normalized * halfDis * scale, (line.end - center).normalized * halfDis * scale);
        }

        /// <summary>
        /// 将圆缩放
        /// </summary>
        /// <param name="circle">圆</param>
        /// <param name="scale">缩放值</param>
        /// <returns>缩放后的圆</returns>
        public static GCircle Scale(GCircle circle, FP scale)
        {
            return new GCircle(circle.center, circle.radius * scale);
        }

        /// <summary>
        /// 将多边形缩放
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <param name="scale">缩放值</param>
        /// <returns>缩放后的多边形</returns>
        public static GPolygon Scale(GPolygon polygon, FP scale)
        {
            var lines = polygon.GetLines();
            for (int i = 0; i < lines.Length; i++) lines[i] = Scale(lines[i], scale);

            return new GPolygon(lines);
        }

        /// <summary>
        /// 计算线的圆形包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="deg">角度</param>
        /// <param name="scale">缩放</param>
        /// <param name="line">线</param>
        /// <returns>圆形包围盒</returns>
        public static GCircle CalcCircle(TSVector2 position, FP deg, FP scale, GLine line)
        {
            var tmp = Rotate(line, deg);
            var center = (tmp.begin + tmp.end) * FP.Half;
            var radius = TSVector2.Distance(tmp.end, tmp.end) * FP.Half;

            return Scale(new GCircle(position + center, radius), scale);
        }

        /// <summary>
        /// 计算圆形的圆形包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="deg">角度</param>
        /// <param name="scale">缩放</param>
        /// <param name="circle">圆形</param>
        /// <returns>圆形包围盒</returns>
        public static GCircle CalcCircle(TSVector2 position, FP deg, FP scale, GCircle circle)
        {
            return Scale(new GCircle(position + circle.center, circle.radius), scale);
        }

        /// <summary>
        /// 计算多边形的圆形包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="deg">角度</param>
        /// <param name="scale">缩放</param>
        /// <param name="polygon">多边形</param>
        /// <returns>圆形包围盒</returns>
        public static GCircle CalcCircle(TSVector2 position, FP deg, FP scale, GPolygon polygon)
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

            var point0 = new TSVector2(minX, maxY);
            TSVector2 center = new TSVector2((minX + maxX) * FP.Half, (minY + maxY) * FP.Half);

            return new GCircle(center, TSVector2.Distance(center, point0));
        }

        /// <summary>
        /// 计算形状列表的圆形包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="deg">角度</param>
        /// <param name="scale">缩放</param>
        /// <param name="shapes">形状列表</param>
        /// <returns>圆形包围盒</returns>
        public static GCircle CalcCircle(TSVector2 position, FP deg, FP scale, List<IGShape> shapes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 计算线的 AABB 包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="deg">角度</param>
        /// <param name="scale">缩放</param>
        /// <param name="line">线</param>
        /// <returns>AABB 包围盒</returns>
        public static GPolygon CalcAABB(TSVector2 position, FP deg, FP scale, GLine line)
        {
            var tmp = Rotate(line, deg);
            var minX = TSMath.Min(tmp.begin.x, tmp.end.x);
            var minY = TSMath.Min(tmp.begin.y, tmp.end.y);
            var maxX = TSMath.Max(tmp.begin.x, tmp.end.x);
            var maxY = TSMath.Max(tmp.begin.y, tmp.end.y);

            return Scale(new GPolygon(new List<TSVector2>
            {
                new TSVector2(minX, maxY) + position,
                new TSVector2(maxX, maxY) + position,
                new TSVector2(maxX, minY) + position,
                new TSVector2(minX, minY) + position
            }), scale);
        }

        /// <summary>
        /// 计算圆形的 AABB 包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="deg">角度</param>
        /// <param name="scale">缩放</param>
        /// <param name="circle">圆形</param>
        /// <returns>AABB 包围盒</returns>
        public static GPolygon CalcAABB(TSVector2 position, FP deg, FP scale, GCircle circle)
        {
            return Scale(new GPolygon(new List<TSVector2>
            {
                new TSVector2(-circle.radius, circle.radius) + position + circle.center,
                new TSVector2(circle.radius, circle.radius) + position + circle.center,
                new TSVector2(circle.radius, -circle.radius) + position + circle.center,
                new TSVector2(-circle.radius, -circle.radius) + position + circle.center,
            }), scale);
        }

        /// <summary>
        /// 计算多边形的 AABB 包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="deg">角度</param>
        /// <param name="scale">缩放</param>
        /// <param name="polygon">多边形</param>
        /// <returns>AABB 包围盒</returns>
        public static GPolygon CalcAABB(TSVector2 position, FP deg, FP scale, GPolygon polygon)
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

            return Scale(new GPolygon(new List<TSVector2>
            {
                new TSVector2(minX, maxY) + position,
                new TSVector2(maxX, maxY) + position,
                new TSVector2(maxX, minY) + position,
                new TSVector2(minX, minY) + position
            }), scale);
        }

        /// <summary>
        /// 计算形状列表的 AABB 包围盒
        /// </summary>
        /// <param name="position">偏移坐标</param>
        /// <param name="deg">角度</param>
        /// <param name="scale">缩放</param>
        /// <param name="shapes">形状列表</param>
        /// <returns>圆形包围盒</returns>
        public static GPolygon CalcAABB(TSVector2 position, FP deg, FP scale, List<IGShape> shapes)
        {
            throw new NotImplementedException();
        }
    }
}