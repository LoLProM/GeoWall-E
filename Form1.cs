using System.Numerics;
using System.Runtime.InteropServices;
using GSharpProject;
using GSharpProject.Evaluator;
using GSharpProject.Parsing;

namespace GEOWALL_E
{


    public partial class GEOWALL_E : Form
    {

        public static Graphics Papel { get; set; }

        public GEOWALL_E()
        {
            InitializeComponent();
            //quitar bordes
            this.Text = string.Empty;
            this.ControlBox = false;
            //this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            Papel = PANEL_DIBUJO.CreateGraphics();
        }

        //permitir movimiento de la interfaz
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void PANEL_COMANDOS_TextChanged(object sender, EventArgs e)
        {

        }

        private void PANEL_DIBUJO_Click(object sender, EventArgs e)
        {

        }

        private void DRAW_Click(object sender, EventArgs h)
        {

            Papel.Clear(BackColor); Papel.Clear(ForeColor);


            string Entrada = PANEL_COMANDOS.Text;
            if (Entrada != string.Empty)
            {
                try
                {

                    StandardLibrary.Initialize();
                    WallEColors.InitializeColor();
                    var statements = StatementsTree.Create(Entrada);
                    TypeChecker.CheckType(statements);


                    GSharpEvaluator evaluator = new(statements);
                    var lista = evaluator.Evaluate();
                    if (lista is not null)
                    {


                        Draw(lista);
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

        }
        public static void Draw(List<(object, Color, string)> escenes)
        {
            foreach (var escene in escenes)
            {
                switch (escene.Item1)
                {
                    case GSharpProject.Point:
                        DrawGSharpPoint((GSharpProject.Point)escene.Item1, escene.Item3, escene.Item2);
                        break;
                    case Circle:
                        DrawGSharpCircle((Circle)escene.Item1, escene.Item3, escene.Item2);
                        break;
                    case Arc:
                        DrawGSharpArc((Arc)escene.Item1, escene.Item3, escene.Item2);
                        break;
                    case Ray:
                        DrawGSharpRay((Ray)escene.Item1, escene.Item3, escene.Item2);
                        break;
                    case Segment:
                        DrawGSharpSegment((Segment)escene.Item1, escene.Item3, escene.Item2);
                        break;
                    case Line:
                        DrawGSharpLine((Line)escene.Item1, escene.Item3, escene.Item2);
                        break;
                    case LiteralSequence:
                        {
                            var sequence = ((LiteralSequence)escene.Item1).Elements;
                            foreach (var element in sequence)
                            {
                                Draw(new List<(object, Color, string)>() { (element, escene.Item2, escene.Item3) });
                            }

                        }
                        break;
                    case IEnumerator<GSharpProject.Point>:
                        {
                            var sample = (IEnumerator<GSharpProject.Point>)escene.Item1;
                            int contador = 0;
                            while (sample.MoveNext() && contador < 1000)
                            {
                                Draw(new List<(object, Color, string)>() { (sample.Current, escene.Item2, escene.Item3) });
                                contador++;
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private static void DrawGSharpLine(Line a, string Message, Color color)
        {
            Pen lapiz = new Pen(color, 2);
            GEOWALL_E.Papel.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var startPoint = new Vector2((float)a.StartPoint.X, (float)a.StartPoint.Y);
            var endPoint = new Vector2((float)a.EndPoint.X, (float)a.EndPoint.Y);

            Vector2 vectorDirector = Vector2.Normalize((endPoint - startPoint));
            Vector2 start = startPoint - vectorDirector * 100000;
            Vector2 end = endPoint + vectorDirector * 100000;
            GEOWALL_E.Papel.DrawLine(lapiz, start.X, start.Y, end.X, end.Y);

            Draw_Target((int)start.X, (int)start.Y, Message);


        }
        private static void DrawGSharpRay(Ray a, string Message, Color color)
        {
            Pen lapiz = new Pen(color, 2);
            GEOWALL_E.Papel.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var startPoint = new Vector2((float)a.StartPoint.X, (float)a.StartPoint.Y);
            var endPoint = new Vector2((float)a.EndPoint.X, (float)a.EndPoint.Y);

            Vector2 vectorDirector = Vector2.Normalize((endPoint - startPoint));
            Vector2 end = endPoint + vectorDirector * 100000;

            GEOWALL_E.Papel.DrawLine(lapiz, startPoint.X, startPoint.Y, end.X, end.Y);
            Draw_Target((int)startPoint.X, (int)startPoint.Y, Message);

        }
        private static void DrawGSharpCircle(Circle a, string Message, Color color)
        {
            Pen lapiz = new Pen(color, 2);
            GEOWALL_E.Papel.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var center = new Vector2((float)a.Center.X, (float)a.Center.Y);
            var measure = (int)Math.Floor(a.Radius);
            GEOWALL_E.Papel.DrawEllipse(lapiz, center.X - measure, center.Y - measure, measure * 2, measure * 2);
            Draw_Target((int)center.X, (int)center.Y, Message);

        }
        private static void DrawGSharpPoint(GSharpProject.Point a, string Message, Color color)
        {
            Brush brush = new SolidBrush(color);
            GEOWALL_E.Papel.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var p1 = new Vector2((float)a.X, (float)a.Y);

            GEOWALL_E.Papel.FillEllipse(brush, p1.X - 2, p1.Y - 2, 4, 4);
            if (Message != "")
                Draw_Target((int)p1.X + 1, (int)p1.Y + 1, Message);
        }
        private static void DrawGSharpSegment(Segment a, string Message, Color color)
        {
            Pen lapiz = new Pen(color, 2);
            GEOWALL_E.Papel.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var start = new Vector2((float)a.StartPoint.X, (float)a.StartPoint.Y);
            var end = new Vector2((float)a.EndPoint.X, (float)a.EndPoint.Y);
            GEOWALL_E.Papel.DrawLine(lapiz, start.X, start.Y, end.X, end.Y);
            Draw_Target((int)start.X, (int)start.Y, Message);

        }


        private static void DrawGSharpArc(Arc arc, string Message, Color color)
        {
            Pen lapiz = new Pen(color, 2);
            GEOWALL_E.Papel.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int diameter = (int)(arc.Radius * 2);

            Rectangle rectangle = new Rectangle((int)(arc.Center.X - arc.Radius), (int)(arc.Center.Y - arc.Radius), diameter, diameter);
            float start = (float)Math.Atan2(arc.StartRay.Y, arc.StartRay.X);

            float end = (float)Math.Atan2(arc.EndRay.Y, arc.EndRay.X);
            if (start < 0)
                start += 2 * MathF.PI;
            if (end < 0)
                start += 2 * MathF.PI;

            if (start <= end)
                GEOWALL_E.Papel.DrawArc(lapiz, rectangle, ToDegree(start), ToDegree(end - start));

            else
            {
                GEOWALL_E.Papel.DrawArc(lapiz, rectangle, 0, ToDegree(end));
                GEOWALL_E.Papel.DrawArc(lapiz, rectangle, ToDegree(start), 360 - ToDegree(start));
            }
            Draw_Target((int)arc.Center.X, (int)arc.Center.Y, Message);

        }

        public static float ToDegree(float swipangle)
        {
            return swipangle * (180 / MathF.PI);
        }

        public static float GetSweepAngle(float startAngle, float endAngle)
        {
            float SweepAngle = 0;
            if (endAngle > startAngle)
            {
                SweepAngle = endAngle - startAngle;
            }
            return 0;

        }

        public static float GetAngle(GSharpProject.Point center, GSharpProject.Point startRay)
        {
            float angle = (float)Math.Atan2(startRay.Y - center.Y, startRay.X - center.Y);
            return (float)(angle * (180 / Math.PI));
        }

        private static void Draw_Target(int valor_x, int valor_y, string identificador)
        {
            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            System.Drawing.Point location = new System.Drawing.Point(valor_x, valor_y);
            //visualizar etiqueta
            GEOWALL_E.Papel.DrawString(identificador, drawFont, drawBrush, location);
        }

        private static float GetAngle(Vector2 vector2)
        {
            var angle = (float)Math.Atan2(vector2.Y, vector2.X);
            if (angle < 0)
                angle += (float)Math.PI * 2;
            return angle;
        }

        // boton para importar contenido
        private void IMPORT_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                string import = File.ReadAllText(path);
                PANEL_COMANDOS.Text = PANEL_COMANDOS.Text + "\n" + import;
            }
        }

        //Boton para guardar contenido
        private void SAVE_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog1.FileName;
                string code = PANEL_COMANDOS.Text;
                StreamWriter streamWriter = File.CreateText(path);
                streamWriter.Write(code);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }



        private void GEOWALL_E_Load(object sender, EventArgs e)
        {

        }

        private void IMAGEN_WALLE_Click(object sender, EventArgs e)
        {

        }


        //botones para maximizar, cerrar, y minimizar aplicacion

        private void CLOSE_BTN_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void MIN_BTN_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //movimiento de la interfaz
        private void GEOWALL_E_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void MAX_BTN_Click_1(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

    }
}