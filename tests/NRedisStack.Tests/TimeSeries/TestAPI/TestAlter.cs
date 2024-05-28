﻿using NRedisStack.DataTypes;
using NRedisStack.Literals.Enums;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using Xunit;

namespace NRedisStack.Tests.TimeSeries.TestAPI
{
    public class TestAlter : AbstractNRedisStackTest, IDisposable
    {
        private readonly string key = "ALTER_TESTS";

        public TestAlter(RedisFixture redisFixture) : base(redisFixture) { }


        [Fact]
        [Obsolete]
        public void TestAlterRetentionTime()
        {
            long retentionTime = 5000;
            IDatabase db = redisFixture.Redis.GetDatabase();
            db.Execute("FLUSHALL");
            var ts = db.TS();
            ts.Create(key);
            Assert.True(ts.Alter(key, retentionTime: retentionTime));
            TimeSeriesInformation info = ts.Info(key);
            Assert.Equal(retentionTime, info.RetentionTime);
        }

        [Fact]
        [Obsolete]
        public void TestAlterLabels()
        {
            TimeSeriesLabel label = new TimeSeriesLabel("key", "value");
            var labels = new List<TimeSeriesLabel> { label };
            IDatabase db = redisFixture.Redis.GetDatabase();
            db.Execute("FLUSHALL");
            var ts = db.TS();
            ts.Create(key);
            Assert.True(ts.Alter(key, labels: labels));
            TimeSeriesInformation info = ts.Info(key);
            Assert.Equal(labels, info.Labels);
            labels.Clear();
            Assert.True(ts.Alter(key, labels: labels));
            info = ts.Info(key);
            Assert.Equal(labels, info.Labels);
        }

        [Fact]
        [Obsolete]
        public void TestAlterPolicyAndChunk()
        {
            IDatabase db = redisFixture.Redis.GetDatabase();
            db.Execute("FLUSHALL");
            var ts = db.TS();
            ts.Create(key);
            Assert.True(ts.Alter(key, chunkSizeBytes: 128, duplicatePolicy: TsDuplicatePolicy.MIN));
            TimeSeriesInformation info = ts.Info(key);
            Assert.Equal(128, info.ChunkSize);
            Assert.Equal(TsDuplicatePolicy.MIN, info.DuplicatePolicy);
        }

        [Fact]
        public void TestAlterAndIgnoreValues()
        {
            IDatabase db = redisFixture.Redis.GetDatabase();
            db.Execute("FLUSHALL");
            var ts = db.TS();
            ts.Create(key, new TsCreateParamsBuilder().build());
            var parameters = new TsAlterParamsBuilder().AddIgnoreValues(13, 14).build();
            Assert.True(ts.Alter(key, parameters));

            int j = -1, k = -1;
            RedisResult info = TimeSeriesHelper.getInfo(db, key, out j, out k);
            Assert.NotNull(info);
            Assert.True(info.Length > 0);
            Assert.NotEqual(j, -1);
            Assert.NotEqual(k, -1);
            Assert.Equal(13, (long)info[j + 1]);
            Assert.Equal(14, (long)info[k + 1]);
        }
    }
}
