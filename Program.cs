using System;
using System.Collections.Generic;
public class Es1
{
    /*
     * CONSEGNA:
     * Es. 1 — Quadrato magico
     * Scrivi static bool IsMagico(int[,] m). Una matrice n×n è un quadrato magico se la somma di ogni riga,
     * di ogni colonna e delle due diagonali è la stessa. Se la matrice non è quadrata, il metodo lancia
     * ArgumentException. Usa m.GetLength(0) e m.GetLength(1).
     * Prova con:
     * 2 7 6
     * 9 5 1
     * 4 3 8
     */

    public static int[,] MatriceTest =
    {
        { 2, 7, 6 },
        { 9, 5, 1 },
        { 4, 3, 8 }
    };

    public static bool IsMagico(int[,] m)
    {
        // SCELTA ALGORITMICA: Controlliamo subito 'm == null' per evitare eccezioni di puntatore nullo.
        // Successivamente verifichiamo che righe e colonne coincidano; se differiscono, la matrice non è quadrata.
        if (m == null || m.GetLength(0) != m.GetLength(1))
        {
            throw new ArgumentException("La matrice deve essere non nulla e quadrata.");
        }

        int n = m.GetLength(0);

        // Una matrice vuota o 1x1 ha un solo elemento o nessuno, quindi per definizione è magica.
        if (n <= 1) return true;

        // SCELTA ALGORITMICA: Usiamo la somma della prima riga (indice 0) come "costante magica di riferimento".
        // Tutte le altre righe, colonne e diagonali dovranno confrontarsi con questo valore.
        int sommaTarget = 0;
        for (int j = 0; j < n; j++)
        {
            sommaTarget += m[0, j];
        }

        // Verifica di tutte le righe successive (da 1 a n-1).
        // Appena una riga ha una somma diversa, interrompiamo con false (fail-fast per ottimizzare il tempo).
        for (int i = 1; i < n; i++)
        {
            int sommaRiga = 0;
            for (int j = 0; j < n; j++)
            {
                sommaRiga += m[i, j];
            }
            if (sommaRiga != sommaTarget) return false;
        }

        // Verifica delle colonne: invertiamo l'ordine dei cicli (prima j per le colonne, poi i per scendere lungo le righe).
        for (int j = 0; j < n; j++)
        {
            int sommaColonna = 0;
            for (int i = 0; i < n; i++)
            {
                sommaColonna += m[i, j];
            }
            if (sommaColonna != sommaTarget) return false;
        }

        // Verifica delle due diagonali: le calcoliamo in un unico ciclo lineare O(n).
        // Diagonale principale: coordinate coincidenti [i, i].
        // Diagonale secondaria: la colonna decresce all'aumentare della riga [i, n - 1 - i].
        int sommaDiagPrincipale = 0;
        int sommaDiagSecondaria = 0;
        for (int i = 0; i < n; i++)
        {
            sommaDiagPrincipale += m[i, i];
            sommaDiagSecondaria += m[i, n - 1 - i];
        }

        return sommaDiagPrincipale == sommaTarget && sommaDiagSecondaria == sommaTarget;
    }
}

public class Es2
{
    /*
     * CONSEGNA:
     * Es. 2 — Sala cinema
     * Una sala è una bool[,] (true = posto occupato), 8 file da 12 posti. Scrivi:
     * 1. static int PostiLiberi(bool[,] sala)
     * 2. static int FilaPiuPiena(bool[,] sala) - restituisce l'indice della fila con più posti occupati; a parità, la prima.
     * 3. static int CercaPostiContigui(bool[,] sala, int fila, int n) - restituisce l'indice del primo posto di un blocco di n posti liberi consecutivi nella fila, oppure -1.
     * 4. static void StampaSala(bool[,] sala) - stampa x per occupato e . per libero, con il numero di fila a sinistra.
     */

    public static bool[,] SalaTest = new bool[8, 12];

    public static int PostiLiberi(bool[,] sala)
    {
        int contatore = 0;
        int file = sala.GetLength(0);
        int posti = sala.GetLength(1);

        // SCELTA ALGORITMICA: Scorriamo l'intera matrice cella per cella.
        // Poiché true = occupato, la negazione '!sala[f, p]' identifica esattamente i posti liberi (false).
        for (int f = 0; f < file; f++)
        {
            for (int p = 0; p < posti; p++)
            {
                if (!sala[f, p]) contatore++;
            }
        }
        return contatore;
    }

    public static int FilaPiuPiena(bool[,] sala)
    {
        int file = sala.GetLength(0);
        int posti = sala.GetLength(1);

        int maxOccupati = -1;
        int indiceFila = 0;

        // SCELTA ALGORITMICA: Usiamo il confronto stretto '>' (occupatiFila > maxOccupati).
        // In questo modo, se una fila successiva ha lo stesso numero di occupati, non sovrascrive 'indiceFila',
        // garantendo il rispetto del requisito "a parità, la prima fila".
        for (int f = 0; f < file; f++)
        {
            int occupatiFila = 0;
            for (int p = 0; p < posti; p++)
            {
                if (sala[f, p]) occupatiFila++;
            }

            if (occupatiFila > maxOccupati)
            {
                maxOccupati = occupatiFila;
                indiceFila = f;
            }
        }
        return indiceFila;
    }

    public static int CercaPostiContigui(bool[,] sala, int fila, int n)
    {
        int posti = sala.GetLength(1);
        int consecutivi = 0;

        // SCELTA ALGORITMICA: Algoritmo a finestra scorrevole su singola riga.
        // Incrementiamo il contatore quando troviamo un posto libero; se troviamo un occupato, azzeriamo il conteggio.
        // Appena il conteggio raggiunge n, ricaviamo la posizione iniziale con la formula: 'indiceCorrente - n + 1'.
        for (int p = 0; p < posti; p++)
        {
            if (!sala[fila, p])
            {
                consecutivi++;
                if (consecutivi == n)
                {
                    return p - n + 1;
                }
            }
            else
            {
                consecutivi = 0;
            }
        }
        return -1;
    }

    public static void StampaSala(bool[,] sala)
    {
        int file = sala.GetLength(0);
        int posti = sala.GetLength(1);

        // SCELTA ALGORITMICA: Formattiamo l'indice con 'D2' per mantenere le colonne allineate anche quando si passa da cifre singole a doppie.
        for (int f = 0; f < file; f++)
        {
            Console.Write($"Fila {f:D2}: ");
            for (int p = 0; p < posti; p++)
            {
                Console.Write(sala[f, p] ? "x " : ". ");
            }
            Console.WriteLine();
        }
    }
}

public class Es3
{
    /*
     * CONSEGNA:
     * Es. 3 — Campo minato
     * Il campo è una char[,] dove '*' è una mina e '.' una cella vuota. Scrivi static int[,]
     * CalcolaNumeri(char[,] campo) che restituisce, per ogni cella vuota, il numero di mine nelle 8 celle
     * adiacenti; per le celle con mina scrivi -1. Attenzione ai bordi: non leggere fuori dalla matrice. Suggerimento:
     * due array dr e dc con gli 8 spostamenti.
     */

    public static char[,] CampoTest = new char[,]
    {
        { '*', '.', '.', '.' },
        { '.', '*', '.', '.' },
        { '.', '.', '.', '.' }
    };

    public static int[,] CalcolaNumeri(char[,] campo)
    {
        int righe = campo.GetLength(0);
        int colonne = campo.GetLength(1);
        int[,] risultato = new int[righe, colonne];

        // SCELTA ALGORITMICA: Vettori di direzione (Delta Row e Delta Col).
        // Invece di scrivere 8 istruzioni if annidate per ogni cella adiacente, usiamo due array paralleli.
        // Ogni coppia (dr[k], dc[k]) rappresenta uno degli 8 vicini (Nord, Sud, Est, Ovest e le 4 diagonali).
        int[] dr = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dc = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int r = 0; r < righe; r++)
        {
            for (int c = 0; c < colonne; c++)
            {
                if (campo[r, c] == '*')
                {
                    risultato[r, c] = -1; // Cella contenente una mina
                }
                else
                {
                    int mineTrovate = 0;

                    // Esploriamo gli 8 vicini
                    for (int k = 0; k < 8; k++)
                    {
                        int nr = r + dr[k];
                        int nc = c + dc[k];

                        // SCELTA ALGORITMICA: Controllo dei limiti di confine (Boundary Check).
                        // Verifichiamo che la coordinata calcolata non esca dalla matrice prima di accedere a campo[nr, nc].
                        if (nr >= 0 && nr < righe && nc >= 0 && nc < colonne)
                        {
                            if (campo[nr, nc] == '*') mineTrovate++;
                        }
                    }
                    risultato[r, c] = mineTrovate;
                }
            }
        }
        return risultato;
    }
}

public class Es4
{
    /*
     * CONSEGNA:
     * Es. 4 — Voti irregolari
     * Ogni studente ha un numero diverso di voti: double[][] voti, con un array parallelo string[] nomi. Scrivi
     * Media(double[] v) (0 se vuoto), StudenteConPiuVoti(double[][] voti) e un metodo che stampi nome,
     * numero di voti e media di ciascuno.
     */

    public static string[] NomiTest = { "Mario", "Luigi", "Anna" };
    public static double[][] VotiTest = new double[][]
    {
        new double[] { 7.5, 8.0, 6.5 },
        new double[] { 9.0, 9.5 },
        new double[] { }
    };

    public static double Media(double[] v)
    {
        // SCELTA ALGORITMICA: Gestione casi limite.
        // Se l'array è nullo o vuoto (0 voti), restituiamo 0.0 per evitare la divisione per zero (NaN).
        if (v == null || v.Length == 0) return 0.0;

        double somma = 0;
        for (int i = 0; i < v.Length; i++) somma += v[i];
        return somma / v.Length;
    }

    public static int StudenteConPiuVoti(double[][] voti)
    {
        int maxIndex = 0;
        int maxCount = -1;

        // SCELTA ALGORITMICA: In un jagged array ogni riga 'voti[i]' ha una propria lunghezza indipendente (.Length).
        // Scorriamo le righe tenendo traccia dell'indice che possiede la lunghezza massima.
        for (int i = 0; i < voti.Length; i++)
        {
            if (voti[i] != null && voti[i].Length > maxCount)
            {
                maxCount = voti[i].Length;
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    public static void StampaRiepilogo(string[] nomi, double[][] voti)
    {
        // Gli array sono paralleli: l'indice 'i' associa nomi[i] a voti[i].
        for (int i = 0; i < nomi.Length; i++)
        {
            int numVoti = voti[i] != null ? voti[i].Length : 0;
            double media = Media(voti[i]);
            Console.WriteLine($"Studente: {nomi[i],-10} | Voti: {numVoti,2} | Media: {media:F2}");
        }
    }
}

public class Es5
{
    /*
     * CONSEGNA:
     * Es. 5 — Triangolo di Tartaglia
     * Scrivi static long[][] Tartaglia(int righe). La riga i ha i+1 elementi; il primo e l'ultimo valgono 1,
     * gli altri sono la somma dei due elementi sopra. Stampa il triangolo centrato. Domanda di riflessione: quanta
     * memoria risparmi rispetto a una long[righe, righe]?
     */

    public static long[][] Tartaglia(int righe)
    {
        // SCELTA ALGORITMICA: Usiamo long[][] (jagged array) perché ogni riga 'i' alloca esattamente 'i + 1' celle.
        // Risposta alla riflessione: una matrice rettangolare alloca righe * righe celle.
        // Il triangolo alloca righe*(righe+1)/2 celle, risparmiando circa il 50% della memoria.
        long[][] t = new long[righe][];

        for (int i = 0; i < righe; i++)
        {
            t[i] = new long[i + 1];
            t[i][0] = 1;      // Bordo sinistro sempre 1
            t[i][i] = 1;      // Bordo destro sempre 1

            // Gli elementi interni sono la somma dei due elementi adiacenti della riga precedente
            for (int j = 1; j < i; j++)
            {
                t[i][j] = t[i - 1][j - 1] + t[i - 1][j];
            }
        }
        return t;
    }

    public static void StampaTartaglia(long[][] t)
    {
        int n = t.Length;
        // SCELTA ALGORITMICA: Per centrare a piramide il triangolo calcoliamo uno spazio iniziale decrescente per ciascuna riga.
        for (int i = 0; i < n; i++)
        {
            Console.Write(new string(' ', (n - i - 1) * 2));
            for (int j = 0; j < t[i].Length; j++)
            {
                Console.Write($"{t[i][j],3} ");
            }
            Console.WriteLine();
        }
    }
}

public class Es6
{
    /*
     * CONSEGNA:
     * Es. 6 — Linee del trasporto
     * string[][] linee contiene, per ogni linea di autobus, la sequenza delle fermate (lunghezze diverse). Scrivi:
     * 1. static int[] LineePerFermata(string[][] linee, string fermata) - gli indici delle linee che passano per la fermata.
     * 2. static string FermataDiScambio(string[][] linee, int a, int b) - la prima fermata della linea a servita anche dalla linea b, oppure null.
     * 3. static int Distanza(string[] linea, string da, string a) - numero di fermate tra due fermate della stessa linea, -1 se una delle due manca.
     */

    public static string[][] LineeTest = new string[][]
    {
        new string[] { "A", "B", "C", "D" },
        new string[] { "E", "C", "F" },
        new string[] { "G", "H" }
    };

    public static int[] LineePerFermata(string[][] linee, string fermata)
    {
        // SCELTA ALGORITMICA: Usiamo una List<int> temporanea poiché non possiamo sapere a priori quante linee contengano la fermata.
        List<int> result = new List<int>();
        for (int i = 0; i < linee.Length; i++)
        {
            if (linee[i] != null)
            {
                for (int j = 0; j < linee[i].Length; j++)
                {
                    if (linee[i][j] == fermata)
                    {
                        result.Add(i);
                        break; // Trovata sulla linea: passiamo alla linea successiva senza contare duplicati
                    }
                }
            }
        }
        return result.ToArray();
    }

    public static string FermataDiScambio(string[][] linee, int a, int b)
    {
        if (a < 0 || a >= linee.Length || b < 0 || b >= linee.Length) return null;

        // SCELTA ALGORITMICA: Rispettiamo l'ordine della linea 'a'.
        // Appena troviamo una fermata di 'a' che compare anche in 'b', la restituiamo immediatamente (è la prima fermata di scambio).
        for (int i = 0; i < linee[a].Length; i++)
        {
            string f = linee[a][i];
            for (int j = 0; j < linee[b].Length; j++)
            {
                if (f == linee[b][j]) return f;
            }
        }
        return null;
    }

    public static int Distanza(string[] linea, string da, string a)
    {
        if (linea == null) return -1;

        // SCELTA ALGORITMICA: Array.IndexOf scansiona la linea O(n) restituendo la posizione.
        // Se una delle due fermate non esiste (-1), il metodo restituisce -1 come richiesto.
        // La distanza è data dal valore assoluto della differenza tra gli indici.
        int idxDa = Array.IndexOf(linea, da);
        int idxA = Array.IndexOf(linea, a);

        if (idxDa == -1 || idxA == -1) return -1;
        return Math.Abs(idxDa - idxA);
    }
}

public class Es7
{
    /*
     * CONSEGNA:
     * Es. 7 — Lista della spesa
     * Con una List<string> realizza un menu con: aggiungi (rifiuta stringhe vuote), rimuovi per nome, rimuovi
     * per posizione, cerca, stampa numerata. Usa Add, Remove, RemoveAt, IndexOf, Count.
     */

    public static List<string> ListaSpesa = new List<string>();

    public static void Aggiungi(string elemento, List<string> lista)
    {
        // Validazione preventiva: rifiuta null, stringhe vuote o composti di soli spazi
        if (string.IsNullOrWhiteSpace(elemento))
            throw new ArgumentException("L'elemento non può essere vuoto.");
        lista.Add(elemento);
    }

    public static void RimuoviPerNome(string nome, List<string> lista)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Il nome da rimuovere non può essere vuoto.");
        lista.Remove(nome); // Remove elimina solo la prima occorrenza trovata
    }

    public static void RimuoviPerPosizione(int posizione, List<string> lista)
    {
        // Validazione dell'indice entro il range valido [0, Count - 1]
        if (posizione < 0 || posizione >= lista.Count)
            throw new ArgumentOutOfRangeException(nameof(posizione), "Indice fuori dai limiti della lista.");
        lista.RemoveAt(posizione);
    }

    public static void Cerca(string elemento, List<string> lista)
    {
        if (string.IsNullOrWhiteSpace(elemento))
            throw new ArgumentException("L'elemento da cercare non può essere vuoto.");

        int indice = lista.IndexOf(elemento);
        if (indice != -1)
            Console.WriteLine($"'{elemento}' trovato alla posizione {indice}.");
        else
            Console.WriteLine($"'{elemento}' non presente nella lista.");
    }

    public static void StampaNumerata(List<string> lista)
    {
        if (lista == null || lista.Count == 0)
        {
            Console.WriteLine("La lista è vuota.");
            return;
        }

        // Stampa a base 1 leggibile dall'utente finale
        for (int i = 0; i < lista.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {lista[i]}");
        }
    }
}

public class Es8
{
    /*
     * CONSEGNA:
     * Es. 8 — Fusione di liste ordinate
     * Scrivi static List<int> Fondi(List<int> a, List<int> b): date due liste già ordinate, restituisce una
     * lista ordinata con tutti gli elementi, in un solo passaggio con due indici. Non usare Sort. Variante: versione
     * che scarta i duplicati.
     */

    public static List<int> ListaA = new List<int> { 1, 3, 5, 7 };
    public static List<int> ListaB = new List<int> { 2, 3, 6, 8, 9 };

    public static List<int> Fondi(List<int> a, List<int> b)
    {
        List<int> fusa = new List<int>();
        int i = 0, j = 0;

        // SCELTA ALGORITMICA: Algoritmo di Two-Pointers (la base del Merge Sort).
        // Poiché entrambe le liste sono già ordinate, confrontiamo i valori correnti a[i] e b[j].
        // Il minore entra nella lista fusa e il rispettivo indice avanza. Questo garantisce complessità lineare O(a.Count + b.Count).
        while (i < a.Count && j < b.Count)
        {
            if (a[i] <= b[j])
            {
                fusa.Add(a[i]);
                i++;
            }
            else
            {
                fusa.Add(b[j]);
                j++;
            }
        }

        // Recuperiamo gli elementi residui della lista non ancora terminata
        while (i < a.Count) { fusa.Add(a[i]); i++; }
        while (j < b.Count) { fusa.Add(b[j]); j++; }

        return fusa;
    }

    public static List<int> FondiSenzaDuplicati(List<int> a, List<int> b)
    {
        List<int> fusa = new List<int>();
        int i = 0, j = 0;

        // SCELTA ALGORITMICA: Per scartare i duplicati senza HashSet né Sort, controlliamo solo l'ultimo elemento inserito in coda.
        void AggiungiSeNuovo(int val)
        {
            if (fusa.Count == 0 || fusa[fusa.Count - 1] != val)
            {
                fusa.Add(val);
            }
        }

        while (i < a.Count && j < b.Count)
        {
            if (a[i] < b[j])
            {
                AggiungiSeNuovo(a[i]);
                i++;
            }
            else if (b[j] < a[i])
            {
                AggiungiSeNuovo(b[j]);
                j++;
            }
            else
            {
                // Elementi identici: ne inseriamo uno solo e avanziamo entrambi gli indici
                AggiungiSeNuovo(a[i]);
                i++;
                j++;
            }
        }

        while (i < a.Count) { AggiungiSeNuovo(a[i]); i++; }
        while (j < b.Count) { AggiungiSeNuovo(b[j]); j++; }

        return fusa;
    }
}

public class Es9
{
    /*
     * CONSEGNA:
     * Es. 9 — Coda di stampa con priorità
     * Crea la classe Lavoro (Id, Documento, Pagine, Priorita 1-5) e la classe CodaStampa che incapsula una
     * List<Lavoro> privata. Metodi:
     * 1. Accoda(Lavoro l) inserisce mantenendo la lista ordinata per priorità decrescente; a parità di priorità rispetta l'ordine di arrivo.
     * 2. Lavoro Prossimo() estrae il primo lavoro; InvalidOperationException se la coda è vuota.
     * 3. bool Annulla(int id)
     * 4. int PagineInAttesa()
     * 5. IReadOnlyList<Lavoro> Elenco esposizione in sola lettura.
     */

    public class Lavoro
    {
        public int Id { get; set; }
        public string Documento { get; set; }
        public int Pagine { get; set; }
        public int Priorita { get; set; }

        public Lavoro(int id, string documento, int pagine, int priorita)
        {
            Id = id;
            Documento = documento;
            Pagine = pagine;
            Priorita = priorita;
        }
    }

    public class CodaStampa
    {
        // Incapsulamento: la lista è privata per impedire modifiche esterne incontrollate
        private List<Lavoro> _lavori = new List<Lavoro>();

        public void Accoda(Lavoro l)
        {
            int index = 0;

            // SCELTA ALGORITMICA: Insertion Sort mirato.
            // Usiamo '>= l.Priorita' per fare in modo che un nuovo elemento con uguale priorità
            // venga posizionato dopo quelli già arrivati (stabilità FIFO a parità di priorità).
            while (index < _lavori.Count && _lavori[index].Priorita >= l.Priorita)
            {
                index++;
            }
            _lavori.Insert(index, l);
        }

        public Lavoro Prossimo()
        {
            // Estrazione in testa O(1) ammortizzato: l'elemento in posizione 0 ha sempre la priorità massima
            if (_lavori.Count == 0)
                throw new InvalidOperationException("La coda di stampa è vuota.");

            Lavoro primo = _lavori[0];
            _lavori.RemoveAt(0);
            return primo;
        }

        public bool Annulla(int id)
        {
            // Ricerca lineare per identificativo univoco
            int index = _lavori.FindIndex(x => x.Id == id);
            if (index != -1)
            {
                _lavori.RemoveAt(index);
                return true;
            }
            return false;
        }

        public int PagineInAttesa()
        {
            int tot = 0;
            for (int i = 0; i < _lavori.Count; i++) tot += _lavori[i].Pagine;
            return tot;
        }

        // Esposizione in sola lettura: chi usa la classe può leggere senza poter aggiungere o rimuovere elementi arbitrari
        public IReadOnlyList<Lavoro> Elenco => _lavori.AsReadOnly();
    }

    public static CodaStampa CodaTest = new CodaStampa();
}

public class Es10
{
    /*
     * CONSEGNA:
     * Es. 10 — Parole distinte
     * Dato un testo, conta le parole distinte ignorando maiuscole e punteggiatura. Usa Split con i separatori
     * ',', '.', ';', ':', ' ', '!', '?' e StringSplitOptions.RemoveEmptyEntries. Stampa quante sono e se una
     * parola scelta è presente (Contains).
     */

    public static string TestoProva = "Ciao mondo! Questo è un test; un semplice mondo di prova, ciao?";

    public static void EseguiConteggio(string testo, string parolaDaCercare)
    {
        char[] separatori = { ',', '.', ';', ':', ' ', '!', '?' };

        // SCELTA ALGORITMICA: StringSplitOptions.RemoveEmptyEntries evita che due segni di punteggiatura consecutivi generino token vuoti.
        string[] parole = testo.Split(separatori, StringSplitOptions.RemoveEmptyEntries);

        // SCELTA ALGORITMICA: HashSet<string> implementa una tabella hash O(1).
        // Scartiamo duplicati convertendo le stringhe in ToLower(), garantendo case-insensitivity.
        HashSet<string> paroleDistinte = new HashSet<string>();
        foreach (string p in parole)
        {
            paroleDistinte.Add(p.ToLower());
        }

        Console.WriteLine($"Numero di parole distinte: {paroleDistinte.Count}");

        // Ricerca istantanea O(1) con Contains
        bool presente = paroleDistinte.Contains(parolaDaCercare.ToLower());
        Console.WriteLine($"La parola '{parolaDaCercare}' è presente? {presente}");
    }
}

public class Es11
{
    /*
     * CONSEGNA:
     * Es. 11 — Iscrizioni ai corsi
     * Tre HashSet<string> contengono gli iscritti a Robotica, Teatro e Coding. Senza modificare gli insiemi
     * originali (crea copie con new HashSet<string>(s)), calcola:
     * 1. chi frequenta almeno un corso (UnionWith);
     * 2. chi frequenta Robotica e Coding (IntersectWith);
     * 3. chi frequenta solo Teatro (ExceptWith);
     * 4. chi frequenta esattamente uno dei due tra Robotica e Coding (SymmetricExceptWith).
     * Domanda: perché Add restituisce un bool? Usalo per segnalare un'iscrizione doppia.
     */

    public static HashSet<string> Robotica = new HashSet<string> { "Marco", "Luca", "Sara" };
    public static HashSet<string> Teatro = new HashSet<string> { "Sara", "Elena", "Davide" };
    public static HashSet<string> Coding = new HashSet<string> { "Marco", "Anna", "Luca" };

    public static void EseguiAnalisi()
    {
        // SCELTA ALGORITMICA: Operazioni insiemistiche distruttive.
        // Poiché i metodi UnionWith, IntersectWith, ecc. modificano l'istanza su cui vengono chiamati,
        // creiamo sempre una copia indipendente dell'insieme con 'new HashSet<string>(s)'.

        // 1. Unione totale: chi frequenta almeno un corso
        HashSet<string> almenoUno = new HashSet<string>(Robotica);
        almenoUno.UnionWith(Teatro);
        almenoUno.UnionWith(Coding);

        // 2. Intersezione: chi frequenta sia Robotica che Coding contemporaneamente
        HashSet<string> roboticaECoding = new HashSet<string>(Robotica);
        roboticaECoding.IntersectWith(Coding);

        // 3. Differenza insiemistica: iscritti a Teatro togliendo chiunque faccia anche Robotica o Coding
        HashSet<string> soloTeatro = new HashSet<string>(Teatro);
        soloTeatro.ExceptWith(Robotica);
        soloTeatro.ExceptWith(Coding);

        // 4. Differenza simmetrica: chi fa Robotica oppure Coding, ma NON entrambi
        HashSet<string> esattoUno = new HashSet<string>(Robotica);
        esattoUno.SymmetricExceptWith(Coding);

        Console.WriteLine($"Almeno un corso: {string.Join(", ", almenoUno)}");
        Console.WriteLine($"Robotica e Coding: {string.Join(", ", roboticaECoding)}");
        Console.WriteLine($"Solo Teatro: {string.Join(", ", soloTeatro)}");
        Console.WriteLine($"Uno solo tra Robotica e Coding: {string.Join(", ", esattoUno)}");
    }

    public static bool Iscrivi(HashSet<string> corso, string studente)
    {
        // SCELTA ALGORITMICA: In C# HashSet.Add restituisce true se l'elemento è stato aggiunto,
        // false se l'elemento esisteva già nell'insieme. Non serve un controllo preventivo con Contains.
        bool aggiunto = corso.Add(studente);
        if (!aggiunto)
        {
            Console.WriteLine($"Iscrizione respinta: {studente} è già iscritto a questo corso.");
        }
        return aggiunto;
    }
}

public class Es12
{
    /*
     * CONSEGNA:
     * Es. 12 — Amici suggeriti
     * Una rete sociale è una Dictionary<string, HashSet<string>> (persona → amici). L'amicizia è simmetrica.
     * Scrivi:
     * 1. AggiungiAmicizia(a, b) - aggiorna entrambi i lati, rifiuta a == b.
     * 2. HashSet<string> AmiciInComune(a, b)
     * 3. HashSet<string> Suggeriti(p) - amici degli amici di p, esclusi p e i suoi amici diretti.
     * 4. bool Raggiungibile(a, b) esiste una catena di amicizie da a a b? Usa una Queue<string> e un HashSet<string> dei visitati.
     */

    public class ReteSociale
    {
        // Modello a grafo non orientato: ogni nodo (string) punta a un insieme di nodi adiacenti (HashSet<string>)
        private static Dictionary<string, HashSet<string>> _rete = new Dictionary<string, HashSet<string>>();

        public static void AggiungiAmicizia(string a, string b)
        {
            if (a == b) throw new ArgumentException("Una persona non può essere amica di se stessa.");

            // Creazione lazily dei nodi se non ancora censiti nel dizionario
            if (!_rete.ContainsKey(a)) _rete[a] = new HashSet<string>();
            if (!_rete.ContainsKey(b)) _rete[b] = new HashSet<string>();

            // Simmetria del grafo: aggiunta bidirezionale
            _rete[a].Add(b);
            _rete[b].Add(a);
        }

        public static HashSet<string> AmiciInComune(string a, string b)
        {
            if (!_rete.ContainsKey(a) || !_rete.ContainsKey(b)) return new HashSet<string>();

            // Intersezione tra i due insiemi di adiacenza
            HashSet<string> inComune = new HashSet<string>(_rete[a]);
            inComune.IntersectWith(_rete[b]);
            return inComune;
        }

        public static HashSet<string> Suggeriti(string p)
        {
            HashSet<string> suggeriti = new HashSet<string>();
            if (!_rete.ContainsKey(p)) return suggeriti;

            HashSet<string> amiciDiretti = _rete[p];

            // SCELTA ALGORITMICA: Esploriamo i vicini a profondità 2.
            // Un amico di un amico viene inserito solo se:
            // 1. Non è la persona 'p' stessa;
            // 2. Non è già un amico diretto di 'p'.
            foreach (string amico in amiciDiretti)
            {
                if (_rete.ContainsKey(amico))
                {
                    foreach (string amicoDiAmico in _rete[amico])
                    {
                        if (amicoDiAmico != p && !amiciDiretti.Contains(amicoDiAmico))
                        {
                            suggeriti.Add(amicoDiAmico);
                        }
                    }
                }
            }
            return suggeriti;
        }

        public static bool Raggiungibile(string a, string b)
        {
            if (!_rete.ContainsKey(a) || !_rete.ContainsKey(b)) return false;
            if (a == b) return true;

            // SCELTA ALGORITMICA: BFS (Breadth-First Search, visita in ampiezza).
            // Usiamo una Queue<string> FIFO per esplorare la rete livello per livello
            // e un HashSet<string> visitati per evitare loop infiniti generati da cicli nel grafo sociale.
            Queue<string> coda = new Queue<string>();
            HashSet<string> visitati = new HashSet<string>();

            coda.Enqueue(a);
            visitati.Add(a);

            while (coda.Count > 0)
            {
                string curr = coda.Dequeue();
                if (curr == b) return true;

                foreach (string vicino in _rete[curr])
                {
                    if (!visitati.Contains(vicino))
                    {
                        visitati.Add(vicino);
                        coda.Enqueue(vicino);
                    }
                }
            }
            return false;
        }
    }
}

public class Es13
{
    /*
     * CONSEGNA:
     * Es. 13 — Frequenza delle lettere
     * Con una Dictionary<char, int> conta quante volte compare ogni lettera di una frase, ignorando spazi e
     * maiuscole. Usa TryGetValue. Stampa le lettere in ordine alfabetico scorrendo 'a'..'z'.
     */

    public static string FraseTest = "Ciao come stai tutto bene";

    public static void ContaEFrequenza(string frase)
    {
        Dictionary<char, int> conteggi = new Dictionary<char, int>();

        // SCELTA ALGORITMICA: Filtriamo solo le lettere 'a'..'z' ignorando punteggiatura e spazi.
        // TryGetValue evita due lookup distinti (ContainsKey seguito dall'indicizzatore), dimezzando gli accessi hash.
        foreach (char c in frase.ToLower())
        {
            if (c >= 'a' && c <= 'z')
            {
                if (conteggi.TryGetValue(c, out int qta))
                {
                    conteggi[c] = qta + 1;
                }
                else
                {
                    conteggi[c] = 1;
                }
            }
        }

        // SCELTA ALGORITMICA: Scorrendo direttamente il codice ASCII da 'a' a 'z',
        // otteniamo l'output perfettamente ordinato senza dover ricorrere a metodi di ordinamento costosi.
        for (char c = 'a'; c <= 'z'; c++)
        {
            if (conteggi.TryGetValue(c, out int qta))
            {
                Console.WriteLine($"{c}: {qta}");
            }
        }
    }
}

public class Es14
{
    /*
     * CONSEGNA:
     * Es. 14 — Anagrammi
     * Data una lista di parole, raggruppa gli anagrammi in una Dictionary<string, List<string>>. La chiave è
     * la parola con le lettere ordinate (ToCharArray, Array.Sort, new string(...)). Stampa solo i gruppi con
     * almeno due parole.
     */

    public static List<string> ParoleTest = new List<string> { "roma", "mora", "amor", "casa", "saca", "pane" };

    public static void RaggruppaAnagrammi(List<string> parole)
    {
        Dictionary<string, List<string>> gruppi = new Dictionary<string, List<string>>();

        // SCELTA ALGORITMICA: Firma canonica degli anagrammi.
        // Due parole sono anagrammi se e solo se, ordinando i loro caratteri, generano la stessa stringa identica (es. "amor", "mora" -> "amor").
        // Usiamo questa stringa normalizzata come chiave del dizionario.
        foreach (string p in parole)
        {
            char[] lettere = p.ToLower().ToCharArray();
            Array.Sort(lettere);
            string chiave = new string(lettere);

            if (!gruppi.ContainsKey(chiave))
            {
                gruppi[chiave] = new List<string>();
            }
            gruppi[chiave].Add(p);
        }

        // Stampiamo solo i raggruppamenti con almeno 2 parole per escludere parole isolate prive di anagrammi
        foreach (var gruppo in gruppi)
        {
            if (grupoValido(gruppo.Value))
            {
                Console.WriteLine($"Gruppo anagrammi ({gruppo.Key}): {string.Join(", ", gruppo.Value)}");
            }
        }

        bool grupoValido(List<string> lista) => lista.Count >= 2;
    }
}

public class Es15
{
    /*
     * CONSEGNA:
     * Es. 15 — Registro voti annidato
     * Il registro è una Dictionary<string, Dictionary<string, List<int>>>: studente -> materia -> voti.
     * Scrivi:
     * 1. AggiungiVoto(studente, materia, voto) crea le strutture interne se mancano; voti ammessi 1-10.
     * 2. double Media(studente, materia) - KeyNotFoundException se studente o materia non esistono.
     * 3. List<string> Insufficienti(materia) studenti con media < 6 nella materia.
     * 4. Dictionary<string, double> MediePerMateria(studente)
     */

    // Struttura a 3 livelli: Studente -> Materia -> Elenco voti
    public static Dictionary<string, Dictionary<string, List<int>>> Registro =
        new Dictionary<string, Dictionary<string, List<int>>>();

    public static void AggiungiVoto(string studente, string materia, int voto)
    {
        // Controllo validità intervallo voto
        if (voto < 1 || voto > 10)
            throw new ArgumentOutOfRangeException(nameof(voto), "Il voto deve essere compreso tra 1 e 10.");

        // SCELTA ALGORITMICA: Creazione a cascata (Lazy initialization).
        // Se la chiave dello studente o della materia non esiste, creiamo i dizionari/liste vuoti prima di inserire il voto.
        if (!Registro.ContainsKey(studente))
            Registro[studente] = new Dictionary<string, List<int>>();

        if (!Registro[studente].ContainsKey(materia))
            Registro[studente][materia] = new List<int>();

        Registro[studente][materia].Add(voto);
    }

    public static double Media(string studente, string materia)
    {
        // Se la gerarchia non è presente, lanciamo KeyNotFoundException come esplicitamente richiesto
        if (!Registro.ContainsKey(studente) || !Registro[studente].ContainsKey(materia))
            throw new KeyNotFoundException("Studente o materia non presenti nel registro.");

        List<int> voti = Registro[studente][materia];
        if (voti.Count == 0) return 0.0;

        double somma = 0;
        for (int i = 0; i < voti.Count; i++) somma += voti[i];
        return somma / voti.Count;
    }

    public static List<string> Insufficienti(string materia)
    {
        List<string> elenco = new List<string>();

        // SCELTA ALGORITMICA: Iteriamo solo sugli studenti che hanno la materia registrata nel loro piano di studi
        foreach (var entryStudente in Registro)
        {
            if (entryStudente.Value.ContainsKey(materia))
            {
                double m = Media(entryStudente.Key, materia);
                if (m < 6.0)
                {
                    elenco.Add(entryStudente.Key);
                }
            }
        }
        return elenco;
    }

    public static Dictionary<string, double> MediePerMateria(string studente)
    {
        if (!Registro.ContainsKey(studente))
            throw new KeyNotFoundException($"Lo studente '{studente}' non esiste nel registro.");

        // Creiamo una mappa riassuntiva Materia -> Media per il singolo studente
        Dictionary<string, double> riepilogo = new Dictionary<string, double>();
        foreach (var entryMateria in Registro[studente])
        {
            riepilogo[entryMateria.Key] = Media(studente, entryMateria.Key);
        }
        return riepilogo;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ESECUZIONE TEST ESERCIZI PARTE A ===");

        // Test Es1 - Quadrato magico
        Console.WriteLine($"Es1 - È magico: {Es1.IsMagico(Es1.MatriceTest)}");

        // Test Es2 - Sala cinema
        Es2.SalaTest[0, 1] = true;
        Es2.SalaTest[0, 2] = true;
        Console.WriteLine($"Es2 - Posti liberi: {Es2.PostiLiberi(Es2.SalaTest)}");
        Console.WriteLine($"Es2 - Fila più piena: {Es2.FilaPiuPiena(Es2.SalaTest)}");
        Console.WriteLine($"Es2 - Posti contigui (fila 0, 3 posti): {Es2.CercaPostiContigui(Es2.SalaTest, 0, 3)}");
        Es2.StampaSala(Es2.SalaTest);

        // Test Es3 - Campo minato
        int[,] mineNumeri = Es3.CalcolaNumeri(Es3.CampoTest);
        Console.WriteLine($"Es3 - Mine attorno alla cella (0,1): {mineNumeri[0, 1]}");

        // Test Es4 - Voti irregolari
        Es4.StampaRiepilogo(Es4.NomiTest, Es4.VotiTest);

        // Test Es5 - Triangolo di Tartaglia
        long[][] tartaglia = Es5.Tartaglia(5);
        Es5.StampaTartaglia(tartaglia);

        // Test Es6 - Linee del trasporto
        int[] lineePerC = Es6.LineePerFermata(Es6.LineeTest, "C");
        Console.WriteLine($"Es6 - Linee per la fermata 'C': {string.Join(", ", lineePerC)}");

        // Test Es7 - Lista della spesa
        Es7.Aggiungi("Pane", Es7.ListaSpesa);
        Es7.Aggiungi("Latte", Es7.ListaSpesa);
        Es7.StampaNumerata(Es7.ListaSpesa);

        // Test Es8 - Fusione di liste ordinate
        List<int> fusa = Es8.Fondi(Es8.ListaA, Es8.ListaB);
        Console.WriteLine($"Es8 - Fusione ordinata: {string.Join(", ", fusa)}");

        // Test Es9 - Coda di stampa con priorità
        Es9.CodaTest.Accoda(new Es9.Lavoro(1, "Relazione.pdf", 5, 2));
        Es9.CodaTest.Accoda(new Es9.Lavoro(2, "Urgente.pdf", 2, 5));
        Console.WriteLine($"Es9 - Pagine totali in attesa: {Es9.CodaTest.PagineInAttesa()}");

        // Test Es10 - Parole distinte
        Es10.EseguiConteggio(Es10.TestoProva, "mondo");

        // Test Es11 - Iscrizioni ai corsi
        Es11.EseguiAnalisi();

        // Test Es12 - Amici suggeriti
        Es12.ReteSociale.AggiungiAmicizia("Alice", "Bob");
        Es12.ReteSociale.AggiungiAmicizia("Bob", "Carlo");
        Console.WriteLine($"Es12 - Alice può raggiungere Carlo? {Es12.ReteSociale.Raggiungibile("Alice", "Carlo")}");

        // Test Es13 - Frequenza delle lettere
        Es13.ContaEFrequenza(Es13.FraseTest);

        // Test Es14 - Anagrammi
        Es14.RaggruppaAnagrammi(Es14.ParoleTest);

        // Test Es15 - Registro voti annidato
        Es15.AggiungiVoto("Mario", "Informatica", 8);
        Es15.AggiungiVoto("Mario", "Informatica", 5);
        Console.WriteLine($"Es15 - Media Mario Informatica: {Es15.Media("Mario", "Informatica")}");
    }
}