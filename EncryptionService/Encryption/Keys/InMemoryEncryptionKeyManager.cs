using System;
using System.Collections.Generic;
using System.Linq;

namespace EncryptionService.Encryption.Keys
{
    public class InMemoryEncryptionKeyManager<T> : IEncryptionKeyManager<T> where T : class, IKey
    {
        private readonly int _maxNumberActiveKeys;
        private readonly SortedDictionary<int, T> _activeKeys;
        
        public InMemoryEncryptionKeyManager()
        {
            _maxNumberActiveKeys = Int32.MaxValue;
            _activeKeys = new SortedDictionary<int, T>();
        }

        public InMemoryEncryptionKeyManager(int maxNumberActiveKeys)
        {
            if (maxNumberActiveKeys < 1)
                throw new ArgumentOutOfRangeException(nameof(maxNumberActiveKeys));
            
            _maxNumberActiveKeys = maxNumberActiveKeys;
            _activeKeys = new SortedDictionary<int, T>();
        }

        private int? GetLatestVersion()
        {
            return _activeKeys.Keys.LastOrDefault();
        }

        public T GetLatest()
        {
            var latestVersion = GetLatestVersion();
            if (latestVersion == null || latestVersion < 1) throw new EncryptionKeyNotFoundException();

            return GetByVersion(latestVersion.Value);
        }

        public T GetByVersion(int version)
        {
            _activeKeys.TryGetValue(version, out T? key);
            if (key == null) throw new EncryptionKeyNotFoundException();

            return key;
        }

        public void Rotate(IEncryptionKeyCreator<T> creator)
        {
            lock (_activeKeys)
            {
                var latestKeyVersion = GetLatestVersion() ?? 0;
                var newVersion = latestKeyVersion + 1;
                var newKey = creator.Create(newVersion);
            
                _activeKeys.Add(newVersion, newKey);

                if (_activeKeys.Count > 1 && _activeKeys.Count > _maxNumberActiveKeys)
                    _activeKeys.Remove(_activeKeys.Keys.First());
            }
        }
    }
}