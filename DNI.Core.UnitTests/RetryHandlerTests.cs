using DNI.Core.Contracts;
using DNI.Core.Contracts.Options;
using DNI.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.UnitTests
{
    public class RetryHandlerTests
    {
        private IRetryHandler _sut;
        private Mock<ILogger> _loggerMock;
        private Mock<IRetryHandlerOptions> _retryHandlerOptionsMock;
        private int _count;
        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger>();
            _sut = new DefaultRetryHandler(_retryHandlerOptionsMock.Object, _loggerMock.Object);
            _count = 0;
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(8)]
        public void Handle_retries_and_throws_on_handled_exception_after_x_attempts(int attempts)
        {
            _retryHandlerOptionsMock.SetupGet(options => options.Timeout)
                .Returns(10);
            Assert.Throws<InvalidOperationException>(() => _sut.Handle(() => { 
                _count++; 
                throw new InvalidOperationException(); }, attempts, false, typeof(InvalidOperationException)));

            Assert.AreEqual(attempts + 1, _count);
        }
    }
}
