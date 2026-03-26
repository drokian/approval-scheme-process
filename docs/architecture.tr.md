# Mimari - Approval Scheme Process

Bu doküman, Approval Scheme Process sisteminin yüksek seviye mimarisini, ana bileşenlerini, karar akışlarını ve genişleme noktalarını tanımlar.

[English version](architecture.md) | [Akışlar](flows.tr.md) | [Kenar Durumlar](edge-cases.tr.md) | [Oturum ve Token Süre Sonu](session-and-token-expiry.tr.md)

## 1. Genel Bakış

Approval Scheme Process, devlet kurumları için tasarlanmış bağlam tabanlı bir erişim yönetişimi modelidir.

Model dört temel fikir üzerine kuruludur:

- Serbest erişim yalnızca geçerli bir randevuya dayanan aktif oturum bağlamında verilir
- Bağlam dışı erişimler güvenlik seviyelerine göre değerlendirilir
- Onay akışları işlem türü bazında tanımlanır
- Her karar izlenebilirlik ve denetim için loglanır

Model kurumdan bağımsızdır ve tapu, nüfus, vergi ve sosyal hizmetler gibi alanlara uyarlanabilir.

## 2. Temel İlkeler

### 2.1 Bağlam Tabanlı Erişim

Çalışanlar yalnızca şu koşulların tamamı sağlandığında serbest sorgu yapabilir:

- Geçerli bir randevu vardır
- Randevu aktif bir oturuma dönüştürülmüştür
- Çalışan bu oturuma atanmıştır
- Sorgu hedefi oturum bağlamıyla eşleşmektedir

### 2.2 Güvenlik Seviyesi Odaklı Erişim

Tapu satışı, doğum kaydı veya vergi denetimi gibi her işlem türüne bir güvenlik seviyesi atanır.

Güvenlik seviyeleri şunları belirler:

- Onay gerekip gerekmediği
- Kaç onay adımı gerektiği
- Hangi rollerin onay vereceği
- İlave kontrollerin uygulanıp uygulanmayacağı

### 2.3 Dinamik Onay Şemaları

Her işlem türü kendi onay akışına sahip olabilir.

Örnek kalıplar:

- Seviye 1 -> Amir onayı
- Seviye 2 -> Amir ve Güvenlik onayı
- Seviye 3 -> Amir, Hukuk ve Veri Koruma onayı
- Seviye 4 -> Çoklu onay ve özel yetkilendirme

### 2.4 Tam Loglama ve Denetim

Her sorguda şu bilgiler kaydedilmelidir:

- İşlemi yapan kişi
- İşlem zamanı
- Oturum veya istek bağlamı
- İşlem türü ve güvenlik seviyesi
- Onay durumu ve onay geçmişi
- Varsa cihaz ve ağ metaverisi

## 3. Yüksek Seviye Mimari

Kullanıcı -> Randevu Sistemi -> Oturum Motoru -> Erişim Motoru -> Onay Motoru -> Loglama ve Denetim

### 3.1 Randevu Sistemi

Sorumluluklar:

- Vatandaş randevularını yönetmek
- Talep edilen işlem türünü belirlemek
- İlk iş bağlamını sağlamak
- Randevu zamanında oturum oluşumunu tetiklemek

### 3.2 Oturum Motoru

Sorumluluklar:

- Randevuları aktif oturumlara dönüştürmek
- Oturumları çalışanlara atamak
- Aktif, beklemede ve kapalı gibi yaşam döngüsü durumlarını yönetmek
- Erişim doğrulaması için bağlam sağlamak

Oturum ömrü ve süre sonu beklentileri [session-and-token-expiry.tr.md](session-and-token-expiry.tr.md) içinde tanımlanmıştır.

### 3.3 Erişim Motoru

Bu bileşen ana karar mekanizmasıdır.

Sorumluluklar:

- Bir sorgunun bağlam içi mi bağlam dışı mı olduğunu kontrol etmek
- Rol ve oturum kısıtlarını değerlendirmek
- Talep edilen işlem için güvenlik seviyesini belirlemek
- Onay gerekip gerekmediğine karar vermek
- Onay gerektiren talepleri Onay Motoruna yönlendirmek
- Gerekli bağlam veya yetki yoksa hızlı şekilde reddetmek

### 3.4 Onay Motoru

Sorumluluklar:

- İşlem için tanımlı onay şemasını getirmek
- Onay adımlarını tanımlı sıra ile başlatmak
- Onaylayıcıları bilgilendirmek
- Onay kararlarını kaydetmek
- Sonucu Erişim Motoruna izin veya red olarak döndürmek

### 3.5 Güvenlik Seviyesi Yöneticisi

Sorumluluklar:

- Güvenlik seviyelerini oluşturmak ve güncellemek
- Güvenlik seviyelerini işlem türleriyle eşleştirmek
- Risk sınıflandırma mantığını sürdürmek

### 3.6 Onay Şeması Yöneticisi

Sorumluluklar:

- Onay adımlarını, rolleri ve sıralamayı tanımlamak
- Gerekirse koşullu dallanmaları desteklemek
- Şema tutarlılığını doğrulamak
- Onay zincirini Onay Motoruna sağlamak

### 3.7 Loglama ve Denetim Katmanı

Sorumluluklar:

- Erişim ve onay loglarını saklamak
- Denetim raporlarını desteklemek
- İleride eklenecek anomali tespitine zemin hazırlamak
- KVKK ve benzeri uyumluluk gereksinimlerine destek olmak

## 4. Karar Sırası

Kontrol modelinin öngörülebilir olması için Erişim Motoru talepleri şu sırayla değerlendirmelidir:

1. Oturum ve bağlam doğrulaması
2. Çalışan ve rol yetkilendirmesi
3. İşlem türü tespiti
4. Güvenlik seviyesi belirleme
5. Onay gereksinimi kararı
6. İzin ver, reddet veya onay akışına gönder
7. İstek ve sonucun loglanması

Bu sıra, onaya geçmeden önce hangi kontrollerin zorunlu olduğunu açık hale getirir.

## 5. Bileşen Etkileşimi

### 5.1 Bağlam İçi Sorgu Akışı

Çalışan -> Oturum Motoru -> Erişim Motoru -> Loglama -> İzin

Adımlar:

1. Çalışan aktif bir oturum içinde sorgu yapar
2. Oturum Motoru güncel oturum bağlamını sağlar
3. Erişim Motoru hedefin oturum ve rol kısıtlarıyla uyumlu olduğunu doğrular
4. İstek bağlam içi olarak sınıflandırılır
5. Ek onay olmadan sorguya izin verilir
6. İstek ve sonuç loglanır

### 5.2 Bağlam Dışı Sorgu Akışı

Çalışan -> Erişim Motoru -> Onay Motoru -> Loglama -> İzin veya Red

Adımlar:

1. Çalışan geçerli oturum olmadan veya uyumsuz bağlamla sorgu yapar
2. Erişim Motoru isteği bağlam dışı olarak sınıflandırır
3. İşlem türü ve güvenlik seviyesi belirlenir
4. Onay şeması yüklenir
5. Onay akışı çalıştırılır
6. Onay verilirse sorguya izin verilir
7. Reddedilirse veya süre aşımına uğrarsa sorgu reddedilir
8. İstek, onay sonucu ve nihai sonuç loglanır

## 6. İstisnalar ve Kenar Durumlar

Mimari şu durumları açıkça tanımlamalıdır:

- Gerekçeli acil durum erişimi
- Onay zaman aşımı veya onay süresinin dolması
- Onaylayıcının yokluğu veya devir senaryoları
- Zorunlu sonradan inceleme ile manuel override
- Tekrarlayan red talepleri
- Mesai dışı saatlerde yüksek riskli erişim

Bu durumlar kamu ortamı için özellikle önemlidir.

Bu senaryoların detaylı operasyonel tanımı [edge-cases.tr.md](edge-cases.tr.md) içinde yer alır.

## 7. Veri Modeli Durumu

Yüksek seviyede planlanan temel varlıklar şunlardır:

- User
- Appointment
- Session
- OperationType
- SecurityLevel
- ApprovalScheme
- ApprovalStep
- Query
- Approval

Depo artık [db/schema.sql](/d:/source/drokian/approval-scheme-process/db/schema.sql) içinde ilişkisel bir şema taslağı içermektedir. Bu dosya, kavramsal modelle hizalı ama halen geliştirilen bir uygulama taslağı olarak değerlendirilmelidir.

## 8. Genişletilebilirlik

Mimari şu şekilde tasarlanmıştır:

### Modüler

Her bileşen birbirinden bağımsız olarak değiştirilebilir veya genişletilebilir.

### Kurumdan Bağımsız

Her kurum şu alanları kendi ihtiyacına göre tanımlayabilir:

- Kendi işlem türleri
- Kendi güvenlik seviyeleri
- Kendi onay şemaları

### Teknolojiden Bağımsız

Arka uç şu teknolojilerle uygulanabilir:

- Python
- Node.js
- Java
- Go
- .NET

### Gelecek Genişlemeleri

- Anomali tespiti
- Gerçek zamanlı risk puanlaması
- Kurumlar arası erişim federasyonu
- Zero-trust entegrasyonu

## 9. Özet

Approval Scheme Process şunları sağlar:

- Birleşik bir erişim yönetişimi modeli
- Daha güçlü kişisel veri koruması
- Yetkisiz erişimin önlenmesi
- Kuruma özel esnek onay akışları
- Tam izlenebilirlik ve denetlenebilirlik

Bu doküman, devlet sistemlerinde güvenli ve bağlam tabanlı erişim kontrolü uygulamak için kavramsal temeli tanımlar.
