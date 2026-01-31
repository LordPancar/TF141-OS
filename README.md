# TF141 OS

Windows yerel ağları (LAN) ve VPN (ZeroTier/Radmin) üzerindeki paylaşımlı klasörleri yönetmek ve iletişim kurmak için geliştirilmiş CLI (Komut Satırı Arayüzü) aracıdır.

Bu proje, Windows Gezgini'nin ağ erişiminde yarattığı performans sorunlarını (donma/zaman aşımı) aşmak ve dosya işlemlerini hızlandırmak amacıyla C# ile yazılmıştır.

## Özellikler

### 1. Depolama Yöneticisi (Storage Commander)
* **Gezinti (Navigation):** `cd` komutu ile sunucu içindeki klasörlerde gezilebilir (Sub-directory support).
* **Anti-Freeze:** Bağlantı öncesi ICMP (Ping) kontrolü yapar. Sunucu kapalıysa arayüz kilitlenmez.
* **Hızlı Transfer:** Sürükle-bırak desteği ile dosya yükleme ve indirme işlemleri yapılır.
* **Güvenlik:** Kullanıcının ana dizin (Root) dışına çıkmasını engelleyen sınır kontrolü bulunur.

### 2. Sohbet Modülü (Chat)
* **Real-time:** `FileSystemWatcher` ile anlık mesajlaşma sağlar.
* **Log:** Odaya giriş yapıldığında geçmiş mesajları otomatik yükler.
* **Bildirim:** Yeni mesaj geldiğinde sesli uyarı verir.

### 3. Yapılandırma
* Kullanıcı adı ve IP adresi `config.txt` dosyasında saklanır.
* Kurulum gerektirmez (Portable / Single File Executable).

## Kurulum

1.  Releases sayfasından son sürümü (`TF141-OS.exe`) indirin.
2.  Dosyayı çalıştırın.
3.  İlk açılışta Kullanıcı Adı ve Hedef Sunucu IP adresini girin.

## Komut Listesi

Uygulama 3 ana menüden oluşur. Menüler arası geçiş için modülden çıkış yapılmalıdır.

### Depolama Modülü (Storage)

| Komut | Kullanım | Açıklama |
| :--- | :--- | :--- |
| `ls` | `ls` | Mevcut konumdaki dosyaları ve klasörleri listeler. |
| `cd` | `cd Oyunlar` | Belirtilen klasörün içine girer. |
| `cd ..` | `cd ..` | Bir üst klasöre geri döner. |
| `cd` (boş) | `cd` | En baştaki ana dizine (Root) döner. |
| `upload` | `upload C:\dosya.zip` | Bilgisayarınızdan mevcut konuma dosya yükler. |
| `download` | `download notlar.txt` | Mevcut konumdaki dosyayı Masaüstüne indirir. |
| `delete` | `delete log.txt` | Dosyayı kalıcı olarak siler. |
| `menu` | `menu` | Ana menüye döner. |

### Sohbet Modülü (Chat)

* **Mesaj Gönderme:** Metni yazıp `Enter` tuşuna basın.
* **Çıkış:** `/menu` komutu ile ana menüye dönülür.

## Teknik Detaylar

* **Framework:** .NET 8.0
* **Platform:** Windows (x64)
* **Protokol:** SMB (Dosya Paylaşımı)
* **Bağımlılıklar:** System.Net.NetworkInformation

## Lisans

MIT License
