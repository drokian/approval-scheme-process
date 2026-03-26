# Güvenlik Seviyeleri - Approval Scheme Process

Bu doküman, Approval Scheme Process içinde kullanılan güvenlik seviyesi modelini tanımlar. Güvenlik seviyeleri, Erişim Motorunun bağlam dışı talepleri tutarlı biçimde değerlendirmesine ve erişim verilmeden önce hangi onay kontrollerinin uygulanacağını belirlemesine yardımcı olur.

[English version](security-levels.md) | [Mimari](architecture.tr.md) | [Akışlar](flows.tr.md)

## 1. Genel Bakış

Bir devlet kurumundaki her işlem türü farklı düzeyde hassasiyet, hukuki maruziyet ve operasyonel risk taşıyabilir. Tutarlı erişim yönetişimi sağlamak için her işlem türüne bir güvenlik seviyesi atanır.

Güvenlik seviyeleri şunları tanımlar:

- Onay gerekip gerekmediği
- Kaç onay adımı gerektiği
- Hangi rollerin onay vereceği
- Ek uyumluluk veya politika kontrollerinin uygulanıp uygulanmayacağı

Bu model, kurumların onay sıkılığını her işlemin hassasiyetine göre ayarlamasını sağlar.

## 2. Güvenlik Seviyesi Modeli

Varsayılan model, bağlam tabanlı serbest erişimden yüksek derecede kontrollü erişime kadar uzanan beş standart seviye tanımlar.

### Seviye 0 - Bağlam Tabanlı Serbest Erişim

Açıklama:
Randevu bağlamıyla eşleşen aktif bir oturum içinde yapılan sorgular.

Özellikler:

- Onay gerekmez
- Hızlı ve düşük sürtünmelidir
- Tam loglanır
- Geçerli oturum ve rol kısıtlarıyla sınırlıdır

Örnekler:

- Çalışanın mevcut aktif oturuma atanmış vatandaşı sorgulaması
- Randevu akışıyla doğrudan ilişkili kayıtlara erişim

### Seviye 1 - Düşük Riskli Bağlam Dışı Erişim

Açıklama:
Sınırlı hassasiyete ve düşük beklenen etkiye sahip bağlam dışı sorgular.

Onay gereksinimleri:

- 1 onay adımı
- Genellikle Amir veya Takım Lideri tarafından onaylanır

Örnekler:

- Aktif oturum dışında temel kimlik doğrulama sorguları
- Hassas olmayan kayıt sorguları
- Düşük mahremiyet etkisine sahip iç tutarlılık kontrolleri

### Seviye 2 - Orta Riskli Erişim

Açıklama:
Kişisel veri veya orta düzeyde hassas operasyonel bilgi içeren bağlam dışı sorgular.

Onay gereksinimleri:

- 2 onay adımı
- Genellikle:
  - Adım 1: Amir
  - Adım 2: Güvenlik veya Uyumluluk Yetkilisi

Örnekler:

- Atanmış oturum dışında kişisel kayıtlara erişim
- Geçmiş işlemlerin incelenmesi
- Birimler arası veri sorguları

### Seviye 3 - Yüksek Riskli Erişim

Açıklama:
Hukuken hassas kategorileri, korunan kişisel verileri veya kurumsal açıdan önemli risk taşıyan talepleri içeren sorgular.

Onay gereksinimleri:

- 3 kontrol noktası
- Genellikle:
  - Adım 1: Amir
  - Adım 2: Hukuk
  - Adım 3: Veri Koruma veya Uyumluluk incelemesi

Örnekler:

- Kamu görevlilerine veya korunmuş kişilere ait kayıtlara erişim
- Hassas demografik veya sağlık verileri
- Hukuki, disipliner veya itibar etkisi doğurabilecek sorgular

### Seviye 4 - Kritik Erişim

Açıklama:
En güçlü inceleme düzeyini ve açık gerekçelendirmeyi gerektiren istisnai veya çok hassas erişimler.

Onay gereksinimleri:

- Çoklu onay ve özel yetkilendirme
- Genellikle:
  - Adım 1: Amir
  - Adım 2: Güvenlik
  - Adım 3: Hukuk veya Veri Koruma
  - Adım 4: Üst yönetici veya özel yetkili makam

Örnekler:

- Yüksek hassasiyetli vatandaş kayıtlarına acil erişim
- Büyük soruşturmalar veya kriz yönetimiyle ilişkili erişimler
- Özellikle kısıtlı veri kümeleri veya yüksek profilli vakalarla ilgili sorgular

## 3. Güvenlik Seviyelerinin Erişim Kararlarına Etkisi

Bir çalışan sorgu yaptığında:

1. Erişim Motoru talebin bağlam içi olup olmadığını kontrol eder.
2. Talep bağlam dışıysa Erişim Motoru işlem türünü belirler.
3. Erişim Motoru bu işlem için güvenlik seviyesini çözümler.
4. Erişim Motoru onay gerekip gerekmediğine karar verir.
5. Onay gerekiyorsa ilgili onay şeması yüklenir ve çalıştırılır.
6. Sorguya yalnızca gerekli tüm kontroller sağlanırsa izin verilir.
7. Talep, güvenlik seviyesi, onay sonucu ve nihai çıktı loglanır.

Bu yapı, erişim kontrol davranışını kurumlar ve birimler arasında öngörülebilir kılar.

## 4. Tasarım Notları

Güvenlik seviyeleri en iyi şu niteliklere sahip olduğunda çalışır:

### Tutarlı

Açık bir politika istisnası yoksa aynı işlem türü her zaman aynı temel seviyeye çözülmelidir.

### Açıklanabilir

Çalışanlar ve onaylayıcılar, bir talebin neden belirli bir seviyede sınıflandırıldığını anlayabilmelidir.

### Yapılandırılabilir

Kurumlar, kendi yönetişim modellerine göre rol eşlemelerini, onay zincirlerini ve seviye eşiklerini uyarlayabilmelidir.

### Denetlenebilir

Güvenlik seviyesi kararları loglarda, raporlarda ve sonradan yapılacak incelemelerde görünür olmalıdır.

## 5. Genişletilebilirlik

Model şu ihtiyaçları destekleyecek şekilde tasarlanmıştır:

- Kuruma özel işlem katalogları
- İşlem türü bazında özel onay zincirleri
- Mesai saati, talep eden rolü veya acil durum gerekçesi gibi ek koşullar
- Gelecekte eklenecek risk puanlama veya anomali tespiti geliştirmeleri

## 6. Özet

Güvenlik seviyeleri, işlem hassasiyetini sınıflandırmak ve orantılı kontroller uygulamak için yapılandırılmış ve kurumdan bağımsız bir yaklaşım sunar.

Şunları sağlamaya yardımcı olur:

- Düşük riskli erişimin verimli kalması
- Orta ve yüksek riskli erişimin daha güçlü gözetim alması
- Kritik erişimin en yüksek inceleme düzeyiyle değerlendirilmesi

Bu model, devlet sistemlerinde güvenli ve bağlam tabanlı erişim yönetişiminin temel bileşenlerinden biridir.
