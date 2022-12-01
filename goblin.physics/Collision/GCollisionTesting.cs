using GoblinFramework.Physics.Entity;
using GoblinFramework.Physics.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace GoblinFramework.Physics.Collision
{
    /// <summary>
    /// 碰撞检测
    /// </summary>
    public static class GCollisionTesting
    {
        /// <summary>
        /// 测试两点碰撞
        /// </summary>
        /// <param name="point0">点 1</param>
        /// <param name="point1">点 2</param>
        /// <returns>是否碰撞</returns>
        public static bool Test(TSVector2 point0, TSVector2 point1)
        {
            return point0 == point1;
        }

        /// <summary>
        /// 测试点与线的碰撞
        /// </summary>
        /// <param name="point0">点 1</param>
        /// <param name="line0">线 1</param>
        /// <returns>是否碰撞</returns>
        public static bool Test(TSVector2 point0, GLine line0)
        {
            var lineLen = TSVector2.Distance(line0.begin, line0.end);
            var dis0 = TSVector2.Distance(point0, line0.begin);
            var dis1 = TSVector2.Distance(point0, line0.end);

            // 应该相等才是相撞，避免计算误差，降低精度 FP.EN1(0.1f)
            return TSMath.Abs((dis0 + dis1) - lineLen) < FP.EN1;
        }

        /// <summary>
        /// 测试点与圆的碰撞
        /// </summary>
        /// <param name="point0">点 1</param>
        /// <param name="circle0">圆 1</param>
        /// <returns>是否碰撞</returns>
        public static bool Test(TSVector2 point0, GCircle circle0)
        {
            return TSVector2.Distance(point0, circle0.center) <= circle0.radius;
        }

        /// <summary>
        /// 测试点与多边形的碰撞检测
        /// </summary>
        /// <param name="point0">点 1</param>
        /// <param name="polygon0">多边形 1</param>
        /// <returns>是否碰撞</returns>
        public static bool Test(TSVector2 point0, GPolygon polygon0)
        {
            // from https://docs.godotengine.org/zh_CN/latest/tutorials/math/vectors_advanced.html#distance-to-plane
            var planes = polygon0.GetPlanes();
            for (int i = 0; i < planes.Length; i++)
            {
                var plane = planes[i];
                // side > 0 表示该点位于平面的外部，反之则是内部。此处凸面多边形，如果点在任意一条平面的外部，表示未碰撞。
                var side = TSVector2.Dot(point0, plane.ToTSVector2()) - plane.z;
                if (side > 0) return false;
            }

            return true;
        }

        /// <summary>
        /// 测试线与线的碰撞
        /// </summary>
        /// <param name="line0">线 1</param>
        /// <param name="line1">线 2</param>
        /// <returns>是否碰撞</returns>
        public static bool Test(GLine line0, GLine line1)
        {
            // from https://github.com/XXHolic/blog/issues/61#situation3
            // (x1, y1) 线1的一个端点
            // (x2, y2) 线1的另一个端点
            // (x3, y3) 线2的一个端点
            // (x4, y4) 线2的另一个端点
            FP x1 = line0.begin.x; FP y1 = line0.begin.y;
            FP x2 = line0.end.x; FP y2 = line0.end.y;
            FP x3 = line1.begin.x; FP y3 = line1.begin.y;
            FP x4 = line1.end.x; FP y4 = line1.end.y;

            FP t1 = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            FP t2 = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            return t1 >= 0 && t1 <= 1 && t2 >= 0 && t2 <= 1;
        }

        /// <summary>
        /// 检测线与圆的碰撞
        /// </summary>
        /// <param name="line0">线 1</param>
        /// <param name="circle0">圆 1</param>
        /// <returns>是否相撞</returns>
        public static bool Test(GLine line0, GCircle circle0)
        {
            // 快速检测，端点是否在圆内
            if (Test(line0.begin, circle0) || Test(line0.end, circle0)) return true;

            // from https://github.com/XXHolic/blog/issues/61#situation3
            // (x1, y1) 线的一个端点
            // (x2, y2) 线的另一个端点
            // (px, py) 圆心的坐标
            // radius   圆的半径
            FP x1 = line0.begin.x; FP y1 = line0.begin.y;
            FP x2 = line0.end.x; FP y2 = line0.end.y;
            FP cx = circle0.center.x; FP cy = circle0.center.y;
            FP radius = circle0.radius;

            FP pointVectorX = x1 - x2;
            FP pointVectorY = y1 - y2;
            FP t = (pointVectorX * (cx - x1) + pointVectorY * (cy - y1)) / (pointVectorX * pointVectorX + pointVectorY * pointVectorY);
            FP closestX = x1 + t * pointVectorX;
            FP closestY = y1 + t * pointVectorY;

            if (false == Test(new TSVector2(closestX, closestY), line0)) return false;

            FP distX = closestX - cx;
            FP distY = closestY - cy;
            FP distance = TSMath.Sqrt((distX * distX) + (distY * distY));

            return distance <= radius;
        }

        /// <summary>
        /// 测试线与多边形的碰撞
        /// </summary>
        /// <param name="line0">线 1</param>
        /// <param name="polygon0">多边形 1</param>
        /// <returns>是否相撞</returns>
        public static bool Test(GLine line0, GPolygon polygon0)
        {
            // 检测线的端点是否在多边形内
            if (Test(line0.begin, polygon0) || Test(line0.end, polygon0)) return true;

            var lines = polygon0.GetLines();
            for (int i = 0; i < lines.Length; i++) if (Test(line0, lines[i])) return true;

            return false;
        }

        /// <summary>
        /// 测试圆与圆的碰撞检测
        /// </summary>
        /// <param name="circle0">圆 1</param>
        /// <param name="circle1">圆 2</param>
        /// <returns>是否碰撞</returns>
        public static bool Test(GCircle circle0, GCircle circle1)
        {
            return TSVector2.Distance(circle0.center, circle1.center) <= circle0.radius + circle1.radius;
        }

        /// <summary>
        /// 测试圆与多边形的碰撞检测
        /// </summary>
        /// <param name="circle0">圆 1</param>
        /// <param name="polygon0">多边形 1</param>
        /// <returns>是否碰撞</returns>
        public static bool Test(GCircle circle0, GPolygon polygon0)
        {
            var lines = polygon0.GetLines();
            for (int i = 0; i < lines.Length; i++) if (Test(lines[i], circle0)) return true;

            return false;
        }

        /// <summary>
        /// 测试多边形与多边形的碰撞检测
        /// </summary>
        /// <param name="polygon0">多边形 1</param>
        /// <param name="polygon1">多边形 2</param>
        /// <returns>是否碰撞</returns>
        public static bool Test(GPolygon polygon0, GPolygon polygon1)
        {
            var lines0 = polygon0.GetLines();
            for (int i = 0; i < lines0.Length; i++) if (Test(lines0[i], polygon1)) return true;

            var lines1 = polygon1.GetLines();
            for (int i = 0; i < lines1.Length; i++) if (Test(lines1[i], polygon0)) return true;

            return false;
        }

        /// <summary>
        /// 测试实体与实体的碰撞检测
        /// </summary>
        /// <param name="entity0">实体 1</param>
        /// <param name="entity1">实体 2</param>
        /// <returns>是否相撞</returns>
        public static bool Test(GEntity entity0, GEntity entity1) { throw new NotImplementedException(); }

        /// <summary>
        /// 测试实体与点的碰撞检测
        /// </summary>
        /// <param name="entity0">实体 1</param>
        /// <param name="point0">点 1</param>
        /// <returns>是否相撞</returns>
        public static bool Test(GEntity entity0, TSVector2 point0) { throw new NotImplementedException(); }

        /// <summary>
        /// 测试实体与线的碰撞检测
        /// </summary>
        /// <param name="entity0">实体 1</param>
        /// <param name="line0">线 1</param>
        /// <returns>是否相撞</returns>
        public static bool Test(GEntity entity0, GLine line0) { throw new NotImplementedException(); }

        /// <summary>
        /// 测试实体与圆的碰撞检测
        /// </summary>
        /// <param name="entity0">实体 1</param>
        /// <param name="circle0">圆 1</param>
        /// <returns>是否相撞</returns>
        public static bool Test(GEntity entity0, GCircle circle0) { throw new NotImplementedException(); }

        /// <summary>
        /// 测试实体与多边形的碰撞检测
        /// </summary>
        /// <param name="entity0">实体 1</param>
        /// <param name="polygon0">多边形 1</param>
        /// <returns>是否相撞</returns>
        public static bool Test(GEntity entity0, GPolygon polygon0) { throw new NotImplementedException(); }
    }
}
