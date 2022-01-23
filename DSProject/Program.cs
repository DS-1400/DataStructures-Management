using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;

namespace DSProject
{
    // include two functions to checking existence of diseases & drugs
    interface IContains
    {
        bool ContainsDisease(String name);
        bool ContainsDrug(String name);
    }

    // Find the drugs which are associated with a specific disease
    // Find the drugs & diseases which are associated with a another drug
    interface IFinder
    {
        String FindDiseaseDrugs(String name);
        String FindDrugAssociated(String name);
    }

    // Using for persisting our data about drugs & diseases
    interface IPersistence
    {
        bool PersistDiseases(string path);
        bool PersistDrugs(string path);
        bool PersistDiseasesDrugs(string path);
        bool PersistDrugsEffects(string path);
    }

    // Apply CRUD operations to MyOperator class
    interface ICrd
    {
        bool CreateDisease(string name);
        void CreateDrug(string name,
            string drugEffects,
            string diseasesDrugs,
            string selfDrugEffects);
        bool DeleteDrug(string name);
        bool DeleteDisease(string name);
    }

    interface ILogger
    { 
        void Error(string message);
        void Warning(string message);
        void Info(string message);
    }

    interface IMalfunction
    {
        String DiseaseMalfunction(string diseaseName, string[] drugs);
        String DrugMalfunction(string[] drugs);
    }

    // Required functions that should be implemented
    interface IConsole
    {
        void IncreaseDrugsCost();

        void CalcPrescription();

        void FindDrug();

        void FindDisease();

        void AddDrug();

        void AddDisease();

        void DeleteDrug();

        void DeleteDisease();

        void DrugMalfunction();

        void DiseaseMalfunction();
    }

    class MyConsole: IConsole
    {

        private DiseaseDrugDb Db;
        private MyOperator Operator;
        private MyLogger Logger;
        private String input2; // Input 2 Operator::CreateDrug()
        private String input4; // Input 4 Operator::CreateDrug()

        public MyConsole(DiseaseDrugDb db, MyOperator op, MyLogger logger)
        {
            this.Db = db;
            this.Operator = op;
            this.Logger = logger;
        }

        /// <inheritdoc />
        public void IncreaseDrugsCost() // Proxy is not tested
        {
            this.Logger.Message("Please Enter the inflation rate : ");
            var input = Console.ReadLine();
            if (input == "")
            {
                this.Logger.Warning("You have Entered nothing !");
                input = "1";
            }

            int inflationRate = 1;
            if (int.TryParse(input, out inflationRate))
            {
                this.Operator.ApplyInflationRate(inflationRate);
                this.Logger.Info("Inflation Rate has applied.");
            }
            else
            {
                this.Logger.Error("Your input was not a number !");
                return;
            }

        }

        /// <inheritdoc />
        public void CalcPrescription() // Proxy is not tested
        {
            this.Logger.Message("Please enter the drugs, separate them with comma without any space :");
            var result = Console.ReadLine(); 
            
            var outcome = this.Operator.CalcPrescription(result.Trim().Split(","));
            this.Logger.Message("The result of Calculation is " + outcome);
        }

        public void CalcPrescriptionReplacement()
        {
            this.Logger.Message("Please enter the drugs, separate them with comma without any space :");
            var result = Console.ReadLine();

            var outcome = this.Operator.CalcPrescriptionReplacement(result.Trim().Split(","));
            this.Logger.Message("The result of Calculation is " + outcome);
        }

        /// <inheritdoc />
        public void FindDrug() // Proxy is not tested
        {
            this.Logger.Message("Enter the drug name :");
            var drugName = Console.ReadLine();

            var result = this.Operator.FindDrugAssociated(drugName);
            this.Logger.Message(result);
        }

        /// <inheritdoc />
        public void FindDisease() // Proxy is not tested
        {
            this.Logger.Message("Enter the disease name : ");
            var diseaseName = Console.ReadLine();

            var result = this.Operator.FindDiseaseDrugs(diseaseName);
            this.Logger.Message(result);
        }

        /// <inheritdoc />
        public void AddDrug()  // Proxy is not tested
        {
            this.Logger.Message("Enter the drug name : ");
            var drugName = Console.ReadLine();

            this.Logger.Message("Enter the drug price :");
            var drugPriceStr = Console.ReadLine();

            int drugPrice = 1;
            if (int.TryParse(drugPriceStr, out drugPrice))
            {
                drugPrice = int.Parse(drugPriceStr);
            }
            else
            {
                this.Logger.Error("You have entered wrong input to specify the price");
                return;
            }

            String input1 = drugName + " : " + drugPrice; // Input 1 Operator::CreateDrug()
            String input3 = drugName + ":"; // Input 3 Operator::CreateDrug()
            Thread th1 = new Thread(this.AddDrugHelper); // Produces input2 Operator::CreateDrug()
            Thread th2 = new Thread(this.AddDrugHelper2); // Produces input4 Operator::CreateDrug()

            th1.Start(drugName);
            th2.Start(drugName);

            Random rand = new Random();
            var identifier = rand.Next(1, 5);
            int idx = 0;

            for (int i = 0; i < identifier; i++)
            {
                idx = rand.Next(0, this.Db.DiseaseNames.Count);
                input3 += this.Db.DiseaseNames[idx] + "," + (rand.Next(0, 2) == 1 ? "+" : "-") + ";";
            }

            input3 = input3.TrimEnd(this.Operator.TrimParams);

            th1.Join();
            th2.Join();
            
            this.Operator.CreateDrug(input1, this.input2, 
                input3, this.input4);

            this.Logger.Message("Drug was created");

        }

        private void AddDrugHelper(object drugName) // Produces input2 Operator::CreateDrug()
        {
            String input = drugName + ":";
            Random rand = new Random();
            var identifier = rand.Next(1, 3);
            int idx = 0;
            Char[] trimParams = new[] {'(', ')', ' '};

            String tmp;
            String[] temp;
            for (int i = 0; i < identifier; i++)
            {
                idx = rand.Next(0, this.Db.DrugsEffectsNames.Count);
                tmp = this.Db.DrugsEffectsNames[idx].Split(":")[1];
                temp = tmp.Split(";");
                foreach (var str in temp)
                {
                    input += str.Trim(trimParams) + ";";
                }
            }

            this.input2 = input.TrimEnd(this.Operator.TrimParams);
        }
        
        private void AddDrugHelper2(object drugName) // Produces input4 Operator::CreateDrug()
        {
            String input = drugName + " :";
            Random rand = new Random();
            var identifier = rand.Next(1, 4);
            int idx = 0;

            for (int i = 0; i < identifier; i++)
            {
                idx = rand.Next(0, this.Db.DrugsEffectsNames.Count);
                input += this.Db.DrugsEffectsNames[idx].Split(":")[1] + " ;";
            }

            this.input4 = input.TrimEnd(this.Operator.TrimParams);
        }

        /// <inheritdoc />
        public void AddDisease()  // Proxy is not tested
        {
            this.Logger.Message("Please enter the disease name : ");
            var diseaseName = Console.ReadLine();

            // Choose several drugs to associate with the disease
            Random rand = new Random();
            int[] idx = new int[rand.Next(1, 5)];

            String output = diseaseName + " :";
            for (int i = 0; i < idx.Length; i++)
            {
                idx[i] = rand.Next(0, this.Db.DrugsNames.Count);
                output += " (" + this.Db.DrugsNames[idx[i]].Split(":")[0].TrimEnd(this.Operator.TrimParams) + "," + (rand.Next(0, 2) == 1 ? "+" : "-") + ") ;";
            }

            bool outcome = 
                this.Operator.CreateDisease(output.TrimEnd(this.Operator.TrimParams));
            if (outcome)
            {
                this.Logger.Info("The disease was created !");
            }
            else
            {
                this.Logger.Warning("The disease was NOT created !");
            }
        }

        /// <inheritdoc />
        public void DeleteDrug()  // Proxy is not tested
        {
            this.Logger.Message("Please enter the drug name : ");
            var drugName = Console.ReadLine();

            bool result = this.Operator.DeleteDrug(drugName);

            if (result)
            {
                this.Logger.Info("Drug was founded & deleted !");
                return;
            }
            else
            {
                this.Logger.Info("Drug was not founded !");
                return;
            }
        }

        /// Used for Deleting Disease
        public void DeleteDisease()  // Proxy is not tested
        {
            this.Logger.Message("Please enter the disease name : ");
            var diseaseName = Console.ReadLine();
            bool result = this.Operator.DeleteDisease(diseaseName);

            if (result)
            {
                this.Logger.Info("Disease was found & deleted");
                return;
            }
            else
            {
                this.Logger.Warning("Disease was NOT found !");
                return;
            }
        }

        /// <inheritdoc />
        public void DrugMalfunction()  // Proxy is not tested
        {
            this.Logger.Message("Please enter the drug names , separate them with comma :");

            var drugs = Console.ReadLine();
            var result = this.Operator.DrugMalfunction(drugs.Trim().Split(","));

            this.Logger.Message(result);
        }

        /// <inheritdoc />
        public void DiseaseMalfunction()  // Proxy is not tested
        {
            this.Logger.Message("Enter the disease name : ");
            var diseaseName = Console.ReadLine();
            
            this.Logger.Message("Enter the drug names , separate them with comma : ");
            var drugs = Console.ReadLine();
           
            var result = this.Operator.DiseaseMalfunction(diseaseName, drugs.Trim().Split(","));
            this.Logger.Message(result);
        }

        public void PrintRecentLogs()
        {
            this.Logger.PrintRecentLogs();
        }
    }

    class MyLogger: ILogger
    {
        private String LogPath;

        public MyLogger()
        {
            this.LogPath = @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\project_logs_ds.txt";
            File.Delete(this.LogPath);
        }

        
        public void Message(string message)
        { 
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
            Console.ResetColor();
            File.AppendAllText(this.LogPath, message + "\n");
        }

        public void Error(string message)
        { 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR : " + message);
            Console.ResetColor();
            File.AppendAllText(this.LogPath, "ERROR : " + message + "\n");
        }

        public void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning : " + message);
            Console.ResetColor();
            File.AppendAllText(this.LogPath, "WARNING : " + message + "\n");
        }

        public void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("INFO : " + message);
            Console.ResetColor();
            File.AppendAllText(this.LogPath, "INFO : " + message + "\n");
        }

        public void PrintRecentLogs()
        {
            var lines = File.ReadAllLines(this.LogPath);
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }

    // Application BD class in sync mode
    class DiseaseDrugDb
    {
        public List<String> DrugsNames;
        public List<String> DiseaseNames;
        public List<String> DiseaseDrugsNames;
        public List<String> DrugsEffectsNames;

        public DiseaseDrugDb() // 170 Milliseconds for first time 
        {
            this.DiseaseNames =
               new List<string>(
                   File.ReadAllLines(
                       @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\diseases.txt"));

           this.DiseaseDrugsNames =
               new List<string>(
                   File.ReadAllLines(
                       @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\alergies.txt"));

           this.DrugsNames =
               new List<string>(
                   File.ReadAllLines(
                       @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\drugs.txt"));

           this.DrugsEffectsNames =
               new List<string>(
                   File.ReadAllLines(
                       @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\effects.txt"));
        }
    }

    // Application BD class in Async mode
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

    // Do all of fundamental operations for us
    class MyOperator: IFinder, IContains, IPersistence, ICrd, IMalfunction
    {
        private DiseaseDrugDb DB;
        private MyLogger Logger;

        private String Temp;
        private int TempResult; // Used in CalcPrescription Computing
        public Char[] TrimParams;

        private String DiseasePath;
        private String DiseaseDrugsPath;
        private String DrugsPath;
        private String DrugsEffectsPath;

        private String SDiseasePath;
        private String SDiseaseDrugsPath;
        private String SDrugsPath;
        private String SDrugsEffectsPath;

        private ReaderWriterLockSlim RWL;

        public MyOperator(DiseaseDrugDb db, MyLogger logger)
        {
            this.DB = db;
            this.Logger = logger;

            this.DiseasePath = 
                @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\diseases.txt";

            this.DiseaseDrugsPath = 
                @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\alergies.txt";

            this.DrugsEffectsPath =
                @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\effects.txt";

            this.DrugsPath =
                @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\drugs.txt";

            this.SDiseasePath =
                @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\diseases_2.txt";

            this.SDiseaseDrugsPath =
                @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\alergies_2.txt";

            this.SDrugsEffectsPath =
                @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\effects_2.txt";

            this.SDrugsPath =
                @"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\drugs_2.txt";


            this.TrimParams = new[] {';', ' '};
        }

        /// It will return the actual line which contains the disease
        public string FindDiseaseDrugs(string name) // 18 Milliseconds
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            String[] dual;
            String[] after;
            String result = name + " : ";
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
                            result += after[i] + " ; ";
                        }
                    }
                    break;
                }
            }

            watch10.Stop();
            this.Logger.Info("The Time for DiseaseDrugs function : " + watch10.ElapsedMilliseconds);
            return result.TrimEnd(this.TrimParams);
        }

        // It will print three lines first for diseases, third for drugs have effects on target,
        // second for target has effects on other drugs
        public string FindDrugAssociated(string name) // 69 51 31 Milliseconds
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            String[] dual;
            String[] after;
            String result1 = name + " : ";
            this.Temp = name + " : ";

            Thread th = new Thread(this.FindDrugAssociatedHelper);
            th.Start(name);

            foreach (var line in this.DB.DiseaseDrugsNames)
            {
                if (line.Contains(name))
                {
                    dual = line.Split(":");
                    dual[0] = dual[0].Trim();
                    dual[1] = dual[1].Trim();

                    //result1 += dual[0];

                    after = dual[1].Split(";");
                    for (int i = 0; i < after.Length; i++)
                    {
                        if (after[i].Contains(name))
                        {
                            result1 += "(" + dual[0] + "," + after[i].Split(",")[1] + "; ";
                            break;
                        }
                    }
                }
            }

            String result2 = "";
            foreach (var drugEffects in this.DB.DrugsEffectsNames)
            {
                dual = drugEffects.Split(":");
                if (dual[0].Contains(name))
                {
                    result2 = drugEffects;
                    break;
                }
            }

            th.Join();

            watch.Stop();
            this.Logger.Info("The Time for FindDrugsAssociation function : " + watch.ElapsedMilliseconds);
            return result1.TrimEnd(this.TrimParams) + "\n" + this.Temp.TrimEnd(this.TrimParams) + 
                   "\n" + result2;
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

                    //this.Temp += dual[0];
                    for (int i = 0; i < after.Length; i++)
                    {
                        if (after[i].Contains(name_))
                        {
                            this.Temp += "(" + dual[0] + "," + after[i].Split(",")[1] + "; ";
                            break;
                        }
                    }
                }
            }

            return;
        }

        /// <inheritdoc />
        public bool ContainsDisease(string name) // 7 Milliseconds
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            var result = this.DB.DiseaseNames.Contains(name);

            watch10.Stop(); 
            this.Logger.Info("The Time for ContainsDrug function : " + watch10.ElapsedMilliseconds);
            return result;
        }

        /// <inheritdoc />
        public bool ContainsDrug(string name) // 0 Milliseconds
        {
            var watch10 = System.Diagnostics.Stopwatch.StartNew();

            bool result = false;
            foreach (var drug in this.DB.DrugsNames)
            {
                result = drug.Contains(name);
                if (result == true)
                {
                    break;
                }
            }

            watch10.Stop();
            this.Logger.Info("The Time for ContainsDrug function : " + watch10.ElapsedMilliseconds);
            return result;
        }

        /// <inheritdoc />
        public bool PersistDiseases(string path)
        {
            File.WriteAllLines(@path, this.DB.DiseaseNames);
            return true;
        }

        /// <inheritdoc />
        public bool PersistDrugs(string path)
        {
            File.WriteAllLines(@path, this.DB.DrugsNames);

            return true;
        }

        /// <inheritdoc />
        public bool PersistDiseasesDrugs(string path)
        {
            File.WriteAllLines(@path, this.DB.DiseaseDrugsNames);
            
            return true;
        }

        /// <inheritdoc />
        public bool PersistDrugsEffects(string path)
        {
            File.WriteAllLines(@path, this.DB.DrugsEffectsNames);

            return true;
        }

        /// <inheritdoc />
        public bool CreateDisease(string input) // The input should be in document format // Test is required Need handle duplicated diseases
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            String[] dual = input.Split(":");
            bool modifiedFlag = false;

            if (!this.DB.DiseaseNames.Contains(dual[0]))
            {
                this.DB.DiseaseNames.Add(dual[0]);
                this.DB.DiseaseDrugsNames.Add(input);
                modifiedFlag = true;
            }

            if (modifiedFlag)
            {
                // Theses addresses must get from input
                this.PersistDiseasesDrugs(
                    this.DiseaseDrugsPath);
                this.PersistDiseases(
                    this.DiseasePath);
            } 

            watch.Stop();
            this.Logger.Info("The time for creating a disease" +
                             " with associated drugs : " + watch.ElapsedMilliseconds);

            return modifiedFlag;
        }

        /// <inheritdoc />
        public void CreateDrug( // test is required
            string drugName, // drug_aa : price
            string drugsEffects, //  must be in this format   drug_aa:drug_name,drug_effect;drug_name,drug_effect
            string diseasesDrugs, // must be in this format   drug_aa:disease_name,+;disease_name,-
            string selfDrugEffects // must be in document format
            ) // Test is required
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (this.DB.DrugsNames.Contains(drugName))
            {
                this.Logger.Error("The drug is repetitious");
                return;
            }

            Thread th1 = new Thread(this.CreateDrugHelper);
            Thread th2 = new Thread(this.CreateDrugHelper2);

            th1.Start(drugsEffects);
            th2.Start(diseasesDrugs);

            this.DB.DrugsNames.Add(drugName);
            this.PersistDrugs(this.DrugsPath);

            th1.Join();
            th2.Join();

            this.DB.DrugsEffectsNames.Add(selfDrugEffects);
            this.PersistDrugsEffects(this.DrugsEffectsPath);

            watch.Stop();
            this.Logger.Info("The time for adding a drug is " + watch.ElapsedMilliseconds);
        }

        private void CreateDrugHelper(object input) // Handle the drug Effects
        {
            string drugsEffects = input as string;
            String[] dual = drugsEffects.Split(":");
            String[] after = dual[1].Split(";");
            Dictionary<String, String> drugsEffectsDict = new Dictionary<string, string>();

            String[] tmp;
            foreach (var str in after)
            {
                tmp = str.Split(",");
                drugsEffectsDict.Add(tmp[0], tmp[1]);
            }

            while (drugsEffectsDict.Count > 0)
            {
                foreach (var kv in drugsEffectsDict)
                {
                    bool found = false;
                    for (int i = 0; i < this.DB.DrugsEffectsNames.Count; i++)
                    {
                        if (this.DB.DrugsEffectsNames[i].Split(":")[0].Contains(kv.Key))
                        {
                            this.DB.DrugsEffectsNames[i] += " ; (" + dual[0] + "," + kv.Value + ")";
                            drugsEffectsDict.Remove(kv.Key);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        this.DB.DrugsEffectsNames.Add(kv.Key + " :" + " (" + dual[0] + "," + kv.Value + ")");
                        drugsEffectsDict.Remove(kv.Key);
                    }
                    break;
                }
            }

            while (drugsEffectsDict.Count > 0)
            {
                foreach (var kv in drugsEffectsDict)
                {
                    
                }
            }

            
            this.PersistDrugsEffects(this.DrugsEffectsPath);
        }

        private void CreateDrugHelper2(object input) // Handle the disease Drugs
        {
            string diseasesDrugs = input as string;
            String[] dual = diseasesDrugs.Split(":"); // dual[0] is the drug_name like drug_aa
            String[] after = dual[1].Split(";");
            Dictionary<String, String> diseaseEffects = new Dictionary<string, string>();

            String[] tmp;
            foreach (var str in after)
            {
                tmp = str.Split(",");
                diseaseEffects.Add(tmp[0], tmp[1]);
            }

            foreach (var kv in diseaseEffects)
            {
                for (int i = 0; i < this.DB.DiseaseDrugsNames.Count; i++)
                {
                    if (this.DB.DiseaseDrugsNames[i].Contains(kv.Key))
                    {
                        this.DB.DiseaseDrugsNames[i] += " ; (" + dual[0] + "," + kv.Value + ")";
                        diseaseEffects.Remove(kv.Key);
                        break;
                    }
                }
            }

            if (diseaseEffects.Count > 0)
            {
                foreach (var kv in diseaseEffects)
                {
                    this.DB.DiseaseDrugsNames.Add(kv.Key + " : " + "(" + dual[0] + "," + kv.Value + ")");
                }
            }
            this.PersistDiseasesDrugs(this.DiseaseDrugsPath);
        }

        /// Deletes specific drug from drugs.txt
        public bool DeleteDrug(string name)
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
                    break;
                }
            }

            if (modifiedFlag) // Theses addresses must get from input
            {
                this.PersistDrugs(
                    this.SDrugsPath);
            }

            th.Join();
            th2.Join();

            watch.Stop();
            this.Logger.Info("The time for deleting Drug : " + watch.ElapsedMilliseconds);

            return modifiedFlag;
        }

        /// Deletes specific drug from alergies.txt 
        private void DeleteDrugHelper(object name)
        {
            String name_ = name as string;
            String[] dual;
            String[] after;
            String tmp = "";
            bool modifiedFlag = false;

            for (int i = 0; i < this.DB.DiseaseDrugsNames.Count; i++)
            {
                if (this.DB.DiseaseDrugsNames[i].Contains(name_))
                {
                    modifiedFlag = true;
                    dual = this.DB.DiseaseDrugsNames[i].Split(":");
                    after = dual[1].Split(";");

                    tmp += dual[0] + ":";

                    for (int j = 0; j < after.Length; j++)
                    {
                        if (!after[j].Contains(name_))
                        {
                            tmp += after[j] + ";";
                        }
                    }

                    if (tmp.Contains('('))
                    {
                        this.DB.DiseaseDrugsNames[i] = tmp.TrimEnd(this.TrimParams);
                    }
                    else
                    {
                        this.DB.DiseaseDrugsNames.RemoveAt(i);
                        i -= 1;
                    }
                }

                tmp = "";
            }

            if (modifiedFlag) // Theses addresses must get from input
            {
                this.PersistDiseasesDrugs(
                    this.SDiseaseDrugsPath);
            }
        }

        /// Delete specific drug from effects.txt
        private void YetAnotherDeleteDrugHelper(object name)
        {
            string name_ = name as string;
            String[] dual;
            String[] after;
            String tmp = "";
            bool modifiedFlag = false;

            for (int j = 0; j < this.DB.DrugsEffectsNames.Count; j++)
            {
                dual = this.DB.DrugsEffectsNames[j].Split(":");
                if (dual[0].Contains(name_))
                {
                    this.DB.DrugsEffectsNames.RemoveAt(j);
                    j -= 1;
                    modifiedFlag = true;
                }
                else if (dual[1].Contains(name_))
                {
                    modifiedFlag = true;
                    after = dual[1].Split(";");

                    tmp += dual[0] + ":";
                    for (int i = 0; i < after.Length; i++)
                    {
                        if (!after[i].Contains(name_))
                        {
                            tmp += after[i] + ";";
                        }
                    }

                    if (tmp.Contains('('))
                    {
                        this.DB.DrugsEffectsNames[j] = tmp.TrimEnd(this.TrimParams);
                    }
                    else
                    {
                        this.DB.DrugsEffectsNames.RemoveAt(j);
                        j -= 1;
                    }
                }

                tmp = "";
            }

            if (modifiedFlag) // Theses addresses must get from input
            {
                this.PersistDrugsEffects(
                    this.SDrugsEffectsPath);
            }
        }

        /// Deletes specific disease from disease.txt
        public bool DeleteDisease(string name)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            bool modifiedFlag = false;
            Thread th = new Thread(this.DeleteDiseaseHelper);

            th.Start(name);

            if (this.DB.DiseaseNames.Contains(name))
            {
                modifiedFlag = true;
                this.DB.DiseaseNames.Remove(name);
            }

            if (modifiedFlag) // These addresses should take from input
            {
                this.PersistDiseases(
                    this.SDiseasePath);
            }

            th.Join();

            watch.Stop();
            this.Logger.Info("The time for deleting Disease : " + watch.ElapsedMilliseconds);

            return modifiedFlag;
        }

        /// Deletes specific disease from alergies.txt 
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

            if (modifiedFlag) // Theses addresses must get from input
            {
                this.PersistDiseasesDrugs(
                    this.SDiseaseDrugsPath);
            }
        }

        public void ApplyInflationRate(int infRate) // 170 Milliseconds
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Thread th1 = new Thread(this.ApplyInflationRateHelper);
            th1.Start(infRate);

            String[] dual;
            for (int i = 0; i < this.DB.DrugsNames.Count / 2; i++)
            {
                dual = this.DB.DrugsNames[i].Split(":");
                int price = int.Parse(dual[1].Trim());
                price *= infRate;

                this.DB.DrugsNames[i] = dual[0] + ": " + price;
            }

            th1.Join();
            this.PersistDrugs(this.DrugsPath);

            watch.Stop();
            this.Logger.Info("The time to Apply Inflation Rate : " + watch.ElapsedMilliseconds);
        }

        private void ApplyInflationRateHelper(object input)
        {
            int infRate = (int) input;

            String[] dual;
            for (int i = (this.DB.DrugsNames.Count / 2); i < this.DB.DrugsNames.Count; i++)
            {
                dual = this.DB.DrugsNames[i].Split(":");

                int price = int.Parse(dual[1].Trim());
                price *= infRate;

                this.DB.DrugsNames[i] = dual[0] + ": " + price;
            }
        }

        public int CalcPrescription(string[] inputs) // Test is required, a little bug was founded
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            this.RWL = new ReaderWriterLockSlim();
            List<String> inputs1 = new List<string>(inputs[0..(inputs.Length/2)]);
            List<String> inputs2 = new List<string>(inputs[(inputs.Length/2)..(inputs.Length)]);
            Thread th1 = new Thread(CalcPrescriptionHelper);
            
            th1.Start(inputs1);
            int result = 0;

            try
            {
                this.RWL.EnterReadLock();

                foreach (var drug in this.DB.DrugsNames)
                {
                    string[] dual = drug.Split(":");
                    dual[0] = dual[0].Trim();
                    dual[1] = dual[1].Trim();
                    for (int i = 0; i < inputs2.Count; i++)
                    {
                        if (dual[0] == inputs2[i])
                        {
                            result += int.Parse(dual[1]);
                            inputs2.RemoveAt(i);
                            break;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                this.RWL.ExitReadLock();
            }

            th1.Join();

            result += this.TempResult;
            this.TempResult = 0;

            watch.Stop();
            this.Logger.Info("The time for Prescription Calculation : " + watch.ElapsedMilliseconds);

            return result;
        }

        private void CalcPrescriptionHelper(object inputs)
        {
            this.TempResult = 0;
            List<String> inputs2 = inputs as List<String>;

            try
            {
                this.RWL.EnterReadLock();
                foreach (var drug in this.DB.DrugsNames)
                {
                    string[] dual = drug.Split(":");
                    dual[0] = dual[0].Trim();
                    dual[1] = dual[1].Trim();
                    for (int i = 0; i < inputs2.Count; i++)
                    {
                        if (dual[0] == inputs2[i])
                        {
                            this.TempResult += int.Parse(dual[1]);
                            inputs2.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);

            }
            finally
            {
                this.RWL.ExitReadLock();
            }
        }

        public int CalcPrescriptionReplacement(string[] inputs)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            List<string> inputs2 = new List<string>(inputs);
            int result = 0;

            foreach (var drug in this.DB.DrugsNames)
            {
                string[] dual = drug.Split(":");
                dual[0] = dual[0].Trim();
                dual[1] = dual[1].Trim();
                for (int i = 0; i < inputs2.Count; i++)
                {
                    if (dual[0] == inputs2[i])
                    {
                        result += int.Parse(dual[1]);
                        inputs2.RemoveAt(i);
                        break;
                    }
                }
            }

            watch.Stop();
            this.Logger.Info("The time for Prescription Calculation : " + watch.ElapsedMilliseconds);
            return result;
        }

        /// <inheritdoc />
        public string DiseaseMalfunction(string diseaseName, string[] drugs) // 2 Milliseconds
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            String result = diseaseName + " :";
            foreach (var disease in this.DB.DiseaseDrugsNames)
            {
                if (disease.Contains(diseaseName))
                {
                    String drugss = disease.Split(":")[1];
                    String[] splitedDrugs = drugss.Split(";");

                    for (int i = 0; i < drugs.Length; i++)
                    {
                        if (drugss.Contains(drugs[i]))
                        {
                            for (int j = 0; j < splitedDrugs.Length; j++)
                            {
                                if (splitedDrugs[j].Contains(drugs[i]) && splitedDrugs[j].Contains("-"))
                                {
                                    result += splitedDrugs[j] + ";";
                                    break;
                                }
                            }
                        }
                    }

                    break;
                }
            }

            watch.Stop();
            this.Logger.Info("The time for DiseaseMalfunction : " + watch.ElapsedMilliseconds);
            return result.TrimEnd(this.TrimParams);
        }

        /// <inheritdoc />
        public string DrugMalfunction(string[] drugs)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            String result = "";
            String[] dual;
            String[] after;

            foreach (var drugEffects in this.DB.DrugsEffectsNames)
            {
                dual = drugEffects.Split(":");
                after = dual[1].Split(";");
                for (int i = 0; i < drugs.Length; i++)
                {
                    if (dual[0].Contains(drugs[i]))
                    {
                        String tmpResult = dual[0] + ":";
                        for (int j = 0; j < drugs.Length; j++)
                        {
                            if (dual[1].Contains(drugs[j]))
                            {
                                for (int k = 0; k < after.Length; k++)
                                {
                                    if (after[k].Contains(drugs[j]))
                                    {
                                        tmpResult += after[k] + ";";
                                        break;
                                    }
                                }
                            }
                        }

                        result += tmpResult.TrimEnd(this.TrimParams) + "\n";
                        break;
                    }
                }
            }

            watch.Stop();
            this.Logger.Info("The time for diseaseMalfunction : " + watch.ElapsedMilliseconds);
            return result;
        }
    }

    class Program
    {
        private static DiseaseDrugDb DB;

        private static MyConsole Cons;

        private static MyLogger Logg;

        private static MyOperator Op;

        // Initialize the Console
        private static void InitConsole(DiseaseDrugDb db, MyOperator op, MyLogger logger)
        {
            if (Program.Cons == null)
            {
                Cons = new MyConsole(db, op, logger);
            }
        }

        // Initialized the DB
        private static void InitDb()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //watch.Start();

            if (Program.DB == null)
            {
                DB = new DiseaseDrugDb();
            }

            watch.Stop();
            Logg.Info("The time for Initialization of DB is " + watch.ElapsedMilliseconds);
        }

        private static void Panel()
        { 
            String panel = "1.Init DB & Operator (1)\n2.Find specific drug (2)\n";
            panel += "3.Find specific disease (3)\n4.Apply inflation rate (4)\n";
            panel += "5.Calculate Prescription (5)\n6.Create drug (6)\n";
            panel += "7.Create disease (7)\n8.Drug malfunction detection (8)\n";
            panel += "9.Disease malfunction detection (9)\n10.Delete drug (10)\n";
            panel += "11.Delete disease (11)\n0.Exit (0)\n";
            panel += "12.Print logs (12)\n";
            panel += "Enter the number : ";

            while (true)
            { 
                Console.WriteLine(panel);
                var input = Console.ReadLine();
                if (DB == null && input != "1" && input != "0")
                {
                    Console.Clear();
                    Logg.Error("There is no DB, Operator & Console; You have to initialize them first. Choose first option.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                if (input == "1")
                {
                    Console.Clear();

                    InitDb();
                    Op = new MyOperator(DB, Logg);
                    InitConsole(DB, Op, Logg);

                    Logg.Info("DB was initialized");
                    Console.ReadLine();
                    Console.Clear();
                }
                else if (input == "0")
                {
                    Console.Clear();
                    Logg.Info("Exit ...");
                    return;
                } else if (input == "2")
                {
                    Console.Clear();
                    Cons.FindDrug();
                    Console.ReadKey();
                    Console.Clear();
                } else if (input == "3")
                {
                    Console.Clear();
                    Cons.FindDisease();
                    Console.ReadKey();
                    Console.Clear();
                } else if (input == "4")
                {
                    Console.Clear();
                    Cons.IncreaseDrugsCost();
                    Console.ReadKey();
                    Console.Clear();
                } else if(input == "5")
                {
                    Console.Clear();
                    Cons.CalcPrescriptionReplacement(); // we can change it with CalcPrescription
                    Console.ReadKey();
                    Console.Clear();
                } else if (input == "6")
                {
                    Console.Clear();
                    Cons.AddDrug();
                    Console.ReadKey();
                    Console.Clear();
                } else if (input == "7")
                {
                    Console.Clear();
                    Cons.AddDisease();
                    Console.ReadKey();
                    Console.Clear();
                } else if (input == "8")
                {
                    Console.Clear();
                    Cons.DrugMalfunction();
                    Console.ReadKey();
                    Console.Clear();
                } 
                else if (input == "9")
                {
                    Console.Clear();
                    Cons.DiseaseMalfunction();
                    Console.ReadKey();
                    Console.Clear();
                } else if (input == "10")
                {
                    Console.Clear();
                    Cons.DeleteDrug();
                    Console.ReadKey();
                    Console.Clear();
                } else if (input == "11")
                {
                    Console.Clear();
                    Cons.DeleteDisease();
                    Console.ReadKey();
                    Console.Clear();
                } else if (input == "12")
                {
                    Console.Clear();
                    Cons.PrintRecentLogs();
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                { 
                    Console.Clear();
                }
            }
        }

        static void Main(string[] args)
        {
            // Initialization 153 Milliseconds
            MyLogger logger = new MyLogger();
            Logg = logger;
            // InitDb();
            // Op = new MyOperator(DB, Logg);
            // InitConsole(DB, Op, Logg);

            // Initialization

            //Console
            
            Panel(); // TODO this is the panel, so important

            //Console

            //Console.WriteLine(op.ContainsDisease("Dis_xmbjdyijco"));

            //Console.WriteLine(op.ContainsDrug("Drug_hvtiayzegc"));

            //Console.WriteLine(op.FindDiseaseDrugs("Dis_lbqblqdzoo"));

            // Console.WriteLine(op.FindDrugAssociated("Drug_vfsskclbhk"));
            // Console.WriteLine(op.FindDrugAssociated("Drug_vobddjeuyu"));
            // Console.WriteLine(op.FindDrugAssociated("Drug_mlsvozghuj"));

            // op.PersistDiseases(@"C:\Users\Asus\Desktop\DS-Final-Project\DS-Final-Project\datasets\diseases_2.txt");

            // op.DeleteDisease("Dis_xmbjdyijco"); // Test the diseases.txt look at end of file
            // op.DeleteDisease("Dis_lbqblqdzoo"); // Test the alergies.txt look at end of file


            // op.DeleteDrug("Drug_ucxnqwcpsf"); // Test the drugs.txt, look at end of file
            // op.DeleteDrug("Drug_vobddjeuyu"); // Test the alergies.txt look at end of file
            // op.DeleteDrug("Drug_uecqvzgzwq"); // Test the alergies.txt look at end of file
            // op.DeleteDrug("Drug_wdqjyjytrl"); // Test the alergies.txt look at end of file
            // op.DeleteDrug("Drug_xkmxoweplh"); // Test the alergies.txt look at end of file
            // op.DeleteDrug("Drug_mlsvozghuj"); // Test the effects.txt look at end of file

            // op.CreateDisease("Dis_aaaaaaaaaa : (Drug_ddddddd,+) ; (Drug_eeeeeee,-) ; (Drug_dfwdfwdfw,+)"); // Test for creating disease

            // Op.CreateDrug( // Test for create drug
            //     "Drug_zzzzzzzzzz : 7676",
            //     "Drug_zzzzzzzzzz:Drug_igqkzmfllx,Eff_tvhidekyud;Drug_pxouyeenru,Eff_bsmcbsnxps;Drug_eeeeeeeeee,Eff_xyhezrxkml",
            //     "Drug_zzzzzzzzzz:Dis_rpqivyelnn,+;Dis_gmtkgthudh,-",
            //     "Drug_zzzzzzzzzz : (Drug_ugqzkbyryr,Eff_kbbhexfirm) ; (Drug_qlihgxyjok,Eff_fsmsfgmihc)");


            // op.ApplyInflationRate(2);

            // var resultCalcaCalcPrescription = op.CalcPrescription(new[]
            // {
            //     "Drug_ucxnqwcpsf",
            //     "Drug_buprxehepe",
            //     "Drug_hvtiayzegc",
            //     "Drug_vhlwunmpuu",
            // });
            // Console.WriteLine("The result CalcPrescription : " + resultCalcaCalcPrescription);

            // String[] inputs = new[] {"kfroiaefi", "safdhaisfiu", "hdfasbi", "aushdfyadf"};
            // var newin = new List<String>(inputs[3..4]);
            // Console.WriteLine(newin.Count);

            // var result = Op.DiseaseMalfunction("Dis_mvkepinytj", new[]
            // {
            //     "Drug_wuynwadycl",
            //     "Drug_qclktzzkyi",
            //     "Drug_qoyexsylpd",
            //     "Drug_jmjobqnovn",
            // });
            // Console.WriteLine(result);

            // var result = Op.DrugMalfunction(new[]
            // {
            //     "Drug_mlsvozghuj",
            //     "Drug_mtystjzxzf",
            //     "Drug_fqjiihjvbp",
            //     "Drug_igqkzmfllx",
            //     "Drug_iazfuwyaan",
            // });
            // Console.WriteLine(result);

            return;
        }
    }
}
