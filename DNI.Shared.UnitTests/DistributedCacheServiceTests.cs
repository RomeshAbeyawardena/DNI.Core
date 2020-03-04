using DNI.Shared.Contracts;
using DNI.Shared.Services;
using DNI.Shared.UnitTests.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.UnitTests
{
    public class DistributedCacheServiceTests
    {
        private DefaultDistributedCacheService sut;
        private Mock<IDistributedCache> distributedCacheMock;
        private Mock<IMessagePackService> messagePackServiceMock;
        private Mock<IOptions<DistributedCacheEntryOptions>> distributedCacheEntryOptionsMock;
        [SetUp]
        public void SetUp()
        {
            distributedCacheMock = new Mock<IDistributedCache>();
            messagePackServiceMock = new Mock<IMessagePackService>();
            distributedCacheEntryOptionsMock = new Mock<IOptions<DistributedCacheEntryOptions>>();

            distributedCacheEntryOptionsMock.Setup(m => 
                m.Value)
                .Returns(new DistributedCacheEntryOptions());

            sut = new DefaultDistributedCacheService(distributedCacheMock.Object, 
                messagePackServiceMock.Object, distributedCacheEntryOptionsMock.Object);
        } 

        [Test]
        public async Task Get_when_not_null_calls_deserialize()
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

            var result = await sut.Get<TestCacheModel>(nameof(TestCacheModel), cancellationToken);
            distributedCacheMock.Verify(distributedCache => distributedCache
                .GetAsync(nameof(TestCacheModel), cancellationToken), Times.Once);
            messagePackServiceMock.Verify(messagePackService => messagePackService
                .Deserialise<TestCacheModel>(cachedByteResults, sut._messagePackOptions), Times.Once);
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
