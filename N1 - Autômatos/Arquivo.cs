using System;
using System.IO;
using System.Text;

namespace N1___Autômatos
{
    public class Arquivo
    {
        private static string nomeArquivo;

        public static Gramatica ValidacaoIN(string arq)
        {
            nomeArquivo = arq.Replace(".IN", "");
            string[] arquivo = File.ReadAllLines(arq, Encoding.Default), regras = new string[arquivo.Length - 5];
            char[] nTerm, term;
            char inicio;

            if (arquivo[0].Trim().ToUpper() != "GRAMATICA" && arquivo[0].Trim().ToUpper() != "GRAMÁTICA")
                throw new Exception("A 1ª linha do arquivo está fora da estrutura aceita!");

            string[] nTerminais = arquivo[1].Split(',');
            foreach (string letra in nTerminais)
                if (letra.Trim().Length != 1 || !Char.IsUpper(Convert.ToChar(letra.Trim())))
                    throw new Exception("A 2ª linha do arquivo está fora da estrutura aceita!");
            nTerm = new char[nTerminais.Length];
            for (int i = 0; i < nTerm.Length; i++)
                nTerm[i] = Convert.ToChar(nTerminais[i].Trim());

            string[] terminais = arquivo[2].Split(',');
            foreach (string letra in terminais)
                if (letra.Trim().Length != 1 || (!Char.IsLower(Convert.ToChar(letra.Trim())) && !Char.IsNumber(Convert.ToChar(letra.Trim()))))
                    throw new Exception("A 3ª linha do arquivo está fora da estrutura aceita!");
            term = new char[terminais.Length];
            for (int i = 0; i < term.Length; i++)
                term[i] = Convert.ToChar(terminais[i].Trim());

            if (arquivo[3].Trim().Length != 1 || !Char.IsUpper(Convert.ToChar(arquivo[3].Trim())))
                throw new Exception("A 4ª linha do arquivo está fora da estrutura aceita!");
            inicio = Convert.ToChar(arquivo[3].Trim());

            int num;
            for (int i = 4; i < arquivo.Length - 1; i++)
            {
                string linha = arquivo[i].Trim();
                string[] param = linha.Substring(1, linha.Length - 2).Split(',');
                if (linha[0] != '(' || linha[linha.Length - 1] != ')' || param[0].Trim().Length < 1 || param[1].Trim().Length < 1 || !Int32.TryParse(param[2].Trim(), out num))
                    throw new Exception($"A {++i}ª linha do arquivo está fora da estrutura aceita!");
                regras[i - 4] = linha;
            }

            if (arquivo[arquivo.Length - 1].Trim() != "####")
                throw new Exception($"A {arquivo.Length}ª linha do arquivo está fora da estrutura aceita!");

            return new Gramatica(nTerm, term, inicio, regras);
        }

        public static void SalvarOUT(string[] palavras)
        {
            File.WriteAllLines(nomeArquivo + ".OUT", palavras, Encoding.Default);
        }
    }
}
