using System;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using NCalc;

public class GraphForm : Form
{
    TextBox input1, input2;
    Button drawButton;
    PlotView plotView;

    public GraphForm()
    {
        this.Text = "lab4";
        this.Width = 800;
        this.Height = 600;

        var label1 = new Label{Left = 10, Top = 10, Width = 100, Text = "Функция 1:"};
        var label2 = new Label{Left = 10, Top = 60, Width = 100, Text = "Функция 2:"};

        input1 = new TextBox { Left = 10, Top = 30, Width = 760, Text = "Sqrt(1 - Pow(Abs(x) - 1, 2)) * 3 + 16" };
        input2 = new TextBox { Left = 10, Top = 80, Width = 760, Text = "Acos(-Abs(x / 2)) * 5" };

        plotView = new PlotView{Top = 150, Left = 10, Width = 760, Height = 400};

        drawButton = new Button{Left = 10, Top = 110, Width = 180, Height = 30, Text = "Нарисовать график"};
        drawButton.Click += DrawButton_Click;

        Controls.Add(label1);
        Controls.Add(label2);
        Controls.Add(input1);
        Controls.Add(input2);
        Controls.Add(drawButton);
        Controls.Add(plotView);
    }
    void DrawButton_Click(object sender, EventArgs e)
    {
        var model = new PlotModel { Title = "Графики функций" };

        AddFunctionToModel(model, input1.Text, OxyColors.Blue, MarkerType.Circle); // Circle, Square, Triangle, Diamond, Cross, Plus, Star
        AddFunctionToModel(model, input2.Text, OxyColors.Red, MarkerType.Diamond);

        model.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom, Title = "X" });
        model.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Left, Title = "Y" });

        plotView.Model = model;
    }

    void AddFunctionToModel(PlotModel model, string formula, OxyColor color, MarkerType marker)
    {
        var series = new LineSeries
        {
            Title = formula,
            Color = color,
            MarkerType = marker,
            MarkerSize = 4,
            MarkerFill = color,
            MarkerStroke = OxyColors.Black,
            MarkerStrokeThickness = 1.5
        };

        for (double x = -5; x <= 5; x += 0.2)
        {
            try
            {
                var expr = new Expression(formula);
                expr.Parameters["x"] = x;
                double y = (double)expr.Evaluate();
                series.Points.Add(new DataPoint(x, y));
            }
            catch {}
        }

        model.Series.Add(series);
    }

    [STAThread]
    public static void Main()
    {
        Application.Run(new GraphForm());
    }
}
