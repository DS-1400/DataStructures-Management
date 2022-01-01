using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TestDS
{
    class Program
    {
        private static Dictionary<String, DSTest> mytestSortedDictionary1 = new Dictionary<String, DSTest>();
        private static Dictionary<String, DSTest> mytestSortedDictionary2 = new Dictionary<String, DSTest>();
        private static Dictionary<String, DSTest> mytestSortedDictionary3 = new Dictionary<String, DSTest>();
        private static Dictionary<String, DSTest> mytestSortedDictionary4 = new Dictionary<String, DSTest>();
        private static Dictionary<String, DSTest> mytestSortedDictionary5 = new Dictionary<String, DSTest>();
        // OrderedDictionary

        public static void LineByLineProcessing()
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            String userInput = "Dis_wtpqrhyjwd ";
            String st = "Nothing";
            using (StreamReader sr = new StreamReader(@"C:\Users\Asus\Desktop\DS-Final-Project\datasets\alergies.txt"))
            {
                while ((st = sr.ReadLine()) != null)
                {
                    //Console.WriteLine(st);
                    String[] inputs = st.Split(":");
                    if (inputs[0] == userInput)
                    {
                        Console.WriteLine("We found it!");
                    }
                }
            }
            watch10.Stop();
            Console.WriteLine("The fucking time of Reading of Diseases: " + watch10.ElapsedMilliseconds);
        }

        public static void LineByLineCreatingObjects()
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            String userInput = "Dis_lbqblqdzoo";
            String st = "Nothing";
            using (StreamReader sr = new StreamReader(@"C:\Users\Asus\Desktop\DS-Final-Project\datasets\alergies.txt"))
            {
                st = sr.ReadLine();
                String[] inputs = st.Split(":");
                if (inputs[0] == userInput)
                {
                    Console.WriteLine("We found it!");
                    DSTest dsTest1 = new DSTest();
                    DSTest dsTest2 = new DSTest();
                    MyTest myTest1 = new MyTest();
                }
            }
            watch10.Stop();
            Console.WriteLine("The fucking time of Reading of Diseases: " + watch10.ElapsedMilliseconds);
        }

        static void Main(string[] args)
        {
            // Thread th1 = new Thread(DoTheOperation1);
            // Thread th2 = new Thread(DoTheOperation2);
            // Thread th3 = new Thread(DoTheOperation3);
            // Thread th4 = new Thread(DoTheOperation4);
            //
            // th1.Name = "Thread1";
            // th2.Name = "Thread2";
            // th3.Name = "Thread3";
            // th4.Name = "Thread4";
            //
            //
            // var watch = System.Diagnostics.Stopwatch.StartNew();
            // th1.Start();
            // th2.Start();
            // th3.Start();
            // th4.Start();
            //
            // th1.Join();
            // th2.Join();
            // th3.Join();
            // th4.Join();
            // watch.Stop();
            // Console.WriteLine("The fucking time : " + watch.ElapsedMilliseconds);
            //
            // PersistManager2();
            // FileChecker();
            //
            // FindTheKey();
            //
            // var watch3 = System.Diagnostics.Stopwatch.StartNew();
            // mytestSortedDictionary3.Add("hsfadgusfdhvuoi", new DSTest());
            // watch3.Stop();
            // Console.WriteLine("The fucking time for add : " + watch.ElapsedMilliseconds);
            //
            // ReadingData();
            //
            // DoTheOperationReadingDiseases();
            LineByLineProcessing();
            ChangingALineAlergies();
        }

        public static void PersistOrderedDict()
        {
            
        }

        public static void ReadingData()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var res1 = mytestSortedDictionary1["FuckingHell500001"];
            var res2 = mytestSortedDictionary2["FuckingHell500002"];
            var res3 = mytestSortedDictionary3["FuckingHell500003"];
            var res4 = mytestSortedDictionary4["FuckingHell500004"];
            watch.Stop();
            Console.WriteLine("The fucking time For returning : " + watch.ElapsedMilliseconds);
        }

        public static bool FindTheKey()
        {
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            bool res1 = mytestSortedDictionary1.ContainsKey("FuckingHell01");
            bool res2 = mytestSortedDictionary2.ContainsKey("FuckingHell01");
            bool res3 = mytestSortedDictionary3.ContainsKey("FuckingHell01");
            bool res4 = mytestSortedDictionary4.ContainsKey("FuckingHell01");
            watch.Stop();
            Console.WriteLine("The fucking time For searching : " + watch.ElapsedMilliseconds);

            return true;
        }

        public static bool PersistManager()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            using (StreamWriter sw = new StreamWriter(@"D:/Alireza/test_ds2.txt"))
            {
                foreach (var obj in mytestSortedDictionary1)
                {
                    sw.WriteLine(obj.Key + obj.Value.ToString());
                }
        
                foreach (var obj in mytestSortedDictionary2)
                {
                    sw.WriteLine(obj.Key + obj.Value.ToString());
                }
        
                foreach (var obj in mytestSortedDictionary3)
                {
                    sw.WriteLine(obj.Key + obj.Value.ToString());
                }
        
                foreach (var obj in mytestSortedDictionary4)
                {
                    sw.WriteLine(obj.Key + obj.Value.ToString());
                }
            }
        
            watch.Stop();
            Console.WriteLine("The fucking time : " + watch.ElapsedMilliseconds);
            return true;
        }

        public static bool PersistManager2()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            using (StreamWriter sw = File.AppendText(@"D:/Alireza/test_ds2.txt"))
            {
                foreach (var obj in mytestSortedDictionary1)
                {
                    sw.WriteLine(obj.Key + obj.Value.ToString());
                    
                }
                sw.Flush();
        
                foreach (var obj in mytestSortedDictionary2)
                {
                    sw.WriteLine(obj.Key + obj.Value.ToString());
                    
                }
                sw.Flush();
        
                foreach (var obj in mytestSortedDictionary3)
                {
                    sw.WriteLine(obj.Key + obj.Value.ToString());
                    
                }
                sw.Flush();
        
                foreach (var obj in mytestSortedDictionary4)
                {
                    sw.WriteLine(obj.Key + obj.Value.ToString());
                    
                }
                sw.Flush();
            }
        
            watch.Stop();
            Console.WriteLine("The fucking time Persist Manager : " + watch.ElapsedMilliseconds);
            return true;
        }

        public static void FileChecker()
        {
            String st = "Hello Everything is Alright";
            var watch = System.Diagnostics.Stopwatch.StartNew();
            using (StreamReader sr = new StreamReader(@"D:/Alireza/test_ds2.txt"))
            { 
                String st2 = sr.ReadLine();
                if (st2 == st)
                {
                    // Do nothing
                    st += st2;
                }
            }
            watch.Stop();
            Console.WriteLine("The fucking time of Reading: " + watch.ElapsedMilliseconds);
        }

        public static void DoTheOperation1()
        {
            Console.WriteLine("Hello Fucking World!" + Thread.CurrentThread.Name);

            for (int i = 0; i <= 50000; i++)
            {
                DSTest dsTest = new DSTest();
                mytestSortedDictionary1.Add("FuckingHell" + i + 1, dsTest);
            }
        }
        
        public static void DoTheOperation2()
        {
            Console.WriteLine("Hello Fucking World!" + Thread.CurrentThread.Name);

            for (int i = 0; i <= 50000; i++)
            {
                DSTest dsTest = new DSTest();
                mytestSortedDictionary2.Add("FuckingHell" + i + 2, dsTest);
            }
        }

        public static void DoTheOperation3()
        {
            Console.WriteLine("Hello Fucking World!" + Thread.CurrentThread.Name);

            for (int i = 0; i <= 50000; i++)
            {
                DSTest dsTest = new DSTest();
                mytestSortedDictionary3.Add("FuckingHell" + i + 3, dsTest);
            }
        }

        public static void DoTheOperation4()
        {
            Console.WriteLine("Hello Fucking World!" + Thread.CurrentThread.Name);

            for (int i = 0; i <= 50000; i++)
            {
                DSTest dsTest = new DSTest();
                mytestSortedDictionary4.Add("FuckingHell" + i + 4, dsTest);
            }
        }

        public static void DoTheOperationReadingDiseases()
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            String st = "Nothing";
            using (StreamReader sr = new StreamReader(@"C:\Users\Asus\Desktop\DS-Final-Project\datasets\diseases.txt"))
            {
                st = sr.ReadLine();
                mytestSortedDictionary5.Add(st, new DSTest());
            }
            watch10.Stop();
            Console.WriteLine("The fucking time of Reading of Diseases: " + watch10.ElapsedMilliseconds);
        }

        public static void ChangingALineAlergies()
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();
            String[] myLines = File.ReadAllLines(@"C:\Users\Asus\Desktop\DS-Final-Project\datasets\diseases.txt");
            Thread.Sleep(10);
            int index = myLines.Length - 2;
            String tmp = "Nothing";

            using (StreamWriter sw = new StreamWriter(@"C:\Users\Asus\Desktop\DS-Final-Project\datasets\alergies.txt"))
            {
                for (int i = 1; i <= index + 2; i++)
                {
                    if (i == index)
                    {
                        sw.WriteLine(tmp);
                    }
                    else
                    {
                        sw.WriteLine(myLines[i - 1]);
                    }
                }
            }
            watch10.Stop();
            Console.WriteLine("The fucking time of Reading & modifying a line of Diseases: " + watch10.ElapsedMilliseconds);
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

            /// <inheritdoc />
            public override string ToString()
            {
                return this.name + this.age;
            }
        }

        class DSTest
        {
            private List<String> myList;
            private Dictionary<String, MyTest> myDict;

            public override string ToString()
            {
                String ret = "Hello:";
                foreach (var obj in this.myDict)
                {
                    ret += obj.Key + obj.Value.ToString();
                }

                foreach (var obj in this.myList)
                {
                    ret += obj;
                }

                return ret;
            }

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
