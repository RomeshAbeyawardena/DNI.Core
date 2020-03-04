using DNI.Shared.Contracts.Providers;
using NUnit.Framework;
using DNI.Shared.Services.Providers;
using Moq;
using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Factories;
using System.Threading.Tasks;
using DNI.Shared.Contracts.Enumerations;
using System.Threading;
using DNI.Shared.Services;
using DNI.Shared.Contracts.Services;

namespace DNI.Shared.UnitTests
{
    public class DefaultCacheProviderTests
    {
        private Mock<IIs> _iisMock;
        private Mock<ICacheProviderFactory> _cacheProviderFactoryMock;
        private Mock<ICacheService> _distributedCacheServiceMock;
        private Mock<ICacheService> _sessionCacheServiceMock;
        private ICacheProvider sut;
        [SetUp]
        public void Setup()
        {
            _cacheProviderFactoryMock = new Mock<ICacheProviderFactory>();
            _distributedCacheServiceMock = new Mock<ICacheService>();
            _sessionCacheServiceMock = new Mock<ICacheService>();
            _iisMock = new Mock<IIs>();
            
            _cacheProviderFactoryMock.Setup(cacheProvider => cacheProvider.GetCache(CacheType.DistributedMemoryCache))
                .Returns(_distributedCacheServiceMock.Object)
                .Verifiable();


            sut = new DefaultCacheProvider(_iisMock.Object, 
                _cacheProviderFactoryMock.Object);
        }


        [Test]
        public async Task Get_calls_GetCacheService()
        {
            var cancellationToken = CancellationToken.None;

            var myTestObjectInstance = new MyTestObject();
            
            _distributedCacheServiceMock.Setup(cacheService => cacheService
                .Get<MyTestObject>(nameof(myTestObjectInstance), cancellationToken))
                .Returns(Task.FromResult(myTestObjectInstance))
                .Verifiable();

            var result = await sut.Get<MyTestObject>(CacheType.DistributedMemoryCache, nameof(myTestObjectInstance), cancellationToken);

            _cacheProviderFactoryMock.Verify();
            _distributedCacheServiceMock.Verify();
        }

        [Test]
        public async Task Set_calls_CacheService_when_value_is_not_null()
        {
            var cancellationToken = CancellationToken.None;

            var myTestObjectInstance = new MyTestObject();
            
            _distributedCacheServiceMock.Setup(cacheService => cacheService
                .Set(nameof(myTestObjectInstance), myTestObjectInstance, cancellationToken))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await sut.Set(CacheType.DistributedMemoryCache, 
                nameof(myTestObjectInstance), myTestObjectInstance, cancellationToken);

            _cacheProviderFactoryMock.Verify();
            _distributedCacheServiceMock.Verify();
        }

        [Test]
        public async Task Set_does_not_call_CacheService_when_value_is_null()
        {
            var cancellationToken = CancellationToken.None;

            var myTestObjectInstance = new MyTestObject();
            
            _distributedCacheServiceMock.Setup(cacheService => cacheService
                .Set(nameof(myTestObjectInstance), It.IsAny<MyTestObject>(), cancellationToken))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await sut.Set(CacheType.DistributedMemoryCache, 
                nameof(myTestObjectInstance), default(MyTestObject), cancellationToken);

            _cacheProviderFactoryMock.Verify(cacheProvider => cacheProvider.GetCache(CacheType.DistributedMemoryCache), Times.Never);
            _distributedCacheServiceMock.Verify(cacheService => cacheService
                .Set(nameof(myTestObjectInstance), It.IsAny<MyTestObject>(), cancellationToken), Times.Never);
        }

        internal class MyTestObject
        {

        }
    }
}