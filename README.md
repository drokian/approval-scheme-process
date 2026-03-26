# Approval Scheme Process

A general-purpose, context-aware access governance and multi-level approval framework for government institutions.

---

# English: Context-Based Access and Multi-Level Approval System

This project provides a comprehensive access governance architecture for all transactions in government institutions that:

- Allows employees to make free queries only within the context of **appointment-based transactions**,
- Ensures all out-of-context queries pass through at least one approval process according to **security levels defined by transaction type**,
- Enables the definition of **dynamic approval schemes** for each transaction type.

This project is designed to enhance personal data security, prevent abuse, and make access authorizations context-based in public institutions.

---

## Why This Project Exists

Government information systems often allow broad internal access, relying on after-the-fact auditing rather than proactive access control.  
This creates risks such as:

- Unauthorized access to personal data  
- Curiosity-driven or politically motivated queries  
- Lack of standardized approval workflows  
- Inconsistent security practices across institutions  

This project proposes a unified, context-aware, security-level–based access governance model.

---

## Who Is This For?

- Government IT architects  
- Public sector software developers  
- Security and compliance teams  
- Researchers working on digital governance  
- Open-source contributors interested in access control systems  

---

## Features

- Context-based access control  
- Security levels defined per transaction type  
- Dynamic approval scheme creation  
- Multi-level approval mechanism  
- Full logging and auditing  
- Institution-independent general architecture  

---

## High-Level Architecture

User → Appointment System → Session Engine → Access Engine → Approval Engine → Logging & Audit

---

## Roadmap

- **Phase 1:** Core documentation and database schema  
- **Phase 2:** Access Engine prototype  
- **Phase 3:** Approval Engine implementation  
- **Phase 4:** Logging & audit layer  
- **Phase 5:** Example integrations (land registry, population registry, tax office)  

---

## License

MIT License – free to use, modify, and distribute.

---

# Turkish: Bağlam Tabanlı Erişim ve Çok Kademeli Onay Sistemi

Bu proje, devlet kurumlarında yapılan tüm işlemler için:

- Memurların yalnızca **randevuya bağlı işlem bağlamında** serbest sorgu yapabilmesini,
- Bağlam dışı tüm sorguların **işlem türüne göre tanımlanmış güvenlik seviyelerine** göre en az bir onaydan geçmesini,
- Her işlem türü için **dinamik onay şemalarının** tanımlanabilmesini

sağlayan genel bir erişim yönetişim mimarisi sunar.

Bu proje, kamu kurumlarında kişisel veri güvenliğini artırmak, suistimali önlemek ve erişim yetkilerini bağlam tabanlı hale getirmek için tasarlanmıştır.

---

## Bu Proje Neden Var?

Birçok devlet bilgi sistemi, geniş iç erişime izin verir ve erişim kontrolünü proaktif olarak değil, **sonradan denetim** yoluyla sağlar.  
Bu durum şu riskleri doğurur:

- Kişisel verilere yetkisiz erişim  
- Merak veya siyasi motivasyonla yapılan sorgular  
- Standart bir onay akışının olmaması  
- Kurumlar arasında tutarsız güvenlik uygulamaları  

Bu proje, bağlam tabanlı ve güvenlik seviyesine dayalı birleşik bir erişim yönetişim modeli sunar.

---

## Kimler İçindir?

- Devlet kurumlarında çalışan BT mimarları  
- Kamu yazılım geliştiricileri  
- Güvenlik ve uyumluluk ekipleri  
- Dijital yönetişim üzerine çalışan araştırmacılar  
- Erişim kontrol sistemleriyle ilgilenen açık kaynak katkıcıları  

---

## Özellikler

- Bağlam tabanlı erişim kontrolü  
- İşlem türüne göre güvenlik seviyesi tanımlama  
- Dinamik onay şeması oluşturma  
- Çok kademeli onay mekanizması  
- Tam loglama ve denetim  
- Kurumdan bağımsız genel mimari  

---

## Yüksek Seviye Mimari

Kullanıcı → Randevu Sistemi → İşlem Motoru → Erişim Motoru → Onay Motoru → Log & Denetim

---

## Yol Haritası

- **Faz 1:** Temel dokümantasyon ve veritabanı şeması  
- **Faz 2:** Erişim Motoru prototipi  
- **Faz 3:** Onay Motoru uygulaması  
- **Faz 4:** Loglama ve denetim katmanı  
- **Faz 5:** Örnek entegrasyonlar (tapu, nüfus, vergi dairesi)  

---

## Lisans

MIT Lisansı – kullanmak, değiştirmek ve dağıtmak serbesttir.
