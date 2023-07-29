using System;

namespace BBL.Authorization.Gerenciadores
{
    public class GerenciadorSenhas
    {
        public const string Letras = "abcdefghijklmnopqrstuvxz";
        public const string CaracteresEspeciais = "!@#$%&*";
        public const string Numeros = "123456789";
        public const int TamanhoMinimoSenha = 8;

        public static string GerarSenhaTemporaria()
        {
            int numeroRepeticoes = Convert.ToInt32((TamanhoMinimoSenha / 4));
            string senha = string.Empty;
            Random random = new Random();

            for (int i = 0; i < numeroRepeticoes; i++)
            {
                senha += ObterLetra(random);
                senha += ObterCaracterEspecial(random);
                senha += ObterNumero(random);
                senha += ObterLetraMaiuscula(random);
            }

            while (senha.Length < TamanhoMinimoSenha)
            {
                senha += ObterLetra(random);
            }

            return senha;
        }

        public static bool SenhaValida(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                return false;
            }

            if (senha.Length < TamanhoMinimoSenha)
            {
                return false;
            }

            return true;
        }

        private static char ObterLetra(Random random) =>
            Letras[random.Next(0, CaracteresEspeciais.Length)];

        private static char ObterLetraMaiuscula(Random random) =>
            Letras.ToUpper()[random.Next(0, CaracteresEspeciais.Length)];

        private static char ObterCaracterEspecial(Random random) =>
            CaracteresEspeciais[random.Next(0, CaracteresEspeciais.Length)];

        private static char ObterNumero(Random random) =>
            Numeros[random.Next(0, Numeros.Length)];
    }
}
