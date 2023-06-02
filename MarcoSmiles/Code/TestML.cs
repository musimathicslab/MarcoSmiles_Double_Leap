using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Classe statica che gestisce le operazioni riguardanti il machine learning e mantiene lo stato della rete neurale per la configurazione selezionata.
/// </summary>
public static class TestML
{
    private static int delaytime = 12;
    private static int counter = -1;
    private static int oldIndexToSend=0;   //qui -1
    private static int toCompare = 0;
    /// <summary>
    /// Weights, layer 1
    /// </summary>
    private static List<List<double>> W1 = new List<List<double>>();
    /// <summary>
    /// Bias, layer 1
    /// </summary>
    private static List<double> B1 = new List<double>();
    /// <summary>
    /// Weights, layer 2
    /// </summary>
    private static List<List<double>> W2 = new List<List<double>>();
    /// <summary>
    /// Bias, layer 2
    /// </summary>
    private static List<double> B2 = new List<double>();

    /// <summary>
    /// Data dell'ultimo learning effettuato per la configurazione (dataset) selezionata
    /// </summary>
    public static System.DateTime DateLatestLearning;

    ///Variabile temporanea per leggere valori min e max
    private static string[] readText;
    /// <summary>
    /// Il minimo valore per ogni features nel dataset (per scalare)
    /// </summary>
    private static string[] min;
    /// <summary>
    /// Il massimo valore per ogni features nel dataset (per scalare)
    /// </summary>
    private static string[] max;


    /// <summary>
    ///  ReadArraysFromFormattedFile Restituisce una lista di array float. Gli array sono inseriti nella lista nell'ordine in cui vengono letti dal file.
    ///  La lista è formattata nel seguente modo:
    /// Finchè leggendo il file vengono letti vettori, allora questi vettori vengono aggiunti alla lista e restituiti.
    /// Nel momento in cui la funzione capisce di aver letto una matrice, inserisce nella lista un array vuoto [].
    ///
    /// Ad esempio:
    /// - nel file bias.txt, sono contenuti array. Ogni array rappresenta un bias: 
    /// Il primo array rappresenta B1; il secondo array B2 etc...
    ///  
    ///  - nel file weights.txt, sono contenute matrici, ognuna contiene una lista di array. Quindi tutti gli array contenuti nella lista restituita,
    /// finchè non si legge un array vuoto [], faranno parte della prima matrice letta.
    /// In questo modo, si formatta la lista in n blocchi di array, dove n è il numero di matrici contenuti nel file:
    ///  Tutti gli array contenuti nel primo blocco di array (quelli che si trovano prima del primo array vuoto[]) rappresentano la matrice W1,
    /// Tutti gli array contenuti nel secondo blocco di array (quelli che si trovano dopo il primo array vuoto[] e prima del secondo array vuoto [])
    /// rappresentano la matrice W2 
    /// etc...
    /// </summary>
    /// <param name="name">Nome del file da aprire</param>
    /// <returns>
    /// Una lista di float contenente i numeri letti da file. La lista e formattato logicamente in modo tale da poter costrutire gli eventuali array contenuti nel file.
    /// </returns>
    private static List< List<double> > ReadArraysFromFormattedFile(string name)
    {

        Debug.Log("name" + name);
        //stringa
        var text = FileUtils.LoadFile(name);

        if (text == null)
            return null;

        DateLatestLearning = File.GetLastWriteTime(FileUtils.GeneratePath(name));

        text = text.Replace("[", "");
        text = text.Replace("\n", "");

        string[] vettori = text.Split(']');       // StringSplitOptions.RemoveEmptyEntries toglie le entrate vuote (potrebbe servire lasciarle) 

        //lista degli array letti, i valori degli array sono strighe
        List< List <string> > temp = new List< List<string> >();

        foreach (string vettore in vettori)
        {
            temp.Add(vettore.Split(' ')                                       //divide per ' ' e mette l'array di stringhe all'interno della lista temp
                .Select(tag => tag.Trim())                                    //elimina le entrate vuote
                .Where(tag => !string.IsNullOrEmpty(tag)).ToList());
        }

        //lista degli array letti che verrà restituita
        List<List<double>> listOfReadArrays = new List< List<double> >();


        //Converte tutti i valori degli array letti, da string a float.
        foreach ( var arr in temp)
        {
            double[] t = new double[arr.Count];
            for (int i = 0; i < arr.Count; i++)
            {
                t[i] = double.Parse(arr[i], CultureInfo.InvariantCulture);
            }
            listOfReadArrays.Add(t.ToList());
        }
        return listOfReadArrays;
    }


    /// <summary>
    /// Riempe le Matrici W1 ; B1 ; W2; B2.  Legge il file dei minimi e massimi e lo carica in memoria
    /// Usa il metodo ReadArraysFromFormattedFile, per leggere da un file una lista di arrays di tipo float. 
    /// Questa lista restituita è formattata logicamente.
    /// </summary>
    public static bool Populate()
    { 
        B1.Clear(); B2.Clear(); W1.Clear(); W2.Clear();
        
        List<List<double>> biasArrays = ReadArraysFromFormattedFile("bias_out.txt");
        if (biasArrays == null)
            return false;

        B1 = biasArrays.ElementAt(0);
        B2 = biasArrays.ElementAt(1);

        List<List<double>> weightsArrays = ReadArraysFromFormattedFile("weights_out.txt");
        if (weightsArrays == null)
            return false;

        
        //finchè non arrivo ad un array uguale a {} sono array che rappresentano W1
        int j = 0; 
        while (Enumerable.SequenceEqual(weightsArrays.ElementAt(j), new List<double> { } ) == false)
        {
            j++;
        }

        for(int i = 0; i < j ; i++)
        {
            W1.Add(weightsArrays.ElementAt(i));     
        }

        //da j+1 alla fine sono array che rappresentano W2
        int k = j+1;
        while (Enumerable.SequenceEqual(weightsArrays.ElementAt(k), new List<double> { } ) == false)
        {
            k++;
        }

        for (int i = 0 ; i < k - (j + 1) ; i++)
        {
            W2.Add(weightsArrays.ElementAt(j + 1 + i));
        }

        try
        {
            readText = File.ReadAllLines(FileUtils.GeneratePath("min&max_values_dataset_out.txt"));
            //primo elemento contiene riga contenente valori min                                                                                           
            //secondo elemento contiene riga contenente valori max
            min = readText[0].Split(' ');
            max = readText[1].Split(' ');
           // Debug.Log("MAX==" + max);
            //Debug.Log("MIN==" + min);
        }
        catch(Exception)
        {
            Debug.LogError("File MinMax non esistente.");
        }


        return true;
        
    }

    /// <summary>
    /// Predice la nota associata alle features passato come parametro.
    /// Usa le matrici W1, B1, W2 e B2 per effttuare i calcoli.
    /// Restituisce un indice che va da 0 a 23, corrispondente alla nota da suonare.
    /// La funzione supporta il caso in cui il dataset contenga meno note rispetto agli indici 0-23. Questo è fatto calcolando in fase di
    /// computazione la grandezza delle matrici W1, B1, W2, B2.
    /// Conseguentemente, la funzione ReteNeurale, restituirà indici che si trovano fra il range di note allenate.
    /// </summary>
    /// <param name="features">nota da trovare</param>
    /// <returns>Id nota trovata</returns>
    public static int ReteNeurale(float[] features)
    {
      for(int i = 0; i < features.Length; i++)
        {
            //Debug.Log("features " + i + "  =" + features[i]);
        }

        double[] MyFeatures = new double[36];


        //FINE PROVA
        MyFeatures = ScaleValues(features);
        //float[] scaledFeatures = features;
       /* for (int i = 0; i < scaledFeatures.Length; i++)
        {

            MyFeatures[i] = scaledFeatures[i];
           
            //Debug.Log("SCALED FEATURES " + i + "    " + MyFeatures[i]);
        }
       */
            
        // output_hidden1 ha lo stesso numero di elementi di B1 BiasLAYER1
        var output_hidden1 = new double[B1.Count];
        // output_hidden2 ha lo stesso numero di elementi di B2 BIAS LAYR2
        var output_hidden2 = new double[B2.Count];

        double x, w, r;

        for (int i = 0; i < output_hidden1.Length; i++)
        {
            output_hidden1[i] += B1.ElementAt(i);
            //Debug.Log("BIAS LAYER 1 di "+i +" " + output_hidden1[i]);
            for (int j = 0; j < features.Length; j++)
            {
                x = MyFeatures[j];
                w = W1[j][i];
                r = x * w;

                output_hidden1[i] += r;
                //Debug.Log("WORKED BIAS LAYER 1 di " + i + " " + output_hidden1[i]);
            }

            if (output_hidden1[i] <= 0)
                output_hidden1[i] = 0;
            //Debug.Log("==0 this " + i);
        }

        for (int i = 0; i < output_hidden2.Length; i++)
        {
            output_hidden2[i] += B2.ElementAt(i);
            //Debug.Log("bias2===" + output_hidden2[i]);
            for (int j = 0; j < output_hidden1.Length; j++)
            {
                x = output_hidden1[j];
                w = W2[j][i];
                r = x * w;

                output_hidden2[i] += r;
                //Debug.Log("WORKEDbias2===" + output_hidden2[i]);
            }
        }



        double sum = 0;
        foreach (var item in output_hidden2)
        {
            //Debug.Log("ITEM IN OUTPUTHIDDEN2==" + item);
            sum += Mathf.Exp((float)item);
        }

        var toRet = new double[output_hidden2.Length];
        
        for (int i = 0; i < output_hidden2.Length; i++)
        {

            toRet[i] = Mathf.Exp((float)output_hidden2[i]) / sum;
           // toRet[i]=Mathf.Exp((float)output_hidden2[i]);
           // Debug.Log("THIS " + i + "======" + toRet[i]);
        }


        //Debug.Log("=========== toRet.max " + toRet.Max());
        //return toRet.ToList().IndexOf(toRet.Max());
        var toRetMax = toRet.Max();  //max proba rete neurale
        int index=toRet.ToList().IndexOf(toRet.Max());
       //STAMPA DI PROVA PER L'INDICE toRet
       /*
        Debug.Log("In TestML\n");
        for (int i = 0; i < toRet.Length; i++) {
            Debug.Log("toRet[i]" + toRet[i]);
        }
       */
        //int toSend = -1;
        int toSend=0;
        if (counter == -1 && toRetMax > 0.9)
        {
            //la prima volta setto counter a 0 e old index to send == index
            Debug.Log("THE FIRST");
            counter = 0;
            oldIndexToSend = index;
            toSend= index;
            toCompare = index;
        }
        Debug.Log("index=" + index + "            old=" + oldIndexToSend);
        toSend = oldIndexToSend;
        if (index == toCompare && toRetMax > 0.9)
        {

            counter++;
            if (counter > delaytime && toRetMax > 0.9)
            {
                toSend = toCompare;
                oldIndexToSend=toCompare;
            }
        }
        else
        {
            counter = 0;
            toCompare = index;
        }
        /*
        if (toRetMax > 0.9)
        {
            toSend = index;
            oldIndexToSend = toSend;
        }
        */
        

       
        Debug.Log("SENDED note " + toSend);
        return toSend;



        //mio
        //riconosce le configurazioni delle mani ma non utilizzando la label relativa alla nota e utilizzando solo un array di float 
        //alla fine restituisce l'indice dell' elemento all' interno della lista con valore max
        //ma l'indice non corrisponde alla label della nota
        //TO-DO trovare il punto in cui legge la label
        //ipotesi: la funzione non legge in alcun modo il file Excell dove sono salvate le label
        //se in questa fase utilizziamo solo i file bias_out, wieights:out e min&MaxValues non possiamo in alcun modo conoscere la label in quanto non presente
        //possibile soluzione: salvare nel file weights outative all' output layer
        //path::   C:\Users\Dan\AppData\LocalLow\DefaultCompany\MarcoSmiles\MyDatasets\DefaultDataset

    }


    /// <summary>
    /// Scala i valori convertendo le features non scalate in valori tra 0 e 1
    /// </summary>
    /// <param name="unscaledFeatures">features da scalare</param>
    /// <returns>features scalate con valori tra min e max</returns>
    private static double[] ScaleValues(float[] unscaledFeatures)
    {
        var scaledFeatures = new double[unscaledFeatures.Length];
        var minValues = new double[unscaledFeatures.Length];
        var maxValues = new double[unscaledFeatures.Length];
        
        for (int i = 0; i < unscaledFeatures.Length; i++)
        {
            
            minValues[i] = double.Parse(min[i], CultureInfo.InvariantCulture);
            maxValues[i] = double.Parse(max[i], CultureInfo.InvariantCulture);
            //Debug.Log("MIN" + i + "==" + minValues[i] + "   MAX" + i + "==" + maxValues[i]);
        }
        var work = new double[unscaledFeatures.Length];
        for (int i = 0; i < unscaledFeatures.Length; i++)
        {
            scaledFeatures[i] = (unscaledFeatures[i] - minValues[i]) / (maxValues[i] - minValues[i]);
            


        }

        return scaledFeatures;
    }
}