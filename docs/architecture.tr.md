# Mimari - Approval Scheme Process

Bu dokuman, Approval Scheme Process sisteminin yuksek seviye mimarisini, ana bilesenlerini, karar akislarini ve genisleme noktalarini tanimlar.

[English version](architecture.md)

## 1. Genel Bakis

Approval Scheme Process, devlet kurumlari icin tasarlanmis baglam tabanli bir erisim yonetisimi modelidir.

Model dort temel fikir uzerine kuruludur:

- Serbest erisim yalnizca gecerli bir randevuya dayanan aktif oturum baglaminda verilir
- Baglam disi erisimler guvenlik seviyelerine gore degerlendirilir
- Onay akislari islem turu bazinda tanimlanir
- Her karar izlenebilirlik ve denetim icin loglanir

Model kurumdan bagimsizdir ve tapu, nufus, vergi ve sosyal hizmetler gibi alanlara uyarlanabilir.

## 2. Temel Ilkeler

### 2.1 Baglam Tabanli Erisim

Calisanlar yalnizca su kosullarin tamami saglandiginda serbest sorgu yapabilir:

- Gecerli bir randevu vardir
- Randevu aktif bir oturuma donusturulmustur
- Calisan bu oturuma atanmistir
- Sorgu hedefi oturum baglamiyla eslesmektedir

### 2.2 Guvenlik Seviyesi Odakli Erisim

Tapu satisi, dogum kaydi veya vergi denetimi gibi her islem turune bir guvenlik seviyesi atanir.

Guvenlik seviyeleri sunlari belirler:

- Onay gerekip gerekmedigi
- Kac onay adimi gerektigi
- Hangi rollerin onay verecegi
- Ilave kontrollerin uygulanip uygulanmayacagi

### 2.3 Dinamik Onay Semalari

Her islem turu kendi onay akisina sahip olabilir.

Ornek kaliplar:

- Seviye 1 -> Amir onayi
- Seviye 2 -> Amir ve Guvenlik onayi
- Seviye 3 -> Amir, Hukuk ve Veri Koruma onayi
- Seviye 4 -> Coklu onay ve ozel yetkilendirme

### 2.4 Tam Loglama ve Denetim

Her sorguda su bilgiler kaydedilmelidir:

- Islemi yapan kisi
- Islem zamani
- Oturum veya istek baglami
- Islem turu ve guvenlik seviyesi
- Onay durumu ve onay gecmisi
- Varsa cihaz ve ag metaverisi

## 3. Yuksek Seviye Mimari

Kullanici -> Randevu Sistemi -> Oturum Motoru -> Erisim Motoru -> Onay Motoru -> Loglama ve Denetim

### 3.1 Randevu Sistemi

Sorumluluklar:

- Vatandas randevularini yonetmek
- Talep edilen islem turunu belirlemek
- Ilk is baglamini saglamak
- Randevu zamaninda oturum olusumunu tetiklemek

### 3.2 Oturum Motoru

Sorumluluklar:

- Randevulari aktif oturumlara donusturmek
- Oturumlari calisanlara atamak
- Aktif, beklemede ve kapali gibi yasam dongusu durumlarini yonetmek
- Erisim dogrulamasi icin baglam saglamak

### 3.3 Erisim Motoru

Bu bilesen ana karar mekanizmasidir.

Sorumluluklar:

- Bir sorgunun baglam ici mi baglam disi mi oldugunu kontrol etmek
- Rol ve oturum kisitlarini degerlendirmek
- Talep edilen islem icin guvenlik seviyesini belirlemek
- Onay gerekip gerekmedigine karar vermek
- Onay gerektiren talepleri Onay Motoruna yonlendirmek
- Gerekli baglam veya yetki yoksa hizli sekilde reddetmek

### 3.4 Onay Motoru

Sorumluluklar:

- Islem icin tanimli onay semasini getirmek
- Onay adimlarini tanimli sira ile baslatmak
- Onaylayicilari bilgilendirmek
- Onay kararlarini kaydetmek
- Sonucu Erisim Motoruna izin veya red olarak dondurmek

### 3.5 Guvenlik Seviyesi Yoneticisi

Sorumluluklar:

- Guvenlik seviyelerini olusturmak ve guncellemek
- Guvenlik seviyelerini islem turleriyle eslestirmek
- Risk siniflandirma mantigini surdurmek

### 3.6 Onay Semasi Yoneticisi

Sorumluluklar:

- Onay adimlarini, rolleri ve siralamayi tanimlamak
- Gerekirse kosullu dallanmalari desteklemek
- Sema tutarliligini dogrulamak
- Onay zincirini Onay Motoruna saglamak

### 3.7 Loglama ve Denetim Katmani

Sorumluluklar:

- Erisim ve onay loglarini saklamak
- Denetim raporlarini desteklemek
- Ileride eklenecek anomali tespitine zemin hazirlamak
- KVKK ve benzeri uyumluluk gereksinimlerine destek olmak

## 4. Karar Sirasi

Kontrol modelinin ongorulebilir olmasi icin Erisim Motoru talepleri su sirayla degerlendirmelidir:

1. Oturum ve baglam dogrulamasi
2. Calisan ve rol yetkilendirmesi
3. Islem turu tespiti
4. Guvenlik seviyesi belirleme
5. Onay gereksinimi karari
6. Izin ver, reddet veya onay akisina gonder
7. Istek ve sonucun loglanmasi

Bu sira, onaya gecmeden once hangi kontrollerin zorunlu oldugunu acik hale getirir.

## 5. Bilesen Etkilesimi

### 5.1 Baglam Ici Sorgu Akisi

Calisan -> Oturum Motoru -> Erisim Motoru -> Loglama -> Izin

Adimlar:

1. Calisan aktif bir oturum icinde sorgu yapar
2. Oturum Motoru guncel oturum baglamini saglar
3. Erisim Motoru hedefin oturum ve rol kisitlariyla uyumlu oldugunu dogrular
4. Istek baglam ici olarak siniflandirilir
5. Ek onay olmadan sorguya izin verilir
6. Istek ve sonuc loglanir

### 5.2 Baglam Disi Sorgu Akisi

Calisan -> Erisim Motoru -> Onay Motoru -> Loglama -> Izin veya Red

Adimlar:

1. Calisan gecerli oturum olmadan veya uyumsuz baglamla sorgu yapar
2. Erisim Motoru istegi baglam disi olarak siniflandirir
3. Islem turu ve guvenlik seviyesi belirlenir
4. Onay semasi yuklenir
5. Onay akisi calistirilir
6. Onay verilirse sorguya izin verilir
7. Reddedilirse veya sure asimina ugrarsa sorgu reddedilir
8. Istek, onay sonucu ve nihai sonuc loglanir

## 6. Istisnalar ve Kenar Durumlar

Mimari su durumlari acikca tanimlamalidir:

- Gerekceli acil durum erisimi
- Onay zaman asimi veya onay suresinin dolmasi
- Onaylayicinin yoklugu veya devir senaryolari
- Zorunlu sonradan inceleme ile manuel override
- Tekrarlayan red talepleri
- Mesai disi saatlerde yuksek riskli erisim

Bu durumlar kamu ortami icin ozellikle onemlidir.

## 7. Veri Modeli Durumu

Yuksek seviyede planlanan temel varliklar sunlardir:

- User
- Appointment
- Session
- OperationType
- SecurityLevel
- ApprovalScheme
- ApprovalStep
- Query
- Approval

Detayli sema ileride `db/schema.sql` altinda eklenmesi planlanan bir artefakttir; su anda depoda bulunmamaktadir.

## 8. Genisletilebilirlik

Mimari su sekilde tasarlanmistir:

### Moduler

Her bilesen birbirinden bagimsiz olarak degistirilebilir veya genisletilebilir.

### Kurumdan Bagimsiz

Her kurum su alanlari kendi ihtiyacina gore tanimlayabilir:

- Kendi islem turleri
- Kendi guvenlik seviyeleri
- Kendi onay semalari

### Teknolojiden Bagimsiz

Arka uc su teknolojilerle uygulanabilir:

- Python
- Node.js
- Java
- Go
- .NET

### Gelecek Genislemeleri

- Anomali tespiti
- Gercek zamanli risk puanlamasi
- Kurumlar arasi erisim federasyonu
- Zero-trust entegrasyonu

## 9. Ozet

Approval Scheme Process sunlari saglar:

- Birlesik bir erisim yonetisimi modeli
- Daha guclu kisisel veri korumasi
- Yetkisiz erisimin onlenmesi
- Kuruma ozel esnek onay akislari
- Tam izlenebilirlik ve denetlenebilirlik

Bu dokuman, devlet sistemlerinde guvenli ve baglam tabanli erisim kontrolu uygulamak icin kavramsal temeli tanimlar.
