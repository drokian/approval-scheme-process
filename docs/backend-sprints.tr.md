# Backend Sprint Planı - Approval Scheme Process

Bu doküman, MVP için ilk backend uygulama sırasını tanımlar.

[English version](backend-sprints.md) | [MVP Kapsamı](mvp-scope.tr.md)

## 1. Planlama Varsayımları

- Sprint boyutu bilerek küçüktür
- Her sprint `1-2` odaklı saate sığmalıdır
- Her sprint genellikle `3-5` issue'ya bölünmelidir
- Temel hedef, büyük kod paketleri değil izlenebilir ilerlemedir

## 2. Hedef Solution Yapısı

```text
ApprovalSchemeProcess.sln
├── src/
│   ├── ApprovalSchemeProcess.Domain
│   ├── ApprovalSchemeProcess.Application
│   ├── ApprovalSchemeProcess.Infrastructure
│   └── ApprovalSchemeProcess.Api
└── tests/
    ├── ApprovalSchemeProcess.UnitTests
    ├── ApprovalSchemeProcess.IntegrationTests
    └── ApprovalSchemeProcess.ArchitectureTests
```

Katman kuralları:

- `Domain` hiçbir şeye bağımlı değildir
- `Application` sadece `Domain`'e bağlıdır
- `Infrastructure`, application tarafına bakan sözleşmeleri implemente eder
- `Api`, HTTP ve composition sorumluluklarını taşır

## 3. Sprint Sırası

## S1 - Solution Kurulumu ve Kalıcılık Temeli

Kapsam:

- Solution ve proje yapısını oluşturmak
- EF Core yapılandırmasını eklemek
- İlk şema eşlemelerini tanımlamak
- Seed migration desteği eklemek

Çıktı:

- Derlenen solution
- Çalışan veritabanı başlangıç yapısı
- Kalıcılığa eşlenmiş entity'ler

## S2 - Oturum Motoru

Kapsam:

- Oturumla ilgili domain ve application mantığını uygulamak
- `IsInContext()` davranışını eklemek
- Temel bağlam doğrulama durumlarını unit testlerle kapsamak

Çıktı:

- Oturum bağlam değerlendirme servisi
- Unit testlerle doğrulanmış randevu/oturum kontrol yolu

## S3 - Erişim Motoru

Kapsam:

- Bağlam dışı talepleri tespit etmek
- İstek için güvenlik seviyesini çözümlemek
- Onay yönlendirmesine uygun erişim değerlendirme sonucu üretmek

Çıktı:

- Erişim değerlendirme hattı
- Onay motoruna aktarılabilir karar nesnesi
- Bağlam ve güvenlik seviyesi kombinasyonları için unit testler

## S4 - Onay Motoru

Kapsam:

- Onay şeması tanımlarını yüklemek
- Seviye 2 zinciri çalıştırmak
- Sıralı `Supervisor` ve `Security Officer` adımlarını desteklemek

Çıktı:

- Veri odaklı onay şeması yürütümü
- Deterministik adım sırası yönetimi
- İzin ve ret senaryoları için testler

## S5 - Loglama Katmanı

Kapsam:

- Uçtan uca akış için denetim kayıtları yazmak
- Talep, karar, onay ve sonuç metadatasını tutmak
- Loglamayı engine'lere bağlamak

Çıktı:

- Kalıcı audit log kayıtları
- Arayüz olmadan çalışan uçtan uca backend akışı
- Çekirdek senaryo için integration test kapsamı

## S6 - API Katmanı

Kapsam:

- MVP akışı için minimal REST endpoint'leri açmak
- Engine'leri request handler'lara bağlamak
- Çekirdek senaryoyu demo olarak çalıştırılabilir hale getirmek

Çıktı:

- Minimal API yüzeyi
- `WebApplicationFactory` kullanan integration testler
- Demo'ya hazır backend dilimi

## 4. Önerilen Test Dağılımı

- `UnitTests`: engine mantığı, kural değerlendirme, onay adımı davranışları
- `IntegrationTests`: endpoint sözleşmeleri, veritabanı destekli akış doğrulaması
- `ArchitectureTests`: `NetArchTest` ile katman bağımlılık kuralları

## 5. Önerilen Definition of Done

Her sprint sonunda aşağıdakiler tamamlanmış olmalıdır:

- Kod, tanımlı branch iş akışı ile commitlenmiş olmalı
- Testler eklenmiş veya güncellenmiş olmalı
- Davranış değişikliği varsa ilgili İngilizce ve Türkçe dokümanlar güncellenmiş olmalı
- PR içinde net bir demo veya doğrulama notu bulunmalı

## 6. Sonraki Planlama Adımı

Bu backend sprintleri kabul edildikten sonra bir sonraki adım, her sprinti açık acceptance criteria'lara sahip issue boyutlu demo dilimlerine ayırmak olmalıdır.
