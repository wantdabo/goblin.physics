using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace GoblinFramework.Physics.Shape
{
    /// <summary>
    /// 多边形
    /// </summary>
    public struct GPolygon : IGShape
    {
        /// <summary>
        /// 多边形的顶点，不得小于三个
        /// </summary>
        private List<TSVector2> vertexes;

        /// <summary>
        /// 请使用有参的构造函数构造, GPolygon(List<TSVector2> vertexes)
        /// </summary>
        /// <exception cref="Exception">请使用有参的构造函数构造, GPolygon(List<TSVector2> vertexes)</exception>
        public GPolygon() 
        {
            throw new Exception("请使用有参的构造函数构造, GPolygon(List<TSVector2> vertexes)");
        }

        /// <summary>
        /// 多边形构造
        /// </summary>
        /// <param name="vertexes">顶点列表，数量不得小于三</param>
        public GPolygon(List<TSVector2> vertexes) 
        {
            if (vertexes.Count < 3) throw new Exception("顶点列表，数量不得小于三");
            this.vertexes = vertexes;
        }

        /// <summary>
        /// 获得线段列表
        /// </summary>
        /// <returns>线段列表</returns>
        public GLine[] GetLines()
        {
            var vertexCount = vertexes.Count;
            GLine[] lines = new GLine[vertexCount];
            // 为了内存命中率，n - 1 的连续遍历采用高速计算线段，末端计算
            for (int i = 0; i < vertexCount - 1; i++) lines[i] = new GLine(vertexes[i], vertexes[i + 1]);

            // 最后一个顶点与第一个顶点的计算线段，末端分段计算
            lines[vertexCount - 1] = new GLine(vertexes[vertexCount - 1], vertexes[0]);

            return lines;
        }

        /// <summary>
        /// 获取法线列表
        /// </summary>
        /// <param name="normalize">是否归一化/标准化向量</param>
        /// <returns>多边形的法线列表</returns>
        public TSVector2[] GetNormals(bool normalize = false)
        {
            var lines = GetLines();
            TSVector2[] normals = new TSVector2[lines.Length];
            for (int i = 0; i < lines.Length; i++) normals[i] = lines[i].GetNormal(normalize);

            return normals;
        }

        /// <summary>
        /// 获取平面列表
        /// </summary>
        /// <returns></returns>
        public TSVector[] GetPlanes() 
        {
            var lines = GetLines();
            TSVector[] planes = new TSVector[lines.Length];
            for (int i = 0; i < lines.Length; i++) { 
                planes[i] = lines[i].GetPlane();
            }

            return planes;
        }

        public GCircle CalcCircle(TSVector2 position, FP degress)
        {
            throw new NotImplementedException();
        }

        public GPolygon CalcRect(TSVector2 position, FP degress)
        {
            throw new NotImplementedException();
        }
    }
}
