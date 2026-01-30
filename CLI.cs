using System;
using System.IO;
using System.Threading;
using System.Net.NetworkInformation; 

class SystemOS
{
    static string username = "";
    static string serverIP = "";
    static string configPath = "config.txt";

    static void Main()
    {
        Console.Title = "TF141 OS v1.1 (Anti-Freeze)";
        AyarlariYukle();

        while (true)
        {
            Console.Clear();
            RenkliYaz("=== TF141 OPERATING SYSTEM ===", ConsoleColor.Cyan);
            RenkliYaz($"Kullanıcı: {username} | Sunucu: {serverIP}", ConsoleColor.DarkGray);
            Console.WriteLine("");
            Console.WriteLine("[1] Chat Odasına Gir");
            Console.WriteLine("[2] Dosya Yöneticisi");
            Console.WriteLine("[3] Ayarları Değiştir");
            Console.WriteLine("[4] Çıkış");
            Console.WriteLine("");
            Console.Write("Seçiminiz: ");

            string secim = Console.ReadLine();

            switch (secim)
            {
                case "1":
                    BaslatChat();
                    break;
                case "2":
                    BaslatStorage();
                    break;
                case "3":
                    AyarlariSifirla();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;
                default:
                    HataVer("Geçersiz seçim!");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }

    // --- MODÜL 1: CHAT ---
    static void BaslatChat()
    {
        Console.Clear();
        // ÖNCE PING ATIYORUZ (Donmayı engellemek için)
        if (!SunucuAcikMi(serverIP)) return;

        RenkliYaz("--- CHAT ODASI ---", ConsoleColor.Green);
        Console.WriteLine("Çıkış: '/menu'");

        string path = $@"\\{serverIP}\tf141\chat.txt";

        if (!DosyaErisimKontrol(path)) return;

        // Geçmişi Göster
        try
        {
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                int start = Math.Max(0, lines.Length - 15);
                for (int i = start; i < lines.Length; i++) Console.WriteLine(lines[i]);
            }
        }
        catch { }

        // Watcher (Dinleyici)
        bool chatAktif = true;
        FileSystemWatcher watcher = new FileSystemWatcher();
        try
        {
            watcher.Path = Path.GetDirectoryName(path);
            watcher.Filter = Path.GetFileName(path);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += (s, e) => {
                if (!chatAktif) return;
                try
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var sr = new StreamReader(fs))
                    {
                        string content = sr.ReadToEnd();
                        string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        if (lines.Length > 0)
                        {
                            string lastMsg = lines[lines.Length - 1];
                            if (!lastMsg.Contains(username)) Console.Beep();
                            Console.WriteLine(lastMsg);
                        }
                    }
                }
                catch { }
            };
            watcher.EnableRaisingEvents = true;
        }
        catch
        {
            HataVer("Bağlantı koptu!");
            Console.ReadKey();
            return;
        }

        // Yazma Döngüsü
        while (chatAktif)
        {
            string input = Console.ReadLine();

            // Ekran Temizleme
            if (Console.CursorTop > 0)
            {
                try
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop);
                }
                catch { }
            }

            if (!string.IsNullOrWhiteSpace(input))
            {
                if (input == "/menu")
                {
                    chatAktif = false;
                    watcher.EnableRaisingEvents = false;
                    return;
                }
                string msg = $"[{DateTime.Now:dd.MM HH:mm}] {username}: {input}";
                try { File.AppendAllText(path, msg + Environment.NewLine); } catch { }
            }
        }
    }

    // --- MODÜL 2: STORAGE ---
    static void BaslatStorage()
    {
        Console.Clear();
        // ÖNCE PING ATIYORUZ
        if (!SunucuAcikMi(serverIP)) return;

        string rootPath = $@"\\{serverIP}\tf141";
        if (!Directory.Exists(rootPath))
        {
            HataVer("Klasöre erişilemiyor (İzinleri kontrol et).");
            Console.ReadKey();
            return;
        }

        RenkliYaz("--- DEPOLAMA YÖNETİCİSİ ---", ConsoleColor.Yellow);
        Console.WriteLine("Komutlar: ls, upload, download, delete, menu");

        bool storageAktif = true;
        while (storageAktif)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"TF141@{serverIP}> ");
            Console.ResetColor();

            string input = Console.ReadLine().Trim();
            string[] parts = input.Split(' ');
            string cmd = parts[0].ToLower();
            string arg = parts.Length > 1 ? input.Substring(cmd.Length + 1) : "";

            switch (cmd)
            {
                case "menu":
                case "exit":
                    storageAktif = false;
                    break;
                case "ls":
                case "list":
                    try
                    {
                        var files = Directory.GetFiles(rootPath);
                        Console.WriteLine("\n--- Dosyalar ---");
                        foreach (var f in files)
                        {
                            FileInfo fi = new FileInfo(f);
                            Console.WriteLine($"{fi.Name,-25} | {fi.Length / 1024} KB");
                        }
                        Console.WriteLine("");
                    }
                    catch (Exception ex) { HataVer(ex.Message); }
                    break;
                case "upload":
                    arg = arg.Replace("\"", "");
                    if (File.Exists(arg))
                    {
                        try
                        {
                            Console.Write("Yükleniyor... ");
                            File.Copy(arg, Path.Combine(rootPath, Path.GetFileName(arg)), true);
                            RenkliYaz("BAŞARILI", ConsoleColor.Green);
                        }
                        catch (Exception ex) { HataVer(ex.Message); }
                    }
                    else HataVer("Dosya bulunamadı.");
                    break;
                case "download":
                    string serverFile = Path.Combine(rootPath, arg);
                    if (File.Exists(serverFile))
                    {
                        try
                        {
                            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            File.Copy(serverFile, Path.Combine(desktop, arg), true);
                            RenkliYaz("Masaüstüne İndirildi.", ConsoleColor.Green);
                        }
                        catch (Exception ex) { HataVer(ex.Message); }
                    }
                    else HataVer("Dosya sunucuda yok.");
                    break;
                case "delete":
                    string delFile = Path.Combine(rootPath, arg);
                    if (File.Exists(delFile))
                    {
                        Console.Write("Silinsin mi? (e/h): ");
                        if (Console.ReadLine() == "e")
                        {
                            try { File.Delete(delFile); RenkliYaz("Silindi.", ConsoleColor.Green); }
                            catch (Exception ex) { HataVer(ex.Message); }
                        }
                    }
                    else HataVer("Dosya yok.");
                    break;
                case "clear":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Bilinmeyen komut.");
                    break;
            }
        }
    }

    // --- YENİ EKLENEN PING SİSTEMİ ---
    static bool SunucuAcikMi(string ip)
    {
        // Eğer sunucu kendimizsek (localhost) ping atmaya gerek yok
        if (ip == "localhost" || ip == "127.0.0.1") return true;

        Console.WriteLine($"Sunucuya ulaşılıyor ({ip})...");
        try
        {
            Ping ping = new Ping();
            // 2000 ms (2 saniye) cevap bekle, gelmezse pes et
            PingReply reply = ping.Send(ip, 2000);

            if (reply.Status != IPStatus.Success)
            {
                HataVer("Sunucu KAPALI veya ulaşılamıyor!");
                Console.WriteLine("Sebep: " + reply.Status);
                Console.WriteLine("Bir tuşa basıp menüye dönün...");
                Console.ReadKey();
                return false;
            }
            return true;
        }
        catch
        {
            HataVer("Ping atılamadı! IP adresini kontrol edin.");
            Console.ReadKey();
            return false;
        }
    }
    // ---------------------------------

    static void AyarlariYukle()
    {
        if (File.Exists(configPath))
        {
            string[] lines = File.ReadAllLines(configPath);
            if (lines.Length >= 2)
            {
                username = lines[0];
                serverIP = lines[1];
                return;
            }
        }
        AyarlariSifirla();
    }

    static void AyarlariSifirla()
    {
        Console.Clear();
        RenkliYaz("--- İLK KURULUM ---", ConsoleColor.Magenta);
        Console.Write("Kullanıcı Adınız: ");
        username = Console.ReadLine();
        Console.Write("Sunucu IP (Örn: 10.190.18.31): ");
        serverIP = Console.ReadLine();
        File.WriteAllLines(configPath, new string[] { username, serverIP });
        RenkliYaz("Ayarlar kaydedildi!", ConsoleColor.Green);
        Thread.Sleep(1500);
    }

    static bool DosyaErisimKontrol(string path)
    {
        if (!File.Exists(path))
        {
            HataVer("Sunucu açık ama chat.txt dosyası bulunamadı!");
            Console.ReadKey();
            return false;
        }
        return true;
    }

    static void RenkliYaz(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    static void HataVer(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"HATA: {text}");
        Console.ResetColor();
    }
}