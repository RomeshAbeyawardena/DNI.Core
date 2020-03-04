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
        private Mock<ICacheService> _cacheServiceMock;
        private ICacheProvider sut;
        [SetUp]
        public void Setup()
        {
            sut = new DefaultCacheProvider(_iisMock.Object, 
                _cacheProviderFactoryMock.Object);
            _cacheProviderFactoryMock = new Mock<ICacheProviderFactory>();
            _cacheServiceMock = new Mock<ICacheService>();
        }

        [Test]
        public async Task Get_calls_GetCacheService()
        {
            var myTestObjectInstance = new MyTestObject();
            _cacheProviderFactoryMock.Setup(cacheProvider => cacheProvider.GetCache(CacheType.DistributedMemoryCache))
                .Returns(_cacheServiceMock.Object);
        }

        [Test]
        public async Task GetOrSet_when_cached_value_unavailable_calls_set_and_returns(CancellationToken cancellationToken)
        {
            
            
            Assert.Pass();
        }

        internal class MyTestObject
        {

        }
    }
}