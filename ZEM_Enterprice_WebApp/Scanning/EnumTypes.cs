using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZEM_Enterprice_WebApp.Scanning
{
    /// <summary>
    /// Header type to control whether scan have been completed successfully 
    /// </summary>
    public enum HeaderTypes
    {
        basic = 0,
        error = 1
    };

    /// <summary>
    /// Types of errors and notifications scanning might return
    /// </summary>
    public enum FlagType
    {
        basic = -1,
        ping = 0,
        endConnection = 1,
        notInTech = 100,
        notInDeclared = 101,
        isKanban = 102,
        isDeleted = 103,
        quantityIncorrect = 110,
        quantityOverLimit = 111,
        quantityOverDeclated = 112,
        codeExists = 200,
        codeExistsBack = 201,
        codeFitsBack = 300,
        binNotFound = 400,
        nonScanned = 410,
    };
}
