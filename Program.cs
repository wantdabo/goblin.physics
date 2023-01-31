using GoblinFramework.Physics;
using GoblinFramework.Physics.Collision;
using GoblinFramework.Physics.Shape;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using TrueSync;

static void TestPointPoint()
{
    var p0 = new TSVector2(1, 1);
    //var p0 = new TSVector2(1, 2);
    var p1 = new TSVector2(1, 1);

    //Console.WriteLine($"TestPointPoint -> {GCollisionTesting.Test(p0, p1)}");
}

static void TestPointLine()
{
    var p0 = new TSVector2(1, 1);
    //var p0 = new TSVector2(1, 10);
    var l0 = new GLine(new TSVector2(-10, 1), new TSVector2(10, 1));

    //Console.WriteLine($"TestPointLine -> {GCollisionTesting.Test(p0, l0)}");
}

static void TestPointCircle()
{
    var p0 = new TSVector2(1, 1);
    //var p0 = new TSVector2(9, 1);
    var c0 = new GCircle(TSVector2.zero, 2);

    //Console.WriteLine($"TestPointCircle -> {GCollisionTesting.Test(p0, c0)}");
}

static void TestPointPolygon()
{
    var p0 = new TSVector2(3, 3);
    //var p0 = new TSVector2(-3, 3);
    var p1 = new GPolygon(new List<TSVector2>
    {
        new TSVector2(0, 4),
        new TSVector2(4, 3),
        new TSVector2(0, 0),
    });

    //Console.WriteLine($"TestPointPolygon -> {GCollisionTesting.Test(p0, p1)}");
}

static void TestLineLine() 
{
    var l0 = new GLine(new TSVector2(-1, 0), new TSVector2(1, 0));
    //var l0 = new GLine(new TSVector2(-2, 0), new TSVector2(2, 0));
    var l1 = new GLine(new TSVector2(0, 1), new TSVector2(0, -1));

    //Console.WriteLine($"TestLineLine -> {GCollisionTesting.Test(l0, l1)}");
}

static void TestLineCircle()
{
    var l0 = new GLine(new TSVector2(-2, -2), new TSVector2(2, 2));
    //var l0 = new GLine(new TSVector2(10, 10), new TSVector2(20, 20));
    var c0 = new GCircle(TSVector2.zero, 2);

    //Console.WriteLine($"TestLineCircle -> {GCollisionTesting.Test(l0, c0)}");
}

static void TestLinePolygon()
{
    var l0 = new GLine(new TSVector2(1, 2), new TSVector2(2, 3));
    //var l0 = new GLine(new TSVector2(-2, -2), new TSVector2(2, 2));
    //var l0 = new GLine(new TSVector2(-2, -2), new TSVector2(2, 1));
    var p0 = new GPolygon(new List<TSVector2>
    {
        new TSVector2(0, 4),
        new TSVector2(4, 3),
        new TSVector2(0, 0),
    });

    //Console.WriteLine($"TestLinePolygon -> {GCollisionTesting.Test(l0, p0)}");
}

static void TestCircleCircle()
{
    var c0 = new GCircle(TSVector2.zero, 2);
    //var c0 = new GCircle(TSVector2.zero, FP.Half);
    //var c0 = new GCircle(TSVector2.one * 4, 2);
    var c1 = new GCircle(TSVector2.zero, 2);

    //Console.WriteLine($"TestCircleCircle -> {GCollisionTesting.Test(c0, c1)}");
}

static void TestCirclePolygon()
{
    var c0 = new GCircle(new TSVector2(1, 2), 1);
    //var c0 = new GCircle(new TSVector2(-1, 0), 1);
    //var c0 = new GCircle(new TSVector2(-2, 0), 10);
    //var c0 = new GCircle(new TSVector2(-2, 0), 1);
    var p0 = new GPolygon(new List<TSVector2>
    {
        new TSVector2(0, 4),
        new TSVector2(4, 3),
        new TSVector2(0, 0),
    });

    //Console.WriteLine($"TestCirclePolygon -> {GCollisionTesting.Test(c0, p0)}");
}

static void TestPolygonPolygon()
{
    var p0 = new GPolygon(new List<TSVector2>
    {
        new TSVector2(1, 2),
        new TSVector2(3, 4),
        new TSVector2(3, 2),
    });

    //var p0 = new GPolygon(TSVector2.zero, new List<TSVector2>
    //{
    //    new TSVector2(-5, -1),
    //    new TSVector2(2, 11),
    //    new TSVector2(9, 0),
    //});

    //var p0 = new GPolygon(TSVector2.zero, new List<TSVector2>
    //{
    //    new TSVector2(-5, -1),
    //    new TSVector2(4, 7),
    //    new TSVector2(9, 0),
    //});

    //var p0 = new GPolygon(TSVector2.zero, new List<TSVector2>
    //{
    //    new TSVector2(-3, 1),
    //    new TSVector2(-1, 3),
    //    new TSVector2(-1, 1),
    //});

    var p1 = new GPolygon(new List<TSVector2>
    {
        new TSVector2(0, 6),
        new TSVector2(6, 3),
        new TSVector2(0, 0),
    });

    //Console.WriteLine($"TestPolygonPolygon -> {GCollisionTesting.Test(p0, p1)}");
}

static long StopwatchInvoke(int count, Action action) 
{
    var stopwatch = Stopwatch.StartNew();
    stopwatch.Start();
    for (int i = 0; i < count; i++) action.Invoke();
    stopwatch.Stop();
    Console.WriteLine($"ms {stopwatch.ElapsedMilliseconds} \t {action.Method.Name}");

    return stopwatch.ElapsedMilliseconds;
}

static void StressTesting()
{
    /// <summary>
    /// 一百万次碰撞检测
    /// </summary>
    int count = 100 * 10000;
    StopwatchInvoke(count, TestPointPoint);
    StopwatchInvoke(count, TestPointLine);
    StopwatchInvoke(count, TestPointCircle);
    StopwatchInvoke(count, TestPointPolygon);
    StopwatchInvoke(count, TestLineLine);
    StopwatchInvoke(count, TestLineCircle);
    StopwatchInvoke(count, TestLinePolygon);
    StopwatchInvoke(count, TestCircleCircle);
    StopwatchInvoke(count, TestCirclePolygon);
    StopwatchInvoke(count, TestPolygonPolygon);

    //ms 14     << Main >$> g__TestPointPoint | 0_0
    //ms 36     << Main >$> g__TestPointLine | 0_1
    //ms 20     << Main >$> g__TestPointCircle | 0_2
    //ms 76     << Main >$> g__TestPointPolygon | 0_3
    //ms 48     << Main >$> g__TestLineLine | 0_4
    //ms 33     << Main >$> g__TestLineCircle | 0_5
    //ms 80     << Main >$> g__TestLinePolygon | 0_6
    //ms 23     << Main >$> g__TestCircleCircle | 0_7
    //ms 73     << Main >$> g__TestCirclePolygon | 0_8
    //ms 108    << Main >$> g__TestPolygonPolygon | 0_9
}

static void WorldEntityTesting()
{
    GWorld world = new GWorld();

    var player0 = world.BornEntity(new GCircle(TSVector2.zero, 5));
    player0.onCollisionUpdate += () =>
    {
        Console.WriteLine(player0.collisions.Count > 0 ? "Player0 碰撞" : "Player0 未碰撞");
    };

    var player1 = world.BornEntity(new GCircle(TSVector2.zero, 5));
    player1.onCollisionUpdate += () =>
    {
        Console.WriteLine(player1.collisions.Count > 0 ? "Player1 碰撞" : "Player1 未碰撞");
    };

    var player2 = world.BornEntity(new GCircle(TSVector2.zero, 5));
    player2.position = new TSVector2(20, 0);
    player2.onCollisionUpdate += () =>
    {
        Console.WriteLine(player2.collisions.Count > 0 ? "Player2 碰撞" : "Player2 未碰撞");
    };

    while (true)
    {
        Thread.Sleep(1000);
        world.Update();
        player1.position += TSVector2.right * 5;

        Console.WriteLine("---------");
    }
}

//StressTesting();
WorldEntityTesting();

Console.ReadKey();