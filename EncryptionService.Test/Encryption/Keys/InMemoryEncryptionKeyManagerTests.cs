using System;
using System.Linq;
using System.Threading.Tasks;
using EncryptionService.Encryption.Keys;
using FluentAssertions;
using Moq;
using Xunit;

namespace EncryptionService.Test.Encryption.Keys
{
    public class InMemoryEncryptionKeyManagerTests
    {
        private readonly InMemoryEncryptionKeyManager<TestKey> _keyManager;
        private readonly Mock<IEncryptionKeyCreator<TestKey>> _mockKeyCreator;

        public class TestKey : IKey
        {
            public TestKey(int version)
            {
                Version = version;
            }

            public int Version { get; }
        }

        public InMemoryEncryptionKeyManagerTests()
        {
            _keyManager = new InMemoryEncryptionKeyManager<TestKey>(Int32.MaxValue);
            _mockKeyCreator = new Mock<IEncryptionKeyCreator<TestKey>>();
        }

        private TestKey SetupKey(IEncryptionKeyManager<TestKey> keyManager)
        {
            TestKey key = null;
            _mockKeyCreator
                .Setup(x => x.Create(It.IsAny<int>()))
                .Returns<int>(v => { key = new TestKey(v); return key; });
            keyManager.Rotate(_mockKeyCreator.Object);
            return key;
        }
        
        [Fact]
        public void GetByVersion_Throws_EncryptionKeyNotFoundException_If_Key_With_Version_Does_Not_Exists()
        {
            _keyManager.Invoking(x => x.GetByVersion(1)).Should().Throw<EncryptionKeyNotFoundException>();
        }

        [Fact]
        public void GetByVersion_ReturnsKey_With_VersionNumber()
        {
            var key = SetupKey(_keyManager);
            var result = _keyManager.GetByVersion(1);

            result.Should().Be(key);
        }
        
        [Fact]
        public void GetLatestKey_Throws_EncryptionKeyNotFoundException_If_None_Exists()
        {
            _keyManager.Invoking(x => x.GetLatest()).Should().Throw<EncryptionKeyNotFoundException>();
        }
        
        [Fact]
        public void GetLatestKey_Returns_Key_If_Only_One_Exists()
        {
            var key = SetupKey(_keyManager);
            var result = _keyManager.GetLatest();

            result.Should().Be(key);
        }
        
        [Fact]
        public void GetLatestKey_Returns_Key_With_Highest_Version_Number()
        {
            SetupKey(_keyManager);
            var version2Key = SetupKey(_keyManager);

            var result = _keyManager.GetLatest();

            result.Should().Be(version2Key);
        }
        
        [Fact]
        public void Rotate_Calls_EncryptionKeyCreator_Version_1_If_No_keys_Exist()
        {
            _keyManager.Rotate(_mockKeyCreator.Object);
            _mockKeyCreator.Verify(x => x.Create(1));
        }
        
        [Fact]
        public void Rotate_Calls_EncryptionKeyCreator_Next_Version()
        {
            SetupKey(_keyManager);
            _keyManager.Rotate(_mockKeyCreator.Object);
            _mockKeyCreator.Verify(x => x.Create(2));
        }
        
        [Fact]
        public void Rotate_Is_Thread_Safe()
        {
            Parallel.Invoke(() => SetupKey(_keyManager), () => SetupKey(_keyManager), () => SetupKey(_keyManager));

            _keyManager.Rotate(_mockKeyCreator.Object);
            
            _mockKeyCreator.Verify(x => x.Create(1), Times.Once);
            _mockKeyCreator.Verify(x => x.Create(2), Times.Once);
            _mockKeyCreator.Verify(x => x.Create(3), Times.Once);
        }
        
        [Fact]
        public void Rotate_Removes_Old_Keys_If_MaxNumber_Exceeded()
        {
            var keyManager = new InMemoryEncryptionKeyManager<TestKey>(1);
            var key1 = SetupKey(keyManager);
            var key2 = SetupKey(keyManager);

            keyManager.Invoking(x => x.GetByVersion(key1.Version)).Should().Throw<EncryptionKeyNotFoundException>();
        }
    }
}