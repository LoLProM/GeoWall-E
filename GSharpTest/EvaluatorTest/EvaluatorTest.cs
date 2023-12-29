using GSharpProject;
using GSharpProject.Evaluator;
using GSharpProject.Parsing;

namespace GSharpTest.Evaluator;

public class EvaluatorTest
{
    [Theory]
    [InlineData("3;")]
    [InlineData("2+3;")]
    [InlineData("-2;")]
    public void EvaluateLiteralNumberEvalOk(string input)
    {
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        var evaluator = new GSharpEvaluator(statements);
        evaluator.Evaluate();
    }

    [Theory]
    [InlineData("sin(0);")]
    [InlineData("cos(0);")]
    public void EvaluateFunctionsReferenceNumberEvalOk(string input)
    {
        StandardLibrary.Initialize();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        var evaluator = new GSharpEvaluator(statements);
        evaluator.Evaluate();
    }

    [Theory]
    [InlineData("let a = 2; in a;", 2)]
    [InlineData("let a = 2+3 -3; in a;", 2)]
    [InlineData("let a = if let b = 2; in b == --(2*1) then 3 else 1; in 2+(a * 2 )/6;", 3)]
    [InlineData("(let c = 3; in c) + 4;", 7)]
    [InlineData("let a = let b = 2; in b; in a *2;", 2)]
    [InlineData("Q(x) = if x == 1 then 1 else x; let a = if let b = 2; in b == --(2*1) then 3 else 1; in Q(3-2) * 2 /12;", 2)]
    // [InlineData("F(x) = if x == 1 then 1 else x; F({1,2,3}) + 2;", 2)]

    public void EvalLetInIsOk(string input, double expectedResult)
    {
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        var evaluator = new GSharpEvaluator(statements);
        evaluator.Evaluate();
    }

    [Theory]
    [InlineData("a = 2; f(p) = 2+p; f(a);", 4)]
    [InlineData("F(x) = x; F(2);", 2)]
    [InlineData("A(x) = x + 2; A(4);", 6)]
    [InlineData("B(x,y) = x + y; B(2,3);", 5)]
    [InlineData("fact(x) = if x == 1 then 1 else x * fact(x-1);", 2)]
    [InlineData("fact(x) = if x == 1 then 1 else x * 2; fib(x) = if x == 1 | x == 0 then 1 else x-1 + x-2; Sum(x,y) = x + y; fib(7) * fact(3) + Sum(fib(3) + fact(5),fact(2)) + let a = 2; in a * 2;", 2)]
    [InlineData("fact(x) = if x == 1 then 1 else x * fact(x-1); fact(if let a = if 2 < 3 then 2 else 1; in fact(3+a) == fact(5) then 5 else 1);", 2)]

    public void EvalFunctionsIsOk(string input, double expectedResult)
    {
        StandardLibrary.Initialize();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        var evaluator = new GSharpEvaluator(statements);
        evaluator.Evaluate();
    }


    [Theory]
    [InlineData("a,b,_ = {1,2,3}; b;")]
    [InlineData("a,_,b,_ = {1,2,3}; b;")]
    [InlineData("_,b,_ = {1,2,3}; b;")]
    [InlineData("_,b,_ = {1...3}; b;")]
    [InlineData("a = {1...3} + {4,5}; a;")]
    [InlineData("_,b,_ = {1...}; b;")]
    [InlineData("a,b,c,_ = {{1,2},{1,2},{1,2}}; d,_ = c;")]

    public void EvalAssignment(string input)
    {
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        GSharpEvaluator evaluator = new(statements);
        evaluator.Evaluate();
    }

    [Theory]
    // [InlineData("point p; draw p;")]
    // [InlineData("point a; point b; draw circle(a,measure(a,b));")]
    [InlineData("point a; point b; c =  circle(a,measure(a,b));")]
    // [InlineData("ray p;")]
    // [InlineData("arc p;")]
    // [InlineData("circle p;")]
    // [InlineData("line p;")]
    // [InlineData("segment p;")]
    // [InlineData("p = point(2,3);")]
    // [InlineData("point m; point n; p = line(m,n);")]
    // [InlineData("point p; point m; point x; a = circle(p,measure(m,x));")]
    // [InlineData("p = point(2,3); point m; point x; a = circle(p,measure(m,x));")]
    // [InlineData("p = point(2,3); m = point(3,2); point x; a = circle(p,measure(m,x));")]

    public void EvalFiguresOk(string input)
    {
        StandardLibrary.Initialize();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        GSharpEvaluator evaluator = new(statements);
        evaluator.Evaluate();
    }

    [Theory]
    [InlineData("IsMandelBrot(a,b,R,I,m)= if(m<=0) then (if ((a*a)+(b*b)<4) then 1 else 0) else IsMandelBrot(((a*a)-(b*b)+R),(2*a*b+I),R,I,(m-1)); MandelBrot01(x,y,R,I) = if (IsMandelBrot(x,y,R,I,30)) then let draw point(x*100+350,y*100+350); M1=MandelBrot01(x+1,y,R+1,I); in 1 else 0; MandelBrot11(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30))then let draw point(x*100+350,y*100+350);M1=MandelBrot11(x-1,y,R-1,I); in 1 else 0; MandelBrot02(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30)) then let draw point(x*100+350,y*100+350); M1=MandelBrot02(x,y+1,R,I+1); in 1 else 0; MandelBrot22(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30)) then let draw point(x*100+350,y*100+350); M22=MandelBrot22(x,y-1,R,I-1); in 1 else 0; MandelBrot3(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30))then let draw point(x*100+350,y*100+350); M3=MandelBrot3(x,y+1,R,I+1); M01=MandelBrot01(x,y+1,R,I+1); M11=MandelBrot11(x,y+1,R,I+1); in 1 else 0; MandelBrot4(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30)) then let draw point(x*100+350,y*100+350); M4=MandelBrot4  (x,y-1,R,I-1); M01=MandelBrot01(x,y-1,R,I-1); M11=MandelBrot11(x,y-1,R,I-1); in 1 else 0; MandelBrot5(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30)) then let draw point(x*100+350,y*100+350); M5=MandelBrot5  (x+1,y,R+1,I); M02=MandelBrot02(x+1,y,R+1,I); M22=MandelBrot22(x+1,y,R+1,I); in 1 else 0; MandelBrot6(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30)) then let draw point(x*100+350,y*100+350); M1=MandelBrot6 (x-1,y,R-1,I); M2=MandelBrot02(x-1,y,R-1,I); M3=MandelBrot22(x-1,y,R-1,I); in 1 else 0; k01= MandelBrot01(0,0,0,0); k11= MandelBrot11(0,0,0,0); k02= MandelBrot02(0,0,0,0); k22= MandelBrot22(0,0,0,0); k3= MandelBrot3(0,0,0,0); k4= MandelBrot4(0,0,0,0); k5= MandelBrot5(0,0,0,0); k6= MandelBrot6(0,0,0,0);")]

    public void EvalMandelBrot(string input)
    {
        StandardLibrary.Initialize();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        GSharpEvaluator evaluator = new(statements);
        evaluator.Evaluate();
    }

}