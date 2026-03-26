# Akışlar - Approval Scheme Process

Bu doküman, Approval Scheme Process sisteminin temel operasyonel akışlarını tanımlar. Yüksek seviye mimariyi adım adım istek işleme, onay yönetimi ve loglama davranışı ile detaylandırır.

[English version](flows.md) | [Mimari](architecture.tr.md)

## 1. Kapsam

Bu akışlar, mimari dokümanında tanımlanan kontrol sırasıyla hizalıdır:

1. Oturum ve bağlam doğrulaması
2. Çalışan ve rol yetkilendirmesi
3. İşlem türü tespiti
4. Güvenlik seviyesi belirleme
5. Onay gereksinimi kararı
6. İzin ver, reddet veya onay akışına gönder
7. İstek ve sonucun loglanması

## 2. Randevuya Dayalı İşlem Akışı (Bağlam İçi Akış)

Bu akış, vatandaşın geçerli bir randevusu olduğunda ve çalışanın ilgili aktif oturuma atanmış olduğu durumda geçerlidir.

Vatandaş -> Randevu Sistemi -> Oturum Motoru -> Çalışan -> Erişim Motoru -> Loglama -> İzin

### Adımlar

1. Randevu, işlem türü ve vatandaş, kayıt veya varlık gibi ilk bağlam bilgileriyle oluşturulur.
2. Randevu zamanı geldiğinde Oturum Motoru aktif bir oturum oluşturur ve sorumlu çalışana atar.
3. Çalışan, oturum farkındalığı olan arayüz üzerinden sorgu yapar.
4. Erişim Motoru şunları doğrular:
   - Aktif bir oturum var mıdır
   - Çalışan bu oturuma atanmış mıdır
   - Sorgu hedefi oturum bağlamıyla eşleşiyor mudur
   - Çalışan rolü istenen işlem için yetkili midir
5. İstek bağlam içi olarak sınıflandırılır.
6. Ek onay gerekmez.
7. Sorgu çalıştırılır ve istek sonucu loglanır.

## 3. Bağlam Dışı Sorgu Akışı

Bu akış, bir çalışanın geçerli bir oturum olmadan veya atanmış oturum bağlamı dışında veri sorgulamaya çalıştığı durumda geçerlidir.

Çalışan -> Erişim Motoru -> Onay Motoru -> Loglama -> İzin veya Red

### Adımlar

1. Çalışan, geçerli bir oturum olmadan veya aktif oturum bağlamıyla eşleşmeyen bir hedef için sorgu başlatır.
2. Erişim Motoru isteği bağlam dışı olarak sınıflandırır.
3. Erişim Motoru, çalışan rolünün bu işlem türünü talep etmeye uygun olup olmadığını kontrol eder.
4. Erişim Motoru işlem türünü belirler.
5. Erişim Motoru, gerektiğinde Güvenlik Seviyesi Yöneticisi verilerini kullanarak güvenlik seviyesini çözümler.
6. Erişim Motoru onay gerekip gerekmediğine karar verir.
7. Onay gerekiyorsa istek Onay Motoruna gönderilir.
8. Onay Motoru işlem türüne ait onay şemasını yükler ve akış başlatır.
9. Gerekli tüm onaylar verilirse sorguya izin verilir.
10. Herhangi bir adım reddedilirse, süresi dolarsa veya politika kontrolü başarısız olursa sorgu reddedilir.
11. İstek, onay geçmişi ve nihai sonuç loglanır.

## 4. Onay Akışı

Bu akış, Erişim Motoru onay gerektiğine karar verdikten sonra onay adımlarının nasıl işletildiğini açıklar.

Erişim Motoru -> Onay Motoru -> Onaylayıcı(lar) -> Onay Motoru -> Erişim Motoru

### Adımlar

1. Erişim Motoru bir onay talebi oluşturur ve istek bağlamını Onay Motoruna aktarır.
2. Onay Motoru, belirlenen işlem türü için onay şemasını yükler.
3. Onay Motoru gerekli sıra ile onay adımlarını oluşturur ve ilgili rollere yönlendirir.
4. Her onaylayıcı şu bilgileri inceler:
   - Sorgu detayları
   - Çalışan gerekçesi
   - İşlem türü
   - Güvenlik seviyesi
   - İlgili oturum veya bağlam bilgileri
5. Her onaylayıcı onay verir, reddeder veya talebin süresinin dolmasına neden olur.
6. Onay Motoru tüm karar kayıtlarını tutar ve akışın devam edip etmeyeceğini belirler.
7. Gerekli tüm adımlar onaylanırsa Onay Motoru Erişim Motoruna izin kararı döndürür.
8. Herhangi bir zorunlu adım reddedilir veya süresi dolarsa Onay Motoru red kararı döndürür.
9. Nihai karar tam onay geçmişiyle birlikte loglanır.

## 5. Loglama ve Denetim Akışı

Her İstek veya Onay İşlemi -> Loglama ve Denetim Katmanı -> Raporlama ve Analiz

### Loglanan Veriler

- Çalışan ID
- Zaman damgası
- Oturum ID, varsa
- Sorgu hedefi
- İşlem türü
- Güvenlik seviyesi
- Onay durumu
- Onay geçmişi
- Varsa cihaz ve ağ metaverisi

### Denetim Yetenekleri

- Bağlam dışı sorgu raporları
- Yüksek riskli erişim raporları
- Çalışan bazlı risk puanlaması
- Kurum geneli erişim istatistikleri
- İleride eklenecek anomali tespiti özelliklerine destek

## 6. Kenar Durum Notları

Operasyonel akışlar şu durumlar için de davranış tanımlamalıdır:

- Gerekçeli acil durum erişimi
- Onay zaman aşımı veya süre dolumu
- Onaylayıcı devri veya yokluğu
- Zorunlu sonradan inceleme ile manuel override
- Tekrarlayan reddedilmiş erişim girişimleri
- Mesai dışında yüksek riskli erişim

## 7. Özet

Bu akışlar Approval Scheme Process sisteminin operasyonel davranışını tanımlar:

- Bağlam içi sorgular hızlı ve düşük sürtünmelidir
- Bağlam dışı sorgular politika ve onay mekanizması ile kontrol edilir
- Onay akışları dinamik ve rol tabanlıdır
- Loglama izlenebilirlik, gözetim ve sonraki analiz için temel sağlar

Bu akışlar birlikte kamu kurumları için tutarlı ve denetlenebilir bir erişim yönetişimi modeli sunar.
