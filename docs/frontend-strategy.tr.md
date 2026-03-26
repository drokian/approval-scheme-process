# Frontend Stratejisi - Approval Scheme Process

Bu doküman, frontend tarafındaki mevcut kararı ve bunun gerekçesini kayda alır.

[English version](frontend-strategy.md) | [MVP Kapsamı](mvp-scope.tr.md)

## 1. Mevcut Karar

Frontend çalışması, backend `S5` sprinti tamamlanmadan başlamamalıdır.

Gerekçe:

- MVP backend önceliklidir
- Ana risk alanı arayüz tasarımı değil domain ve akış doğruluğudur
- Engine'ler ve API sözleşmeleri halen otururken erken UI geliştirmek gereksiz yeniden iş çıkartır

## 2. Aday Seçenekler

### Seçenek A - Blazor

Güçlü yanları:

- `.NET` merkezli teknoloji yığınına güçlü uyum sağlar
- Ürün uzun vadede Microsoft ağırlıklı kamu ortamında kalacaksa iyi bir uyum sunar
- Ortak dil ve araç seti ileride ekip işletimini kolaylaştırabilir

Sınırları:

- React'e göre daha küçük bir frontend ekosistemi vardır
- Demo cilası ve hızlı UI denemeleri daha yavaş olabilir

### Seçenek B - React + Vite + Tailwind

Güçlü yanları:

- Cilalı bir demo arayüzü için en hızlı yoldur
- Bileşen yapısı ve testleme için güçlü bir ekosisteme sahiptir
- API ile istemci arasında net bir ayrım kurar

Sınırları:

- İkinci bir ana dil ve araç zinciri ekler
- Mimari yüzeyi daha erken genişletir

## 3. Öneri

Mevcut aşama için:

- Resmi MVP kapsamını backend odaklı tut
- Nihai frontend stack kararını backend `S5` sonrasına ertele

Eğer bir sonraki hedef hızlı bir demo ise `React + Vite + Tailwind` tercih edilmelidir.

Eğer bir sonraki hedef uzun vadeli kurumsal uyum ve `.NET` ağırlıklı teslim modeli ise `Blazor` tercih edilmelidir.

## 4. Frontend Başlangıç Kriteri

Frontend planlaması ancak aşağıdaki alanlar oturduğunda başlamalıdır:

- Oturum bağlam sözleşmesi
- Erişim değerlendirme response yapısı
- Onay aksiyonu endpoint'leri
- Demo görünürlüğü için audit log çıktı modeli

## 5. Önerilen İlk Frontend Dilimi

Frontend başladığında ilk arayüz çok dar kapsamda tutulmalıdır:

- Sorgu talep ekranı
- Bağlam dışı karar görünümü
- Onay zinciri durum görünümü
- Nihai audit log sonucu görünümü

Bu sayede frontend çalışması, backend için tanımlanmış aynı tekil MVP akışıyla hizalı kalır.
