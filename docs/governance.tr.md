# Yönetişim - Approval Scheme Process

Bu doküman, Approval Scheme Process sisteminin kurumsal bir ortamda işletilebilmesi için gereken yönetişim modelini tanımlar. Politika sahipliği, değişiklik kontrolü, hesap verebilirlik ve gözetim konularına odaklanır.

[English version](governance.md)

## 1. Amaç

Approval Scheme Process yalnızca teknik bir mimari değildir. Aynı zamanda bir yönetişim modelidir. Sistemin güvenilir olabilmesi için kurumların kuralların kim tarafından sahiplenildiğini, kimlerin bu kuralları değiştirebildiğini, etkinliğin kim tarafından gözden geçirildiğini ve suistimalin nasıl ele alındığını tanımlaması gerekir.

## 2. Yönetişim Hedefleri

Yönetişim modeli şunları sağlamalıdır:

- Erişim kurallarının yetkili paydaşlar tarafından onaylanması
- Güvenlik seviyelerinin tutarlı biçimde atanması
- Onay şemalarının gözden geçirilmesi ve sürdürülmesi
- Acil durum ve override yollarının sıkı kontrol edilmesi
- Logların denetimlerde savunulabilir ve incelenebilir olması
- Politika değişikliklerinin zaman içinde izlenebilir olması

## 3. Yönetişim Yapıları

### İş Süreci Sahibi

İşlem türlerinin anlam ve kapsamını tanımlamaktan ve iş ihtiyacını doğrulamaktan sorumludur.

### Bilgi Güvenliği Fonksiyonu

Risk sınıflandırması, güvenlik kontrol gereksinimleri, izleme beklentileri ve yüksek riskli erişim modellerinin gözden geçirilmesinden sorumludur.

### Hukuk ve Uyumluluk Fonksiyonu

Hukuki dayanağın, veri koruma etkilerinin ve politikaların geçerli mevzuatla uyumunun incelenmesinden sorumludur.

### Sistem Yönetimi Fonksiyonu

Onaylanan yapılandırma değişikliklerini kontrollü ortamlarda uygulamaktan ve operasyonel bütünlüğü sürdürmekten sorumludur.

### İç Denetim veya Gözetim Fonksiyonu

Logların, politika uyumunun ve suistimal göstergelerinin bağımsız biçimde gözden geçirilmesinden sorumludur.

## 4. Yönetişim Kapsamı

Yönetişim en az şu alanları kapsamalıdır:

- İşlem türü kataloğu
- Güvenlik seviyesi atamaları
- Onay şeması tanımları
- Rol eşlemeleri
- Acil durum erişim politikası
- Override politikası
- Loglama ve saklama politikası
- Gözden geçirme ve denetim takvimi

## 5. Değişiklik Kontrolü

Kontrol modelindeki her değişiklik belgeli bir süreçten geçmelidir:

1. İş gerekçesiyle değişiklik talebi açılır
2. Risk ve uyumluluk etkisi değerlendirilir
3. Gerekli onaylayıcılar önerilen değişikliği inceler
4. Onaylanan değişiklikler kontrollü sürümleme süreciyle uygulanır
5. Değişiklik, kim tarafından ne zaman onaylandığı bilgisiyle loglanır
6. Değişiklik sonrası gözden geçirme ile kontrollerin etkinliğinin korunduğu doğrulanır

Kontrollü değişiklik örnekleri:

- Yeni bir işlem türü eklemek
- Mevcut bir işlemin güvenlik seviyesini değiştirmek
- Bir onay zincirini güncellemek
- Yeni bir acil durum override rolü tanımlamak

## 6. Gözden Geçirme Sıklığı

Kurumlar aşağıdaki alanlar için düzenli gözden geçirme tanımlamalıdır:

- Güvenlik seviyesi atamaları
- Onay şemalarının etkinliği
- Kullanılmayan veya çok seyrek kullanılan onay yolları
- Acil durum erişim olayları
- Override kullanımı
- Tekrarlayan reddedilmiş talepler
- Mesai dışı erişim örüntüleri

Önerilen asgari sıklık:

- Aylık operasyonel gözden geçirme
- Üç aylık politika gözden geçirmesi
- Yıllık kontrol etkinliği gözden geçirmesi

## 7. Görev Ayrılığı

Sınıflandırma, onay, icra ve inceleme zincirinin tamamı tek bir rolde toplanmamalıdır.

Asgari olarak:

- Talep sahibi kendi talebini onaylamamalıdır
- Politika yazarları politika sonuçlarının tek denetleyicisi olmamalıdır
- Acil durum erişimleri olay sonrası bağımsız bir fonksiyon tarafından incelenmelidir
- Denetim inceleyicileri operasyonel override yetkisine değil, salt okuma inceleme yetkisine sahip olmalıdır

## 8. İstisna Yönetimi

İstisnalar açık, süreli ve denetlenebilir olmalıdır.

Her istisna en az şu bilgileri içermelidir:

- İş gerekçesi
- Talep edilen süre
- Erişim kapsamı
- Adlandırılmış onaylayıcı
- Olay sonrası inceleme gereksinimi

## 9. Kanıt ve Denetlenebilirlik

Yönetişim modeli aşağıdaki başlıklar için kanıt üretmeyi zorunlu kılmalıdır:

- Bir güvenlik seviyesinin neden atandığı
- Bir onay şemasının neden onaylandığı
- Kuralı kimin değiştirdiği
- İstisnayı kimin onayladığı
- Gözden geçirmenin zamanında yapılıp yapılmadığı

## 10. Özet

Approval Scheme Process ancak teknik model açık bir yönetişim modeliyle desteklenirse devlet kurumlarında güvenilir biçimde çalışabilir.

Yönetişim, mimariyi kavramsal bir kontrol deseninden hesap verebilir kurumsal bir sürece dönüştürür.
