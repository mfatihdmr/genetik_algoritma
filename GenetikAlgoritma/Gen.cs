using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetikAlgoritma
{
    // Gen sınıfı, genetik algoritmanın bir parçası olarak genetik yapının temsil edilmesini sağlar.
    public class Gen
    {
        // x1 ve x2 değerlerini tutan özellikler.
        public double x1 { get; set; }
        public double x2 { get; set; }

        // Random sınıfından bir nesne oluşturularak rastgele sayılar üretmek için kullanılır.
        Random rnd = new Random(Guid.NewGuid().GetHashCode());

        // Gen sınıfının parametresiz yapıcı metodu.
        public Gen()
        {
            // x1 ve x2 değerleri, -10 ile 10 arasında rastgele sayılarla başlatılır.
            x1 = rnd.NextDouble() * 20 - 10;
            x2 = rnd.NextDouble() * 20 - 10;
        }

        // Gen sınıfının parametreli yapıcı metodu.
        public Gen(double x1, double x2)
        {
            // x1 ve x2 değerleri, parametre olarak verilen değerlerle başlatılır.
            this.x1 = x1;
            this.x2 = x2;
        }

        // Matyas formulüne göre skor hesaplanan özellik.
        public double MatyasFormulSkor
        {
            get
            {
                // Matyas formulüne göre skor hesaplanır ve döndürülür.
                double result = 0.26 * (x1 * x1 + x2 * x2) - 0.48 * x1 * x2;
                return result;
            }
        }

    }
}
