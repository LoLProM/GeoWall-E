using System.Runtime.InteropServices;
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
        WallEColors.InitializeColor();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        GSharpEvaluator evaluator = new(statements);
        evaluator.Evaluate();
    }

    [Theory]
    // [InlineData("color blue;")]
    // [InlineData("color blue; draw point p; restore(); draw point c;")]
    //    [InlineData("a = point(2,2); point p1;c = circle(a,measure(a,p1)+measure(point(0,0),point(0,0.01)));")]
    // [InlineData("color blue; draw point p;")]
    // [InlineData("draw point sequence s;")]
    // [InlineData("draw circle sequence s;")]
    // [InlineData("draw ray sequence s;")]
    // [InlineData("draw samples();")]
    // [InlineData("line p; draw p; draw points(p);")]
    // [InlineData("line p; draw points(p);")]
    // [InlineData("color blue ;ray p; draw p; color red; draw points(p);")]
    // [InlineData("b(x) = x + x;f(x) = b(x); a,b,c,d,e,f,_ = f({1,2,3});")]
    [InlineData(@"mediatrix(p1, p2) = 
    let
        l1 = line(p1, p2);
        m = measure (p1, p2);
        c1 = circle (p1, m);
        c2 = circle (p2, m);
        i1,i2,_ = intersect(c1, c2);
    in line(i1,i2);

drawTriangle(p1,p2,p3) = 
let
   draw {segment(p1,p2),segment(p2,p3),segment(p3,p1)};
in 1;

getTriangleCenter(p1,p2,p3) = 
let
   a = mediatrix(p1,p2);
   b = mediatrix(p2,p3);
   i1,_ = intersect(a,b);
in i1;

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

getReverseTriangle(p1,p2,p3) =
let
   center = getTriangleCenter(p1,p2,p3);
   c = circle(center,measure(center,p1)+measure(point(0,0),point(0,0.01)));
   i2,_ = intersect(ray(p1,center),c);
   i3,_ = intersect(ray(p2,center),c);
   i1,_ = intersect(ray(p3,center),c);
in {i1,i2,i3}; 

a,b,c,_ = getReverseTriangle(i1 = point(0,1), i2 = point(1,0), i3 = point(-1,0));")]

    public void EvalOk(string input)
    {
        StandardLibrary.Initialize();
        WallEColors.InitializeColor();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        GSharpEvaluator evaluator = new(statements);
        evaluator.Evaluate();
    }

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

drawTriangle(p1,p2,p3) = 
let
   draw {segment(p1,p2),segment(p2,p3),segment(p3,p1)};
in 1;

getTriangleCenter(p1,p2,p3) = 
let
   a = mediatrix(p1,p2);
   b = mediatrix(p2,p3);
   i1,_ = intersect(a,b);
in i1;

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

getReverseTriangle(p1,p2,p3) =
let
   center = getTriangleCenter(p1,p2,p3);
   c = circle(center,measure(center,p1)+measure(point(0,0),point(0,0.01)));
   i2,_ = intersect(ray(p1,center),c);
   i3,_ = intersect(ray(p2,center),c);
   i1,_ = intersect(ray(p3,center),c);
in {i1,i2,i3};

findSubTriangle(pPivo,p2,p3,pl1,pl2) =
let 
   i1,_ = intersect(line(pPivo,p2),line(pl1,pl2));
   i2,_ = intersect(line(pPivo,p3),line(pl1,pl2));
in {pPivo,i1,i2};

KorshSnowFly(p1,p2,p3,cant) =
if cant > 0 then
   let
    x = drawTriangle(p1,p2,p3);
   
     t1,t2,t3,_ = getReverseTriangle(p1,p2,p3);
   
    t11,t12,t13,_ = findSubTriangle(p1,p2,p3,t1,t3);
    t21,t22,t23,_ = findSubTriangle(t1,t2,t3,p1,p2);
    t31,t32,t33,_ = findSubTriangle(p2,p1,p3,t1,t2);
    t41,t42,t43,_ = findSubTriangle(t2,t3,t1,p2,p3);
    t51,t52,t53,_ = findSubTriangle(p3,p1,p2,t2,t3);
    t61,t62,t63,_ = findSubTriangle(t3,t1,t2,p3,p1);
    color red;
    x1 = KorshSnowFly(t11,t12,t13,cant-1);
    color blue;
    x2 = KorshSnowFly(t21,t22,t23,cant-1);
    color yellow;
    x3 = KorshSnowFly(t31,t32,t33,cant-1);
    color green;
    x4 = KorshSnowFly(t41,t42,t43,cant-1);
    color magenta;
    x5 = KorshSnowFly(t51,t52,t53,cant-1);
    color cyan;
    x6 = KorshSnowFly(t61,t62,t63,cant-1);
   in 1
else 1;

i1,i2,i3,_ = regularTriangle(point(250,250),measure(point(0,0),point(0,150)));
k = KorshSnowFly(i1,i2,i3,4);")]

    public void EvalKorshOk(string input)
    {
        StandardLibrary.Initialize();
        WallEColors.InitializeColor();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        GSharpEvaluator evaluator = new(statements);
        evaluator.Evaluate();
    }


    [Theory]
    [InlineData(@"regularHexagon(p,m) =
    let
        point p2;
        l1 = line(p,p2);
        c1 = circle(p,m);
        i1,i2,_ = intersect(l1,c1);
        c2 = circle(i1,m);
        c3 = circle(i2,m);
        i3,i4,_ = intersect(c2,c1);
        i5,i6,_ = intersect(c3,c1);
    in {i1,i3,i5,i2,i6,i4};


mediatrix(p1, p2) = 
    let
        l1 = line(p1, p2);
        m = measure (p1, p2);
        c1 = circle (p1, m);
        c2 = circle (p2, m);
        i1,i2,_ = intersect(c1, c2);
    in line(i1,i2);

hexagonalStar(p,m) =
   let 
       v1,v2,v3,v4,v5,v6,_ = regularHexagon(p,m);
       l1 = mediatrix(v1,v2);
       l2 = mediatrix(v2,v3);
       l3 = mediatrix(v3,v4);
       i1,_ = intersect(l1,line(v3,v4));
       i2,_ = intersect(l1,line(v3,v2));
       i3,_ = intersect(l2,line(v1,v2));
       i4,_ = intersect(l2,line(v1,v6));
       i5,_ = intersect(l3,line(v2,v3));
       i6,_ = intersect(l3,line(v2,v1));
   in {v1,i2,v2,i3,v3,i5,v4,i1,v5,i4,v6,i6};

getSpikes(p1,p2,p3,m) =
      if m / measure(p2,p3) > 80 then {} 
      else let
              l1 = mediatrix(p1,p2);
              l2 = mediatrix(p1,p3);
              i1,_ = intersect(l1,line(p1,p2));
              i2,_ = intersect(l2,line(p1,p3));
              i3,_ = intersect(l1,l2);
              draw {segment(i1,i3), segment(i2,i3),segment(i3,p1)};
              in {i1,i2,i3} + getSpikes(i1,p2,i3,m) + getSpikes(i2,p3,i3,m);
        


drawRecursiveSnowFly(p,m) = 
   let
      p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12,_ = hexagonalStar(p,m);
      b = point(416.7268740928237,446.26311596696246); 
      c = point(449.99999999999943,300.0000000000001); 
      d = point(361.7113685375882,395.2896508257555 );
      m1 = measure(p1,p2);
      m = 149.9999999999995;
      s1 = getSpikes(b,c,d,m);
      s2 = getSpikes(p3,p2,p4,m);
      s3 = getSpikes(p5,p4,p6,m);
      s4 = getSpikes(p7,p6,p8,m);
      s5 = getSpikes(p9,p8,p10,m);
      s6 = getSpikes(p11,p10,p12,m);
      draw 
      {
        segment(p1,p2),segment(p2,p3),segment(p3,p4),segment(p4,p5),
        segment(p5,p6),segment(p6,p7),segment(p7,p8),segment(p8,p9),
        segment(p9,p10),segment(p10,p11),segment(p11,p12),segment(p12,p1),
        segment(p1,p),segment(p2,p),segment(p3,p),segment(p4,p),segment(p5,p),
        segment(p6,p),segment(p7,p),segment(p8,p),segment(p9,p),segment(p10,p),
        segment(p11,p),segment(p12,p)
      };
   in 0;
   
   
   
   
pu1 = point(150,0);
pu2 = point(0,0);
m = measure(pu1,pu2);


a = drawRecursiveSnowFly(point(450,300),m);")]

    public void EvalshOk(string input)
    {
        StandardLibrary.Initialize();
        WallEColors.InitializeColor();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        GSharpEvaluator evaluator = new(statements);
        evaluator.Evaluate();
    }



    [Theory]
    // [InlineData("a = circle(point(2,2), measure(point(2,2),point(2,3)) + measure(point(2,2),point(2,3))) ;")]

//     [InlineData(@"
//     getSpikes(p1,p2,p3,m) =
//       if m / measure(p2,p3) > 80 then {} 
//       else let
//               l1 = mediatrix(p1,p2);
//               l2 = mediatrix(p1,p3);
//               i1,_ = intersect(l1,line(p1,p2));
//               i2,_ = intersect(l2,line(p1,p3));
//               i3,_ = intersect(l1,l2);
//               draw {segment(i1,i3), segment(i2,i3),segment(i3,p1)};
//               in {i1,i2,i3} + getSpikes(i1,p2,i3,m) + getSpikes(i2,p3,i3,m);

//     mediatrix(p1, p2) = 
//     let
//         l1 = line(p1, p2);
//         m = measure (p1, p2);
//         c1 = circle (p1, m);
//         c2 = circle (p2, m);
//         i1,i2,_ = intersect(c1, c2);
//     in line(i1,i2);
        
// b = point(416.7268740928237,446.26311596696246); c = point(449.99999999999943,300.0000000000001); d = point(361.7113685375882,395.2896508257555 ); a = getSpikes(b,c,d,measure(b,c));")]

[InlineData(@"
a(n,m) = 
let 
    n1 = n;
    m1 = m;
    x = if n>0 then a(n-1,m+3) else 1;
in 1;

callA =a(2,3);

circle b;
line c;
point t;
segment e;
point f;
line yj;

draw b;
draw c;
draw t;
draw e;
draw f;
draw yj;


f(u)=
let
    circle b;
    line c;
    point t;
    segment e;
    point f;
    line yg;
    line yj;

    draw b;
    draw c;
    draw t;
    draw e;
    draw f;
    draw yg;
    draw yj;
in if u>0 then f(u-1)else 0;

trtrt= f(3);
")]
    public void EvalsRadiusOk(string input)
    {
        StandardLibrary.Initialize();
        WallEColors.InitializeColor();
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        GSharpEvaluator evaluator = new(statements);
        evaluator.Evaluate();
    }
}