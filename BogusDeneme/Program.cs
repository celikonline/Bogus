using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Bogus;

namespace BogusDeneme
{
    class Program
    {
        static void Main(string[] args)
        {

            var InoviceDetailFaker = new Faker<InvoiceDetail>("tr")
                .RuleFor(i => i.TUTAR, i => i.Finance.Amount())
                .RuleFor(i => i.ADET, i => i.Random.Int(0, 100))
                .RuleFor(i => i.ACIKLAMA, i => i.Finance.AccountName())
                .RuleFor(i => i.VERGI_TUTARI, (i, j) => (j.TUTAR * 18 / 100)).Generate(10);

            var InoviceFaker = new Faker<InvoiceData>("tr")
                .RuleFor(i=>i.Details,i=> InoviceDetailFaker)
                .RuleFor(i => i.FATURA_BASLANGIC, i => i.Date.Past(1))
                .RuleFor(i => i.FATURA_BITIS, (i, j) => j.FATURA_BASLANGIC.AddDays(7))
                .RuleFor(i => i.MUSTERI_AD, i => i.Company.CompanyName())
                .RuleFor(i => i.MUSTERI_EPOSTA, i => i.Person.Email);

            var invoices = InoviceFaker.Generate(10);

            var opt = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                IgnoreNullValues=true
            };

            var jsonresult = JsonSerializer.Serialize(invoices, opt);

            Console.WriteLine(jsonresult);

            Console.ReadLine();


        }
    }
}
