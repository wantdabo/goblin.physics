using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace GoblinFramework.Physics.Shape
{
    /// <summary>
    /// 形状
    /// </summary>
    public interface IGShape 
    {
        /// <summary>
        /// 计算圆形包围盒
        /// </summary>
        /// <param name="position">坐标</param>
        /// <param name="degress">角度</param>
        GCircle CalcCircle(TSVector2 position, FP degress);

        /// <summary>
        /// 计算矩形包围盒（AABB）
        /// </summary>
        /// <param name="position">坐标</param>
        /// <param name="degress">角度</param>
        GPolygon CalcRect(TSVector2 position, FP degress);
    }
}
