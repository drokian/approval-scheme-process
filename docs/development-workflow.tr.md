# Geliştirme İş Akışı - Approval Scheme Process

Bu doküman, Approval Scheme Process deposundaki uygulama geliştirme çalışmaları için branch stratejisini, commit standardını, pull request beklentilerini ve test katmanlarını tanımlar.

[English version](development-workflow.md) | [Katkı Rehberi](../CONTRIBUTING.md)

## 1. Amaç

Depo, dokümantasyon odaklı tasarım aşamasından yapılandırılmış uygulama planlamasına geçmektedir. Paralel çalışmaları stabil ve incelenebilir tutmak için geliştirme sürecinin ortak bir iş akışı izlemesi gerekir.

Bu doküman, branch, commit, pull request ve testler için temel çalışma standardını sunar.

## 2. Branch Stratejisi

### `main`

- Stabil branch'tir
- Sprint sonunda gözden geçirilmiş son çıktıyı temsil eder
- Doğrudan push yapılamaz
- Sprint sınırlarında `develop` üzerinden güncellenir

### `develop`

- Aktif entegrasyon branch'idir
- Tamamlanmış feature, fix ve dokümantasyon çalışmalarını alır
- Mevcut sprintin çalışma tabanı olarak kullanılır

### `feature/[issue-no]-[short-description]`

- Tek bir issue ile ilişkili yeni özellik veya geliştirme işi için kullanılır
- Hedef merge branch'i `develop` olur

Örnek:

- `feature/12-session-engine-context-check`

### `fix/[issue-no]-[short-description]`

- Aktif geliştirme sırasında bulunan normal bug düzeltmeleri için kullanılır
- Hedef merge branch'i `develop` olur

### `hotfix/[issue-no]-[short-description]`

- Yalnızca stabil branch'i etkileyen production-kritik düzeltmeler için kullanılır
- `main` üzerinden açılır
- `main`e merge edilir
- Ardından `develop`e back-merge yapılır

### `docs/[issue-no]-[short-description]`

- Sadece dokümantasyon odaklı işler için kullanılır
- Normalde hedef merge branch'i `develop` olur
- README, mimari, akış, yönetişim veya diyagram güncellemelerinde kullanılabilir

## 3. Branch Akış Kuralları

- `feature/*` -> `develop`
- `fix/*` -> `develop`
- `docs/*` -> `develop`
- `develop` -> `main` sprint sonunda
- `hotfix/*` -> `main`, ardından `main` -> `develop` back-merge
- `main`e doğrudan push yok

## 4. Commit Mesaj Standardı

Conventional Commits kullanılmalıdır.

Önerilen tipler:

- `feat`
- `fix`
- `docs`
- `test`
- `refactor`
- `chore`
- `build`
- `ci`

Önerilen format:

`type(scope): short description`

Örnekler:

- `feat(session): validate appointment context on query`
- `fix(access): resolve null session edge case on expired token`
- `test(approval): add step rejection flow unit tests`
- `docs(schema): align SessionAssignment FK references`
- `refactor(logging): extract audit writer to interface`

## 5. Pull Request Standardı

Tüm uygulama geliştirmeleri pull request üzerinden gönderilmelidir.

Her PR en az şunları içermelidir:

- Bağlı issue numarası
- Değişikliğin açık özeti
- Karşılanan acceptance criteria maddeleri
- Test coverage notu
- Gerekiyorsa çift dilli dokümantasyon güncellendi mi bilgisi

## 6. Merge Yaklaşımı

Önerilen merge davranışı:

- `feature/*`, `fix/*` ve `docs/*` -> `develop`: tercihen squash merge
- `develop` -> `main`: sprint sonunda kontrollü merge
- `hotfix/*` -> `main`: gözden geçirilmiş doğrudan merge, ardından `develop`e hemen back-merge

## 7. Test Katmanları

### Unit Tests

- Framework: `xUnit`
- Amaç: engine ve domain mantığının izole doğrulanması
- Kapsam: session kuralları, access değerlendirmesi, approval kararları, expiry davranışı, logging yardımcıları

### Integration Tests

- Framework: `WebApplicationFactory`
- Amaç: API davranışının ve endpoint sözleşmelerinin doğrulanması
- Kapsam: request işleme, approval endpoint'leri, audit endpoint'leri, citizen log inquiry endpoint'leri

### Architecture Tests

- Framework: `NetArchTest`
- Amaç: katman bağımlılık kurallarının korunması
- Kapsam: application, domain, infrastructure ve API sınırları

## 8. Asgari PR Kontrol Listesi

Merge öncesinde en az şunlar doğrulanmalıdır:

- Build başarılı
- İlgili testler geçti
- Yeni veya değişen davranış bir issue ile ilişkilendirildi
- Acceptance criteria gözden geçirildi
- Davranış veya politika değişiyorsa dokümantasyon güncellendi
- İngilizce ve Türkçe dokümanlar, her ikisi de varsa, hizalandı

## 9. Sprint Kullanım Notları

- `develop`, sprint entegrasyon incelemesi için yeterince stabil kalmalıdır
- Büyük işler issue boyutunda branch'lere bölünmelidir
- Demo akışları ilgili issue ve PR'larla eşleştirilmelidir
- Production-kritik düzeltmeler `hotfix/*` kuralını aşmamalıdır

## 10. Özet

Bu iş akışı, projenin dokümantasyondan koda geçerken kontrollü ilerlemesini sağlar. Amaç süreç yükü yaratmak değil; izlenebilirlik, inceleme kalitesi ve temiz sprint teslimidir.
