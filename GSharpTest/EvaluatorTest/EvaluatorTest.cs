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
    [InlineData("2 + 0.02;")]
    [InlineData("0 % 0.024;")]
    [InlineData("3 * 0.3;")]
    [InlineData("4 - 0.01;")]
    [InlineData("0.01 - 4;")]
    [InlineData("0.2 - 4;")]
    [InlineData("0.01 + 4.2;")]
    [InlineData("0.01 * 4;")]


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

    // [Theory]
    // [InlineData("point p; draw p;")]
    // [InlineData("point a; point b; draw circle(a,measure(a,b));")]
    // [InlineData("point a; point b; c =  circle(a,measure(a,b));")]
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

    // public void EvalFiguresOk(string input)
    // {
    //     StandardLibrary.Initialize();
    //     var statements = StatementsTree.Create(input);
    //     TypeChecker.CheckType(statements);
    //     GSharpEvaluator evaluator = new(statements);
    //     evaluator.Evaluate();
    // }

    // // [Theory]
    // // [InlineData("IsMandelBrot(a,b,R,I,m) = if(m<=0) then (if ((a*a)+(b*b)<4) then 1 else 0) else IsMandelBrot(((a*a)-(b*b)+R),(2*a*b+I),R,I,(m-1)); MandelBrot01(x,y,R,I) = if (IsMandelBrot(x,y,R,I,30)) then let draw point(x*100+350,y*100+350);  M1=MandelBrot01(x+0.01,y,R+0.01,I); in 1 else 0;")]

    // // public void EvalMandelBrot2(string input)
    // // {
    // //     StandardLibrary.Initialize();
    // //     var statements = StatementsTree.Create(input);
    // //     TypeChecker.CheckType(statements);
    // //     GSharpEvaluator evaluator = new(statements);
    // //     evaluator.Evaluate();
    // // }

    // [Theory]
    // [InlineData("IsMandelBrot(a,b,R,I,m)= if(m<=0) then (if ((a*a)+(b*b)<4) then 1 else 0) else IsMandelBrot(((a*a)-(b*b)+R),(2*a*b+I),R,I,(m-1)); MandelBrot01(x,y,R,I) = if (IsMandelBrot(x,y,R,I,1)) then let draw point(x*100+350,y*100+350); M1=MandelBrot01(x+0.01,y,R+0.01,I); in 1 else 0; MandelBrot11(x,y,R,I)= if (IsMandelBrot(x,y,R,I,1))then let draw point(x*100+350,y*100+350);M1=MandelBrot11(x-0.01,y,R-0.01,I); in 1 else 0; MandelBrot02(x,y,R,I)= if (IsMandelBrot(x,y,R,I,1)) then let draw point(x*100+350,y*100+350); M1=MandelBrot02(x,y+0.01,R,I+0.01); in 1 else 0; MandelBrot22(x,y,R,I)= if (IsMandelBrot(x,y,R,I,1)) then let draw point(x*100+350,y*100+350); M22=MandelBrot22(x,y-0.01,R,I-0.01); in 1 else 0; MandelBrot3(x,y,R,I)= if (IsMandelBrot(x,y,R,I,1))then let draw point(x*100+350,y*100+350); M3=MandelBrot3(x,y+0.01,R,I+0.01); M01=MandelBrot01(x,y+0.01,R,I+0.01); M11=MandelBrot11(x,y+0.01,R,I+0.01); in 1 else 0; MandelBrot4(x,y,R,I)= if (IsMandelBrot(x,y,R,I,1)) then let draw point(x*100+350,y*100+350); M4=MandelBrot4  (x,y-0.01,R,I-0.01); M01=MandelBrot01(x,y-0.01,R,I-0.01); M11=MandelBrot11(x,y-0.01,R,I-0.01); in 1 else 0; MandelBrot5(x,y,R,I)= if (IsMandelBrot(x,y,R,I,1)) then let draw point(x*100+350,y*100+350); M5=MandelBrot5  (x+0.01,y,R+0.01,I); M02=MandelBrot02(x+0.01,y,R+0.01,I); M22=MandelBrot22(x+0.01,y,R+0.01,I); in 1 else 0; MandelBrot6(x,y,R,I)= if (IsMandelBrot(x,y,R,I,1)) then let draw point(x*100+350,y*100+350); M1=MandelBrot6 (x-0.01,y,R-0.01,I); M2=MandelBrot02(x-0.01,y,R-0.01,I); M3=MandelBrot22(x-0.01,y,R-0.01,I); in 1 else 0; k01= MandelBrot01(0,0,0,0); k11= MandelBrot11(0,0,0,0); k02= MandelBrot02(0,0,0,0); k22= MandelBrot22(0,0,0,0); k3= MandelBrot3(0,0,0,0); k4= MandelBrot4(0,0,0,0); k5= MandelBrot5(0,0,0,0); k6= MandelBrot6(0,0,0,0);")]

    // public void EvalMandelBrot(string input)
    // {
    //     StandardLibrary.Initialize();
    //     var statements = StatementsTree.Create(input);
    //     TypeChecker.CheckType(statements);
    //     GSharpEvaluator evaluator = new(statements);
    //     evaluator.Evaluate();
    // }

    // [Theory]
    // [InlineData(@"
    // IsMandelBrot(a,b,R,I,m) = 
    //     if(m<=0) then 
    //         (if ((a*a)+(b*b)<4) then 1 else 0) 
    //     else 
    //         IsMandelBrot(((a*a)-(b*b)+R),(2*a*b+I),R,I,(m-1));
    
    // MandelBrot11(x,y,R,I) = 
    //     if (IsMandelBrot(x,y,R,I,30)) then 
    //         let draw point(x*100+350,y*100+350);
    //         M1=MandelBrot11(x-0.01,y,R-0.00001,I); 
    //         in 1 
    //     else 
    //         0; 
    
    // k11 = MandelBrot11(0,0,0,0);")]

    // public void EvalMandelBrot3(string input)
    // {
    //     try {
    //     StandardLibrary.Initialize();
    //     var statements = StatementsTree.Create(input);
    //     TypeChecker.CheckType(statements);
    //     GSharpEvaluator evaluator = new(statements);
    //     evaluator.Evaluate();
    //     }
    //     catch (Exception e) {

    //     }
    // }

    // [Theory]
    // [InlineData("IsMandelBrot(a,b,R,I,m)= if(m<=0) then (if ((a*a)+(b*b)<4) then 1 else 0) else IsMandelBrot(((a*a)-(b*b)+R),(2*a*b+I),R,I,(m-1));MandelBrot01(x,y,R,I)= if (IsMandelBrot(x,y,R,I,1))then let draw point(x*100.0+350.0,y*100+350);M1=MandelBrot01(x+0.01,y,R+0.01,I); in 1 else 0; k01= MandelBrot01(0,0,0,0);")]


    // public void EvalMandelBrot22(string input)
    // {
    //     StandardLibrary.Initialize();
    //     var statements = StatementsTree.Create(input);
    //     TypeChecker.CheckType(statements);
    //     GSharpEvaluator evaluator = new(statements);
    //     evaluator.Evaluate();
    // }

    [Theory]
    [InlineData("point a; point b; draw {a,b};")]
    [InlineData("draw point(8.6 +2.4,8);")]
    [InlineData("draw point(11,8);")]

    public void EvalDrawIsOk(string input)
    {
        StandardLibrary.Initialize();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        GSharpEvaluator evaluator = new(statements);
        evaluator.Evaluate();
    }

    [Theory]
    [InlineData("if let a = 2; in a * 2 > 1 then a else 2;")]
    [InlineData("circle c1; circle c2; i1,i2,_ = intersect(c1, c2);")]
    [InlineData("mediatrix(p1, p2) =  let l1 = line(p1, p2); m = measure (p1, p2); c1 = circle (p1, m); c2 = circle (p2, m); i1,i2,_ = intersect(c1, c2); in line(i1,i2); a = mediatrix(point p, point m);")]
    [InlineData("regularTriangle(p,m) = let point p2; l1 = line(p,p2); c1 = circle(p,m); i1,i2,_ = intersect(c1,l1); c2 = circle(i1,m); c3 = circle(i2,m); i3,i4,_ = intersect(c2,c1); i5,i6,_ = intersect(c3,c1); in {i1,i5,i6};  pu1 = point(300,0); pu2 = point(0,0); g = measure(pu1,pu2); b,x,d,_ = regularTriangle(point(300,200),g);")]
    [InlineData("point c1; point c2; i1,i2,_ = intersect(circle(c1,measure(c1,c2)), circle(c2,measure(c1,c2))); draw i1;")]

    public void EvalIfOk(string input)
    {
        StandardLibrary.Initialize();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        GSharpEvaluator evaluator = new(statements);
        evaluator.Evaluate();
    }

    // [Theory]
    // [InlineData("f(x) = if x == 3000 then x else f(x - 1); a = f(33367);",3000)]
    // [InlineData("f(x) = if x <= 0 then x else f(x - 0.0001); a = f(0.0369);",0)]

    // [InlineData("f(x,y,z) = if x <= z then x else f(x+y,y,z); a = f(1,-0.00,0);",0)]

    // public void EvalDouble(string input, int result)
    // {
    //     StandardLibrary.Initialize();
    //     var statements = StatementsTree.Create(input);
    //     TypeChecker.CheckType(statements);
    //     GSharpEvaluator evaluator = new(statements);
    //     evaluator.Evaluate();
    //     var res = StandardLibrary.Variables.GetValue("a");
    //     Assert.True(result >= (double)res);
    // }

    [Theory]
    [InlineData(@"
    mediatrix(p1, p2) = 
    let
        l1 = line(p1, p2);
        m = measure (p1, p2);
        c1 = circle (p1, m);
        c2 = circle (p2, m);
        i1,i2,_ = intersect(c1, c2);
    in line(i1,i2);
    
regularTriangle(p,m) =
    let
        point p2;
        l1 = line(p,p2);
        c1 = circle(p,m);
        i1,i2,_ = intersect(l1,c1);
        c2 = circle(i1,m);
        c3 = circle(i2,m);
        i3,i4,_ = intersect(c2,c1);
        i5,i6,_ = intersect(c3,c1);
    in {i1,i5,i6};  
    
divideTriangle(p1,p2,p3,m1) = if (measure(p1,p2)/m1) < 15 then {} else  
   let
      draw {segment(p1,p2),segment(p2,p3),segment(p3,p1)};
      mid1,_ = intersect(mediatrix(p1,p2),line(p1,p2));
      mid2,_ = intersect(mediatrix(p2,p3),line(p2,p3));
      mid3,_ = intersect(mediatrix(p1,p3),line(p1,p3));
      a = divideTriangle(p2,mid2,mid1,m1);
      b = divideTriangle(p1,mid1,mid3,m1);
      c = divideTriangle(p3,mid2,mid3,m1);
      in {};
      
sierpinskyTriangle(p,m) = 
     let
         pu1 = point(0,0);
         pu2 = point(0,1);
         p1,p2,p3,_ = regularTriangle(p,m);
     in divideTriangle(p1,p2,p3,measure(pu1,pu2));

pu1 = point(300,0);
pu2 = point(0,0);
m = measure(pu1,pu2);

a = sierpinskyTriangle(point(450,300),m);")]

    public void EvalSerpinskiOk(string input)
    {
        StandardLibrary.Initialize();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        GSharpEvaluator evaluator = new(statements);
        evaluator.Evaluate();
    }
}