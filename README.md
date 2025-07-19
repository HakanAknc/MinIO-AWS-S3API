# 📁 S3 Advanced File API with ASP.NET Core + MinIO + MongoDB + JWT

Bu proje, ASP.NET Core Web API kullanılarak geliştirilmiş bir **dosya yönetim servisidir**.  
Dosya yükleme, listeleme, silme, indirme, yetkilendirme ve kullanıcı yönetimi gibi gelişmiş işlemleri destekler.

🔐 **JWT Authentication**,  
☁️ **S3 uyumlu MinIO desteği**,  
📦 **MongoDB tabanlı kullanıcı yönetimi** ile birlikte gelir.

---

## 🚀 Özellikler

- ✅ Dosya Yükleme (`multipart/form-data`)
- 📄 Dosya Listeleme
- 🔍 Dosya Detayı (boyut, uzantı, oluşturulma tarihi)
- 🎯 Dosya İndirme (proxy veya direct URL)
- 🗑️ Dosya Silme
- ✏️ Dosya Yeniden Adlandırma
- 📁 Tarihe göre klasörleme (otomatik)
- 🧑‍💼 Kullanıcı Girişi (JWT Token)
- 🔐 Yetkilendirme (Role-Based: `admin`, `user`)
- 🌍 MinIO ve MongoDB entegrasyonu
- 📎 Swagger UI desteği

---

## 🧰 Kullanılan Teknolojiler

- ASP.NET Core 8 Web API
- MongoDB (Mongo.Driver)
- MinIO (Amazon S3 SDK)
- JWT (JSON Web Tokens)
- Swagger (OpenAPI)
- Docker (isteğe bağlı)

---

## 🏗️ Proje Mimarisi


---


S3AdvancedApp/
│
├── Controllers/ → API uç noktaları
├── Services/ → İş servisleri (S3Service, AuthService, UserService)
├── Models/ → DTO, Entity ve View modeller
├── appsettings.json → Yapılandırma (Mongo, JWT, S3)
└── Program.cs → Uygulama başlangıç ayarları


---

## ⚙️ Kurulum

### 1. Gerekli Paketleri Yükle

```bash
dotnet restore

---


2. MongoDB'yi Başlat
MongoDB yüklü değilse Docker ile çalıştır:

docker run -d -p 27017:27017 --name mongodb mongo

---


3. MinIO’yu Başlat

docker run -d -p 9000:9000 -p 9001:9001 \
  --name minio4 \
  -e "MINIO_ACCESS_KEY=admin" \
  -e "MINIO_SECRET_KEY=admin123" \
  -v ~/minio-data:/data \
  minio/minio server /data --console-address ":9001"

MinIO Web: http://localhost:9001
Bucket adı: my-bucket4 (manuel oluşturulmalı)

---

🔐 Auth API'leri
Endpoint		Açıklama
POST /api/Auth/login	Kullanıcı girişi (JWT alır)
POST /api/Auth/register	Yeni kullanıcı oluşturur
GET /api/Auth/users	Tüm kullanıcıları getirir (Admin)



📁 S3 API'leri
Endpoint			Açıklama
POST /api/S3/upload		Dosya yükler
GET /api/S3/list		Tüm dosyaları listeler (uploads/)
DELETE /api/S3/delete		Belirli bir dosyayı siler
GET /api/S3/download		Dosya indirir (proxy olarak)
GET /api/S3/list-details	Dosya detayları (tarih, boyut, tip)
GET /api/S3/list-by-extension	Sadece belirli uzantıya göre filtreleme
GET /api/S3/list-by-date	Belirli tarih klasörüne göre listeleme
POST /api/S3/rename		Dosya yeniden adlandırma (kopyala+sil)



----

'''
📁 Proje Yapısı

S3AdvancedV2/
│
├── Controllers/
│   ├── AuthController.cs       # Login, Register, User list
│   └── S3Controller.cs         # Tüm dosya işlemleri (upload, list, delete vs.)
│
├── Models/
│   ├── LoginRequest.cs         # Giriş isteği DTO'su
│   ├── RenameFileRequest.cs    # Yeniden adlandırma için DTO
│   ├── S3ObjectInfo.cs         # Detaylı dosya bilgileri modeli
│   ├── S3Settings.cs           # S3 bağlantı ayarları
│   ├── UploadFileRequest.cs    # Yükleme isteği DTO'su
│   ├── UserModel.cs            # MongoDB kullanıcı modeli
│   └── MongoSettings.cs        # Mongo bağlantı ayarları
│
├── Services/
│   ├── AuthService.cs          # JWT üretimi ve kimlik kontrolü
│   ├── S3Service.cs            # Amazon S3/MinIO işlemleri
│   └── UserService.cs          # MongoDB user sorguları
│
├── appsettings.json            # Uygulama ayarları (JWT, S3 vs.)
├── Program.cs                  # Uygulama giriş noktası
└── S3AdvancedV2.http           # HTTP test istekleri (Opsiyonel)

'''
Bu yapı, Swagger’da görünen tüm uç noktaların (auth ve dosya işlemleri) arka plan mimarisini destekler.

---
🧪 Kullanım
1. Kullanıcı Kaydı

POST /api/auth/register
{
  "username": "admin",
  "password": "1234",
  "role": "admin"
}


2. Giriş & Token Al
POST /api/auth/login
{
  "username": "admin",
  "password": "1234"
}

Swagger’da Authorize butonuna Bearer {token} girerek diğer endpoint'leri test edebilirsin.

----
📦 Dosya İşlemleri (API)
Endpoint				Açıklama			Role
POST /api/s3/upload			Dosya yükle			user, admin
GET /api/s3/list			Dosyaları listele		user, admin
GET /api/s3/info?key=...		Dosya bilgisi			user, admin
GET /api/s3/download?key=...		Dosya indir (proxy)		user, admin
DELETE /api/s3/delete			Dosya sil			admin
POST /api/s3/rename			Dosya yeniden adlandır		admin

----

🛡️ Roller
admin: Tüm işlemleri yapabilir
user: Yalnızca yükleme, listeleme ve indirme

---

📸 Ekran Görüntüsü
Swagger UI ile API testi (yetkilendirme sonrası)

----

💡 Geliştirme Önerileri

🔑 Parolaları hash’le (BCrypt veya başka bir yöntemle)
🔁 Refresh Token sistemi
📱 Frontend bağlantısı (React / Blazor / Angular)
🔎 Arama & filtreleme işlemleri
🖼️ Thumbnail oluşturma (görseller için)
---

👨‍💻 Geliştirici
Hakan Akıncı
GitHub: github.com/HakanAknc
