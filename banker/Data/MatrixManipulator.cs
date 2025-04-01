namespace banker.Data
{
    public class MatrixManipulator
    {
        //********************************************************************
        //
        // Matrix Maker Function
        //
        // This function is to make the matrix needed for the Bankers Algorithm,
        // by setting the return to a list of lists of ints we can change the
        // size of the matrix to anything
        //
        // Return Value
        // ------------
        // List<List<int>>                         Returns a completed matrix
        //
        // Value Parameters
        // ----------------
        // int       Number of Processes         The total number of processes the matrix is to build
        // int       Number of Resources         The number of Resources we are expecting
        // string    offset                      This is the offset where the matrix starts
        // string[]  input                       We pass s1.txt here that has been split up by \r\n
        //
        //*******************************************************************
        public List<List<int>> MatrixMaker(int _numOfProcesses, int _numOfResources, int offset ,string[] input)
        {
            List<List<int>> returnMatrix = new List<List<int>>();
            for (int i = 0; i < _numOfProcesses; i++)
            {
                string _process = input[i + offset];
                List<int> listOfProcess = new List<int>();
                
                returnMatrix.Add(VectorBuilder(_process));
            }

            return returnMatrix;
        }

        //********************************************************************
        //
        // Matrix Maker Function
        //
        // This is really similar to the MatrixMaker function except
        // that it performs math operations and returns the result
        //
        // Return Value
        // ------------
        // List<List<int>>                         Returns a matrix that has math work down
        //
        // Value Parameters
        // ----------------
        //  List<List<int>>     Allocation          The Allocation matrix
        //  List<List<int>>     Max                 The Max matrix
        //
        //*******************************************************************
        public List<List<int>> NeedMatrixCalc(List<List<int>> Allocation, List<List<int>> Max)
        {
            List<List<int>> returnMatrix = new List<List<int>>();
            for (int i = 0; i < Allocation.Count; i++)
            {
                returnMatrix.Add(new List<int>());
                for (int j = 0;j < Allocation[i].Count; j++)
                {
                    returnMatrix[i].Add(Max[i][j] - Allocation[i][j]);
                }
            }
            return returnMatrix;
        }
        
        //I was using this chunk of code quite often so I seperated it into its own function
        public List<int> VectorBuilder(string input)
        {
            List<int> returnMatrix = new List<int>();

            string[] _resources = input.Split(" ", StringSplitOptions.None);
            for (int j = 0; j < _resources.Length; j++)
            {
                returnMatrix.Add(Int32.Parse(_resources[j]));
            }
            return returnMatrix;
        }
        
        //here I simply perform a math operation on the given vectors
        public List<int> VectorSubtract(List<int> MainVector, List<int> SubtractVector) 
        {
            if (MainVector.Count != SubtractVector.Count)
            {
                throw new Exception("Vector Length not the same, cannot perform math operation");
            }
            List<int> returnMatrix = new List<int>();
            for (int i = 0; i < MainVector.Count; i++)
            {
                returnMatrix.Add(MainVector[i] - SubtractVector[i]);
            }
            return returnMatrix;
        }
        //Here I am checking if the system is safe with the current available resources
        public bool IsSystemSafe(List<List<int>> Allocation, List<List<int>> Need, List<int> Available)
        {
            int numProcesses = Allocation.Count;
            int numResources = Available.Count;

            List<int> work = new List<int>(Available);
            bool[] finish = new bool[numProcesses];
            bool progressMade = true;

            while (progressMade)
            {
                progressMade = false;
                for (int i = 0; i < numProcesses; i++)
                {
                    if (!finish[i])
                    {
                        bool canFinish = true;
                        for (int j = 0; j < numResources; j++)
                        {
                            if (Need[i][j] > work[j])
                            {
                                canFinish = false;
                            }
                        }

                        if (canFinish)
                        {
                            for (int j = 0; j < numResources; j++)
                            {
                                work[j] += Allocation[i][j];
                            }
                            finish[i] = true;
                            progressMade = true;
                        }
                    }
                }
            } 

            return finish.All(f => f);
        }

        //here I am checking if the request is greater then need, then if Request is greater then available, if either of those are true then we know
        //that we can't grant the request and return a false immediately, we rebuild the available matrix as if the request was granted, and check if the system is safe
        public bool TryGrantRequest(List<List<int>> Allocation, List<List<int>> Max, List<List<int>> Need, List<int> Available, int processIndex, List<int> Request)
        {
            for (int i = 0; i < Request.Count; i++)
            {
                if (Request[i] > Need[processIndex][i])
                {
                    return false;
                }
            }

            for (int i = 0; i < Request.Count; i++)
            {
                if (Request[i] > Available[i])
                {
                    return false;
                }
            }

            List<int> newAvailable = VectorSubtract(Available, Request);
            List<List<int>> newAllocation = Allocation.Select(row => new List<int>(row)).ToList();
            List<List<int>> newNeed = Need.Select(row => new List<int>(row)).ToList();

            for (int i = 0; i < Request.Count; i++)
            {
                newAllocation[processIndex][i] += Request[i];
                newNeed[processIndex][i] -= Request[i];
            }

            return IsSystemSafe(newAllocation, newNeed, newAvailable);
        }

    }
}
