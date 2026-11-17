

namespace MafiaGalonSystem
{
   
    public class Agen
    {
        public string IdAgen { get; set; }
        public string NamaAgen { get; set; }
        public int StokAgen { get; set; }

        
        public Agen(string idAgen, string namaAgen, int stokAgen)
        {
            IdAgen = idAgen;
            NamaAgen = namaAgen;
            StokAgen = stokAgen;
        }

        
        public void TampilInfo()
        {
            Console.WriteLine($"[AGEN] ID: {IdAgen} | Nama: {NamaAgen} | Stok Agen: {StokAgen} galon");
        }
    }

    
    public abstract class ProdukGalon
    {
        
        private string idProduk;
        private string namaProduk;
        private int stokGudang;

       
        public string IdProduk
        {
            get { return idProduk; }
            set { idProduk = value; }
        }

        public string NamaProduk
        {
            get { return namaProduk; }
            set { namaProduk = value; }
        }

        public int StokGudang
        {
            get { return stokGudang; }
            set
            {
                if (value >= 0)
                    stokGudang = value;
                else
                    stokGudang = 0;
            }
        }

        public ProdukGalon(string idProduk, string namaProduk, int stokGudang)
        {
            IdProduk = idProduk;
            NamaProduk = namaProduk;
            StokGudang = stokGudang;
        }

       
        public void KurangiStok(int jumlah)
        {
            if (jumlah > 0 && jumlah <= StokGudang)
            {
                StokGudang -= jumlah;
            }
        }

        
        public abstract int HitungBiaya(int jumlah);
    }

    
    public class GalonAkua : ProdukGalon
    {
        private const int HargaPerGalon = 20000;

        public GalonAkua(string idProduk, string namaProduk, int stokGudang)
            : base(idProduk, namaProduk, stokGudang) { }

      
        public override int HitungBiaya(int jumlah)
        {
            return jumlah * HargaPerGalon;
        }
    }

    public class GalonBerkeran : ProdukGalon
    {
        private const int HargaPerGalon = 12000;
        public string JenisKeran { get; set; }

        public GalonBerkeran(string idProduk, string namaProduk, int stokGudang, string jenisKeran)
            : base(idProduk, namaProduk, stokGudang)
        {
            JenisKeran = jenisKeran;
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * HargaPerGalon;
        }
    }

    
    public interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    public class DistribusiGalon : DapatDidistribusikan
    {
        public Agen AgenTujuan { get; set; }
        public ProdukGalon ProdukDikirim { get; set; }
        public int Jumlah { get; set; }

        public DistribusiGalon(Agen agenTujuan, ProdukGalon produkDikirim, int jumlah)
        {
            AgenTujuan = agenTujuan;
            ProdukDikirim = produkDikirim;
            Jumlah = jumlah;
        }

        public void KirimPaket()
        {
            Console.WriteLine($"\n--- Memproses Pengiriman Kartel untuk {AgenTujuan.NamaAgen} ---");

           
            if (Jumlah <= 0)
            {
                Console.WriteLine($"[GAGAL] Operasi dibatalkan: Jumlah galon ({Jumlah}) harus lebih dari 0.");
                return;
            }

          
            if (Jumlah > ProdukDikirim.StokGudang)
            {
                Console.WriteLine($"[GAGAL] Operasi dibatalkan: Stok gudang pusat tidak mencukupi! (Stok tersedia: {ProdukDikirim.StokGudang}, Diminta: {Jumlah})");
                return;
            }

           
            int totalBiaya = ProdukDikirim.HitungBiaya(Jumlah);
            ProdukDikirim.KurangiStok(Jumlah);
            AgenTujuan.StokAgen += Jumlah;

            Console.WriteLine($"[SUKSES] Paket berhasil dikirimkan ke agen rahasia {AgenTujuan.NamaAgen}!");
            Console.WriteLine($"         Produk         : {ProdukDikirim.NamaProduk}");
            Console.WriteLine($"         Jumlah Kirim   : {Jumlah} galon");
            Console.WriteLine($"         Total Biaya    : Rp {totalBiaya:N0}");
            Console.WriteLine($"         Sisa Stok Gudang: {ProdukDikirim.StokGudang} galon");
            Console.WriteLine($"         Stok Agen Baru : {AgenTujuan.StokAgen} galon");
        }
    }


    public class SindikatGalon
    {
        

        public List<Agen> DaftarAgen { get; set; } = new List<Agen>();
        public List<ProdukGalon> DaftarProduk { get; set; } = new List<ProdukGalon>();

        public void TambahAgen(Agen agen)
        {
            DaftarAgen.Add(agen);
            Console.WriteLine($"[SINDIKAT] Agen rahasia '{agen.NamaAgen}' berhasil direkrut ke dalam jaringan.");
        }

        public void TambahProduk(ProdukGalon produk)
        {
            DaftarProduk.Add(produk);
            Console.WriteLine($"[SINDIKAT] Pasokan produk '{produk.NamaProduk}' berhasil diamankan di gudang pusat.");
        }

        public void TampilkanSindikat()
        {
            
            Console.WriteLine("       STATUS ASET & PASUKAN SINDIKAT GALON     ");
           

            Console.WriteLine("\n[Daftar Agen Rahasia Sektor]:");
            foreach (var agen in DaftarAgen)
            {
                agen.TampilInfo();
            }

            Console.WriteLine("\n[Daftar Produk di Gudang Pusat]:");
            foreach (var produk in DaftarProduk)
            {
                string infoKeran = "";
                if (produk is GalonBerkeran gb)
                {
                    infoKeran = $" | Jenis Keran: {gb.JenisKeran}";
                }
                Console.WriteLine($" - ID: {produk.IdProduk} | Nama: {produk.NamaProduk} | Stok Gudang: {produk.StokGudang} galon{infoKeran}");
            }
            
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== SISTEM MANAJEMEN SINDIKAT 'MAFIA GALON' ===");

          
            Agen agen1 = new Agen("AG-01", "Agen X (Sektor Utara)", 10);
            Agen agen2 = new Agen("AG-02", "Agen Z (Sektor Selatan)", 5);

            
            ProdukGalon produk1 = new GalonAkua("PRD-101", "Air Galon Akua Murni", 100);
            ProdukGalon produk2 = new GalonBerkeran("PRD-102", "Galon Berkeran Impor Misterius", 40, "Keran Stainless Otomatis");

            
            Console.WriteLine("\n--- Demonstrasi Polymorphism (HitungBiaya Dinamis) ---");
            ProdukGalon[] listProduk = { produk1, produk2 };
            int jumlahSample = 15;
            foreach (var p in listProduk)
            {
                Console.WriteLine($"Produk: {p.NamaProduk,-30} | Biaya untuk {jumlahSample} galon: Rp {p.HitungBiaya(jumlahSample):N0}");
            }

          
            SindikatGalon sindikat = new SindikatGalon();
            sindikat.TambahAgen(agen1);
            sindikat.TambahAgen(agen2);
            sindikat.TambahProduk(produk1);
            sindikat.TambahProduk(produk2);

            sindikat.TampilkanSindikat();

            
            DistribusiGalon kirim1 = new DistribusiGalon(agen1, produk1, 25);
            kirim1.KirimPaket();

            
            DistribusiGalon kirim2 = new DistribusiGalon(agen2, produk2, 50);
            kirim2.KirimPaket();

            
            DistribusiGalon kirim3 = new DistribusiGalon(agen2, produk2, 0);
            kirim3.KirimPaket();

            
            sindikat.TampilkanSindikat();

            Console.WriteLine("\nTekan tombol apa saja untuk keluar...");
            Console.ReadKey();
        }
    }
}