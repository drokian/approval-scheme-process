# Oturum ve Token Süre Sonu Politikası - Approval Scheme Process

Bu doküman, Approval Scheme Process kapsamında oturum ömrü, token ömrü, süre sonu davranışı ve ilişkili kontrol tepkileri için asgari politikayı tanımlar.

[English version](session-and-token-expiry.md) | [Mimari](architecture.tr.md) | [Kenar Durumlar](edge-cases.tr.md)

## 1. Amaç

Approval Scheme Process, aktif iş oturumlarının ve kimlik doğrulama ya da yetkilendirme tokenlarının güvenilir biçimde kontrol edilmesine dayanır. Açık bir süre sonu politikası olmadan bağlam içi erişim sınırı zayıflar, denetlenebilirlik belirsizleşir ve gözetimsiz kalan oturumlar suistimal riski oluşturur.

Bu doküman, uygulamaya geçmeden önce gerekli olan asgari politika beklentilerini tanımlar.

## 2. Kapsam

Bu politika şu alanlara uygulanır:

- Randevuya dayalı iş oturumları
- Kullanıcı kimlik doğrulama oturumları
- Token tabanlı kimlik doğrulama kullanılıyorsa access token ve refresh token yapıları
- Onaylayıcıların kullandığı inceleme oturumları
- Kullanıcı arayüzü ve arka uç servislerindeki timeout ve süre sonu davranışları

## 3. Temel İlkeler

- Oturum geçerliliği süresiz değil, süreyle sınırlı olmalıdır
- Token ömrü riskle orantılı olmalıdır
- Süresi dolmuş oturumlar ve tokenlar fail-closed çalışmalıdır
- Hareketsizlik veya uzun süre sonunda hassas işlemler için yeniden kimlik doğrulama gerekmelidir
- Süre sonu olayları loglarda görünür ve denetimde incelenebilir olmalıdır

## 4. Oturum Türleri

### Randevu İş Oturumu

Çalışanın randevuya dayalı etkileşim sırasında kullandığı aktif iş bağlamı.

### Kullanıcı Kimlik Doğrulama Oturumu

Çalışanın kurumsal sistemlere kimliğini kanıtlayan oturumudur.

### Onay İnceleme Oturumu

Onaylayıcının bekleyen onay taleplerini incelerken kullandığı oturumdur.

## 5. Önerilen Oturum Politikası

### Randevu İş Oturumu Kuralları

- Oturum izin verilen erken başlatma penceresinden önce aktif olmamalıdır
- Kısa ve kontrollü bir grace period tanımlanmadıkça randevu bittiğinde oturum otomatik kapanmalıdır
- Randevu iptal edildiğinde veya gelmedi durumuna düştüğünde oturum derhal kapanmalıdır
- Kapanmış veya süresi dolmuş oturum bağlam içi erişim için geçersiz sayılmalıdır

### Boşta Kalma Zaman Aşımı

- Çalışan arayüzündeki oturumlar tanımlı bir hareketsizlik süresinden sonra sona ermelidir
- Yüksek riskli arayüzlerde daha kısa boşta kalma süreleri kullanılmalıdır
- Boşta kalma süresi sonrasında erişimin devamı için yeniden kimlik doğrulama gerekmelidir

### Mutlak Yaşam Süresi

- Kullanıcı aktif kalsa bile oturumların bir azami yaşam süresi olmalıdır
- Uzun süren oturumlar sonsuza kadar açık kalmamalı, yenileme gerektirmelidir

## 6. Token Politikası

Uygulama token kullanıyorsa kurumlar en az şu ayrımı yapmalıdır:

### Access Token

- Kısa ömürlü olmalıdır
- Normal istek yetkilendirmesinde kullanılmalıdır
- Elle iptale bağlı kalmadan otomatik olarak süresi dolmalıdır

### Refresh Token

- Access tokendan daha uzun ömürlü olmalıdır
- Daha sıkı korunmalı ve saklanmalıdır
- Bağımsız şekilde iptal edilebilmelidir
- Çıkış, ihlal veya politika gereği yeniden kimlik doğrulama halinde geçersiz kılınmalıdır

### Step-Up veya Yeniden Kimlik Doğrulama Tokenı

- Override, acil erişim veya yüksek riskli talep onayı gibi özellikle hassas işlemlerde kullanılmalıdır
- Çok kısa ömürlü olmalıdır
- Yakın zamanda yapılmış bir kimlik doğrulama olayına bağlı olmalıdır

## 7. Süre Sonu Tetikleyicileri

Sistem asgari olarak şu durumlarda süre sonu veya geçersizleştirme desteklemelidir:

- Boşta kalma zaman aşımı
- Mutlak oturum süresinin dolması
- Randevu bitiş zamanına ulaşılması
- Manuel çıkış
- Parola veya kimlik bilgisi sıfırlama
- Yönetsel pasifleştirme
- Erişimin artık geçerli olmamasına yol açan rol kaldırımı
- Token ihlali veya oturum ele geçirilmesi şüphesi

## 8. Süre Sonu Davranışı

Oturum veya token süresi dolduğunda:

1. İlgili istek artık geçerli kimlik bilgisi ya da bağlam altında tamamlanamıyorsa güvenli biçimde başarısız olmalıdır.
2. Arayüz, nedenin boşta kalma, mutlak süre sonu veya randevu/oturum kapanışı olduğunu kullanıcıya açıkça göstermelidir.
3. Bağlam içi erişim sessizce bağlam dışı erişime düşmemelidir.
4. Yarım kalmış onay veya veri erişim girişimi yeni ve yetkili bir deneme gerektirmelidir.
5. Olay daha sonra incelenebilecek yeterli ayrıntıyla loglanmalıdır.

## 9. Bağlam İçi Erişim Etkileri

Randevuya dayalı erişimde oturum süresinin dolması doğrudan kontrol etkisi yaratır:

- İş oturumu sona ermişse bağlam içi sınıflandırma artık mümkün olmamalıdır
- Yeni oturum, randevu durumu ve atama bilgisinin yeniden doğrulanmasını gerektirmelidir
- Oturum süresi dolduktan sonra başlatılan sorgular eski bağlam altında çalıştırılmamalı; politika gereğine göre reddedilmeli veya yeniden sınıflandırılmalıdır

## 10. Onay Akışı Etkileri

Onay adımı zaman aşımı ile kullanıcı oturumu zaman aşımı farklı kontrollerdir ve karıştırılmamalıdır.

- Onay adımı zaman aşımı, iş onayının ne kadar süre bekleyebileceğini belirler
- Kullanıcı oturumu zaman aşımı, onaylayıcının yeniden kimlik doğrulamadan ne kadar süre oturum açık tutabileceğini belirler
- Süresi dolmuş onaylayıcı oturumu, onay talebini kendiliğinden expire etmemelidir
- Süresi dolmuş onay talebi, kullanıcı tekrar giriş yaptı diye yeniden açılmamalıdır

## 11. Loglama Gereksinimleri

Sistem en az şu olayları loglamalıdır:

- Oturum oluşturma
- Oturum kapatma
- Boşta kalma nedeniyle süre sonu
- Mutlak süre sonu
- Token yenileme ve token iptal olayları
- Hassas işlemler için yeniden kimlik doğrulama
- Politika nedeniyle engellenen süresi dolmuş istek denemeleri

## 12. Veri Modeli ve Uygulama Notları

Uygulamaya geçmeden önce bu politika şu alanlarla hizalanmalıdır:

- [flows.tr.md](flows.tr.md) içindeki oturum yaşam döngüsü
- [edge-cases.tr.md](edge-cases.tr.md) içindeki onay zaman aşımı davranışı
- [db/schema.sql](/d:/source/drokian/approval-scheme-process/db/schema.sql) içindeki şema taslağı

Mevcut şema taslağı artık oturum süre sonu, son aktivite, geçersizleştirme nedeni ve atama geçmişini temsil edebilmektedir. Yine de ileride şu alanlara ihtiyaç duyulabilir:

- Token veya kimlik doğrulama olayı takibi
- Daha güçlü token iptal kanıtı
- Step-up doğrulama için oturum-kimlik doğrulama bağı

## 13. Özet

Oturum ve token süre sonu politikası yalnızca teknik bir güvenlik ayarı değildir. Bu modelde doğrudan erişim kontrol sınırının parçasıdır. Güçlü süre sonu yönetimi, bağlam içi erişimi güvenilir tutar, eski oturum suistimalini azaltır ve denetim savunulabilirliğini güçlendirir.