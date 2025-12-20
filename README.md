MIT License

Copyright (c) [2025] [Nursima Betül BALLÝ]

SAKARYA ÜNİVERSİTESİ
BİLGİSAYAR VE BİLİŞİM BİLİMLERİ FAKÜLTESİ
BİLGİSAYAR MÜHENDİSLİĞİ BÖLÜMÜ

BSM311- WEB PROGRAMLAMA DERSİ
2025-2026 GÜZ DÖNEMİ
PROJE ÖDEVİ

Proje GitHub linki: https://github.com/nursimabetul/FitnessCenterReservationSystem



2. Öğretim A
Nursima Betül BALLİ
Öğrenci No: G211210553

Proje Özeti
Fitness Center Reservation System, fitness salonları için kullanıcı yönetimi ve randevu sistemini modern web teknolojileri ile geliştirmeyi amaçlayan bir projedir. Projede ASP.NET Core MVC (.NET LTS) teknolojisi temel alınmış, Entity Framework Core ORM ile veritabanı işlemleri yönetilmiş ve SQL Server kullanılmıştır. Kimlik ve rol yönetimi ASP.NET Core Identity ile sağlanmış olup, frontend tarafında Razor Views ve Bootstrap 5 kullanılmıştır. Proje mimarisi, MVC yapısı ve ViewModel kullanımı ile esnek ve genişletilebilir şekilde tasarlanmıştır. Yetkilendirme, Admin, Üye ve Antrenör rolleri bazında yapılmaktadır.

Kullanıcı ve Kimlik Yönetimi
Kullanıcı yönetiminde ApplicationUser sınıfı, IdentityUser’dan türetilmiş olup tüm kullanıcılar tek tabloda tutulmuş ve roller ile ayrılmıştır. Kullanıcı verileri arasında ad, soyad, doğum tarihi, boy, kilo, SalonId ve onay durumu gibi alanlar bulunmaktadır. Ayrıca kullanıcılar için UyeRandevulari, AntrenorRandevulari, AntrenorHizmetler, AntrenorUzmanlikAlanlari ve CalismaSaatleri gibi navigation özellikleri tanımlanmıştır. Antrenör ve üye ayrımı yalnızca rol bazlıdır, bu sayede sistemde esnek yetkilendirme mümkün olmuştur.

Veritabanı Tasarımı
Veritabanı tasarımında ApplicationDbContext, IdentityDbContext<ApplicationUser> sınıfından türetilmiş ve Salon, Hizmet, Randevu, AntrenorCalismaSaati, AntrenorHizmet, AntrenorUzmanlik, UzmanlikAlani, Kampanya, Duyuru ve Haber gibi DbSet’ler tanımlanmıştır. Salon ile hizmetler ve randevular arasında bire çok ilişkiler kurulmuş, Randevu tablosu ile Üye ve Antrenör ilişkileri, hizmet ve salon bağlantıları oluşturulmuştur. Antrenörler ile hizmetler ve uzmanlık alanları arasında çoktan çoğa (Many-to-Many) ilişkiler kurulmuş, antrenörlerin çalışma saatleri ise bire çok ilişkisi ile tanımlanmıştır. Bu veritabanı tasarımı, gerçek hayata uygun, temiz ve genişletilebilir bir mimari sunmaktadır.

Üye girişi yapmayan Kullanıcılar
Uygulamauya giriş yapmamış kullanıcılar web sitesi ile ilgili statik sayfalara erişebilir. Ana Sayfa, Hakkımızda, İletişim ve Gizlilik sayfasını kullanabilir ayrıca kullanıcı girişi ve kayıt işlemini yapabilir. Uygulmanın ana sayfası aşağıda verilmiştir.

Admin Kullanıcılar:
Fitness Center Reservation System’de Admin paneli, sistemin tüm kullanıcı, randevu, salon, hizmet ve içerik yönetimini gerçekleştirebilecek şekilde tasarlanmıştır. Admin kullanıcıları, sisteme giriş yaptığında Dashboard üzerinden genel durumu görüntüleyebilir; bugünkü ve toplam randevular, bekleyen, onaylanan, tamamlanan, reddedilen ve iptal edilen randevuların dağılımı gibi özet bilgiler kolayca takip edilebilir.
Admin panelinde kullanıcı yönetimi modülü, tüm kullanıcıların listelenmesini, onay bekleyen üyelerin kontrolünü, aktif ve pasif/ reddedilmiş kullanıcıların takibini sağlar. Bu sayede sistem yöneticisi, kullanıcıların üyelik durumlarını hızlı ve güvenli bir şekilde yönetebilir. Randevu yönetimi modülü, tüm randevuların durumunu görüntülemeye, bekleyen veya onaylanmış randevuları filtrelemeye, tamamlanan, reddedilen ve iptal edilen randevuların detaylarını incelemeye olanak tanır. Ayrıca, Admin’in randevuları takip etmesi için özel bir alan mevcuttur.
Admin panelinde raporlar bölümü, sistemin analiz ve takip ihtiyaçlarına cevap verecek şekilde tasarlanmıştır. Buradan antrenörler, müsait antrenörler ve üye randevuları ile ilgili API üzerinden raporlar alınabilir, sistem performansı ve iş yükü kolayca izlenebilir. Tanımlamalar kısmı, salonlar, hizmetler, uzmanlık alanları, antrenör uzmanlık ve hizmet atamaları ile çalışma saatlerinin yönetildiği modülleri içerir. Bu modüller sayesinde Admin, sistemdeki tüm temel veri yapılarını ve ilişkilerini kontrol edebilir, güncelleyebilir ve yeni kayıtlar ekleyebilir.
Güncel içerikler bölümü, haberler, duyurular ve kampanyaların yönetilmesini sağlar. Admin, sisteme eklenen güncel içerikleri kolayca görüntüleyebilir, ekleyebilir veya düzenleyebilir. Ayrıca, panel üzerinden kendi profil bilgilerini görüntüleyebilir ve yönetebilir.
Dashboard sayfasında ayrıca istatistiksel özetler yer alır; randevu durumlarına göre ücret dağılımları, randevu durum dağılımları, en popüler hizmetler ve salonlara göre yoğunluk gibi bilgiler görsel olarak sunulur. Ayrıca son eklenen salonlar ve antrenörler listelenir, böylece Admin sistemdeki yenilikleri hızlıca takip edebilir.
Admin paneli ayrıca kullanıcı, randevu, salon, hizmet ve içerik yönetimi gibi tüm kritik işlemleri güvenli ve etkili bir şekilde gerçekleştirmeyi sağlar. 

Antrenör Kullanıcılar:
Fitness Center Reservation System’de Antrenör paneli, antrenörlerin kendi çalışmalarını ve randevularını merkezi bir noktadan yönetebilmesini sağlayacak şekilde tasarlanmıştır. Antrenörler sisteme giriş yaptıklarında Dashboard üzerinden genel durumu görebilir; bugünkü ve yaklaşan randevular, tamamlanan, bekleyen, onaylı, reddedilen ve iptal edilen randevular gibi özet bilgiler takip edilebilir.
Randevu yönetimi modülü, antrenörlerin kendi randevularını görüntülemelerini sağlar. Antrenör, bekleyen randevuları onaylayabilir, reddedebilir veya tamamlanan randevuları sistem üzerinden işaretleyebilir. Böylece hem kendi ajandasını yönetebilir hem de üyelerin randevu süreçleriyle ilgili bilgi sahibi olabilir.
Çalışma saatleri modülü, antrenörlerin haftalık veya günlük olarak hangi saatlerde müsait olduklarını tanımlayabildikleri alandır. Bu alan, sistemde randevu alınırken çakışmaların önlenmesi ve uygun saatlerin gösterilmesi için kritik bir rol oynar. Antrenör, kendi çalışma saatlerini ekleyebilir, düzenleyebilir veya silebilir.
Uzmanlık alanları ve hizmetler modülleri, antrenörlerin hangi alanlarda hizmet verdiğinin kaydedilmesini sağlar.
Profil yönetimi modülü antrenörlerin kişisel bilgilerini görüntülemelerini ve güncellemelerini sağlar. Bu bölümde antrenörler ad, soyad, iletişim ve diğer kişisel bilgilerini güvenli bir şekilde yönetebilir.

Üye Kullanıcılar:
Fitness Center Reservation System’de üyeler için tasarlanan yönetim paneli, üyelerin kendi randevularını takip edebilmelerini, yeni randevular alabilmelerini ve kişisel tercihlerini yönetebilmelerini sağlar. Panel açıldığında üye, aktif randevularının sayısını, katıldığı hizmetleri ve mevcut kampanyaları görebilir. Bu özet bilgiler, üyelerin kendi programlarını ve katıldıkları hizmetleri hızlıca değerlendirmelerine olanak tanır.
Üye panelinde randevu alma modülü, üyelerin istedikleri hizmetler için uygun antrenör ve salon seçeneklerini görerek randevu oluşturmasını sağlar. Ayrıca, üyeler yaklaşan randevularını görebilir ve planlarını buna göre organize edebilir. Favori salonlar listesi, üyelerin sık tercih ettiği salonları kolayca takip etmelerini ve rezervasyon süreçlerini hızlandırmalarını sağlar.
Panelde ayrıca AI önerileri bölümü yer alır. Bu bölümde üyenin boy, kilo ve hedef bilgilerine göre sistem, kişiye özel egzersiz veya hizmet önerileri sunar. Böylece üyeler, kendi fitness hedeflerine daha bilinçli ve verimli bir şekilde ulaşabilirler.
Profil yönetimi ekranı üyelerin kişisel bilgilerini görüntülemelerini ve güncellemelerini sağlar. Üye, ad, soyad, iletişim ve diğer bilgilerini güvenli bir şekilde yönetebilir.
 





