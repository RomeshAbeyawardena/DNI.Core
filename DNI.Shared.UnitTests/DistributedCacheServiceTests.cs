using DNI.Shared.Contracts;
using DNI.Shared.Services;
using DNI.Shared.UnitTests.Models;
using Microsoft.Extensions.Caching.Distributed;
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

        [SetUp]
        public void SetUp()
        {
            distributedCacheMock = new Mock<IDistributedCache>();
            messagePackServiceMock = new Mock<IMessagePackService>();
            sut = new DefaultDistributedCacheService(distributedCacheMock.Object, messagePackServiceMock.Object);
        } 

        [Test]
        public async Task Get_when_not_null_calls_deserialize()
        {
            var cancellationToken = CancellationToken.None;
            var testCacheModel = new TestCacheModel();
            var cachedByteResults = Array.Empty<byte>();
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
    }
}
