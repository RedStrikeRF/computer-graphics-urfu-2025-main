using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class MainForm : Form
{
    private GroupBox groupBoxControlPlane;
    private GroupBox groupBoxShapeSettings;
    private GroupBox groupBoxDrawing;

    private Button buttonEnableGraphics;
    private Button buttonDrawObject;
    private Button buttonEraseObject;
    private Button buttonClearAll;

    private ComboBox comboBoxShapes;
    private Panel panelDrawing;

    private ListBox listBoxPoints;
    private TextBox textBoxX;
    private TextBox textBoxY;
    private Button buttonAddPoint;

    private Line currentLine;
    private Ellipse currentEllipse;
    private Polygon currentPolygon;
    private Polygon currentCustomPolygon;

    public MainForm()
    {
        Text = "Lab 1";
        ClientSize = new Size(800, 600);

        // Группа Управление 
        groupBoxControlPlane = new GroupBox();
        groupBoxControlPlane.Text = "Управление";
        groupBoxControlPlane.SetBounds(10, 10, 200, 230);

        // для выбора типа фигуры
        comboBoxShapes = new ComboBox();
        comboBoxShapes.Items.AddRange(new string[] { "Линия", "Эллипс", "Правильный многоугольник", "Произвольный многоугольник" });
        comboBoxShapes.SelectedIndex = 0;
        comboBoxShapes.SetBounds(10, 20, 180, 30);
        comboBoxShapes.SelectedIndexChanged += ComboBoxShapes_SelectedIndexChanged;

        buttonEnableGraphics = Core.CreateButton("Включить графику", 10, 60,  180, 30, ButtonEnableGraphics_Click);
        buttonDrawObject     = Core.CreateButton("Рисовать объект",  10, 100, 180, 30, ButtonDraw_Click);
        buttonEraseObject    = Core.CreateButton("Стереть объект",   10, 140, 180, 30, ButtonErase_Click);
        buttonClearAll       = Core.CreateButton("Очистить все",     10, 180, 180, 30, ButtonClearAll_Click);

        groupBoxControlPlane.Controls.Add(comboBoxShapes);
        groupBoxControlPlane.Controls.Add(buttonEnableGraphics);
        groupBoxControlPlane.Controls.Add(buttonDrawObject);
        groupBoxControlPlane.Controls.Add(buttonEraseObject);
        groupBoxControlPlane.Controls.Add(buttonClearAll);

        // Группа Картинка
        groupBoxDrawing = new GroupBox();
        groupBoxDrawing.Text = "Картинка";
        groupBoxDrawing.SetBounds(220, 10, 560, 500);

        panelDrawing = new Panel();
        panelDrawing.BorderStyle = BorderStyle.FixedSingle;
        panelDrawing.SetBounds(10, 20, 540, 470);
        panelDrawing.BackColor = Color.White;
        groupBoxDrawing.Controls.Add(panelDrawing);

        // Группа кастомные настройки
        groupBoxShapeSettings = new GroupBox();
        groupBoxShapeSettings.Text = "Нет настроек";
        groupBoxShapeSettings.SetBounds(10, 250, 200, 260);

        // Все группы на форму
        Controls.Add(groupBoxControlPlane);
        Controls.Add(groupBoxShapeSettings);
        Controls.Add(groupBoxDrawing);
    }

    private void ButtonEnableGraphics_Click(object sender, EventArgs e)
    {

    }

    private void ComboBoxShapes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Controls.Contains(groupBoxShapeSettings))
            Controls.Remove(groupBoxShapeSettings);

        var selectedShape = comboBoxShapes.SelectedItem.ToString();

        if (selectedShape == "Произвольный многоугольник")
        {
            groupBoxShapeSettings = new GroupBox();
            groupBoxShapeSettings.Text = "Настройки произвольного многоугольника";

            textBoxX = new TextBox();
            textBoxX.SetBounds(10, 20, 80, 25);

            textBoxY = new TextBox();
            textBoxY.SetBounds(100, 20, 80, 25);

            buttonAddPoint = Core.CreateButton("Добавить точку", 10, 55, 170, 30, ButtonAddPoint_Click);

            listBoxPoints = new ListBox();
            listBoxPoints.SetBounds(10, 95, 170, 150);

            groupBoxShapeSettings.Controls.Add(textBoxX);
            groupBoxShapeSettings.Controls.Add(textBoxY);
            groupBoxShapeSettings.Controls.Add(buttonAddPoint);
            groupBoxShapeSettings.Controls.Add(listBoxPoints);
        }
        else
        {
            groupBoxShapeSettings = new GroupBox();
            groupBoxShapeSettings.Text = "Нет настроек";
        }

        groupBoxShapeSettings.SetBounds(10, 250, 200, 260);
        Controls.Add(groupBoxShapeSettings);
    }

    private void ButtonAddPoint_Click(object sender, EventArgs e)
    {
        if (int.TryParse(textBoxX.Text, out int x) && int.TryParse(textBoxY.Text, out int y))
        {
            listBoxPoints.Items.Add($"{x} {y}");
            textBoxX.Clear();
            textBoxY.Clear();
        }
        else
        {
            MessageBox.Show("Введите корректные координаты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void ButtonDraw_Click(object sender, EventArgs e)
    {
        using (Graphics graphics = panelDrawing.CreateGraphics())
        {
            string selected = comboBoxShapes.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selected))
                return;

            switch (selected)
            {
                case "Линия":
                    DrawLine(graphics);
                    break;

                case "Эллипс":
                    DrawEllipse(graphics);
                    break;

                case "Правильный многоугольник":
                    DrawRegularPolygon(graphics);
                    break;

                case "Произвольный многоугольник":
                    DrawCustomPolygon(graphics);
                    break;
            }
        }
    }

    private void DrawLine(Graphics g)
    {
        currentLine = new Line(
            new Point(50, 50),
            new Point(200, 200),
            3,
            Color.Black,
            Color.White
        );
        currentLine.Draw(g);
    }

    private void DrawEllipse(Graphics g)
    {
        currentEllipse = new Ellipse(
            new Point(150, 150),
            80,
            50,
            Color.Red,
            Color.Yellow,
            panelDrawing.BackColor,
            "Элипс",
            Color.Black
        );
        currentEllipse.Draw(g);
    }

    private void DrawRegularPolygon(Graphics g)
    {
        currentPolygon = new Polygon(
            new Point(200, 200),
            5,
            80,
            2,
            Color.Green,
            Color.LightGreen,
            panelDrawing.BackColor
        );
        currentPolygon.Draw(g);
    }

    private void DrawCustomPolygon(Graphics g)
    {
        int pointCount = listBoxPoints.Items.Count;

        if (pointCount < 3)
        {
            MessageBox.Show(
                "Минимум 3 точки",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return;
        }

        PointF[] points = new PointF[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            string[] coords = listBoxPoints.Items[i].ToString().Split(' ');
            points[i] = new PointF(
                int.Parse(coords[0]),
                int.Parse(coords[1])
            );
        }

        currentCustomPolygon = new Polygon(
            points,
            3,
            Color.Black,
            Color.Blue,
            Color.White
        );

        currentCustomPolygon.Draw(g);
    }

    private void ButtonErase_Click(object sender, EventArgs e)
    {
        var g = panelDrawing.CreateGraphics();
        var shapeType = comboBoxShapes.SelectedItem.ToString();

        if (shapeType == "Линия" && currentLine != null)
        {
            currentLine.Erase(g);
        }
        else if (shapeType == "Эллипс" && currentEllipse != null)
        {
            currentEllipse.Erase(g);
        }
        else if (shapeType == "Правильный многоугольник" && currentPolygon != null)
        {
            currentPolygon.Erase(g);
        }
        else if (shapeType == "Произвольный многоугольник" && currentCustomPolygon != null)
        {
            currentCustomPolygon.Erase(g);
        }
    }

    private void ButtonClearAll_Click(object sender, EventArgs e)
    {
        panelDrawing.CreateGraphics().Clear(panelDrawing.BackColor);
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new MainForm());
    }
}
