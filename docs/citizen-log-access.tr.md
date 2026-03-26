# Vatandaş Log Erişimi - Approval Scheme Process

Bu doküman, bir vatandaşın veya veri sahibinin Approval Scheme Process kapsamında kendi kayıtlarına ilişkin erişim loglarını nasıl talep edip görüntüleyebileceğini tanımlar.

[English version](citizen-log-access.md) | [Uyumluluk](compliance.tr.md) | [Akışlar](flows.tr.md)

## 1. Amaç

Kamu kurumlarının, vatandaşlara kendi kayıtlarına kimlerin, ne zaman ve hangi süreç kapsamında eriştiğini kontrollü biçimde gösterebilmesi gerekebilir.

Bu kabiliyet; şeffaflık, suistimal tespiti, şikâyet yönetimi ve yerel mevzuat gerektiriyorsa veri sahibi hakları için destek sağlar.

## 2. Kapsam

Bu akış, kurum içi çalışan denetim incelemesi için değil, vatandaşa dönük erişim geçmişi sorguları için geçerlidir.

Model en az şu kabiliyetleri desteklemelidir:

- Vatandaşın kendi erişim geçmişini talep etmesi
- Açıklama öncesinde kimlik doğrulama
- Hassas iç detayların filtrelenmesi ve maskelenmesi
- Vatandaş sorgusunun kendisinin de loglanması
- Sonuçta şüpheli erişim örüntüleri varsa eskalasyon

## 3. Temel İlkeler

- Vatandaş yalnızca kendi kayıtlarına ilişkin logları görebilmelidir
- Açıklama politikayla kontrollü ve hukuki sınırlar içinde olmalıdır
- Gerektiğinde iç güvenlik açısından hassas detaylar maskelenmelidir
- Kurum, hangi bilginin açıklandığına dair savunulabilir kayıt tutmalıdır
- Şüpheli örüntüler gözetim fonksiyonlarınca incelenebilir olmalıdır

## 4. Aktörler

### Vatandaş veya Veri Sahibi

Kendi erişim geçmişine ilişkin talepte bulunur.

### Vatandaş Hizmet Kanalı

Talebi portal, çağrı merkezi, şube veya resmî başvuru süreci üzerinden alır.

### Kimlik Doğrulama Fonksiyonu

Talep sahibinin doğru kişi veya hukuken yetkili temsilci olduğunu doğrular.

### Şeffaflık veya Denetim İnceleme Fonksiyonu

İlave inceleme gerektiren istisnaları, şikâyetleri ve şüpheli sonuçları ele alır.

## 5. Vatandaş Log Erişimi Akışı

Vatandaş -> Hizmet Kanalı -> Kimlik Doğrulama -> Log Erişim Servisi -> Loglama ve Denetim -> Vatandaşa Yanıt

### Adımlar

1. Vatandaş kendi kayıtlarına ilişkin erişim geçmişini görmek için talep oluşturur.
2. Hizmet kanalı, kurumun onaylı yöntemiyle kimlik doğrulaması yapar.
3. Sistem, talep sahibinin veri sahibi veya geçerli bir yasal temsilci olup olmadığını kontrol eder.
4. Talep kapsamı doğrulanır:
   - Talep edilen tarih aralığı
   - İlgili kayıt veya dosya kapsamı
   - Talebin politika sınırlarını aşıp aşmadığı
5. Log Erişim Servisi, eşleşen olaylar için alttaki denetim kayıtlarını sorgular.
6. Yanıttan önce açıklama kuralları uygulanır:
   - İzin verilen alanlar gösterilir
   - Kısıtlı iç notlar maskelenir
   - Politika gerektiriyorsa güvenlik açısından hassas teknik metaveri çıkarılır
7. Vatandaşa filtrelenmiş sonuç onaylı yanıt kanalıyla sunulur.
8. Bu sorgunun kendisi ayrı bir denetlenebilir olay olarak loglanır.
9. Sonuç şüpheli veya ihtilaflı erişim içeriyorsa durum resmî incelemeye eskale edilebilir.

## 6. Vatandaşa Gösterilecek Asgari Sonuç Kümesi

Yanıt çoğunlukla şu bilgileri içermelidir:

- Erişim tarihi ve saati
- Kurum veya birim adı
- İşlem ya da amaç kategorisi
- Erişimin bağlam içi, onaylı, acil durum veya override olup olmadığı
- Şikâyet veya inceleme için başvuru yolu

Yanıt çoğunlukla şu bilgileri içermemeli veya kısıtlamalıdır:

- Hukuken uygun değilse tam iç gerekçeler
- Güvenlik kontrol iç detayları
- Hassas ağ veya cihaz tanımlayıcıları
- Yalnızca iç inceleyicilere açık notlar

## 7. Kimlik ve Yetkilendirme Kuralları

- Açıklama öncesinde talep sahibi güçlü biçimde tanımlanmalıdır
- Yasal temsilciler belgelendirilmiş yetki sunmalıdır
- İlgisiz kişiler üzerinde toplu açıklama engellenmelidir
- Talebin kendisi oran sınırlamasına ve izlemeye tabi olmalıdır

## 8. Eskalasyon ve Şikâyet Yönetimi

Vatandaş erişimin usulsüz olduğunu düşünüyorsa kurum en az şu kabiliyetleri desteklemelidir:

- Şikâyet kaydı oluşturma
- Şikâyeti ilgili denetim olaylarıyla ilişkilendirme
- Uyumluluk, gözetim veya teftiş fonksiyonlarınca inceleme
- Sonraki soruşturma için kanıt koruma

## 9. Loglama Gereksinimleri

Sistem en az şunları loglamalıdır:

- Vatandaş log sorgusunu kimin yaptığı
- Hangi kimlik doğrulama yönteminin kullanıldığı
- Hangi kayıtların veya dönemin talep edildiği
- Hangi açıklama kapsamının döndürüldüğü
- Eskalasyon veya şikâyet sürecinin tetiklenip tetiklenmediği

## 10. Veri Modeli ve Politika Notları

Uygulamaya geçmeden önce bu akış şu alanlarla hizalanmalıdır:

- [db/schema.sql](../db/schema.sql) içindeki denetim logu tasarımı
- [compliance.tr.md](compliance.tr.md) içindeki saklama politikası
- [governance.tr.md](governance.tr.md) içindeki gözetim ve şikâyet sahipliği
- [flows.tr.md](flows.tr.md) içindeki temel operasyonel istek akışları

Kurumlar daha sonra vatandaş talepleri, temsil yetkisi, şikâyet dosyaları ve açıklama paketleri için özel varlıklar ekleyebilir.

## 11. Özet

Vatandaş log erişimi, Approval Scheme Process modelini yalnızca iç kontrol yapısından hesap verebilir şeffaflık modeline genişletir. Bu kabiliyet sınırsız ham log erişimi olarak değil, kontrollü bir açıklama yeteneği olarak uygulanmalıdır.
