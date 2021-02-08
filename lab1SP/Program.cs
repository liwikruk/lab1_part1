using System;
using System.IO;

namespace lab1SP
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.Unicode; // і
            int countAllSymbols; //к-сть символів
            string alphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯабвгґдеєжзиіїйклмнопрстуфхцчшщьюя"; //алфавіт 66, бо великі літери        
            double[,] array = new double[alphabet.Length / 2, 2]; //масив для кожної окремої букви, кількості появи в тексті та частоти
            //txt
            string fileOne = @"F:\Bodnar KI 3_2\кс\lab1\stus.txt";
            string fileTwo = @"F:\Bodnar KI 3_2\кс\lab1\kolobok.txt";
            string fileThree = @"F:\Bodnar KI 3_2\кс\lab1\oop.txt";
            //архіви
            string archiveOne = @"F:\Bodnar KI 3_2\кс\lab1\archive\stus";
            string archiveTwo = @"F:\Bodnar KI 3_2\кс\lab1\archive\kolobok";
            string archiveThree = @"F:\Bodnar KI 3_2\кс\lab1\archive\oop";

            string text = ReadText(fileOne);
            CountLetters(array, text, out countAllSymbols, alphabet); // Масив з кількістю кожної окремої букви та кількість усіх букв в тексті
            CountFrequency(countAllSymbols, array); // Частота появи літер у тексті
            double quantityInformation = CountEntropyInformation(array, fileOne, countAllSymbols); // Ентропія та кількість інфомації
            CompareArchive(quantityInformation, archiveOne); // Порівнюємо розміри архівів
            ShowWords(array, alphabet);

            text = ReadText(fileTwo);
            CountLetters(array, text, out countAllSymbols, alphabet); 
            CountFrequency(countAllSymbols, array);
            quantityInformation = CountEntropyInformation(array, fileTwo, countAllSymbols); 
            CompareArchive(quantityInformation, archiveTwo); 
            ShowWords(array, alphabet);

            text = ReadText(fileThree);
            CountLetters(array, text, out countAllSymbols, alphabet); 
            CountFrequency(countAllSymbols, array);
            quantityInformation = CountEntropyInformation(array, fileThree, countAllSymbols); 
            CompareArchive(quantityInformation, archiveThree); 
            ShowWords(array, alphabet);

            Console.WriteLine();
        }

        //Читаємо зміст файлу
        static string ReadText(string file)
        {
            string text = "";
            string line;
            if (File.Exists(file))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        text += line + "\n"; //записуємо весь текст з файлу
                    }
                }
            }
            else
                throw new Exception("Файла не існує");
            Console.WriteLine("================================================================================");
            Console.WriteLine(text);
            Console.WriteLine("================================================================================");
            return text;
        }

        //Кількість кожної окремої букви та усіх букв в тексті
        static void CountLetters(double[,] array, string text, out int countAllSymbols, string alphabet)
        {
            countAllSymbols = 0;
            foreach(char letter in text)
            {
                 countAllSymbols++; 
            }
            for(int i=0; i<alphabet.Length/2; i++)
            {
                foreach(char letter in text)
                {
                    if (letter == alphabet[i] || letter == alphabet[i + 33]) //рівна і великій і малій букві, по-порядку 
                    {
                        array[i, 0]+=1;//визначаємо к-сть появ певної букви алфавіту в тексті
                    }
                }
            }
            Console.WriteLine("================================================================================");
            Console.WriteLine("Всього символів у тексті: {0}", countAllSymbols);
        }

        //Частоту літер у тексті
        static void CountFrequency(int countAllSymbols, double[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++) //array.GetLength(0) - кількість букв записаних в масив при обробці тексту (33), відповідно кожної був раніше вказаний counter(число появи кожної окремої букви)
            {
                if (array[i, 0] != 0)
                {
                    array[i, 1] = array[i, 0] / countAllSymbols; //частота
                }
            }
        }

        //Ентропія та кількість інфомації
        static double CountEntropyInformation(double[,] array, string path, int countAllSymbols)
        {
            double entropy = 0;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                if (array[i, 0] != 0)
                    entropy += - array[i, 1] * Math.Log(array[i, 1], 2); //визначаємо ентропію за формулою
            }            
            double amountOfInformation;
            FileInfo file = new FileInfo(path);
            Console.WriteLine("Середня ентропія (біти): {0:F5}", entropy);
            Console.WriteLine("Кількість інформації у тексті (біти): {0:F5}", entropy * countAllSymbols);
            Console.WriteLine("Кількість інформації у тексті (байти): {0:F5}\n", amountOfInformation = entropy * countAllSymbols / 8);
            Console.WriteLine("Розмір файлу (байти): {0} ", file.Length);
            if (file.Length > amountOfInformation)
                Console.WriteLine("розмір файла > к-сть інформації\n");
            else
            {
                if (file.Length < amountOfInformation)
                {
                    Console.WriteLine("розмір файла < к-сть інформації\n");                  
                }
                else
                {
                    Console.WriteLine("розмір файла = к-сть інформації\n");
                }
            }
            return amountOfInformation;
        }

        //Порівняння кількості інформації та архівів
        static void CompareArchive(double quantityInformation, string path)
        {
            string[] archive = new string[] { ".rar", ".zip", ".gz", ".bz2", ".xz" };
            foreach (string extention in archive)
            {
                FileInfo file = new FileInfo(path + extention);
                Console.WriteLine("розмір архіва {0}: {1}", extention, file.Length);
                if (file.Length > quantityInformation)
                    Console.WriteLine("розмір архіва " + extention + " > кількість інформації\n");
                else
                {
                    if (file.Length == quantityInformation)
                        Console.WriteLine("розмір архіва " + extention + " = кількість інформації\n");
                    else
                        Console.WriteLine("розмір архіва " + extention + " < кількість інформації\n");
                }
            }

        }

        //Масив з кількістю літер та частотою
        static void ShowWords(double[,] array, string alphabet)
        {
            Console.WriteLine("Буква           Кількість  Відносна частота");
            for (int i = 0; i < array.GetLength(0); i++)
            {
                Console.Write("{0}         ", alphabet[i]);
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write("{0,15:F4}", array[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("================================================================================");
            Console.WriteLine();
        }
    }
}
