# Approval Scheme Process

Devlet kurumları için bağlam tabanlı erişim yönetişimi ve çok kademeli onay çerçevesi.

[English README](README.md) | [Mimari](docs/architecture.tr.md) | [English Architecture](docs/architecture.md)

## Proje Durumu

Bu depo şu anda kavramsal tasarım ve dokümantasyon aşamasındadır.

Mevcut kapsama şunlar dahildir:

- Vizyon ve problem tanımı
- Yüksek seviye sistem mimarisi
- Taslak onay ve erişim kontrol modeli
- Uygulama için ilk yol haritası

Depo artık `db/schema.sql` içinde şema taslağını ve `db/seed.sql` içinde örnek seed verisini içermektedir.

## Genel Bakış

Bu proje, kamu kurumlarındaki sistemler için şu yetenekleri sunan bir erişim yönetişimi modeli önerir:

- Memurların yalnızca randevuya bağlı işlem bağlamında serbest sorgu yapabilmesi
- Bağlam dışı sorguların, işlem türüne göre tanımlanan güvenlik seviyelerine göre onaya tabi olması
- Farklı işlem türleri için dinamik onay şemalarının tanımlanabilmesi
- Tüm erişimlerin loglanması, izlenebilirliği ve denetlenebilirliği

Amaç, kişisel verilere yetkisiz erişimi azaltmak, suistimali önlemek ve kurumları sonradan denetimden proaktif kontrole taşımaktır.

## Bu Proje Neden Var

Bir çok kamu bilgi sistemi halen geniş iç erişim tanır ve ağırlıklı olarak olay sonrası denetime dayanır. Bu durum şu riskleri doğurur:

- Kişisel verilere yetkisiz erişim
- Merak veya siyasi motivasyonla yapılan sorgular
- Tutarsız onay akışları
- Kurumlar arasında değişen güvenlik uygulamaları

Approval Scheme Process, kurumlara uyarlanabilir bağlam tabanlı ve güvenlik seviyesi odaklı bir erişim modeli sunar.

## Depo Yapısı

- `README.md`: İngilizce proje özeti
- `README.tr.md`: Türkçe proje özeti
- `docs/`: Çift dilli mimari, akış, yönetişim, uyumluluk ve veri modeli dokümanları
- `db/`: İlişkisel şema taslağı ve örnek seed verisi

## Temel Yetenekler

- Bağlam tabanlı erişim kontrolü
- İşlem türü bazlı güvenlik seviyeleri
- Dinamik onay şeması tanımlama
- Çok kademeli onay akışları
- Tam loglama ve denetim desteği
- Kurumdan bağımsız mimari

## Yüksek Seviye Mimari

Kullanıcı -> Randevu Sistemi -> Oturum Motoru -> Erişim Motoru -> Onay Motoru -> Loglama ve Denetim

## Dokümantasyon

- İngilizce mimari: [docs/architecture.md](docs/architecture.md)
- Türkçe mimari: [docs/architecture.tr.md](docs/architecture.tr.md)
- İngilizce akışlar: [docs/flows.md](docs/flows.md)
- Türkçe akışlar: [docs/flows.tr.md](docs/flows.tr.md)
- İngilizce güvenlik seviyeleri: [docs/security-levels.md](docs/security-levels.md)
- Türkçe güvenlik seviyeleri: [docs/security-levels.tr.md](docs/security-levels.tr.md)
- İngilizce sözlük: [docs/glossary.md](docs/glossary.md)
- Türkçe sözlük: [docs/glossary.tr.md](docs/glossary.tr.md)
- İngilizce yönetişim: [docs/governance.md](docs/governance.md)
- Türkçe yönetişim: [docs/governance.tr.md](docs/governance.tr.md)
- İngilizce roller ve sorumluluklar: [docs/roles-and-responsibilities.md](docs/roles-and-responsibilities.md)
- Türkçe roller ve sorumluluklar: [docs/roles-and-responsibilities.tr.md](docs/roles-and-responsibilities.tr.md)
- İngilizce uyumluluk: [docs/compliance.md](docs/compliance.md)
- Türkçe uyumluluk: [docs/compliance.tr.md](docs/compliance.tr.md)
- İngilizce veri modeli: [docs/data-model.md](docs/data-model.md)
- Türkçe veri modeli: [docs/data-model.tr.md](docs/data-model.tr.md)
- Şema taslağı: [db/schema.sql](db/schema.sql)
- Örnek seed verisi: [db/seed.sql](db/seed.sql)

## Yol Haritası

- Faz 1: Temel dokümantasyon ve veri modeli taslağı
- Faz 2: Erişim Motoru prototipi
- Faz 3: Onay Motoru uygulaması
- Faz 4: Loglama ve denetim katmanı
- Faz 5: Kamu alanları için örnek entegrasyonlar

## Lisans

MIT Lisansı. Kullanmak, değiştirmek ve dağıtmak serbesttir.
