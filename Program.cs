using GoblinFramework.Physics.Collision;
using GoblinFramework.Physics.Shape;
using System.Collections.Generic;
using System.Numerics;
using TrueSync;

static void TestPointPoint()
{
    var p0 = new TSVector2(1, 1);
    var p1 = new TSVector2(1, 1);

    Console.WriteLine($"TestPointPoint -> {GCollisionTesting.Test(p0, p1)}");
}

static void TestPointLine()
{
    var p0 = new TSVector2(1, 1);
    var l0 = new GLine(new TSVector2(-10, 1), new TSVector2(10, 1));

    Console.WriteLine($"TestPointLine -> {GCollisionTesting.Test(p0, l0)}");
}

static void TestPointCircle()
{
    var p0 = new TSVector2(1, 1);
    var c0 = new GCircle(TSVector2.zero, 2);

    Console.WriteLine($"TestPointCircle -> {GCollisionTesting.Test(p0, c0)}");
}

static void TestPointPolygon()
{
    var p0 = new TSVector2(3, 3);
    var p1 = new GPolygon(p0, new List<TSVector2>
    {
        new TSVector2(0, 4),
        new TSVector2(4, 3),
        new TSVector2(0, 0),
    });

    Console.WriteLine($"TestPointPolygon -> {GCollisionTesting.Test(p0, p1)}");
}

TestPointPoint();
TestPointLine();
TestPointCircle();
TestPointPolygon();