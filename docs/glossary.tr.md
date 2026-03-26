# Sözlük - Approval Scheme Process

Bu doküman, Approval Scheme Process doküman setinde kullanılan temel terimleri tanımlar. Amaç, mimari, süreç ve politika dokümanlarında kavram birliğini sağlamaktır.

[English version](glossary.md)

## Temel Terimler

### Access Engine / Erişim Motoru

Bir talebin bağlam içi mi bağlam dışı mı olduğunu değerlendiren, güvenlik seviyesini çözen ve onay gerekip gerekmediğine karar veren ana karar bileşeni.

### Approval / Onay

Yetkili bir onaylayıcı tarafından onay akışının parçası olarak verilen ve kayda alınan resmî karar. Politika kurallarına göre izin, red veya süre dolumu sonucu üretebilir.

### Approval Engine / Onay Motoru

Onay akışlarını işleten, onay adımlarını doğru rollere yönlendiren, kararları kaydeden ve nihai sonucu Erişim Motoruna döndüren bileşen.

### Approval Request / Onay Talebi

Bir çalışanın, işlem devam etmeden önce bir veya daha fazla onay adımı gerektiren bağlam dışı bir işlem yapmaya çalışması durumunda oluşturulan talep.

### Approval Scheme / Onay Şeması

Belirli bir işlem türü veya risk seviyesi için hangi onay adımlarının geçerli olduğunu tanımlayan yapılandırılmış iş akışı.

### Approval Step / Onay Adımı

Bir onay şeması içindeki tekil kontrol noktası. Genellikle Amir, Güvenlik, Hukuk veya Veri Koruma gibi bir role atanır.

### Appointment / Randevu

İşlem türü ve ilgili hedef bilgileri gibi ilk iş bağlamını sağlayan planlı vatandaş etkileşimi.

### Audit / Denetim

Uyumluluğu doğrulamak, suistimali tespit etmek ve incelemeleri desteklemek amacıyla loglanan olayların, onayların ve erişim kararlarının gözden geçirilmesi faaliyeti.

### Context / Bağlam

Bir talebi ek onay olmadan meşru kılan iş koşulları kümesi. Örneğin geçerli randevu, aktif oturum, atanmış çalışan ve eşleşen sorgu hedefi.

### Employee / Çalışan

Kurum sistemlerinde sorgu veya operasyonel işlem yapan yetkili personel.

### Emergency Access / Acil Durum Erişimi

Normal onay süreleri dışında acil erişim gerektiğinde kullanılan kontrollü istisna yolu. Her zaman gerekçe ve sonradan inceleme gerektirir.

### In-Context Request / Bağlam İçi Talep

Geçerli bir oturum içinde ve aktif işlem bağlamıyla uyumlu olarak yapılan talep. Tüm politika kontrolleri geçilirse ek onay gerektirmez.

### Logging and Audit Layer / Loglama ve Denetim Katmanı

İstek, onay ve erişim sonucu verilerini izlenebilirlik ve sonradan inceleme için saklayan bileşen veya hizmet.

### Operation Type / İşlem Türü

Tapu satışı, vergi denetimi, doğum kaydı veya sosyal yardım incelemesi gibi iş odaklı işlem kategorisi. Güvenlik seviyesi ve onay şeması bu bilgiye göre çözülür.

### Out-of-Context Request / Bağlam Dışı Talep

Geçerli oturum olmadan, hedef bağlamı eşleşmeden veya normal işlem sınırının dışında yapılan talep. Ek kontrollere tabidir.

### Override

Açık yetki altında standart yolu aşan, politika kontrollü işlem. Override işlemleri sıkı biçimde loglanmalı ve sonradan incelenmelidir.

### Query / Sorgu

Kurumsal bir sistemde veriye erişmek, veriyi getirmek veya incelemek için yapılan talep.

### Role / Rol

Çalışan, Amir, Güvenlik Yetkilisi, Hukuk İnceleyicisi veya Veri Koruma Yetkilisi gibi kullanıcıya atanmış sorumluluk ya da yetki sınıfı.

### Security Level / Güvenlik Seviyesi

Bağlam dışı erişim için gerekli onay ve gözetim sıkılığını belirleyen, işlem türüne atanmış sınıflandırma.

### Security Level Manager / Güvenlik Seviyesi Yöneticisi

Güvenlik seviyelerini yöneten, bunları işlem türleriyle eşleştiren ve risk sınıflandırma mantığını destekleyen yapılandırma alanı.

### Session / Oturum

Randevudan üretilen ve çalışana atanan aktif iş bağlamı. Oturum, bağlam içi erişim sınırını oluşturur.

### Session Engine / Oturum Motoru

Randevu tabanlı işlerle ilişkili oturumları oluşturan, yöneten ve kapatan bileşen.

## Kullanım Notu

Bir terim birden fazla dokümanda geçtiğinde, belirli bir kullanım için açıkça daraltılmadıkça burada tanımlanan anlam esas alınmalıdır.
