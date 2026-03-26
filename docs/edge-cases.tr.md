# Kenar Durumlar - Approval Scheme Process

Bu doküman, Approval Scheme Process içinde açıkça tanımlanması gereken operasyonel kenar durum akışlarını tanımlar. Bu durumlar özellikle kamu kurumlarında önemlidir; çünkü hukuka uygunluk, hesap verebilirlik ve hizmet sürekliliğini doğrudan etkiler.

[English version](edge-cases.md) | [Mimari](architecture.tr.md) | [Akışlar](flows.tr.md) | [Oturum ve Token Süre Sonu](session-and-token-expiry.tr.md)

## 1. Amaç

Yalnızca temel akışların tanımlanması, üretim ortamına hazır bir kontrol modeli için yeterli değildir. Kurumlar, normal oturum veya onay varsayımları bozulduğunda ne olacağını da açıkça tanımlamalıdır.

Bu doküman, uygulamaya geçmeden önce tanımlanması gereken asgari kenar durum setini kapsar:

- Acil erişim
- Onay zaman aşımı veya süresinin dolması
- Onaylayıcının erişilemez olması veya delegasyon
- Manuel override
- Tekrarlanan reddedilmiş talepler
- Çalışma saatleri dışındaki yüksek riskli erişimler

## 2. Ortak Kontrol Kuralları

Tüm kenar durum akışları şu temel kuralları izlemelidir:

- Normal bağlam içi erişim varsayılan yol olarak kalır
- Kenar durum yönetimi istisnai olmalıdır, rutin hale gelmemelidir
- Her istisna yolu normal erişimden daha güçlü loglama gerektirir
- Süre limitleri, gerekçe ve sorumlu aktörler açık olmalıdır
- Yüksek riskli istisnalarda olay sonrası gözden geçirme zorunlu olmalıdır

## 3. Acil Erişim Akışı

Bu akış, zararı önlemek, hizmet sürekliliğini korumak veya hukuken yetkilendirilmiş acil bir kamu görevini yerine getirmek için erişimin derhal gerekli olduğu durumlarda uygulanır.

### Tetikleyici Koşullar

- Acil operasyonel risk vardır
- Normal onay zincirini beklemek kabul edilemez zarar veya gecikme yaratacaktır
- Talep sahibi acil durum gerekçesi belirtir

### Akış

1. Talep sahibi bağlam dışı bir talep oluşturur ve bunu acil erişim olarak işaretler.
2. Erişim Motoru, talep sahibi rolün acil erişim başlatmaya yetkili olup olmadığını doğrular.
3. Talep, normal işlem türü ve güvenlik seviyesi modeli ile sınıflandırılır.
4. Acil durum politikası kuralları uygulanır:
   - Bazı işlem türleri tamamen engellenebilir
   - Bazı seviyeler kısaltılmış acil onay zinciri gerektirebilir
   - Bazı seviyeler olay sonrası zorunlu inceleme ile geçici erişime izin verebilir
5. Acil erişime izin verilirse, sistem sorgu çalıştırılmadan önce gerekçeyi, acil durum bayrağını, talep sahibi kimliğini ve zamanı kaydeder.
6. Talep yalnızca gerekli en dar hedef kapsamı ile yürütülür.
7. Sistem otomatik olarak zorunlu bir olay sonrası inceleme kaydı oluşturur.

### Zorunlu Kontroller

- Açık acil durum gerekçesi
- Sınırlı rol yetkisi
- Daraltılmış hedef kapsamı
- Daha güçlü log saklama ve gözden geçirme önceliği
- Bağımsız bir fonksiyon tarafından zorunlu olay sonrası inceleme

## 4. Onay Zaman Aşımı veya Süresinin Dolması Akışı

Bu akış, bir veya daha fazla onay adımının zamanında tamamlanmaması durumunda uygulanır.

### Tetikleyici Koşullar

- Adım için tanımlı bir süre penceresi vardır
- Atanan onaylayıcı son tarihten önce yanıt vermez

### Akış

1. Onay Motoru her onay adımı için zaman aşımı değerlerini izler.
2. Son tarih geçer ve karar verilmezse adım süresi dolmuş olarak işaretlenir.
3. Politika tanımlı bir geri dönüş kuralı yoksa iş akışı durur.
4. Onay Motoru Erişim Motoruna reddet veya süresi doldu sonucu döndürür.
5. Talep çalıştırılmaz.
6. Talep, zaman aşımı olayı ve nihai sonuç loglanır.

### Zorunlu Kontroller

- Adım ya da şema bazında tanımlı süre sonu
- Log ve raporlarda açık `expired` durumu
- Sessiz tekrar deneme yerine isteğe bağlı yeniden başvuru yolu
- Pasiflik nedeniyle otomatik onay olmaması

## 5. Onaylayıcının Erişilemez Olması veya Delegasyon Akışı

Bu akış, ilgili onaylayıcının izin, arıza, yeniden atama ya da başka gerekçeli nedenlerle inceleme yapamaması halinde uygulanır.

### Tetikleyici Koşullar

- Onaylayıcı erişilemez durumdadır
- Geçerli bir delegasyon kuralı vardır

### Akış

1. Onay Motoru tanımlı onaylayıcının erişilemez olduğunu tespit eder.
2. Sistem bu rol ve dönem için önceden onaylı bir vekil olup olmadığını kontrol eder.
3. Geçerli vekil varsa adım bu kişiye yeniden atanır.
4. Geçerli vekil yoksa aşağıdaki politika sonuçlarından biri uygulanmalıdır:
   - Talep zaman aşımına kadar beklemede kalır
   - Talep yetkili bir yönetici tarafından yeniden atanır
   - Geçerli kontrol otoritesi olmadığı için talep reddedilir
5. Delegasyon bilgileri, asıl onaylayıcı, vekil, zaman aralığı ve gerekçe ile birlikte loglanır.

### Zorunlu Kontroller

- Delegasyon açık ve süreli olmalıdır
- Vekiller eşit ya da daha yüksek inceleme yetkisine sahip olmalıdır
- Talep sahibi kendi vekilini seçememelidir
- Delegasyon değişiklikleri denetlenebilir olmalıdır

## 6. Manuel Override Akışı

Bu akış, sistemin özel olarak yetkilendirilmiş bir aktöre normal karar yolunu aşma imkânı tanıdığı durumlarda uygulanır.

### Tetikleyici Koşullar

- Normal onay yolu acil hukuki ya da operasyonel ihtiyacı karşılayamaz
- Override yetkisi kurum politikası ile verilmiştir

### Akış

1. Özel yetkili aktör bir override işlemi başlatır.
2. Sistem bu kişi ve işlem türü için override hakkı olup olmadığını doğrular.
3. Override gerekçesi ile hukuki veya idari dayanak kaydedilir.
4. Talep override işaretleri ile birlikte yürütülür.
5. Sistem otomatik olarak zorunlu bir olay sonrası inceleme kaydı oluşturur.
6. Override kullanımı dönemsel gözetim raporlarına dahil edilir.

### Zorunlu Kontroller

- Override hakları belirli rollerle sınırlı olmalıdır
- Güçlü gerekçe zorunluluğu
- Otomatik uyarı veya eskalasyon
- Bağımsız zorunlu olay sonrası inceleme
- Gizli override yolu olmaması

## 7. Tekrarlanan Reddedilmiş Talepler Akışı

Bu akış, aynı talep sahibinin reddedildikten sonra tekrar tekrar erişim istemesi halinde uygulanır.

### Tetikleyici Koşullar

- Aynı hedef, işlem veya dönem için çoklu reddedilmiş ya da süresi dolmuş talepler
- Politika tanımlı tekrar eşiğinin aşılması

### Akış

1. Erişim Motoru veya izleme katmanı tekrarlanan reddedilmiş girişimleri tespit eder.
2. Sistem girişimlerin uyarı eşiklerini aşıp aşmadığını değerlendirir.
3. Eşik aşılırsa ilave kontroller uygulanabilir:
   - Geçici talep yavaşlatma
   - Yönetici veya güvenlik bildirimi
   - Yeni onay talebi kabul edilmeden önce manuel inceleme
4. Tekrarlanan girişimler denetim raporlarında gruplanır.

### Zorunlu Kontroller

- Eşik tanımları
- Örüntü bazlı uyarı üretimi
- Meşru tekrar deneme ile şüpheli davranışın ayrıştırılması
- İncelenebilir kanıt izi

## 8. Çalışma Saatleri Dışındaki Yüksek Riskli Erişim Akışı

Bu akış, yüksek riskli ya da kritik taleplerin standart çalışma saatleri dışında yapılması halinde uygulanır.

### Tetikleyici Koşullar

- Güvenlik seviyesi yüksek riskli ya da kritiktir
- Talep politika ile tanımlı çalışma saatleri dışındadır

### Akış

1. Erişim Motoru işlem türünü ve güvenlik seviyesini çözer.
2. Talep zaman damgası üzerinden çalışma saatleri politikası değerlendirilir.
3. Talep izinli saatlerin dışındaysa aşağıdaki politikalardan biri uygulanmalıdır:
   - İlave onay adımı gerekir
   - Acil durum gerekçesi zorunlu hale gelir
   - İstisna politikası yoksa talep engellenir
4. Çalışma saati dışı durumu nihai log ve gözden geçirme raporlarında işaretlenir.

### Zorunlu Kontroller

- Kurum bazlı çalışma saatleri politikası
- Saat dilimi farkındalığı
- Yüksek riskli seviyeler için ek inceleme
- Mesai dışı faaliyetler için ayrı raporlama

## 9. Asgari Veri ve Loglama Gereksinimleri

Tüm kenar durum akışları en az şu bilgileri kaydetmelidir:

- Talep sahibi kimliği
- Hedef ve işlem türü
- Güvenlik seviyesi
- Tetiklenen kenar durum tipi
- Gerekçe
- İzlenen karar yolu
- Sorumlu inceleyici ya da vekil
- Talep, karar, çalıştırma ve olay sonrası inceleme zamanları

## 10. Uygulama Notları

Uygulamaya geçmeden önce bu kenar durum akışları şu alanlarla hizalanmalıdır:

- Oturum ve token süre sonu politikası
- Delegasyon ve atama geçmişi için şema desteği
- Uyumluluk raporlama gereksinimleri
- Yönetişim gözden geçirme periyodu

Oturum ve token ömrü için temel politika [session-and-token-expiry.tr.md](session-and-token-expiry.tr.md) içinde tanımlanmıştır.

## 11. Özet

Kenar durum akışları, normal varsayımlar bozulduğunda modelin nasıl davranacağını tanımlar. Bu akışlar olmadan Approval Scheme Process gerçek kurumsal kullanım için eksik kalır.

Bu akışlar, şema stabilizasyonu, politika tasarımı ve denetim raporlaması için zorunlu tasarım girdisi olarak ele alınmalıdır.
