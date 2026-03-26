# Roller ve Sorumluluklar - Approval Scheme Process

Bu doküman, Approval Scheme Process sisteminin işletilmesinde yer alan temel kurumsal rolleri tanımlar ve bu rollerin sorumluluklarını netleştirir.

[English version](roles-and-responsibilities.md)

## 1. Amaç

Model, açık hesap verebilirliğe dayanır. Kurumlar erişimi kimin talep ettiğini, kimin onay verdiğini, kontrol modelini kimin yapılandırdığını ve sonuçları sonradan kimin incelediğini net biçimde tanımlamalıdır.

## 2. Temel Roller

### Çalışan

Sorumluluklar:

- Atanmış oturum bağlamı içinde sorgu yapmak
- Bağlam dışı talepler için gerekçe sağlamak
- Politikaya uymak ve gereksiz erişimden kaçınmak
- İnceleme ve soruşturmalara destek olmak

Yapmaması gerekenler:

- Kendi talebini onaylamak
- Onay sürecini dolanmak
- Başkasına ait kimlik bilgilerini kullanmak

### Amir

Sorumluluklar:

- Düşük ve orta riskli talepleri iş bağlamı içinde incelemek
- Operasyonel gerekliliği doğrulamak
- Yetersiz gerekçeye sahip talepleri reddetmek
- Daha yüksek inceleme gerektiren talepleri eskale etmek

Yapmaması gerekenler:

- Çıkar çatışması çözümlenmemiş talepleri onaylamak
- Devredilmemiş yetki alanında işlem yapmak

### Güvenlik Yetkilisi

Sorumluluklar:

- Yüksek güvenlik etkisi olan talepleri incelemek
- Talep edilen erişimin kurumsal güvenlik politikasına uyduğunu doğrulamak
- Güvenlik seviyesi ve onay şeması gözden geçirmelerine katılmak

### Hukuk İnceleyicisi

Sorumluluklar:

- Hukuki etkisi olan talepleri değerlendirmek
- Gerektiğinde hassas erişim için hukuki dayanağı doğrulamak
- Kısıtlı veri kategorilerine ilişkin politika tanımlarını desteklemek

### Veri Koruma veya Uyumluluk Yetkilisi

Sorumluluklar:

- Korunan veya hassas kişisel veri içeren talepleri incelemek
- Veri minimizasyonunu ve amaç uyumunu değerlendirmek
- Uyumluluk raporlaması ve incelemeleri desteklemek

### Kıdemli Yetkilendirme Makamı

Sorumluluklar:

- Politikanın kıdemli gözetim gerektirdiği istisnai veya kritik erişimleri onaylamak
- Normal onay zincirini aşan acil durum veya override vakalarını incelemek

### Sistem Yöneticisi

Sorumluluklar:

- Onaylanmış yapılandırma değişikliklerini uygulamak
- Platformun teknik bütünlüğünü sürdürmek
- Loglama, saklama ve operasyonel kontrolleri desteklemek

Yapmaması gerekenler:

- Yetkili yönetişim onayı olmadan onay politikalarını değiştirmek
- Açık acil durum prosedürü dışında yönetici erişimiyle normal iş onay kurallarını aşmak

### Denetçi veya Gözetim İnceleyicisi

Sorumluluklar:

- Logları, istisnaları, redleri ve olağandışı erişim örüntülerini incelemek
- Gözden geçirmelerin zamanında yapıldığını doğrulamak
- Kontrol boşluklarını ve suistimal göstergelerini raporlamak

## 3. RACI Benzeri Sorumluluk Haritası

Kurumlar aşağıdaki modeli kendi yapılarına uyarlayabilir:

- Talep oluşturma: Çalışan sorumludur
- İş gerekliliği incelemesi: Amir hesap sahibidir
- Güvenlik incelemesi: Gerektiğinde Güvenlik Yetkilisi hesap sahibidir
- Hukuk incelemesi: Gerektiğinde Hukuk İnceleyicisi hesap sahibidir
- Veri koruma incelemesi: Gerektiğinde Uyumluluk Yetkilisi hesap sahibidir
- Yapılandırma uygulaması: Onay sonrası Sistem Yöneticisi sorumludur
- Bağımsız inceleme: Denetçi veya Gözetim İnceleyicisi hesap sahibidir

## 4. Çıkar Çatışması Kuralları

Kurumlar en az şu kuralları tanımlamalıdır:

- Talep sahibi kendi talebini onaylayamaz
- Erişimden doğrudan fayda sağlayan kişi nihai onaylayıcı olmamalıdır
- Politika yazarı, kendi yazdığı politika altındaki istisnaların tek onaylayıcısı olmamalıdır
- Acil durum erişimi her zaman bağımsız olay sonrası inceleme tetiklemelidir

## 5. Asgari Yetkinlik Beklentileri

Her rol, kendi sorumluluğuna uygun eğitim almalıdır.

Örnekler:

- Çalışanlar: kabul edilebilir kullanım ve gerekçe kalitesi
- Amirler: iş gerekliliği incelemesi ve suistimal göstergeleri
- Güvenlik personeli: risk incelemesi ve eskalasyon yönetimi
- Hukuk ve uyumluluk ekipleri: düzenlemeye tabi veri işleme
- Denetçiler: kanıt inceleme ve istisna izleme

## 6. Özet

Approval Scheme Process açıkça ayrılmış sorumluluklara dayanır. Güçlü rol tanımları suistimali azaltır, kontrol atlatmayı önler ve denetlenebilirliği artırır.
