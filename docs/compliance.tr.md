# Uyumluluk - Approval Scheme Process

Bu doküman, Approval Scheme Process sisteminin devlet kurumlarında işletilmesi için gereken asgari uyumluluk beklentilerini tanımlar. Odak noktası yargı alanına özel hukuk tavsiyesi vermek değil, kişisel veri koruması, hesap verebilirlik ve kanıt gereksinimlerini netleştirmektir.

[English version](compliance.md) | [Vatandaş Log Erişimi](citizen-log-access.tr.md)

## 1. Amaç

Approval Scheme Process, kamu verilerine hukuka uygun, orantılı ve denetlenebilir erişimi desteklemek için tasarlanmıştır. Uyumluluk gereksinimleri mimari ve yönetişim ile birlikte ele alınmalı, sonradan eklenmemelidir.

## 2. Kapsam

Bu uyumluluk çerçevesi aşağıdaki alanlara uygulanır:

- Oturum bağlamı içinde yapılan talepler
- Onay gerektiren bağlam dışı talepler
- Acil durum erişimi ve override vakaları
- Onay kayıtları ve denetim logları
- Erişim kontrol davranışını etkileyen yapılandırma değişiklikleri

## 3. Uyumluluk İlkeleri

Model aşağıdaki ilkeler doğrultusunda uygulanmalıdır:

### Hukuka Uygunluk

Her erişim yolu tanımlı bir hukuki veya düzenleyici dayanağa sahip olmalıdır.

### Amaçla Sınırlılık

Erişim yalnızca geçerli işlem türüne bağlı, belgelenmiş kurumsal amaç için kullanılmalıdır.

### Veri Minimizasyonu

Çalışanlar yalnızca onaylanmış işi yürütmek için gerekli en az veriyi görmelidir.

### Hesap Verebilirlik

Önemli her erişim kararı kişi, rol, zaman ve gerekçe ile ilişkilendirilebilir olmalıdır.

### İzlenebilirlik

Loglar ve onay kayıtları sonradan inceleme, soruşturma ve raporlama için yeterli olmalıdır.

### Orantılılık

Hassasiyet ve risk arttıkça daha güçlü kontroller uygulanmalıdır.

## 4. Zorunlu Kontrol Alanları

Kurumlar en az aşağıdaki alanlar için kontrol tanımlamalı ve belgelemelidir:

### Hukuki Dayanak ve Politika Eşlemesi

- Hangi işlem türlerinin izinli olduğu
- Hangi veri kategorilerine erişilebileceği
- Hangi iş amaçlarının erişimi gerekçelendirdiği
- Her kategori için hangi hukuki veya düzenleyici dayanağın bulunduğu

### Erişim Gerekçesi

Bağlam dışı talepler en az şu bilgileri gerektirmelidir:

- Açık iş gerekçesi
- Beyan edilen işlem türü
- Hedef kapsamı
- Talep eden kişi
- Talep zamanı

### Veri Minimizasyonu ve Maskeleme

Sistem mümkün olduğunda şunları desteklemelidir:

- Alan bazlı veya kayıt bazlı kısıtlama
- Yüksek riskli veriler için kısmi gösterim
- Sınırsız ham erişim yerine amaç odaklı görünüm

### Loglama ve Kanıt

Sistem şu bilgileri kaydetmelidir:

- Talep eden kimlik bilgisi
- Varsa oturum bağlamı
- İşlem türü
- Güvenlik seviyesi
- Onay geçmişi
- Nihai karar
- Varsa cihaz veya ağ bağlamı

### Saklama

Kurumlar şunları tanımlamalıdır:

- Talep ve onay kayıtlarının ne kadar süre saklanacağı
- Denetim loglarının ne kadar süre saklanacağı
- Kayıtların ne zaman arşivleneceği
- Silmenin hangi koşullarda hukuken mümkün olduğu

### Gözden Geçirme ve Gözetim

Kurumlar şunları tanımlamalıdır:

- Logları kimin inceleyeceği
- İncelemelerin hangi sıklıkla yapılacağı
- Hangi durumların soruşturma tetikleyeceği
- Politika ihlallerinin nasıl eskale edileceği

## 5. Hassas Veri İşleme

Aşağıdaki veri türleri için özel kurallar tanımlanmalıdır:

- Korunan kişisel veriler
- Sağlıkla ilgili veriler
- Çocuklara ilişkin kayıtlar
- Yüksek profilli veya siyasi açıdan hassas kayıtlar
- Hukuki imtiyaz veya kısıtlı açıklama kapsamındaki veriler

Bu kategoriler yükseltilmiş güvenlik seviyeleri ve onay gereksinimleriyle açıkça eşleştirilmelidir.

## 6. Veri Sahibi ve Vatandaş Koruması

Kurumsal model şu kabiliyetleri desteklemelidir:

- Kişisel kayıtlara kontrollü erişim
- Hassas sorgular için incelenebilir gerekçe
- Çalışanların gereksiz görünürlüğünün sınırlandırılması
- Suistimal şikayetlerinin incelenmesi

Yerel mevzuat veri sahibi taleplerinin yönetimini gerektiriyorsa, bu süreçler ayrı belgelenmeli ancak bu kontrol modeliyle izlenebilir biçimde ilişkilendirilmelidir.

Vatandaşa dönük şeffaflık akışı [citizen-log-access.tr.md](citizen-log-access.tr.md) içinde tanımlanmıştır.

## 7. Olay ve Suistimal Yönetimi

Uyumluluk modeli şu durumlarda ne olacağını tanımlamalıdır:

- Yetkisiz erişim girişimi olduğunda
- Onay kuralları aşıldığında
- Acil durum erişimi kötüye kullanıldığında
- Loglar olağandışı veya tekrarlayan davranış gösterdiğinde

Asgari olarak kurumlar şunları tanımlamalıdır:

- Olay sınıflandırması
- Eskalasyon yolu
- Kanıt koruma yaklaşımı
- İç bildirim yolu
- Olay sonrası inceleme

## 8. Uyumluluk Kanıtı

Model aşağıdaki başlıklar için kanıt üretebilmelidir:

- Erişime neden izin verildiği
- Erişimin neden reddedildiği
- Talebi kimin onayladığı
- Hangi politika veya seviyenin uygulandığı
- Gözden geçirmenin zamanında yapılıp yapılmadığı
- İstisnaların sonradan incelenip incelenmediği

## 9. Gözden Geçirme Sıklığı

Önerilen asgari sıklık:

- Acil durum ve override erişimleri için aylık inceleme
- Yüksek riskli erişim örüntüleri için üç aylık inceleme
- Kontrol tasarımı ve saklama politikası için yıllık inceleme

## 10. Özet

Approval Scheme Process içinde uyumluluk; gerekçe, orantılı kontrol, izlenebilir onaylar ve savunulabilir kayıtlar üzerine kuruludur. Bu kontroller, devlet kurumlarının modeli sorumlu biçimde işletebilmesi için gereklidir.
