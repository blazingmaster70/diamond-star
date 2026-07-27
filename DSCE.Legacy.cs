using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace DiamondStar
{
static class LegacyProgram
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Editor());
    }
}

class Editor : Form
{
    CodeEditor code;
    TextBox output;
    Label state;

    const string Sample =
    "box [\r\n" +
    "  ! Your first DiamondStar program\r\n" +
    "  say \"Welcome to DiamondStar!\"\r\n" +
    "  make name = \"friend\"\r\n" +
    "  ask for name \"What is your name?\"\r\n" +
    "  say \"Hello, {name}!\"\r\n" +
    "]\r\n";

    public Editor()
    {
        Text="DSCE — Diamond Star Code Editor";
        Width=1100;
        Height=740;
        MinimumSize=new Size(760,520);
        BackColor=Color.FromArgb(27,30,42);

        Icon=DsLogo.MakeIcon();

        code=new CodeEditor();
        code.Text=Sample;

        output=TextBox(true);
        output.BackColor=Color.FromArgb(20,24,35);
        output.ForeColor=Color.FromArgb(220,236,255);

        FlowLayoutPanel bar=new FlowLayoutPanel();
        bar.Dock=DockStyle.Top;
        bar.Height=48;
        bar.Padding=new Padding(8);
        bar.BackColor=Color.FromArgb(42,47,66);

        bar.Controls.Add(new DsLogo());

        AddButton(bar,"▶ Run",delegate { Run(); });
        AddButton(bar,"Clear output",delegate { output.Clear(); });
        AddButton(bar,"Load example",delegate { code.Text=Sample; });
        AddButton(bar,"Language guide",delegate { Guide(); });

        state=new Label();
        state.AutoSize=true;
        state.Text="Ready";
        state.ForeColor=Color.White;
        state.Padding=new Padding(10,7,10,7);

        bar.Controls.Add(state);

        Panel editorPanel=new Panel();
        editorPanel.Dock=DockStyle.Fill;
        editorPanel.BackColor=Color.FromArgb(20,24,35);

        LineGutter gutter=new LineGutter(code);

        editorPanel.Controls.Add(code);
        editorPanel.Controls.Add(gutter);

        SplitContainer split=new SplitContainer();
        split.Dock=DockStyle.Fill;
        split.Orientation=Orientation.Horizontal;
        split.SplitterDistance=380;

        split.Panel1.Controls.Add(Section("Program",editorPanel));
        split.Panel2.Controls.Add(Section("Output",output));

        Controls.Add(split);
        Controls.Add(bar);
    }

    TextBox TextBox(bool readOnly)
    {
        TextBox x=new TextBox();
        x.Multiline=true;
        x.AcceptsTab=!readOnly;
        x.ReadOnly=readOnly;
        x.ScrollBars=ScrollBars.Both;
        x.WordWrap=readOnly;
        x.Font=new Font("Consolas",readOnly?11:12);
        x.Dock=DockStyle.Fill;
        return x;
    }

    void AddButton(FlowLayoutPanel p,string text,EventHandler h)
    {
        Button b=new Button();
        b.Text=text;
        b.AutoSize=true;
        b.FlatStyle=FlatStyle.Flat;
        b.BackColor=Color.FromArgb(67,81,137);
        b.ForeColor=Color.White;
        b.Margin=new Padding(4,2,4,2);
        b.Padding=new Padding(8,3,8,3);
        b.Click+=h;
        p.Controls.Add(b);
    }

        Control Section(string title,Control content)
    {
        Panel p=new Panel();
        p.Dock=DockStyle.Fill;
        p.Padding=new Padding(8);
        p.BackColor=Color.FromArgb(27,30,42);

        Label l=new Label();
        l.Text=title;
        l.Dock=DockStyle.Top;
        l.Height=25;
        l.ForeColor=Color.FromArgb(180,196,230);
        l.Font=new Font("Segoe UI",9,FontStyle.Bold);

        p.Controls.Add(content);
        p.Controls.Add(l);

        return p;
    }

    void Run()
    {
        output.Clear();
        state.Text="Running…";

        try
        {
            new Runner(Write,Ask).Run(code.Text);
            state.Text="Finished";
        }
        catch(Exception ex)
        {
            Write("✦ Error: "+ex.Message);
            state.Text="Stopped";
        }
    }

    void Write(string s)
    {
        output.AppendText(s+Environment.NewLine);
    }

    string Ask(string prompt)
    {
        Form f=new Form();
        f.Text="DiamondStar input";
        f.Width=440;
        f.Height=170;
        f.StartPosition=FormStartPosition.CenterParent;
        f.FormBorderStyle=FormBorderStyle.FixedDialog;
        f.MaximizeBox=false;
        f.MinimizeBox=false;

        Label l=new Label();
        l.Text=prompt;
        l.SetBounds(15,15,390,22);

        TextBox input=new TextBox();
        input.SetBounds(15,43,390,25);

        Button ok=new Button();
        ok.Text="OK";
        ok.DialogResult=DialogResult.OK;
        ok.SetBounds(330,78,75,28);

        f.AcceptButton=ok;

        f.Controls.Add(l);
        f.Controls.Add(input);
        f.Controls.Add(ok);

        string r=f.ShowDialog(this)==DialogResult.OK?input.Text:"";

        f.Dispose();

        return r;
    }

    void Guide()
    {
        MessageBox.Show(
        "Every program must keep all code inside box [ and ].\n\n" +
        "say \"text\" displays text\n" +
        "make score = 0 creates a variable\n" +
        "ask for name \"Prompt\" asks for input\n" +
        "for name displays a variable\n" +
        "add/remove changes numbers\n" +
        "if: starts a condition\n" +
        "try: true branch\n" +
        "or: false branch\n" +
        "then: runs after try/or\n" +
        "repeat [5] repeats code\n" +
        "loop [] repeats until stop\n" +
        "wait pauses\n\n" +
        "Comments begin with !.",
        "DiamondStar language guide",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
    }
}

class CodeEditor : RichTextBox
{
    bool coloring;

    public CodeEditor()
    {
        Dock=DockStyle.Fill;
        BorderStyle=BorderStyle.None;
        BackColor=Color.FromArgb(20,24,35);
        ForeColor=Color.FromArgb(225,232,245);
        Font=new Font("Consolas",12);
        WordWrap=false;
        AcceptsTab=true;
        ScrollBars=RichTextBoxScrollBars.ForcedBoth;

        TextChanged+=delegate { ColorCode(); };
        VScroll+=delegate { if(Parent!=null)Parent.Invalidate(); };
        KeyDown+=Indent;
    }

    void Indent(object sender,KeyEventArgs e)
    {
        if(e.KeyCode!=Keys.Enter)
            return;

        int start=SelectionStart;
        int line=GetLineFromCharIndex(start);
        string previous=Lines[line];

        Match m=Regex.Match(previous,"^\\s*");

        BeginInvoke((MethodInvoker)delegate
        {
            SelectedText=m.Value;
        });
    }

    void ColorCode()
    {
        if(coloring)
            return;

        coloring=true;

        int pos=SelectionStart;
        int len=SelectionLength;

        SuspendLayout();

        SelectAll();
        SelectionColor=Color.FromArgb(225,232,245);

        foreach(Match m in Regex.Matches(Text,"!.*$",RegexOptions.Multiline))
            ColorMatch(m,Color.FromArgb(110,160,120));

        foreach(Match m in Regex.Matches(Text,"\"[^\"\\r\\n]*\""))
            ColorMatch(m,Color.FromArgb(224,181,104));

        foreach(Match m in Regex.Matches(
            Text,
            "\\b(box|say|make|ask|for|add|remove|is|checks|if|try|or|then|repeat|loop|stop|wait)\\b",
            RegexOptions.IgnoreCase))
            ColorMatch(m,Color.FromArgb(100,181,246));

        foreach(Match m in Regex.Matches(Text,"\\b\\d+(\\.\\d+)?\\b"))
            ColorMatch(m,Color.FromArgb(196,145,232));

        Select(pos,len);

        ResumeLayout();

        coloring=false;
    }

    void ColorMatch(Match m,Color c)
    {
        Select(m.Index,m.Length);
        SelectionColor=c;
    }
}

class LineGutter : Control
{
    CodeEditor editor;

    public LineGutter(CodeEditor e)
    {
        editor=e;
        Dock=DockStyle.Left;
        Width=45;
        BackColor=Color.FromArgb(31,36,52);

        editor.TextChanged+=delegate { Invalidate(); };
        editor.VScroll+=delegate { Invalidate(); };
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.TextRenderingHint=
        System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        using(Brush b=new SolidBrush(Color.FromArgb(120,145,180)))
        using(StringFormat f=new StringFormat())
        {
            f.Alignment=StringAlignment.Far;

            for(int line=0;line<editor.Lines.Length;line++)
            {
                int index=editor.GetFirstCharIndexFromLine(line);
                Point p=editor.GetPositionFromCharIndex(index);

                if(p.Y>=-20&&p.Y<Height)
                {
                    e.Graphics.DrawString(
                    (line+1).ToString(),
                    editor.Font,
                    b,
                    new RectangleF(0,p.Y,Width-7,editor.Font.Height),
                    f);
                }
            }
        }
    }
}

class DsLogo : Control
{
    public DsLogo()
    {
        Size=new Size(32,32);
        Margin=new Padding(0,0,8,0);
        BackColor=Color.FromArgb(28,105,224);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        using(Font f=new Font("Arial",15,FontStyle.Bold,GraphicsUnit.Pixel))
        using(Brush b=new SolidBrush(Color.White))
        {
            SizeF s=e.Graphics.MeasureString("DS",f);
            e.Graphics.DrawString(
            "DS",
            f,
            b,
            (Width-s.Width)/2,
            (Height-s.Height)/2-1);
        }
    }

    public static Icon MakeIcon()
    {
        Bitmap b=new Bitmap(32,32);

        using(Graphics g=Graphics.FromImage(b))
        {
            g.Clear(Color.FromArgb(28,105,224));

            using(Font f=new Font("Arial",16,FontStyle.Bold,GraphicsUnit.Pixel))
            using(Brush brush=new SolidBrush(Color.White))
            {
                SizeF s=g.MeasureString("DS",f);

                g.DrawString(
                "DS",
                f,
                brush,
                (32-s.Width)/2,
                (32-s.Height)/2-1);
            }
        }

        return Icon.FromHandle(b.GetHicon());
    }
}

class Runner
{
    Action write;
    Func<string,string> ask;

    Dictionary<string,object> vars=
    new Dictionary<string,object>(StringComparer.OrdinalIgnoreCase);

    List<string> lines=new List<string>();

    bool stop;

    public Runner(Action w,Func<string,string> a)
    {
        write=w;
        ask=a;
    }

    public void Run(string source)
    {
        foreach(string raw in source.Replace("\r","").Split('\n'))
        {
            string s=raw.Trim();

            if(s.Length>0&&!s.StartsWith("!"))
                lines.Add(s);
        }

        if(lines.Count<2||
           !Eq(lines[0],"box [")||
           lines[lines.Count-1]!="]")
            throw new Exception("All program code must be inside box [ and ].");

        Exec(1,lines.Count-1);
    }

    void Exec(int first,int last)
    {
        for(int i=first;i<last&&!stop;i++)
        {
            string s=lines[i];

            if(Starts(s,"if:"))
            {
                i=If(i,last);
                continue;
            }

            if(Starts(s,"repeat [")||Starts(s,"loop ["))
            {
                i=Loop(i,last);
                continue;
            }

            Do(s);
        }
    }

    int If(int at,int end)
    {
        int tr=Find(at+1,end,"try:");
        int o=Find(at+1,end,"or:");
        int th=Find(at+1,end,"then:");

        if(tr<0||th<0)
            throw new Exception("if: needs try: and then:.");

        if(Condition(lines[at].Substring(3).Trim()))
            Exec(tr+1,o>=0?o:th);
        else if(o>=0)
            Exec(o+1,th);

        Exec(th+1,end);

        return end;
    }

    int Loop(int at,int end)
    {
        int th=Find(at+1,end,"then:");

        if(th<0)
            throw new Exception("A repeat or loop needs then:.");

        stop=false;

        if(Starts(lines[at],"repeat"))
        {
            int n=(int)Num(Brackets(lines[at]));

            for(int i=0;i<n&&!stop;i++)
                Exec(at+1,th);
        }
        else
        {
            int safety=0;

            while(!stop)
            {
                if(++safety>100000)
                    throw new Exception("Loop safety limit reached. Use stop.");

                Exec(at+1,th);
            }
        }

        stop=false;

        return th;
    }

    int Find(int a,int b,string wanted)
    {
        for(int i=a;i<b;i++)
            if(Eq(lines[i],wanted))
                return i;

        return -1;
    }

    void Do(string s)
    {
        if(Eq(s,"stop"))
        {
            stop=true;
            return;
        }

        if(Starts(s,"say "))
        {
            write(Text(s.Substring(4)));
            return;
        }

        if(Starts(s,"for "))
        {
            write(Val(s.Substring(4)).ToString());
            return;
        }

        Match m;

        if(Starts(s,"make "))
        {
            m=Regex.Match(s.Substring(5),@"^(\w+)\s*=\s*(.+)$");

            if(!m.Success)
                throw new Exception("Use make name = value.");

            vars[m.Groups[1].Value]=Val(m.Groups[2].Value);
            return;
        }

        if(Starts(s,"ask for "))
        {
            m=Regex.Match(s.Substring(8),@"^(\w+)\s+(.+)$");

            if(!m.Success)
                throw new Exception("Use ask for name \"Prompt\".");

            vars[m.Groups[1].Value]=ask(Text(m.Groups[2].Value));
            return;
        }

        if(Starts(s,"add ")||Starts(s,"remove "))
        {
            string[] p=s.Split(new char[]{' '},3);

            if(p.Length!=3||!vars.ContainsKey(p[1]))
                throw new Exception("Use add/remove variable amount.");

            vars[p[1]]=Num(vars[p[1]])+
            (Starts(s,"add ")?Num(p[2]):-Num(p[2]));

            return;
        }

        if(Starts(s,"wait "))
        {
            Thread.Sleep((int)(Num(s.Substring(5))*1000));
            return;
        }

        throw new Exception("I don't recognize: "+s);
    }

    bool Condition(string s)
    {
        Match c=Regex.Match(
        s,
        @"^checks if\s+(\w+)\s+(.+)$",
        RegexOptions.IgnoreCase);

        if(c.Success)
            return Val(c.Groups[1].Value)
            .ToString()
            .IndexOf(
            Text(c.Groups[2].Value),
            StringComparison.OrdinalIgnoreCase)>=0;

        Match e=Regex.Match(
        s,
        @"^(.+?)\s+is\s+(.+)$",
        RegexOptions.IgnoreCase);

        if(!e.Success)
            throw new Exception("Use if: value is value, or checks if variable value.");

        return Object.Equals(
        Val(e.Groups[1].Value),
        Val(e.Groups[2].Value));
    }

    object Val(string raw)
    {
        raw=raw.Trim();

        object x;

        if(vars.TryGetValue(raw,out x))
            return x;

        double d;

        if(double.TryParse(
        raw,
        NumberStyles.Float,
        CultureInfo.InvariantCulture,
        out d))
            return d;

        return Text(raw);
    }

    double Num(object o)
    {
        double n;

        if(Double.TryParse(
        o.ToString(),
        NumberStyles.Float,
        CultureInfo.InvariantCulture,
        out n))
            return n;

        throw new Exception("Expected a number, got "+o+".");
    }

    string Text(string s)
    {
        s=s.Trim();

        if(s.Length>=2&&s[0]=='"'&&s[s.Length-1]=='"')
            s=s.Substring(1,s.Length-2);

        return Regex.Replace(
        s,
        @"\{(\w+)\}",
        delegate(Match m)
        {
            object v;

            return vars.TryGetValue(
            m.Groups[1].Value,
            out v)
            ?v.ToString()
            :"";
        });
    }

    static string Brackets(string s)
    {
        int a=s.IndexOf('[');
        int b=s.LastIndexOf(']');

        if(a<0||b<=a)
            throw new Exception("Use brackets, like repeat [3].");

        return s.Substring(a+1,b-a-1);
    }

    static bool Starts(string a,string b)
    {
        return a.StartsWith(b,StringComparison.OrdinalIgnoreCase);
    }

    static bool Eq(string a,string b)
    {
        return String.Equals(a,b,StringComparison.OrdinalIgnoreCase);
    }
}
}
