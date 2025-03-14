# ConfigurationLibrary

## Genel Bakış

`ConfigurationLibrary`, uygulama yapılandırma ayarlarını veritabanından okuyan ve yöneten bir kütüphanedir. Bu kütüphane, belirli bir uygulama için yapılandırma ayarlarını önbelleğe alır ve belirli aralıklarla bu ayarları yeniler.

## Sınıflar ve Arayüzler

### ConfigurationReader

`ConfigurationReader`, yapılandırma ayarlarını veritabanından okuyan ve yöneten ana sınıftır. Bu sınıf, belirli bir uygulama için yapılandırma ayarlarını önbelleğe alır ve belirli aralıklarla bu ayarları yeniler.

#### Özellikler

- `_applicationName`: Uygulama adı.
- `_options`: Veritabanı bağlamı seçenekleri.
- `_refreshInterval`: Yenileme aralığı.
- `_timer`: Yenileme işlemini zamanlayan zamanlayıcı.
- `_cache`: Yapılandırma ayarlarının önbelleği.
- `_semaphore`: Eşzamanlılık kontrolü için semafor.

#### Yapıcı Metot


#### Metotlar

- `LoadConfigurationAsync()`: Yapılandırma ayarlarını veritabanından yükler.
- `RefreshConfigurationAsync(object state)`: Yapılandırma ayarlarını belirli aralıklarla yeniler.
- `GetValueAsync(string key)`: Belirli bir anahtar için yapılandırma değerini döner.
- `GetTypeAsync(string key)`: Belirli bir anahtar için yapılandırma türünü döner.
- `GetValueAsync<T>(string key)`: Belirli bir anahtar için yapılandırma değerini belirli bir türde döner.
- `ConvertValue(string value, string type)`: Değeri belirli bir türe dönüştürür.
- `Dispose()`: Kaynakları serbest bırakır.

### ConfigurationDbContext

`ConfigurationDbContext`, yapılandırma ayarlarını içeren veritabanı bağlamıdır.

#### Özellikler

- `ConfigurationSettings`: Yapılandırma ayarlarını içeren DbSet.

### ConfigurationSetting

`ConfigurationSetting`, yapılandırma ayarlarını temsil eden model sınıfıdır.

#### Özellikler

- `Id`: Ayarın benzersiz kimliği.
- `Name`: Ayarın adı.
- `Type`: Ayarın türü.
- `Value`: Ayarın değeri.
- `IsActive`: Ayarın aktif olup olmadığını belirten bayrak.
- `ApplicationName`: Ayarın ait olduğu uygulama adı.
- `CreatedDate`: Ayarın oluşturulma tarihi.

### IConfigurationReader

`IConfigurationReader`, yapılandırma okuyucusu için arayüzdür.

#### Metotlar

- `Task<T> GetValueAsync<T>(string key)`: Belirli bir anahtar için yapılandırma değerini belirli bir türde döner.
- `Task<object> GetValueAsync(string key)`: Belirli bir anahtar için yapılandırma değerini döner.

## Kullanım

### ConfigurationReader Kullanımı


## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Daha fazla bilgi için `LICENSE` dosyasına bakınız.




