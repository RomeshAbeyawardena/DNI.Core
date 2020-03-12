using DNI.Core.Contracts;
using DNI.Core.Services;
using Moq;
using NUnit.Framework;
using System;

namespace DNI.Core.UnitTests
{
    public class InstanceServiceInjectorTests
    {
        IInstanceServiceInjector sut;
        Mock<IServiceProvider> serviceProviderMock;

        [SetUp]
        public void SetUp()
        {
            serviceProviderMock = new Mock<IServiceProvider>();
            sut = new DefaultInstanceServiceInjector(serviceProviderMock.Object);
        }

        [Test]
        public void CreateInstance_calls_GetService_once_only()
        {
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider
                    .GetService(typeof(MyMockService)))
                .Returns(new MyMockService())
                .Verifiable();

            var instance = sut.CreateInstance<MyMockConsumer>();

            serviceProviderMock.Verify(serviceProvider => serviceProvider
                    .GetService(typeof(MyMockService)), Times.Once);

            Assert.IsInstanceOf<MyMockConsumer>(instance);
            Assert.IsInstanceOf<MyMockService>(instance.MyMockService);
        }

        [Test]
        public void CreateInstance_calls_GetService_for_each_service()
        {
            var mockService = new MyMockService();
            var otherMockService = new MyOtherMockService(mockService);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider
                    .GetService(typeof(MyMockService)))
                .Returns(mockService)
                .Verifiable();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider
                    .GetService(typeof(MyOtherMockService)))
                .Returns(otherMockService)
                .Verifiable();

            var instance = sut.CreateInstance<MyOtherMockConsumer>();

            serviceProviderMock.Verify(serviceProvider => serviceProvider
                    .GetService(typeof(MyMockService)), Times.Once);

            serviceProviderMock.Verify(serviceProvider => serviceProvider
                    .GetService(typeof(MyOtherMockService)), Times.Once);

            Assert.IsInstanceOf<MyOtherMockConsumer>(instance);
            Assert.IsInstanceOf<MyMockService>(instance.MyMockService);
            Assert.IsInstanceOf<MyOtherMockService>(instance.MyOtherMockService);
        }

        [Test]
        public void CreateInstance_throws_ArgumentNullException_when_GetService_returns_null()
        {

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider
                    .GetService(typeof(MyMockService)))
                .Returns(null)
                .Verifiable();

            Assert.Throws<ArgumentNullException>(() =>
            {
                sut.CreateInstance<MyMockConsumer>();
            });

            serviceProviderMock.Verify(serviceProvider => serviceProvider
                    .GetService(typeof(MyMockService)), Times.Once);

        }

        [Test]
        public void CreateInstance_throws_InvalidOperationException_when_multiple_constructors_are_present()
        {
            Assert.Throws<InvalidOperationException>(() => sut.CreateInstance<MyMockConsumerWithNumerousConstructors>());
        }
        
        private class MyMockConsumerWithNumerousConstructors
        {
            public MyMockConsumerWithNumerousConstructors(MyMockService mockService)
            {
                MyMockService = mockService;
            }

            public MyMockConsumerWithNumerousConstructors(MyOtherMockService mockService)
            {
                MyOtherMockService = mockService;
            }

            public MyMockService MyMockService { get; }
            public MyOtherMockService MyOtherMockService { get; }
        }

        private class MyMockConsumer
        {
            public MyMockConsumer(MyMockService mockService)
            {
                MyMockService = mockService;
            }

            public MyMockService MyMockService { get; }
        }

        private class MyOtherMockConsumer
        {
            public MyOtherMockConsumer(MyMockService mockService, MyOtherMockService myOtherMockService)
            {
                MyMockService = mockService;
                MyOtherMockService = myOtherMockService;
            }

            public MyMockService MyMockService { get; }
            public MyOtherMockService MyOtherMockService { get; set; }
        }

        private class MyMockService
        {

        }

        private class MyOtherMockService
        {
            public MyOtherMockService(MyMockService mockService)
            {
                MyMockService = mockService;
            }

            public MyMockService MyMockService { get; set; }
        }
    }
}
