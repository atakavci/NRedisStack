// EXAMPLE: bitmap_tutorial
// HIDE_START

using NRedisStack.Tests;
using StackExchange.Redis;

// HIDE_END

// REMOVE_START
namespace Doc;
[Collection("DocsTests")]
// REMOVE_END

// HIDE_START
public class Bitmap_tutorial : AbstractNRedisStackTest, IDisposable
{
    public Bitmap_tutorial(EndpointsFixture fixture) : base(fixture) { }
  
    [SkippableTheory]
    [MemberData(nameof(EndpointsFixture.Env.StandaloneOnly), MemberType = typeof(EndpointsFixture.Env))]
    public void run(string endpointId)
    {
        var db = GetCleanDatabase(endpointId);
        //REMOVE_START
        // Clear any keys here before using them in tests.
        db.KeyDelete("pings:2024-01-01-00:00");
        //REMOVE_END
        // HIDE_END


        // STEP_START ping
        bool res1 = db.StringSetBit("pings:2024-01-01-00:00", 123, true);
        Console.WriteLine(res1);    // >>> 0

        bool res2 = db.StringGetBit("pings:2024-01-01-00:00", 123);
        Console.WriteLine(res2);    // >>> True

        bool res3 = db.StringGetBit("pings:2024-01-01-00:00", 456);
        Console.WriteLine(res3);    // >>> False
        // STEP_END

        // Tests for 'ping' step.
        // REMOVE_START
        Assert.False(res1);
        Assert.True(res2);
        Assert.False(res3);
        // REMOVE_END


        // STEP_START bitcount
        bool res4 = db.StringSetBit("pings:2024-01-01-00:00", 123, true);
        long res5 = db.StringBitCount("pings:2024-01-01-00:00");
        Console.WriteLine(res5);    // >>> 1
        // STEP_END

        // Tests for 'bitcount' step.
        // REMOVE_START
        Assert.True(res4);
        Assert.Equal(1, res5);
        // REMOVE_END


        // HIDE_START
    }
}
// HIDE_END

