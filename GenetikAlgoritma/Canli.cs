using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetikAlgoritma
{
    // Canli sınıfı, genetik algoritmanın bir parçası olarak bireyleri temsil eder.
    public class Canli
    {
        // TurnuvaCifti özelliği, canlının turnuva seçimi sırasında seçilen diğer canlıyı temsil eder.
        public Canli TurnuvaCifti { get; set; }
        // Gen özelliği, canlının genetik yapısını temsil eder.
        public Gen Gen { get; set; }

        // Belirtilen populasyon sayısı kadar canlı oluşturan metot.
        public List<Canli> Olustur(int populasyon)
        {
            // Rastgele sayı üretmek için kullanılacak random nesnesi oluşturulur.
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            // Oluşturulan canlıların listesi oluşturulur.
            List<Canli> pop = new List<Canli>();

            // Belirtilen populasyon sayısı kadar döngü oluşturulur.
            for (int i = 0; i < populasyon; i++)
            {
                // Her bir döngüde yeni bir canlı oluşturulur ve listeye eklenir.
                pop.Add(new Canli() { Gen = new Gen() });
            }

            // Oluşturulan canlılar listesi döndürülür.
            return pop;
        }
    }
}
