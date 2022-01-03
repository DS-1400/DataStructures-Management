using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Threading;

namespace DSProject
{
    interface ILogger
    {
        
    }

    class MyLogger
    {
        
    }

    interface IContains
    {
        bool ContainsDisease(String name);
        bool ContainsDrug(String name);
    }

    interface IFinder
    {
        String FindDiseaseDrugs(String name);
        String FindDrugAssociated(String name);
    }

    class DiseaseDrugDb
    {
        public List<String> DrugsNames;
        public List<String> DiseaseNames;
        public String[] DiseaseDrugsNames;
        public String[] DrugsEffectsNames;

        public DiseaseDrugDb() // 59 Milliseconds for first time running
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();
            this.DiseaseNames =
                new List<string>(
                    File.ReadAllLines(
                        @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\diseases.txt"));

            this.DrugsNames = 
                new List<string>(
                    File.ReadAllLines(
                    @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\drugs.txt"));
                

            this.DiseaseDrugsNames =
                File.ReadAllLines(
                    @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\alergies.txt");

            this.DrugsEffectsNames = 
                File.ReadAllLines(
                    @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\effects.txt");

            watch10.Stop();
            Console.WriteLine("The fucking time of Reading all files : " + watch10.ElapsedMilliseconds);
        }
    }

    class DiseaseDrugDbAsync
    {
        public String[] DrugsNames;
        public String[] DiseaseNames;
        public String[] DiseaseDrugsNames;
        public String[] DrugsEffectsNames;

        public DiseaseDrugDbAsync()
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();
            this.DoItAsync();
            watch10.Stop();
            Console.WriteLine("The fucking time of Reading all files async : " + watch10.ElapsedMilliseconds);
        }

        private async void DoItAsync()
        { 
            
            this.DiseaseNames = 
                await File.ReadAllLinesAsync(@"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\diseases.txt");
            
            this.DrugsNames = 
                await File.ReadAllLinesAsync(@"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\drugs.txt");

            this.DiseaseDrugsNames = 
                await File.ReadAllLinesAsync(@"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\alergies.txt");

            this.DrugsEffectsNames =
                await File.ReadAllLinesAsync(@"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\effects.txt");
            
        }
    }

    class MyOperator: IFinder, IContains
    {
        private DiseaseDrugDb DB;

        private String Temp;

        public MyOperator(DiseaseDrugDb db)
        {
            this.DB = db;
        }

        /// <inheritdoc />
        public string FindDiseaseDrugs(string name)
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            String[] dual;
            String[] after;
            String result = name + ":";
            foreach (var line in this.DB.DiseaseDrugsNames)
            {
                dual = line.Split(":");
                dual[0] = dual[0].Trim();
                dual[1] = dual[1].Trim();
                if (dual[0] == name)
                {
                    after = dual[1].Split(";");
                    for (int i = 0; i < after.Length; i++)
                    {
                        after[i] = after[i].Trim();
                        if (after[i].Contains('+'))
                        {
                            result += after[i];
                        }
                    }
                    break;
                }
            }

            watch10.Stop();
            Console.WriteLine("The Time for DiseaseDrugs function : " + watch10.ElapsedMilliseconds);
            return result;
        }

        /// <inheritdoc />
        public string FindDrugAssociated(string name)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            String[] dual;
            String[] after;
            String result1 = name + ":";
            this.Temp = name + ":";

            Thread th = new Thread(this.FindDrugAssociatedHelper);
            th.Start(name);

            foreach (var line in this.DB.DiseaseDrugsNames)
            {
                if (line.Contains(name))
                {
                    dual = line.Split(":");
                    dual[0] = dual[0].Trim();
                    dual[1] = dual[1].Trim();

                    result1 += dual[0];

                    after = dual[1].Split(";");
                    for (int i = 0; i < after.Length; i++)
                    {
                        if (after[i].Contains(name))
                        {
                            result1 += after[i] + ";";
                            break;
                        }
                    }
                }
            }

            th.Join();

            watch.Stop();
            Console.WriteLine("The Time for FindDrugsAssociation function : " + watch.ElapsedMilliseconds);
            return result1 + "\n" + this.Temp;
        }

        private void FindDrugAssociatedHelper(object name)
        {
            String name_ = name as string;
            String[] dual;
            String[] after;

            foreach (var line in this.DB.DrugsEffectsNames)
            {
                dual = line.Split(":");
                dual[0] = dual[0].Trim();
                dual[1] = dual[1].Trim();

                if (dual[1].Contains(name_))
                {
                    after = dual[1].Split(";");

                    this.Temp += dual[0];
                    for (int i = 0; i < after.Length; i++)
                    {
                        if (after[i].Contains(name_))
                        {
                            this.Temp += after[i] + ";";
                            break;
                        }
                    }
                }
            }

            return;
        }

        /// <inheritdoc />
        public bool ContainsDisease(string name)
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            var result = this.DB.DrugsNames.Contains(name);

            watch10.Stop();
            Console.WriteLine("The Time for ContainsDrug function : " + watch10.ElapsedMilliseconds);
            return result;
        }

        /// <inheritdoc />
        public bool ContainsDrug(string name)
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            var result = this.DB.DrugsNames.Contains(name);

            watch10.Stop();
            Console.WriteLine("The Time for ContainsDrug function : " + watch10.ElapsedMilliseconds);
            return result;
        }
    }

    class Program
    {
        // static void Main(string[] args)
        // {
        //     Console.WriteLine("Hello World!");
        //     DiseaseDrugDbAsync db = new DiseaseDrugDbAsync();
        //
        //     Thread.Sleep(1000);
        //     Console.WriteLine(db.DrugsEffectsNames[0]);
        //
        //     return;
        // }

        private static DiseaseDrugDb DB;

        private static void InitDb()
        {
            DB = new DiseaseDrugDb();
        }

        static void Main(string[] args)
        {
            Thread th = new Thread(InitDb);
            th.Start();
            
            Console.WriteLine("Hello World!");

            Console.WriteLine("hello enter something :");
            Console.ReadLine();

            MyOperator op = new MyOperator(DB);
            //Console.WriteLine(op.ContainsDrug("Drug_hvtiayzegc : 84845"));
            //Console.WriteLine(op.FindDiseaseDrugs("Dis_lbqblqdzoo"));
            Console.WriteLine(op.FindDrugAssociated("Drug_vfsskclbhk"));
            return;
        }
    }
}
