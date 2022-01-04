using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    interface IPersistence
    {
        bool PersistDiseases(string path);
        bool PersistDrugs(string path);
        bool PersistDiseasesDrugs(string path);
        bool PersistDrugsEffects(string path);
    }

    interface ICRD
    {
        void CreateDisease(string name);
        void CreateDrug(string name);
        void DeleteDrug(string name);
        void DeleteDisease(string name);
    }

    class DiseaseDrugDb
    {
        public List<String> DrugsNames;
        public List<String> DiseaseNames;
        public List<String> DiseaseDrugsNames;
        public List<String> DrugsEffectsNames;

        public DiseaseDrugDb() // 170 Milliseconds for first time 
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
                new List<string>(
                    File.ReadAllLines(
                    @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\alergies.txt"));

            this.DrugsEffectsNames = 
                new List<string>(
                    File.ReadAllLines(
                    @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\effects.txt"));

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

    class MyOperator: IFinder, IContains, IPersistence, ICRD
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public bool PersistDiseases(string path)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            File.WriteAllLines(@path, this.DB.DiseaseNames);

            watch.Stop();
            Console.WriteLine("The time for persisting Diseases : " + watch.ElapsedMilliseconds);
            return true;
        }

        /// <inheritdoc />
        public bool PersistDrugs(string path)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            File.WriteAllLines(@path, this.DB.DrugsNames);

            watch.Stop();
            Console.WriteLine("The time for persisting Drugs : " + watch.ElapsedMilliseconds);
            return true;
        }

        /// <inheritdoc />
        public bool PersistDiseasesDrugs(string path)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            File.WriteAllLines(@path, this.DB.DiseaseDrugsNames);
            
            watch.Stop();
            Console.WriteLine("The time for persisting DiseaseDrugs : " + watch.ElapsedMilliseconds);
            return true;
        }

        /// <inheritdoc />
        public bool PersistDrugsEffects(string path)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            File.WriteAllLines(@path, this.DB.DrugsEffectsNames);

            watch.Stop();
            Console.WriteLine("The time for persisting DrugsEffects : " + watch.ElapsedMilliseconds);
            return true;
        }

        /// <inheritdoc />
        public void CreateDisease(string name)
        {
            
        }

        /// <inheritdoc />
        public void CreateDrug(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void DeleteDrug(string name)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            bool modifiedFlag = false;
            Thread th = new Thread(this.DeleteDrugHelper);
            Thread th2 = new Thread(this.YetAnotherDeleteDrugHelper);

            th.Start(name);
            th2.Start(name);

            for (int i = 0; i < this.DB.DrugsNames.Count; i++)
            {
                if (this.DB.DrugsNames[i].Contains(name))
                {
                    this.DB.DrugsNames.RemoveAt(i);
                    modifiedFlag = true;
                }
            }

            if (modifiedFlag)
            {
                this.PersistDrugs(
                    @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\drugs_2.txt");
            }

            th.Join();
            th2.Join();

            watch.Stop();
            Console.WriteLine("The time for deleting Drug : " + watch.ElapsedMilliseconds);
        }

        private void DeleteDrugHelper(object name)
        {
            String name_ = name as string;
            //String[] dual;
            //String[] after;
            String tmp = "";
            bool modifiedFlag = false;

            for (int i = 0; i < this.DB.DiseaseDrugsNames.Count; i++)
            {
                if (this.DB.DiseaseDrugsNames[i].Contains(name_))
                {
                    modifiedFlag = true;
                    String[] dual = this.DB.DiseaseDrugsNames[i].Split(":");
                    String[] after = dual[1].Split(";");

                    tmp += dual[0] + ":";

                    for (int j = 0; j < after.Length; j++)
                    {
                        if (!after[j].Contains(name_))
                        {
                            tmp += after[j] + ";";
                        }
                    }

                    this.DB.DiseaseDrugsNames[i] = tmp;
                }

                tmp = "";
            }

            if (modifiedFlag)
            {
                this.PersistDiseasesDrugs(
                    @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\alergies_2.txt");
            }
        }

        private void YetAnotherDeleteDrugHelper(object name)
        {
            string name_ = name as string;
            //String[] dual;
            //String[] after;
            String tmp = "";
            bool modifiedFlag = false;

            for (int j = 0; j < this.DB.DrugsEffectsNames.Count; j++)
            {
                String[] dual = this.DB.DrugsEffectsNames[j].Split(":");
                if (dual[0].Contains(name_))
                {
                    this.DB.DrugsEffectsNames.RemoveAt(j);
                    modifiedFlag = true;
                }
                else if (dual[1].Contains(name_))
                {
                    modifiedFlag = true;
                    String[] after = dual[1].Split(";");

                    tmp += dual[0];
                    for (int i = 0; i < after.Length; i++)
                    {
                        if (!after[i].Contains(name_))
                        {
                            tmp += after[i];
                        }
                    }

                    this.DB.DrugsEffectsNames[j] = tmp;
                }

                tmp = "";
            }

            if (modifiedFlag)
            {
                this.PersistDrugsEffects(
                    @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\effects_2.txt");
            }
        }

        /// <inheritdoc />
        public void DeleteDisease(string name)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            bool modifiedFlag = false;
            Thread th = new Thread(this.DeleteDiseaseHelper);

            th.Start(name);

            for (int i = 0; i < this.DB.DiseaseNames.Count; i++)
            {
                if (this.DB.DiseaseNames[i].Contains(name))
                {
                    modifiedFlag = true;
                    this.DB.DiseaseNames.RemoveAt(i);
                    break;
                }
            }
            th.Join();

            if (modifiedFlag)
            {
                this.PersistDiseases(
                    @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\diseases_2.txt");
            }

            watch.Stop();
            Console.WriteLine("The time for deleting Disease : " + watch.ElapsedMilliseconds);
        }

        private void DeleteDiseaseHelper(object name)
        {
            String name_ = name as string;
            bool modifiedFlag = false;

            for (int i = 0; i < this.DB.DiseaseDrugsNames.Count; i++)
            {
                if (this.DB.DiseaseDrugsNames[i].Contains(name_))
                {
                    modifiedFlag = true;
                    this.DB.DiseaseDrugsNames.RemoveAt(i);
                    break;
                }
            }

            if (modifiedFlag)
            {
                this.PersistDiseasesDrugs(
                    @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\alergies_2.txt");
            }
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

            th.Join();
            Console.WriteLine("hello enter something :");
            Console.ReadLine();

            MyOperator op = new MyOperator(DB);
            //Console.WriteLine(op.ContainsDrug("Drug_hvtiayzegc : 84845"));
            
            //Console.WriteLine(op.FindDiseaseDrugs("Dis_lbqblqdzoo"));
            
            //Console.WriteLine(op.FindDrugAssociated("Drug_vfsskclbhk"));
            
            //op.PersistDiseases(@"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\diseases_2.txt");

            //op.DeleteDrug("Drug_ucxnqwcpsf");
            //op.PersistDrugs(@"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\drugs_2.txt");

            //op.DeleteDisease("Dis_xmbjdyijco");
            //op.DeleteDisease("Dis_lbqblqdzoo");


            op.DeleteDrug("Drug_ucxnqwcpsf");
            //op.DeleteDrug("Drug_mtystjzxzf");
            //op.DeleteDrug("Drug_mtystjzxzf");

            return;
        }
    }
}
