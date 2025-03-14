# ConfigurationLibrary.UI

Konfigurasyonlar ile ilgili işlemlerin yapıldığı yönetim portalıdır.

Not: Projedeki appsetting dosyalarını kendiniz eklemeniz gerekmektedir 2 adet database kullanan bu projede database leri ayağa kaldırmak ve içerisine dataları yüklemek için gerekli komutlar aşşağıda tek tek açıklanmıştır aşşağıdaki işlemleri yapmadan proje düzgün bir biçimde ayağa kalkıp çalışmayacaktır.

## Projeyi Çalıştırmadan Önce Dikkat Edilmesi Gerekenler

### 1. appsettings.json Dosyasının Eklenmesi
ConfigurationLibrary.UI Projesine `appsettings.json` dosyasını eklemeniz gerekmektedir. Dosyanın içeriği aşağıdaki gibi olmalıdır:

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "ConfigurationConnection": "Server=Your_Server_Name;Database=ConfigurationDb;User Id=Your_userId;Password=Your_Password.;TrustServerCertificate=true",
        "ConfigurationAppConnection": "Server=Your_Server_Name;Database=ConfigurationAppDb;User Id=Your_User_Id;Password=Your_Password.;TrustServerCertificate=true"
    }
}
```

### 2. Veritabanları Hakkında Bilgi
Projede iki adet MsSQL veritabanı bulunmaktadır:
- **ConfigurationAppDb**: Portal ile ilgili bilgilerin (Kullanıcı, Rol vb.) tutulduğu veritabanıdır.
- **ConfigurationDb**: Konfigurasyon bilgilerinin tutulduğu veritabanıdır.

### 3. Portal Girişi
Portala giriş yapabilmek için aşağıdaki kullanıcı bilgilerini kullanabilirsiniz:
- **Kullanıcı Adı**: `slckarslan93@gmail.com`
- **Şifre**: `Selcuk123.`

### 4. appsettings.json Dosyasının Düzenlenmesi
Projeyi çalıştırmadan önce `appsettings.json` dosyanızdaki bağlantı bilgilerini kendi lokal MsSQL veritabanınızın bilgileri ile güncelleyin.

### 5. Veritabanını Güncelleme
Projeyi çalıştırmadan önce veritabanını oluşturmak için aşağıdaki komutu çalıştırın:
```sh
Update-Database -Context AppDbContext
```

### 6. Kullanıcı Rolü Ekleme
Portal girişinde rol bazlı kimlik doğrulama yapılabilmesi için aşağıdaki SQL sorgusunu çalıştırın (ConfurationAppDb ye AspNetUserRoles bilgisini eklmek için eklenmez ise portala giriş yapamazsınız !!):
```sql
INSERT INTO [ConfigurationAppDb].[dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (1, 1);
```

## Service-A
Service-A, uygulamayı test edebilmek için oluşturulmuş bir API projesidir. Çalıştırmadan önce `appsettings.json` dosyasındaki veritabanı bağlantı bilgilerini kendi lokal bilgilerinize göre düzenleyiniz.

### appsettings.json İçeriği
Service-A projesinin appsettings.json içeriği aşşağıdaki gibi olmalıdır

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "ConfigurationConnection": "Server=Your_Server;Database=Your_Db;User Id=Your_Id;Password=Your_Password.;TrustServerCertificate=true"
    },
    "RefreshTimerIntervalInMs": 50000
}
```

## Tests
Test projesidir. Çalıştırmadan önce `appsettings.json` dosyasındaki veritabanı bağlantı bilgilerini kendi lokal bilgilerinize göre düzenleyiniz.

### appsettings.json İçeriği
Service-A projesinin appsettings.json içeriği aşşağıdaki gibi olmalıdır

```json
{
    "ConnectionStrings": {
        "ConfigurationConnection": "Server=Your_Server;Database=ConfigurationDb;User Id=Your_userId;Password=Your_PassWord.;TrustServerCertificate=true"
    }
}

```

## ConfigurationLibrary

### Veritabanı Kurulumu
Projeyi lokal veritabanınızda çalıştırabilmek için aşağıdaki SQL sorgularını çalıştırın:

```sql
-- ConfigurationDb veritabanını oluştur
CREATE DATABASE ConfigurationDb;
GO

-- Yeni veritabanına geç
USE ConfigurationDb;
GO

-- ConfigurationSettings tablosunu oluştur
CREATE TABLE ConfigurationSettings (
    Id INT PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Value NVARCHAR(255) NOT NULL,
    IsActive BIT NOT NULL,
    ApplicationName NVARCHAR(100) NOT NULL,
    CreatedDate DATETIME NOT NULL
);
GO
```

### Örnek Veri Ekleme
```sql
INSERT INTO ConfigurationSettings (Id, Name, Type, Value, IsActive, ApplicationName, CreatedDate) VALUES
(23, 'SiteName', 'string', 'soty.io', 1, 'SERVICE-A', '2025-03-14 01:10:34.667'),
(24, 'IsBasketEnabled', 'bool', 'true', 1, 'SERVICE-A', '2025-03-14 01:10:34.667'),
(25, 'MaxItemCount', 'int', '50', 0, 'SERVICE-A', '2025-03-14 01:10:34.667'),
(26, 'SupportEmail', 'string', 'support@soty.io', 1, 'SERVICE-A', '2025-03-14 01:10:34.667'),
(27, 'EnableLogging', 'bool', 'false', 1, 'SERVICE-A', '2025-03-14 01:10:34.667'),
(28, 'CacheDuration', 'int', '120', 1, 'SERVICE-A', '2025-03-14 01:10:34.667'),
(29, 'MaintenanceMode', 'bool', 'false', 0, 'SERVICE-A', '2025-03-14 01:10:34.667'),
(30, 'ApiRateLimit', 'int', '1000', 1, 'SERVICE-A', '2025-03-14 01:10:34.667'),
(31, 'FeatureXEnabled', 'bool', 'true', 1, 'SERVICE-A', '2025-03-14 01:10:34.667'),
(32, 'SessionTimeout', 'int', '30', 1, 'SERVICE-A', '2025-03-14 01:10:34.667'),
(33, 'EnableBetaFeatures', 'bool', 'false', 0, 'SERVICE-A', '2025-03-14 01:10:34.667');
GO
```

## Genel Bakış

`ConfigurationLibrary`, uygulama yapılandırma ayarlarını veritabanından okuyan ve yöneten bir kütüphanedir. Belirli bir uygulama için yapılandırma ayarlarını önbelleğe alır ve belirli aralıklarla bu ayarları yeniler.

## Sınıflar ve Arayüzler

### ConfigurationReader
`ConfigurationReader`, yapılandırma ayarlarını veritabanından okuyan ve yöneten ana sınıftır. Yapılandırma ayarlarını önbelleğe alır ve belirli aralıklarla yeniler.

#### Özellikler
- `_applicationName`: Uygulama adı.
- `_options`: Veritabanı bağlamı seçenekleri.
- `_refreshInterval`: Yenileme aralığı.
- `_timer`: Yenileme işlemini zamanlayan zamanlayıcı.
- `_cache`: Yapılandırma ayarlarının önbelleği.
- `_semaphore`: Eşzamanlılık kontrolü için semafor.

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
- `ConfigurationSettings`: Yapılandırma ayarlarını içeren `DbSet`.

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

## Lisans
Bu proje MIT lisansı altında lisanslanmıştır. Daha fazla bilgi için `LICENSE` dosyasına bakınız.

