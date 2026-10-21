using System;
using System.Collections.Generic;
using System.Text;

namespace Preparazione_Verifica
{
    public class Preparazione
    {

        static bool IsMagico(int[,] m)
        {
            if (m.GetLength(0) != m.GetLength(1))
            {
                throw new ArgumentException("La matrice non è quadrata");
            }

            int contatoreMagico = 0;
            int contatoreRighe = 0;
            int contatoreColonne = 0;
            int contatoreDiag1 = 0;
            int contatoreDiag2 = 0;
            int n = m.GetLength(0);

            for (int i = 0; i < m.GetLength(0); i++)
            {
                contatoreMagico += m[0, i];
            }

            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    contatoreRighe += m[i, j];
                    contatoreColonne += m[j, i];
                }

                if (contatoreMagico != contatoreRighe || contatoreMagico != contatoreColonne)
                {
                    return false;
                }

                contatoreDiag1 += m[i, i];
                contatoreDiag2 += m[n - i - 1, i];
            }

            if (contatoreMagico == contatoreDiag1 && contatoreMagico == contatoreDiag2)
            {
                return true;
            }

            return false;
        }


        public static long[][] Tartaglia(int righe)
        {
            long[][] TriangoloTartaglia = new long[righe][];

            for (int i = 0; i < righe; i++)
            {
                TriangoloTartaglia[i] = new long[i + 1];
            }

            for (int i = 0; i < righe; i++)
            {
                for (int j = 0; j < TriangoloTartaglia[i].Length; j++)
                {
                    if (i > 1 && j > 0 && j < TriangoloTartaglia[i].Length - 1)
                    {
                        TriangoloTartaglia[i][j] = TriangoloTartaglia[i - 1][j - 1] + TriangoloTartaglia[i - 1][j];
                    }
                }
                TriangoloTartaglia[i][0] = 1;
                TriangoloTartaglia[i][TriangoloTartaglia[i].Length - 1] = 1;
            }

            return TriangoloTartaglia;
        }
        public static void StampaTriangolo(long[][] triangolo)
        {
            int righe = triangolo.Length;

            for (int i = 0; i < righe; i++)
            {
                // Aggiunge spazi all'inizio per centrare la riga
                Console.Write(new string(' ', (righe - i - 1) * 2));

                // Stampa i numeri della riga formattati
                for (int j = 0; j < triangolo[i].Length; j++)
                {
                    Console.Write($"{triangolo[i][j],4}");
                }

                Console.WriteLine();
            }
        }

        public static void Main()
        {
            int n = 3;
            long[][] t = Tartaglia(n);
            StampaTriangolo(t);
            ParoleDistinte("ciao, mondo! il mondo è bello? sì, ciao a tutti; il mondo è grande!", "grande");
        }



        public static HashSet<string> ParoleDistinte(string testo, string parola)
        {
            List<string> Parole;

            Parole = testo.Split(' ', ',', '.', ';', '!', '?').ToList();
            HashSet<string> HashSetParole = new(Parole);

            int ContaParole = HashSetParole.Count;
            Console.WriteLine($"numero parole: {ContaParole}");

            if (HashSetParole.Contains(parola))
            {
                Console.WriteLine($"{parola} è contenuta nel testo");
            }

            return HashSetParole;
        }
    }
}

