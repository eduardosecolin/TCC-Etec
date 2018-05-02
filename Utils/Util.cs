using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BarberSystem.Utils
{
   public class Util {


     // validar campos vazios
     public static bool vazio;
     public static string VerificarCamposVazios(string txt){
       vazio = false;
       if(txt.Equals(string.Empty)){
                MessageBox.Show("Campo não pode ficar vazio!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                vazio = true;
       }
       return txt;
     }

     // crypt senha
     public static string encrypt(string valor){
            MD5 hash = MD5.Create();
            byte[] valorCriptografado = hash.ComputeHash(Encoding.Default.GetBytes(valor));
            StringBuilder strBuilder = new StringBuilder();
            for(int i = 0; i < valorCriptografado.Length; i++){
                strBuilder.Append(valorCriptografado[i].ToString("x2"));
            }
            return strBuilder.ToString();
     }  

     // descrypt senha
     public static bool descrypt(string valor, string valorCriptografado){
            string novoValorCriptografado = encrypt(valor);
            StringComparer compararSenha = StringComparer.OrdinalIgnoreCase;
            if(compararSenha.Compare(novoValorCriptografado, valorCriptografado) == 0){
                return true;
            }else{
                return false;
            }
     }

     // verificar campos vazios
     public static bool isEmpty(params TextBox[] txt){
            bool valor = false;
            foreach (TextBox item in txt) {
              if(item.Text.Equals(string.Empty)){
                    valor = true;
              }else{
                    valor = false;
              }
            }
            return valor;
     }

   }
}
