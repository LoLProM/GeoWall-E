using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Security.Cryptography;
using GSharpProject;
using GSharpProject.Parsing;

public static class TypeChecker
{
    public static void CheckType(GSharpStatementsCollection statementsTree)
    {
        TypedScope scope = StandardLibrary.VariableTypes;
        statementsTree.CheckType(scope);
    }
}