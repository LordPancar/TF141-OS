# TF141 OS

Windows yerel aðlarý (LAN) ve VPN (ZeroTier/Radmin) üzerinde çalýþan, dosya tabanlý iletiþim ve yönetim aracý.

Bu proje, Windows Gezgini'nin að paylaþýmlarýnda yarattýðý gecikmeleri (timeout/lag) önlemek ve komut satýrý üzerinden hýzlý iþlem yapmak amacýyla C# ile geliþtirilmiþtir.

## Özellikler

### 1. Sohbet Modülü (Chat)
* **Dosya Tabanlý:** Veritabaný gerektirmez, aðdaki paylaþýmlý `.txt` dosyasý üzerinden çalýþýr.
* **Anlýk Senkronizasyon:** `FileSystemWatcher` kullanarak mesajlarý anýnda ekrana yansýtýr.
* **Log Yönetimi:** Odaya giriþ yapýldýðýnda son 15 mesajý otomatik yükler.

### 2. Depolama Yöneticisi (Storage)
* **Anti-Freeze (Ping Kontrolü):** Sunucuya baðlanmadan önce ICMP paketi (Ping) gönderir. Sunucu kapalýysa arayüz donmaz, kullanýcýyý bilgilendirir.
* **Transfer:** Komut satýrýna sürükle-býrak desteði ile dosya yükleme ve indirme iþlemleri yapar.
* **Uzaktan Kontrol:** Sunucudaki dosyalarý listeler ve siler.

### 3. Konfigürasyon
* Kullanýcý adý ve IP adresi `config.txt` dosyasýnda saklanýr.
* Kurulum gerektirmez (Portable/Single File).

## Kurulum

1.  `Releases` kýsmýndan güncel `.exe` dosyasýný indirin.
2.  Uygulamayý çalýþtýrýn.
3.  Ýlk açýlýþta Kullanýcý Adý ve Sunucu IP adresini girin.

## Kullaným Komutlarý

Uygulama 3 ana menüden oluþur:
* `[1]` Sohbet
* `[2]` Depolama
* `[3]` Ayarlar

### Depolama Modülü (Storage)

| Komut | Örnek | Açýklama |
| :--- | :--- | :--- |
| `ls` | `ls` | Dosyalarý listeler. |
| `upload` | `upload C:\dosya.zip` | Sunucuya dosya gönderir. |
| `download` | `download notlar.txt` | Sunucudan masaüstüne dosya indirir. |
| `delete` | `delete eskidosya.tmp` | Dosyayý kalýcý olarak siler. |
| `menu` | `menu` | Ana menüye döner. |

### Sohbet Modülü (Chat)

* Mesaj göndermek için yazýp Enter'a basmak yeterlidir.
* Çýkýþ yapmak için `/menu` komutu kullanýlýr.

## Teknik Detaylar

* **Dil:** C# (.NET 10.0)
* **Mimari:** Console Application (CLI)
* **Baðýmlýlýklar:** System.Net.NetworkInformation (Ping iþlemleri için)

