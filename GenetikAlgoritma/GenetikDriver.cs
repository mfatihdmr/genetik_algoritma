using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetikAlgoritma
{
    class GenetikDriver
    {
        public List<Canli> canliList { get; set; }
        public List<Canli> elitList { get; set; }
        public int elitPop { get; set; }



        //populasyonList özelliği, canlıların listesini döndüren bir özellik (property) olarak tanımlanmıştır. Bu özellik, canliList ve elitList listelerini birleştirerek genel popülasyon listesini oluşturur.
        public List<Canli> populasyonList
        {
            get
            {
                List<Canli> list= new List<Canli>();
                list.AddRange(canliList);
                if(elitList!=null)
                    list.AddRange(elitList);
                return list;
            }
        }
        //GenetikDriver sınıfının kurucu (constructor) metodunda, popülasyon oluşturulur. Bu metod, PopulasyonOlustur metodunu çağırarak belirtilen sayıda canlı oluşturur.
        public GenetikDriver(int pop)
        {
            PopulasyonOlustur(pop);
        }
        //Kiyasla metodu, iki canlı arasında karşılaştırma yapar ve en iyi canlıyı döndürür.
        private Canli Kiyasla(Canli c1,Canli c2)
        {
            Canli c= new Canli();
            c = c1.Gen.MatyasFormulSkor > c2.Gen.MatyasFormulSkor ? c2 : c1;
            return c;
        }
        //PopulasyonOlustur metodu, belirtilen sayıda canlı oluşturur ve canliList özelliğini günceller.
        public List<Canli> PopulasyonOlustur(int pop)
        {
            List<Canli> liste = new Canli().Olustur(pop);
            canliList = liste;
            return liste;
        }
        public List<Canli> TurnuvaCiftiOlustur()
        {
            // Yeni bir rastgele sayı üretmek için Random nesnesi oluşturulur.
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            // Turnuva çiftlerinin tutulacağı liste oluşturulur.
            List<Canli> TurnuvaList = new List<Canli>();

            // CanliList içindeki her bir canlı için döngü oluşturulur.
            for (int i = 0; i < canliList.Count; i++)
            {
                // İki rastgele indis seçilir.
                int rndIndis1 = rnd.Next(0, canliList.Count);
                int rndIndis2 = rnd.Next(0, canliList.Count);

                // Seçilen indislerle canlılar belirlenir.
                var v1 = canliList[rndIndis1];
                var v2 = canliList[rndIndis2];

                // Seçilen iki canlı arasında karşılaştırma yapılır ve en iyi canlı TurnuvaList'e eklenir.
                TurnuvaList.Add(Kiyasla(v1, v2));

                // İkinci bir çift oluşturulur ve TurnuvaList'e eklenir.
                rndIndis1 = rnd.Next(0, canliList.Count);
                rndIndis2 = rnd.Next(0, canliList.Count);
                v1 = canliList[rndIndis1];
                v2 = canliList[rndIndis2];
                // Karşılaştırma yapılır ve en iyi canlı TurnuvaList'e eklenir.
                TurnuvaList[i].TurnuvaCifti = Kiyasla(v1, v2);
            }

            // CanliList, oluşturulan turnuva çiftleri ile güncellenir.
            canliList = TurnuvaList;

            // Oluşturulan turnuva çiftleri listesi döndürülür.
            return TurnuvaList;
        }



        public List<Canli> Caprazla(double ihtimal)
        {
            // Rastgele sayı üretmek için kullanılacak random nesnesi oluşturulur.
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            // Çaprazlama ihtimalini belirlemek için kullanılacak başka bir random nesnesi oluşturulur.
            Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
            // Çaprazlanmış canlıların listesi oluşturulur.
            List<Canli> caprazlanmisList = new List<Canli>();

            // Canlılar listesi üzerinde döngü başlatılır.
            foreach (var canli in canliList)
            {
                // Belirlenen ihtimalden düşük bir rastgele sayı üretilirse çaprazlama yapılmaz,
                // canlı doğrudan caprazlanmisList'e eklenir ve döngü devam eder.
                if (rnd2.NextDouble() > ihtimal)
                {
                    caprazlanmisList.Add(canli);
                    continue;
                }

                // Çaprazlama yapılacak ise, iki ebeveyn canlı arasında rastgele bir nokta belirlenir.
                double rndDouble = rnd.NextDouble();
                // İkinci ebeveynin genleri, belirlenen noktaya göre karıştırılarak yeni nesiller oluşturulur.
                double offspring1a = rndDouble * canli.Gen.x1 + (1 - rndDouble) * canli.TurnuvaCifti.Gen.x1;
                double offspring1b = rndDouble * canli.Gen.x2 + (1 - rndDouble) * canli.TurnuvaCifti.Gen.x2;

                // İkinci çaprazlanmış nesil için de aynı işlem yapılır.
                double offspring2a = (1 - rndDouble) * canli.Gen.x1 + rndDouble * canli.TurnuvaCifti.Gen.x1;
                double offspring2b = (1 - rndDouble) * canli.Gen.x2 + rndDouble * canli.TurnuvaCifti.Gen.x2;

                // Yeni çaprazlanmış canlılar oluşturulur ve caprazlanmisList'e eklenir.
                caprazlanmisList.Add(new Canli()
                {
                    Gen = new Gen()
                    {
                        x1 = offspring1a,
                        x2 = offspring1b
                    },
                    TurnuvaCifti = new Canli()
                    {
                        Gen = new Gen()
                        {
                            x1 = offspring2a,
                            x2 = offspring2b
                        }
                    }
                });
            }

            // Çaprazlanmış canlılar listesi, ana canlılar listesi ile güncellenir.
            canliList = caprazlanmisList;
            // Çaprazlanmış canlılar listesi döndürülür.
            return caprazlanmisList;
        }


        public List<Canli> Mutasyon(double ihtimal)
        {
            // Rastgele sayı üretmek için kullanılacak random nesnesi oluşturulur.
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            // Mutasyon ihtimalini belirlemek için kullanılacak random nesnesi oluşturulur.

            // Mutasyon işlemi sonucunda oluşan canlıların listesi oluşturulur.
            List<Canli> mutasyonList = new List<Canli>();

            // Canlılar listesi üzerinde döngü başlatılır.
            foreach (var canli in canliList)
            {
                // Belirlenen ihtimalden düşük bir rastgele sayı üretilirse mutasyon yapılmaz,
                // canlı doğrudan mutasyonList'e eklenir ve döngü devam eder.
                if (rnd.NextDouble() > ihtimal)
                {
                    mutasyonList.Add(canli);
                    continue;
                }

                // Mutasyon yapıldıysa, canlının genleri üzerinde rastgele bir değişiklik yapılır.
                // Bu değişiklik, genlerin değerlerini rastgele bir şekilde değiştirerek gerçekleştirilir.
                // Yeni bir Canli nesnesi oluşturulur ve mutasyonList'e eklenir.
                mutasyonList.Add(new Canli()
                {
                    // Yeni oluşturulan Canli nesnesinin genleri, rastgele değerlerle oluşturulur.
                    Gen = new Gen(),
                    // Aynı şekilde TurnuvaCifti için de yeni bir Canli nesnesi oluşturulur.
                    TurnuvaCifti = new Canli() { Gen = new Gen() }
                });
            }

            // Mutasyon işlemi sonucunda oluşan canlılar listesi, ana canlılar listesi ile güncellenir.
            canliList = mutasyonList;
            // Mutasyon işlemi sonucunda oluşan canlılar listesi döndürülür.
            return mutasyonList;
        }




        public Canli BestCanli()
        {
            // Popülasyon içindeki en iyi canlıyı bulmak için sıralama yapılır ve ilk eleman döndürülür.
            var c = populasyonList.OrderBy(a => a.Gen.MatyasFormulSkor).FirstOrDefault();
            // En iyi canlının skoru konsola yazdırılır.
            Console.WriteLine("En iyi Canlı:" + c.Gen.MatyasFormulSkor);
            // En iyi canlı döndürülür.
            return c;
        }


        public List<Canli> Elitizm(int elitPop)
        {
            // Popülasyon içinden en iyi elitPop sayısı kadar canlı seçilir ve elitizm listesine atanır.
            List<Canli> elitizm = populasyonList.OrderBy(a => a.Gen.MatyasFormulSkor).Take(elitPop).ToList();

            // Elit olmayan canlılar, popülasyon listesinden çıkarılır ve canliList'e atanır.
            canliList = populasyonList.OrderBy(a => a.Gen.MatyasFormulSkor).Reverse().Take(populasyonList.Count() - elitPop).ToList();

            // Elit canlılar listesi elitList'e atanır.
            elitList = elitizm;

            // En iyi fonksiyon skoru konsola yazdırılır.
            Console.WriteLine("En iyi Fonksiyon:" + populasyonList.OrderBy(a => a.Gen.MatyasFormulSkor).FirstOrDefault().Gen.MatyasFormulSkor);

            // Elit canlılar listesi döndürülür.
            return elitizm;
        }


        public List<Canli> Elitizm()
        {
            // Elitizm metoduna elitPop değeri ile çağrılır.
            return Elitizm(elitPop);
        }

    }
}
