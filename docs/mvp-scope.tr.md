# MVP Kapsamı - Approval Scheme Process

Bu doküman, projenin ilk uygulama hedefini tanımlar. MVP'nin amacı, çekirdek kontrol modelini anlamlı olan en küçük backend dilimiyle uçtan uca kanıtlamaktır.

[English version](mvp-scope.md) | [Geliştirme İş Akışı](development-workflow.tr.md)

## 1. MVP Amacı

MVP aşağıdaki tek tam akışı göstermelidir:

`Yetkisiz sorgu -> Seviye 2 onay zinciri -> Denetim logu`

Bu akış, mimarinin uygulanabilir, test edilebilir ve gözden geçirilebilir olduğunu gösteren en küçük anlamlı dilimdir.

## 2. MVP'ye Dahil Olanlar

MVP aşağıdaki bileşenleri içerir:

- Oturum Motoru bağlam kontrolü
- Erişim Motoru bağlam dışı tespiti
- Talep edilen işlem için güvenlik seviyesi çözümleme
- Seviye 2 şema kullanan Onay Motoru
- İki onay adımı: `Supervisor` ve `Security Officer`
- Nihai sonuç için denetim logu yazımı
- Demo ve test çalıştırması için minimal REST API

## 3. MVP Dışında Kalanlar

MVP aşağıdaki konuları kapsamaz:

- Acil durum erişimi
- Vatandaşın kendi logunu sorgulama akışı
- Frontend kullanıcı arayüzü
- Token ve oturum süre sonu zorlaması
- Vekalet mekanizması
- Tekrarlanan ret tespiti
- Gelişmiş bildirim veya eskalasyon davranışı

Bu maddeler yol haritasında yerini korur, ancak ilk çalışan backend sürümünü geciktirmemelidir.

## 4. Beklenen Kullanıcı Hikayesi

Bir kamu çalışanı, aktif randevu bağlamının dışında bir sorgu yapmaya çalışır.

Sistem:

1. Talebin bağlam dışı olduğunu tespit eder
2. İstek için güvenlik seviyesini çözümler
3. Talebi Seviye 2 onay şemasına yönlendirir
4. Onay adımlarını sırayla çalıştırır
5. İzin veya ret kararı üretir
6. Sonucu denetim loguna yazar

## 5. MVP Başarı Kriterleri

Aşağıdaki koşullar sağlandığında MVP tamamlanmış sayılır:

- Tüm akış API çağrıları üzerinden çalışır
- Onay zinciri istek bazlı sabit kod yerine veri odaklıdır
- Nihai karar denetim loguna yazılır
- Unit ve integration testleri çekirdek yolu kapsar
- Mimari katman sınırları temiz kalır

## 6. Teknik Sınırlar

Hedef backend teknoloji yığını:

- `C#`
- `.NET 9`
- REST API
- EF Core

Önerilen mimari:

- `Domain`: entity'ler, enum'lar, value object'ler, çekirdek kurallar
- `Application`: engine'ler, use case'ler, interface'ler
- `Infrastructure`: EF Core, kalıcılık, repository implementasyonları
- `Api`: minimal endpoint'ler ve taşıma katmanı sorumlulukları

## 7. Bu Kapsam Neden Doğru

Bu kapsam bilerek dardır. Projenin ana vaadini, ikincil özellikleri erken dahil etmeden doğrular.

Bu akış sağlam hale geldikten sonra süre sonu zorlaması, acil durum erişimi, vatandaş şeffaflık akışları ve frontend deneyimi test edilmiş backend temelinin üzerine güvenle eklenebilir.
