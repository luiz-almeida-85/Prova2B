using Prova2B.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Prova2B.Controllers
{
    public class DadosController : Controller
    {
        private Context db = new Context();
        private static string AesIV256BD = @"%j?TmFP6$BbMnY$@";//16 caracteres
        private static string AesKey256BD = @"rxmBUJy]&,;3jKwDTzf(cui$<nc2EQr)";//32 caracteres
        public ActionResult Index()
        {


            return View(db.Dados.ToList());
        }

        #region Create - GET
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        #endregion
        #region Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Dados Dados)
        {
            if (ModelState.IsValid)
            {

                //AesCryptoServiceProvider
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = Encoding.UTF8.GetBytes(AesIV256BD);
                aes.Key = Encoding.UTF8.GetBytes(AesKey256BD);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                //Convertendo string para byte array
                byte[] src = Encoding.Unicode.GetBytes(Dados.txt);


                //Encriptação
                using (ICryptoTransform encrypt = aes.CreateEncryptor())
                {
                    byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                    //Converte byte array para string de base 64
                    Dados.txt = Convert.ToBase64String(dest);
                }
                db.Dados.Add(Dados);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(Dados);
        }
        #endregion

        #region Details - GET
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Dados Dados = db.Dados.Find(id);
            if (Dados == null)
            {
                return HttpNotFound();
            }

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256BD);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256BD);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Converter a String para um byte array 64 bists
            byte[] src = Convert.FromBase64String(Dados.txt);

            // Decriptar
            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                Dados.txt = Encoding.Unicode.GetString(dest);
            }
            return View(Dados);
        }
        #endregion


    }
}