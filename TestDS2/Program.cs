using System;
using System.Collections.Generic;
using System.IO;

namespace TestDS2
{
    class Program
    {
        private static Dictionary<String, DSTest> mytestSortedDictionary5 = new Dictionary<string, DSTest>();
        private static Dictionary<String, DSTest> mytestSortedDictionary6 = new Dictionary<string, DSTest>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // SortedDictionary<String, DSTest> mytestSortedDictionary1 = new SortedDictionary<string, DSTest>();
            //
            // var watch = System.Diagnostics.Stopwatch.StartNew();
            //
            // for (int i = 0; i < 200000; i++)
            // {
            //     DSTest dsTest = new DSTest();
            //     mytestSortedDictionary1.Add("FuckingHell" + i, dsTest);
            // }
            //
            // watch.Stop();
            // Console.WriteLine("The fucking time : " + watch.ElapsedMilliseconds);

            DoTheOperation5();
            DoTheOperation6();
            //DoWritingOnDiseases();
            //DoWritingOnDrugs();
            var watch10 = System.Diagnostics.Stopwatch.StartNew();
            mytestSortedDictionary5.ContainsKey("Dis_ejtsoklnte");
            mytestSortedDictionary5.ContainsKey("Dis_fffljsoagw");
            mytestSortedDictionary5.ContainsKey("Dis_txkalaranf");
            mytestSortedDictionary5.ContainsKey("Dis_wwpvmqpcou");
            watch10.Stop();
            Console.WriteLine("The fucking time of Finding Diseases: " + watch10.ElapsedMilliseconds);


            var watch11 = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine(mytestSortedDictionary5["Dis_ejtsoklnte"]);
            Console.WriteLine(mytestSortedDictionary5["Dis_fffljsoagw"]);
            Console.WriteLine(mytestSortedDictionary5["Dis_txkalaranf"]);
            Console.WriteLine(mytestSortedDictionary5["Dis_wwpvmqpcou"]);
            watch11.Stop();
            Console.WriteLine("The fucking time of showing Diseases: " + watch10.ElapsedMilliseconds);
        }

        // reading diseases
        public static void DoTheOperation5()
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            String st = "Nothing";
            using (StreamReader sr = new StreamReader(@"C:\Users\Asus\Desktop\DS-Final-Project\datasets\diseases.txt"))
            {
                while ((st = sr.ReadLine()) != null)
                {
                    mytestSortedDictionary5.Add(st, new DSTest());
                }
            }
            watch10.Stop();
            Console.WriteLine("The fucking time of Reading of Diseases: " + watch10.ElapsedMilliseconds);
            Console.WriteLine(mytestSortedDictionary5.Count);
        }

        public static void DoWritingOnDiseases()
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            String st = "Nothing";
            using (StreamWriter sw = File.AppendText(@"C:\Users\Asus\Desktop\DS-Final-Project\datasets\diseases.txt"))
            {
                foreach (var obj in mytestSortedDictionary5)
                {
                    sw.WriteLine(obj.Key);
                }
            }
            watch10.Stop();
            Console.WriteLine("The fucking time of Writing of Diseases: " + watch10.ElapsedMilliseconds);
            Console.WriteLine(mytestSortedDictionary5.Count);
        }

        public static void DoWritingOnDrugs()
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            using (StreamWriter sw = File.AppendText(@"C:\Users\Asus\Desktop\DS-Final-Project\datasets\drugs.txt"))
            {
                foreach (var obj in mytestSortedDictionary6)
                {
                    sw.WriteLine(obj.Key);
                }
            }
            watch10.Stop();
            Console.WriteLine("The fucking time of Writing of Drugs: " + watch10.ElapsedMilliseconds);
            Console.WriteLine(mytestSortedDictionary6.Count);
        }

        // reading drugs
        public static void DoTheOperation6()
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            String st = "Nothing";
            using (StreamReader sr = new StreamReader(@"C:\Users\Asus\Desktop\DS-Final-Project\datasets\drugs.txt"))
            {
                while ((st = sr.ReadLine()) != null)
                {
                    mytestSortedDictionary6.Add(st, new DSTest());
                }
            }
            watch10.Stop();
            Console.WriteLine("The fucking time of Reading of Drugs: " + watch10.ElapsedMilliseconds);
            Console.WriteLine(mytestSortedDictionary6.Count);
        }

        class MyTest
        {
            private String name;
            private int age;
            private Dictionary<String, MyTest> myDict;

            public MyTest()
            {
                this.myDict = new Dictionary<string, MyTest>();
                this.name = "Hello";
                this.age = 12947;
            }
        }

        class DSTest
        {
            private List<String> myList;
            private Dictionary<String, MyTest> myDict;

            public DSTest()
            {
                this.myList = new List<string>();
                this.myList.Add("Disease_FuckingHell1");
                this.myList.Add("Disease_FuckingHell2");
                this.myList.Add("Disease_FuckingHell3");
                this.myList.Add("Disease_FuckingHell4");
                this.myList.Add("Disease_FuckingHell5");
                this.myList.Add("Disease_FuckingHell6");

                this.myDict = new Dictionary<string, MyTest>();
                this.myDict.Add("Disease_FuckingHell3", new MyTest());
                this.myDict.Add("Disease_FuckingHell2", new MyTest());
                this.myDict.Add("Disease_FuckingHel4l", new MyTest());
                this.myDict.Add("Disease_FuckingHell4", new MyTest());
                this.myDict.Add("Disease_FuckingHell6", new MyTest());
                this.myDict.Add("Disease_FuckingHell7", new MyTest());
                this.myDict.Add("Disease_FuckingHell8", new MyTest());
            }
        }
    }
}
