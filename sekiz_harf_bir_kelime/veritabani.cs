using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sekiz_harf_bir_kelime
{
    class veritabani
    {
        tdk_kelimeEntities ent = new tdk_kelimeEntities();//entity framework nesnesi oluşturuluyor
        public object vericek()
        {
            string kelime;
            var liste = from k in ent.kelimeler_
                        orderby k.words ascending
                        select new
                        {
                            kelime = k.words
                        };
            return liste.ToList();
        }
    }
}
