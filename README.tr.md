# Approval Scheme Process

Devlet kurumlari icin baglam tabanli erisim yonetisimi ve cok kademeli onay cercevesi.

[English README](README.md) | [Mimari](docs/architecture.tr.md) | [English Architecture](docs/architecture.md)

## Proje Durumu

Bu depo su anda kavramsal tasarim ve dokumantasyon asamasindadir.

Mevcut kapsama sunlar dahildir:

- Vizyon ve problem tanimi
- Yuksek seviye sistem mimarisi
- Taslak onay ve erisim kontrol modeli
- Uygulama icin ilk yol haritasi

`db/schema.sql` gibi planlanan artefaktlar henuz depoya eklenmemistir.

## Genel Bakis

Bu proje, kamu kurumlarindaki sistemler icin su yetenekleri sunan bir erisim yonetisimi modeli onerir:

- Memurlarin yalnizca randevuya bagli islem baglaminda serbest sorgu yapabilmesi
- Baglam disi sorgularin, islem turune gore tanimlanan guvenlik seviyelerine gore onaya tabi olmasi
- Farkli islem turleri icin dinamik onay semalarinin tanimlanabilmesi
- Tum erisimlerin loglanmasi, izlenebilirligi ve denetlenebilirligi

Amac, kisisel verilere yetkisiz erisimi azaltmak, suistimali onlemek ve kurumlari sonradan denetimden proaktif kontrole tasimaktir.

## Bu Proje Neden Var

Bir cok kamu bilgi sistemi halen genis ic erisim tanir ve agirlikli olarak olay sonrasi denetime dayanir. Bu durum su riskleri dogurur:

- Kisisel verilere yetkisiz erisim
- Merak veya siyasi motivasyonla yapilan sorgular
- Tutarsiz onay akislar
- Kurumlar arasinda degisen guvenlik uygulamalari

Approval Scheme Process, kurumlara uyarlanabilir baglam tabanli ve guvenlik seviyesi odakli bir erisim modeli sunar.

## Depo Yapisi

- `README.md`: Ingilizce proje ozeti
- `README.tr.md`: Turkce proje ozeti
- `docs/architecture.md`: Ingilizce mimari dokumani
- `docs/architecture.tr.md`: Turkce mimari dokumani

## Temel Yetenekler

- Baglam tabanli erisim kontrolu
- Islem turu bazli guvenlik seviyeleri
- Dinamik onay semasi tanimlama
- Cok kademeli onay akislari
- Tam loglama ve denetim destegi
- Kurumdan bagimsiz mimari

## Yuksek Seviye Mimari

Kullanici -> Randevu Sistemi -> Oturum Motoru -> Erisim Motoru -> Onay Motoru -> Loglama ve Denetim

## Dokumantasyon

- Ingilizce mimari: [docs/architecture.md](docs/architecture.md)
- Turkce mimari: [docs/architecture.tr.md](docs/architecture.tr.md)

## Yol Haritasi

- Faz 1: Temel dokumantasyon ve veri modeli taslagi
- Faz 2: Erisim Motoru prototipi
- Faz 3: Onay Motoru uygulamasi
- Faz 4: Loglama ve denetim katmani
- Faz 5: Kamu alanlari icin ornek entegrasyonlar

## Lisans

MIT Lisansi. Kullanmak, degistirmek ve dagitmak serbesttir.
