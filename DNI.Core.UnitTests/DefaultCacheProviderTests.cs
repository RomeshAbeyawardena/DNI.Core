//using DNI.Core.Contracts.Providers;
//using NUnit.Framework;
//using DNI.Core.Services.Providers;
//using Moq;
//using DNI.Core.Contracts;
//using DNI.Core.Contracts.Factories;
//using System.Threading.Tasks;
//using DNI.Core.Contracts.Enumerations;
//using System.Threading;
//using DNI.Core.Contracts.Services;
//using DNI.Core.UnitTests.Models;

//namespace DNI.Core.UnitTests
//{
//    public class DefaultCacheProviderTests
//    {
//        private Mock<IIs> _iisMock;
//        private Mock<ICacheProviderFactory> _cacheProviderFactoryMock;
//        private Mock<ICacheService> _distributedCacheServiceMock;
//        private Mock<ICacheService> _sessionCacheServiceMock;
//        private ICacheProvider sut;
//        [SetUp]
//        public void Setup()
//        {
//            _cacheProviderFactoryMock = new Mock<ICacheProviderFactory>();
//            _distributedCacheServiceMock = new Mock<ICacheService>();
//            _sessionCacheServiceMock = new Mock<ICacheService>();
//            _iisMock = new Mock<IIs>();

//            _cacheProviderFactoryMock.Setup(cacheProvider => cacheProvider.GetCache(CacheType.DistributedMemoryCache))
//                .Returns(_distributedCacheServiceMock.Object)
//                .Verifiable();


//            sut = new DefaultCacheProvider(_iisMock.Object, 
//                _cacheProviderFactoryMock.Object,);
//        }


//        [Test]
//        public async Task Get_calls_CacheService()
//        {
//            var cancellationToken = CancellationToken.None;

//            var testCacheModelInstance = new TestCacheModel();

//            _distributedCacheServiceMock.Setup(cacheService => cacheService
//                .Get<TestCacheModel>(nameof(testCacheModelInstance), cancellationToken))
//                .Returns(Task.FromResult(testCacheModelInstance))
//                .Verifiable();

//            var result = await sut.Get<TestCacheModel>(CacheType.DistributedMemoryCache, nameof(testCacheModelInstance), cancellationToken);

//            _cacheProviderFactoryMock.Verify();
//            _distributedCacheServiceMock.Verify();
//        }

//        [Test]
//        public async Task Set_calls_CacheService_when_value_is_not_null()
//        {
//            var cancellationToken = CancellationToken.None;

//            var testCacheModelInstance = new TestCacheModel();

//            _distributedCacheServiceMock.Setup(cacheService => cacheService
//                .Set(nameof(testCacheModelInstance), testCacheModelInstance, cancellationToken))
//                .Returns(Task.CompletedTask)
//                .Verifiable();

//            await sut.Set(CacheType.DistributedMemoryCache, 
//                nameof(testCacheModelInstance), testCacheModelInstance, cancellationToken);

//            _cacheProviderFactoryMock.Verify();
//            _distributedCacheServiceMock.Verify();
//        }

//        [Test]
//        public async Task Set_does_not_call_CacheService_when_value_is_null()
//        {
//            var cancellationToken = CancellationToken.None;

//            var testCacheModelInstance = new TestCacheModel();

//            _distributedCacheServiceMock.Setup(cacheService => cacheService
//                .Set(nameof(testCacheModelInstance), It.IsAny<TestCacheModel>(), cancellationToken))
//                .Returns(Task.CompletedTask)
//                .Verifiable();

//            await sut.Set(CacheType.DistributedMemoryCache, 
//                nameof(testCacheModelInstance), default(TestCacheModel), cancellationToken);

//            _cacheProviderFactoryMock.Verify(cacheProvider => cacheProvider.GetCache(CacheType.DistributedMemoryCache), Times.Never);
//            _distributedCacheServiceMock.Verify(cacheService => cacheService
//                .Set(nameof(testCacheModelInstance), It.IsAny<TestCacheModel>(), cancellationToken), Times.Never);
//        }

//    }
//}