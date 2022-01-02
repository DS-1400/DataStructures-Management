using System;
using System.Collections.Generic;
using System.IO;
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
        String FindDisease(String name);
        String FindDrug(String name);
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

        public MyOperator(DiseaseDrugDb db)
        {
            this.DB = db;
        }

        /// <inheritdoc />
        public string FindDiseaseDrugs(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string FindDisease(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string FindDrug(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string FindDrugAssociated(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool ContainsDisease(string name)
        {
            return this.DB.DrugsNames.Contains(name);
        }

        /// <inheritdoc />
        public bool ContainsDrug(string name)
        {
            return 
                this.DB.DrugsNames.Contains(name);
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

            return;
        }
    }
}
