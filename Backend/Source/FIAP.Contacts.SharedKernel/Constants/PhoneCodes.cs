namespace FIAP.Contacts.SharedKernel.Enumerations
{
    public static class PhoneCodes
    {
        public static IDictionary<StatesEnum, IEnumerable<int>> ValidCodes { get; private set; } = new Dictionary<StatesEnum, IEnumerable<int>>
        {
            { 
                StatesEnum.SP, new List<int> { 11, 12, 13, 14, 15, 16, 17, 18, 19 }            
            },
            {
                StatesEnum.RJ, new List<int> { 21, 22, 24 }
            },
            {
                StatesEnum.ES, new List<int> { 27, 28 }
            },
            {
                StatesEnum.MG, new List<int> { 31, 32, 33, 34, 35, 37, 38 }
            },
            {
                StatesEnum.PR, new List<int> { 41, 42, 43, 44, 45, 46 }
            },
            {
                StatesEnum.SC, new List<int> { 47, 48, 49 }
            },
            {
                StatesEnum.RS, new List<int> { 51, 53, 54, 55 }
            },
            { 
                StatesEnum.DF, new List<int> { 61 }
            },
            {
                StatesEnum.GO, new List<int> { 62, 64 }
            },
            {
                StatesEnum.MT, new List<int> { 65, 66 }
            },
            {
                StatesEnum.MS, new List<int> { 67 }
            },
            {
                StatesEnum.AC, new List<int> { 68 }
            },
            {
                StatesEnum.RO, new List<int> { 69 }
            },
            {
                StatesEnum.BA, new List<int> { 71, 73, 74, 75, 77 }
            },
            {
                StatesEnum.SE, new List<int> { 79 }
            },
            {
                StatesEnum.PE, new List<int> { 81, 87 }
            },
            {
                StatesEnum.AL, new List<int> { 82 }
            },
            {
                StatesEnum.PB, new List<int> { 83 }
            },
            {
                StatesEnum.RN, new List<int> { 84 }
            },
            {
                StatesEnum.CE, new List<int> { 85, 88 }
            },
            {
                StatesEnum.PI, new List<int> { 86, 89 }
            },
            {
                StatesEnum.PA, new List<int> { 91, 93, 94 }
            },
            {
                StatesEnum.AM, new List<int> { 92, 97 }
            },
            {
                StatesEnum.RR, new List<int> { 95 }
            },
            {
                StatesEnum.AP, new List<int> { 96 }
            },
            {
                StatesEnum.MA, new List<int> { 98, 99 }
            },
        };
    }
}
