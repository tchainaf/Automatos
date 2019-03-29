using System;
using System.Linq;

namespace N1___Autômatos
{
    public class Regra
    {
        public string antes, depois;
        public int num;

        private bool Valida(string valor, char[] nTerm, char[] term, bool permiteVazio)
        {
            if (!permiteVazio && valor.Contains("@"))
                return false;
            foreach (char letra in valor)
                if (!nTerm.Contains(letra) && !term.Contains(letra) && valor != "@")
                    return false;
            return true;
        }

        public Regra(string antes, string depois, int num, char[] nTerm, char[] term, int[] numeros)
        {
            if (!Valida(antes, nTerm, term, false))
                throw new Exception($"A regra {num} possui variáveis não declaradas!");
            this.antes = antes;

            if (!Valida(depois, nTerm, term, true))
                throw new Exception($"A regra {num} possui variáveis não declaradas!");
            this.depois = depois;

            foreach (int n in numeros)
                if (num == n)
                    throw new Exception($"Há mais de uma regra de número {num}!");
            this.num = num;
        }
    }
}
