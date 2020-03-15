using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using DNI.Core.Services;
using DNI.Core.UnitTests.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.UnitTests
{
    public class DistributedCacheServiceTests
    {
        private DefaultDistributedCacheService sut;
        private Mock<IDistributedCache> distributedCacheMock;
        private Mock<IMessagePackService> messagePackServiceMock;
        private Mock<IOptions<DistributedCacheEntryOptions>> distributedCacheEntryOptionsMock;
        private Mock<ICacheEntryTracker> cacheEntryTrackerMock;

        [SetUp]
        public void SetUp()
        {
            distributedCacheMock = new Mock<IDistributedCache>();
            messagePackServiceMock = new Mock<IMessagePackService>();
            distributedCacheEntryOptionsMock = new Mock<IOptions<DistributedCacheEntryOptions>>();
            cacheEntryTrackerMock = new Mock<ICacheEntryTracker>();
            distributedCacheEntryOptionsMock.Setup(m => 
                m.Value)
                .Returns(new DistributedCacheEntryOptions());

            sut = new DefaultDistributedCacheService(distributedCacheMock.Object, cacheEntryTrackerMock.Object,
                messagePackServiceMock.Object, distributedCacheEntryOptionsMock.Object);
        } 

        [Test]
        public async Task Get_when_not_null_and_cache_state_is_valid_calls_deserialize()
        {
            var cancellationToken = CancellationToken.None;
            var testCacheModel = new TestCacheModel();

            var cachedByteResults = new byte[32];

            Array.Fill<byte>(cachedByteResults, 255);

            distributedCacheMock
                .Setup(distributedCache => distributedCache.GetAsync(nameof(TestCacheModel), cancellationToken))
                .Returns(Task.FromResult(cachedByteResults)).Verifiable();

            messagePackServiceMock.Setup(messagePackService => messagePackService
                .Deserialise<TestCacheModel>(cachedByteResults, sut._messagePackOptions))
                .Returns(Task.FromResult(testCacheModel))
                .Verifiable();

            cacheEntryTrackerMock.Setup(cacheEntryTrackerMock => cacheEntryTrackerMock
                .GetState(It.IsAny<string>(), cancellationToken))
                .Returns(Task.FromResult(CacheEntryState.Valid));

            var result = await sut.Get<TestCacheModel>(nameof(TestCacheModel), cancellationToken);
            distributedCacheMock.Verify(distributedCache => distributedCache
                .GetAsync(nameof(TestCacheModel), cancellationToken), Times.Once);
            messagePackServiceMock.Verify(messagePackService => messagePackService
                .Deserialise<TestCacheModel>(cachedByteResults, sut._messagePackOptions), Times.Once);
        }

        
        [TestCase(CacheEntryState.New)]
        [TestCase(CacheEntryState.Invalid)]
        public async Task Get_when_cache_state_is_invalid_does_not_call_deserialize(CacheEntryState invalidEntityState)
        {
            var cancellationToken = CancellationToken.None;
            var testCacheModel = new TestCacheModel();

            var cachedByteResults = new byte[32];

            Array.Fill<byte>(cachedByteResults, 255);

            distributedCacheMock
                .Setup(distributedCache => distributedCache.GetAsync(nameof(TestCacheModel), cancellationToken))
                .Returns(Task.FromResult(cachedByteResults)).Verifiable();

            messagePackServiceMock.Setup(messagePackService => messagePackService
                .Deserialise<TestCacheModel>(cachedByteResults, sut._messagePackOptions))
                .Returns(Task.FromResult(testCacheModel))
                .Verifiable();

            cacheEntryTrackerMock.Setup(cacheEntryTrackerMock => cacheEntryTrackerMock
                .GetState(It.IsAny<string>(), cancellationToken))
                .Returns(Task.FromResult(invalidEntityState));

            var result = await sut.Get<TestCacheModel>(nameof(TestCacheModel), cancellationToken);
            distributedCacheMock.Verify(distributedCache => distributedCache
                .GetAsync(nameof(TestCacheModel), cancellationToken), Times.Once);
            messagePackServiceMock.Verify(messagePackService => messagePackService
                .Deserialise<TestCacheModel>(cachedByteResults, sut._messagePackOptions), Times.Never);
        }


        [Test]
        public async Task Get_when_null_does_not_call_deserialize()
        {
            var cancellationToken = CancellationToken.None;
            var testCacheModel = new TestCacheModel();
            var cachedByteResults = Array.Empty<byte>();

            distributedCacheMock
                .Setup(distributedCache => distributedCache.GetAsync(nameof(TestCacheModel), cancellationToken))
                .Returns(Task.FromResult(cachedByteResults)).Verifiable();

            messagePackServiceMock.Setup(messagePackService => messagePackService
                .Deserialise<TestCacheModel>(cachedByteResults, sut._messagePackOptions))
                .Returns(Task.FromResult(default(TestCacheModel)))
                .Verifiable();

            var result = await sut.Get<TestCacheModel>(nameof(TestCacheModel), cancellationToken);
            distributedCacheMock.Verify(distributedCache => distributedCache
                .GetAsync(nameof(TestCacheModel), cancellationToken), Times.Once);
            messagePackServiceMock.Verify(messagePackService => messagePackService
                .Deserialise<TestCacheModel>(cachedByteResults, sut._messagePackOptions), Times.Never);

            Assert.IsNull(result);
        }

        [Test]
        public async Task Set_when_parameter_not_null_calls_Serialize()
        {
            var cancellationToken = CancellationToken.None;
            var testCacheModel = new TestCacheModel();

            var cachedByteResults = new byte[32];

            Array.Fill<byte>(cachedByteResults, 255);

            distributedCacheMock
                .Setup(distributedCache => distributedCache.SetAsync(nameof(TestCacheModel),cachedByteResults, 
                    It.IsAny<DistributedCacheEntryOptions>(), cancellationToken))
                .Returns(Task.CompletedTask)
                .Verifiable();

            messagePackServiceMock.Setup(messagePackService => messagePackService
                .Serialise(testCacheModel, sut._messagePackOptions))
                .Returns(Task.FromResult(cachedByteResults.AsEnumerable()))
                .Verifiable();

            await sut.Set(nameof(TestCacheModel), testCacheModel, cancellationToken);

            distributedCacheMock.Verify(distributedCache => distributedCache.SetAsync(nameof(TestCacheModel),cachedByteResults, 
                    It.IsAny<DistributedCacheEntryOptions>(), cancellationToken), Times.Once);

            messagePackServiceMock.Verify(messagePackService => messagePackService
                .Serialise(testCacheModel, sut._messagePackOptions), Times.Once);
        }

        
        [Test]
        public async Task Set_when_parameter_null_does_not_call_Serialize()
        {
            var cancellationToken = CancellationToken.None;
            TestCacheModel testCacheModel = default;

            var cachedByteResults = new byte[32];

            Array.Fill<byte>(cachedByteResults, 255);

            distributedCacheMock
                .Setup(distributedCache => distributedCache.SetAsync(nameof(TestCacheModel),cachedByteResults, 
                    It.IsAny<DistributedCacheEntryOptions>(), cancellationToken))
                .Returns(Task.CompletedTask)
                .Verifiable();

            messagePackServiceMock.Setup(messagePackService => messagePackService
                .Serialise(testCacheModel, sut._messagePackOptions))
                .Returns(Task.FromResult(cachedByteResults.AsEnumerable()))
                .Verifiable();

            await sut.Set(nameof(TestCacheModel), testCacheModel, cancellationToken);

            distributedCacheMock.Verify(distributedCache => distributedCache.SetAsync(nameof(TestCacheModel),cachedByteResults, 
                    It.IsAny<DistributedCacheEntryOptions>(), cancellationToken), Times.Never);

            messagePackServiceMock.Verify(messagePackService => messagePackService
                .Serialise(testCacheModel, sut._messagePackOptions), Times.Never);
        }
    }
}
