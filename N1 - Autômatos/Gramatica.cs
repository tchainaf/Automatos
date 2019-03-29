using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace N1___Autômatos
{
    public class Gramatica
    {
        char[] nTerm, term;
        char inicio;
        List<Regra> regras = new List<Regra>();
        string[] listaRegras, palavrasGeradas = new string[20];

        public void PreencheRegras()
        {
            int[] numeros = new int[listaRegras.Length];
            for (int i = 0; i < listaRegras.Length; i++)
            {
                string[] param = listaRegras[i].Substring(1, listaRegras[i].Length - 2).Split(',');
                Regra x = new Regra(param[0].Trim(), param[1].Trim(), Convert.ToInt32(param[2].Trim()), nTerm, term, numeros);
                regras.Add(x);
                numeros[i] = Convert.ToInt32(param[2].Trim());
            }
        }

        public List<Regra> ContemLetra(string palavra)
        {
            List<Regra> regrasContem = new List<Regra>();
            foreach (char letra in palavra)
            {
                if (Char.IsUpper(letra))
                {
                    regras.ForEach(delegate (Regra r)
                     {
                         if (r.antes.Contains(letra.ToString()))
                             regrasContem.Add(r);
                     });
                }
            }
            return regrasContem; 
        }

        public string[] GerarPalavras(int tempomax, int qtdepalavras)
        {
            Stopwatch tempo = new Stopwatch();
            tempo.Start();
            Regra regraUsada;
            Random sorteio = new Random();
            int cont = 0;
            do //20 palavras
            {
                string palavraAux = inicio.ToString();
                bool letraNTerm, palavraUnica = true;

                do //palavra terminal
                {
                    letraNTerm = false;
                    List<Regra> regrasPossiveis = ContemLetra(palavraAux);
                    while (true) //procura regra aplicável
                    {
                        int num = sorteio.Next(regrasPossiveis.Count);
                        if (palavraAux.Contains(regrasPossiveis[num].antes))
                        {
                            regraUsada = regrasPossiveis[num];
                            break;
                        }
                    }

                    int posicao = palavraAux.IndexOf(regraUsada.antes);
                    if (regraUsada.depois == "@")
                        palavraAux = palavraAux.Remove(posicao, regraUsada.antes.Length);
                    else
                    {
                        palavraAux = palavraAux.Remove(posicao, regraUsada.antes.Length);
                        palavraAux = palavraAux.Insert(posicao, regraUsada.depois);
                    }

                    if (palavraAux.IndexOf('-') == -1)
                        palavraAux += " - " + regraUsada.num;
                    else
                        palavraAux += "," + regraUsada.num;
                    if (palavraAux.ToLower() != palavraAux)
                            letraNTerm = true;

                    if (tempo.Elapsed >= new TimeSpan(0, 0, tempomax))
                        if (tempomax == 50 && cont > 7)
                            return palavrasGeradas;
                        else
                            return null;
                } while (letraNTerm);

                for (int i = 0; i < cont; i++) //checa palavras repetidas
                    if (palavrasGeradas[i].Substring(0, palavrasGeradas[i].IndexOf(' ')) == palavraAux.Substring(0, palavraAux.IndexOf(' ')))
                    {
                        palavraUnica = false;
                        break;
                    }
                if (palavraUnica)
                {
                    palavrasGeradas[cont] = palavraAux;
                    cont++;
                }
            } while (cont != qtdepalavras);
            tempo.Stop();
            return palavrasGeradas;
        }

        public Gramatica(char[] nTerm, char[] term, char inicio, string[] lista)
        {
            for (int i = 0; i < nTerm.Length; i++)
                for (int j = i + 1; j < nTerm.Length; j++)
                    if (nTerm[i] == nTerm[j])
                        throw new Exception("Há variáveis não terminais iguais!");
            this.nTerm = nTerm;

            for (int i = 0; i < term.Length; i++)
                for (int j = i + 1; j < term.Length; j++)
                    if (term[i] == term[j])
                        throw new Exception("Há variáveis terminais iguais!");
            this.term = term;

            bool existe = false;
            foreach (char letra in nTerm)
                if (letra == inicio)
                    existe = true;
            if (!existe)
                throw new Exception("O símbolo inicial não é válido!");
            this.inicio = inicio;

            listaRegras = lista;
        }
    }
}
