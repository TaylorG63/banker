using banker.Data;

namespace banker
{
    //********************************************************************
    //
    // Taylor Gross
    // Operating Systems
    // Programming Project #4: Banker's Algorithm
    // 3/31/2025
    // Instructor: Dr. Siming Liu
    //
    //********************************************************************
    internal class Program
    {
        static void Main(string[] args)
        {
            string s1 = "";
            try
            {
                s1 = File.ReadAllText(args[0]);
            }
            catch (Exception)
            {
                Console.WriteLine($"File location not found, loading from local data");
                s1 = bankerBackupData.BackupString();
            }
            #region Initializing variables
            int numOfProcesses = 0;
            int numOfResources = 0;
            int RequestVectorIndex = 0;
            bool isSystemSafe = false;
            bool isRequestGranted = false;
            List<List<int>> AllocationMatrix = new List<List<int>>();
            List<List<int>> MaxMatrix = new List<List<int>>();
            List<List<int>> NeedMatrix = new List<List<int>>();
            List<int> AvailableVector = new List<int>();
            List<int> RequestVector = new List<int>();
            MatrixManipulator matrixBuilder = new MatrixManipulator();
            #endregion
            #region Functions
            s1 = s1.Replace("\r\n\r\n", "\r\n");
            string[] strings = s1.Split("\r\n", StringSplitOptions.None);
            numOfProcesses = Int32.Parse(strings[0]);
            numOfResources = Int32.Parse(strings[1]);
            AllocationMatrix = matrixBuilder.MatrixMaker(numOfProcesses, numOfResources, 2, strings);
            MaxMatrix = matrixBuilder.MatrixMaker(numOfProcesses, numOfResources, 2+numOfProcesses , strings);
            NeedMatrix = matrixBuilder.NeedMatrixCalc(AllocationMatrix, MaxMatrix);
            AvailableVector = matrixBuilder.VectorBuilder(strings[strings.Length -2]);
            string[] vectorStrings = strings[strings.Length -1].Split(":");
            RequestVectorIndex = Int32.Parse(vectorStrings[0]);
            RequestVector = matrixBuilder.VectorBuilder(vectorStrings[1]);
            isSystemSafe = matrixBuilder.IsSystemSafe(AllocationMatrix, NeedMatrix, AvailableVector);
            isRequestGranted = matrixBuilder.TryGrantRequest(AllocationMatrix, MaxMatrix, NeedMatrix, AvailableVector, RequestVectorIndex, RequestVector);
            #endregion
            #region Console Writing
            Console.WriteLine($"There are {numOfProcesses} processes in the system.\n");
            Console.WriteLine($"There are {numOfResources} resource types.\n");
            Console.WriteLine("The Allocation Matrix is...");
            Console.Write("   ");
            for (int i = 0; i < numOfResources; i++)
            {
                Console.Write((AlphaEnum)i + " ");
            }
            Console.Write("\n");
            for ( int i = 0; i < AllocationMatrix.Count; i++)
            {
                Console.Write($"{i}: ");
                for (int j = 0; j < AllocationMatrix[i].Count; j++)
                {
                    Console.Write($"{AllocationMatrix[i][j]} ");
                }
                Console.Write("\n");
            }

            Console.Write("\nThe Max Matrix is...");
            Console.Write("\n   ");
            for (int i = 0; i < numOfResources; i++)
            {
                Console.Write((AlphaEnum)i + " ");
            }
            Console.Write("\n");
            for (int i = 0; i < MaxMatrix.Count; i++)
            {
                Console.Write($"{i}: ");
                for (int j = 0; j < MaxMatrix[i].Count; j++)
                {
                    Console.Write($"{MaxMatrix[i][j]} ");
                }
                Console.Write("\n");
            }

            Console.Write("\nThe Need Matrix is...");
            Console.Write("\n   ");
            for (int i = 0; i < numOfResources; i++)
            {
                Console.Write((AlphaEnum)i + " ");
            }
            Console.Write("\n");
            for (int i = 0; i < NeedMatrix.Count; i++)
            {
                Console.Write($"{i}: ");
                for (int j = 0; j < NeedMatrix[i].Count; j++)
                {
                    Console.Write($"{NeedMatrix[i][j]} ");
                }
                Console.Write("\n");
            }
            Console.Write("\nThe Available Vector is ...");
            Console.WriteLine("\n");
            for (int i = 0; i < numOfResources; i++)
            {
                Console.Write((AlphaEnum)i + " ");
            }
            Console.Write("\n");
            for (int i = 0;i < AvailableVector.Count; i++)
            {
                Console.Write($"{AvailableVector[i]} ");
            }

            

            Console.WriteLine();
            
            if (isSystemSafe)
            {
                Console.WriteLine("\nTHE SYSTEM IS IN A SAFE STATE\n");
            }
            else
            {
                Console.WriteLine("\nTHE SYSTEM IS NOT IN A SAFE STATE\n");
            }

            Console.Write("\nThe Request Vector is...");
            Console.Write("\n  ");
            for (int i = 0; i < numOfResources; i++)
            {
                Console.Write((AlphaEnum)i + " ");
            }
            Console.Write("\n");
            Console.Write($"{RequestVectorIndex}:");
            for (int i = 0; i < AvailableVector.Count; i++)
            {
                Console.Write($"{AvailableVector[i]} ");
            }

            

            
            Console.WriteLine();
            if (isRequestGranted)
            {
                Console.WriteLine("\nTHE REQUEST CAN BE GRANTED!\n");

                var updatedAvailable = matrixBuilder.VectorSubtract(AvailableVector, RequestVector);

                Console.WriteLine("\nThe Available Vector is...");
                
                for (int i = 0; i < numOfResources; i++)
                {
                    Console.Write((AlphaEnum)i + " ");
                }
                Console.Write("\n");
                foreach (var res in updatedAvailable)
                {
                    Console.Write($"{res} ");
                }
            }
            else
            {
                Console.WriteLine("\nTHE REQUEST CANNOT BE GRANTED!\n");
            }
            #endregion
        }
    }
}
