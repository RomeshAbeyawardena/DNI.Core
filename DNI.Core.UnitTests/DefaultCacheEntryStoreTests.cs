using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using DNI.Core.Contracts.Services;
using DNI.Core.Contracts.Stores;
using DNI.Core.Domains;
using DNI.Core.Services.Stores;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.UnitTests
{
    public class DefaultCacheEntryStoreTests
    {
        private ICacheTrackerStore _sut;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IFile> _fileMock;
        private Mock<IJsonSerializer> _jsonSerializerMock;
        private MemoryStream CreateMemoryStream(string data)
        {
            MemoryStream memoryStream;
            var streamWriter = new StreamWriter(memoryStream = new MemoryStream());
            
            streamWriter.Write(data);
            streamWriter.Flush();

            memoryStream.Position = 0;

            return memoryStream;
        }

        [SetUp]
        public void SetUp()
        {
            _fileServiceMock = new Mock<IFileService>();
           _jsonSerializerMock = new Mock<IJsonSerializer>();
            _fileMock = new Mock<IFile>();
            _sut = new DefaultJsonFileCacheTrackerStore(
                new JsonFileCacheTrackerStoreOptions { FileName = "My test file" }, 
                _fileServiceMock.Object,
                _jsonSerializerMock.Object);
        }

        [Test]
        public async Task GetItems_when_non_existant_returns_default()
        {
            _fileMock.Setup(file => file.Exists)
                .Returns(false)
                .Verifiable();

            _fileServiceMock.Setup(fileService => fileService.GetFile(It.IsAny<string>()))
                .Returns(_fileMock.Object)
                .Verifiable();

             var result = await _sut.GetItems(CancellationToken.None);

            Assert.IsNull(result);
            _fileMock.Verify();
            _fileServiceMock.Verify();
        }

        [Test]
        public async Task GetItems_when_existant_returns()
        {
            var jsonData = "{\"SAS\":1,\"MRA\":0,\"TMR\":1,\"LOL\":3}";
            //var memoryStream = CreateMemoryStream(jsonData);
            //_fileMock.Setup(file => file.GetFileStream(It.IsAny<ILogger>()))
            //    .Returns(memoryStream)
            //    .Verifiable();

            _fileMock.Setup(file => file.Exists)
                .Returns(true)
                .Verifiable();


            _fileServiceMock.Setup(fileService => fileService.GetFile(It.IsAny<string>()))
                .Returns(_fileMock.Object)
                .Verifiable();


            _fileServiceMock.Setup(fileService => fileService
                .GetTextFromFile(It.IsAny<IFile>(), CancellationToken.None))
                    .Returns(Task.FromResult(jsonData))
                    .Verifiable();

            var result = await _sut.GetItems(CancellationToken.None);

            Assert.IsNotNull(result);

            Assert.IsTrue(result.TryGetValue("SAS", out var value) && value == CacheEntryState.Valid);
            Assert.IsTrue(result.TryGetValue("MRA", out var value1) && value1 == CacheEntryState.Invalid);
            Assert.IsTrue(result.TryGetValue("TMR", out var value2) && value2 == CacheEntryState.Valid);
            Assert.IsTrue(result.TryGetValue("LOL", out var value3) && value3 == CacheEntryState.New);

            _fileMock.Verify();
            _fileServiceMock.Verify();
            //long length;
            //Assert.Throws<ObjectDisposedException>(() => length = memoryStream.Length);
        }

        [Test]
        public async Task GetItems_when_existant_returns_empty_dictionary()
        {
            var jsonData = string.Empty;
            var memoryStream = CreateMemoryStream(jsonData);
            
            _fileMock.Setup(file => file.Exists)
                .Returns(true)
                .Verifiable();

            _fileServiceMock.Setup(fileService => fileService.GetFile(It.IsAny<string>()))
                .Returns(_fileMock.Object)
                .Verifiable();

            var result = await _sut.GetItems(CancellationToken.None);

            Assert.IsNotNull(result);

            Assert.IsEmpty(result);

            _fileMock.Verify();
            _fileServiceMock.Verify();
        }

        [Test]
        public async Task SaveItems_when_state_is_null_or_empty_returns_null()
        {
            var state = default(Dictionary<string, CacheEntryState>);

            var result = await _sut.SaveItems(state, CancellationToken.None);

            Assert.IsNull(result);

            state = new Dictionary<string, CacheEntryState>();

            result = await _sut.SaveItems(state, CancellationToken.None);

            Assert.IsNull(result);
        }

        [Test]
        public async Task SaveItems_when_state_is_valid_and_not_empty_returns()
        {
            
            var state = new Dictionary<string, CacheEntryState>();

            state.Add("SAS", CacheEntryState.Invalid);
            state.Add("MRA", CacheEntryState.Valid);
            state.Add("TMR", CacheEntryState.New);

            _fileServiceMock
                .Setup(fileService => fileService.GetFile(It.IsAny<string>()))
                .Returns(_fileMock.Object)
                .Verifiable();

            _fileServiceMock.Setup(fileService => fileService
            .SaveTextToFile(It.IsAny<IFile>(), It.IsAny<string>(), CancellationToken.None))
                .Verifiable();

            var result = await _sut.SaveItems(state, CancellationToken.None);

            Assert.IsNotNull(result);

            _fileServiceMock.Verify();

        }
    }
}
