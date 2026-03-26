# Veri Modeli - Approval Scheme Process

Bu doküman, Approval Scheme Process sisteminin kavramsal veri modelini tanımlar. Amaç, mimari dokümanlar ile `db/schema.sql` altındaki ilişkisel şema taslağı arasındaki boşluğu kapatmaktır.

[English version](data-model.md)

## 1. Amaç

Veri modeli aşağıdaki ihtiyaçları desteklemek için gereken asgari varlık kümesini tanımlar:

- Randevu tabanlı oturum bağlamı
- Bağlam dışı erişim değerlendirmesi
- Güvenlik seviyesi çözümleme
- Dinamik onay akışları
- Loglama ve denetlenebilirlik

## 2. Temel Varlıklar

### User

Sistemi kullanan çalışanı veya kurumsal aktörü temsil eder.

Örnek temel alanlar:

- Çalışan kimliği
- Görünen ad
- İletişim bilgileri
- Durum bilgisi

### Role

Çalışan, Amir, Güvenlik Yetkilisi veya Denetçi gibi kullanıcıya atanmış yetki sınıfını temsil eder.

### UserRole

Kullanıcılarla rolleri eşler ve gerektiğinde süreli rol atamalarını destekler.

### OperationType

Tapu satışı, vergi denetimi, doğum kaydı veya sosyal yardım incelemesi gibi iş odaklı işlem kategorisini temsil eder.

### SecurityLevel

İşlem türüne atanmış ve onay sıkılığını belirleyen sınıflandırmayı temsil eder.

### ApprovalScheme

Belirli bir işlem türü için geçerli onay zincirini tanımlar.

### ApprovalSchemeStep

Onay zinciri içindeki tekil adımı; rolü, sıralamayı ve zorunluluk bayraklarını tanımlar.

### Appointment

İlk iş bağlamını oluşturan planlı vatandaş etkileşimini temsil eder.

### AppointmentTarget

Randevuyla ilişkili hedef veriyi; örneğin vatandaş, varlık, kayıt veya dosyayı temsil eder.

### Session

Randevudan türetilen ve çalışana atanan aktif iş bağlamını temsil eder.

### Query

Çalışan tarafından yapılan veri erişim talebini temsil eder. Sorgu bağlam içi veya bağlam dışı olabilir.

### ApprovalRequest

Onay gerektiren sorgular için oluşturulan onay akışı örneğini temsil eder.

### Approval

Onay akışı içindeki tekil onaylayıcı kararını temsil eder.

### AuditLog

Erişim, onay ve yönetim olaylarına ilişkin kanıt zincirini temsil eder.

## 3. İlişki Özeti

Temel ilişkiler şunlardır:

- Bir User birden fazla Role sahip olabilir
- Bir OperationType bir SecurityLevel ile eşleşir
- Bir OperationType zaman içinde bir veya daha fazla ApprovalScheme kullanabilir
- Bir ApprovalScheme bir veya daha fazla ApprovalSchemeStep içerir
- Bir Appointment bir veya daha fazla AppointmentTarget içerebilir
- Bir Appointment bir Session üretebilir
- Bir Session bir User'a atanır
- Bir Query bir User tarafından talep edilir
- Bir Query bir Session'a referans verebilir
- Bir Query bir ApprovalRequest oluşturabilir
- Bir ApprovalRequest bir veya daha fazla Approval kaydı içerir

## 4. Yaşam Döngüsü Notları

### Randevudan Oturuma

Randevu planlı iş olarak başlar ve etkileşim başladığında aktif oturuma dönüşür.

### Sorgudan Onaya

Bağlam içi sorgu politika kontrolleri geçilirse doğrudan çalıştırılabilir.

Bağlam dışı sorgu sınıflandırılır, güvenlik seviyesi atanır ve gerekirse onay talebi oluşturur.

### Onaydan Denetime

Her onay işlemi ve her nihai erişim kararı denetim izine yansıtılmalıdır.

## 5. Bütünlük Kuralları

Veri modeli aşağıdaki kuralları desteklemelidir:

- Hedef, oturum bağlamıyla eşleşmiyorsa sorgu bağlam içi işaretlenemez
- Talep sahibi aynı onay adımının onaylayıcısı olamaz
- Onay adımı sırası aynı şema içinde benzersiz olmalıdır
- Bir oturum aynı anda birden fazla aktif çalışana atanamaz
- Güvenlik seviyesi tanımlanmadan işlem türüne atanamaz

## 6. Genişletilebilirlik Notları

Kurumlar daha sonra aşağıdaki alanlar için ek varlıklar ekleyebilir:

- Vekalet
- Acil durum erişimi
- Override incelemesi
- Olay yönetimi
- Risk puanlama
- Harici sistem bağlayıcıları

## 7. Şema Eşlemesi

Bu kavramsal modelin ilişkisel taslak uygulaması [db/schema.sql](/d:/source/drokian/approval-scheme-process/db/schema.sql) içinde yer alır.

Gösterim ve erken aşama testleri için örnek başlangıç verisi [db/seed.sql](/d:/source/drokian/approval-scheme-process/db/seed.sql) içinde sağlanır.
