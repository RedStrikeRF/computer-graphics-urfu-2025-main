using System;
using System.Drawing;
using System.Windows.Forms;

public class MainForm : Form
{
    private Button buttonLoadImage, buttonGetPixel, buttonBrighten;
    private PictureBox pictureBoxOriginal, pictureBoxModified;
    private OpenFileDialog openFileDialog;
    private TextBox textBoxX, textBoxY, textBoxBrightness;
    private Label labelResult, labelX, labelY, labelBright;
    private Bitmap loadedImage, modifiedImage;

    public MainForm()
    {
        this.Text = "Lab 5";
        this.Width = 1000;
        this.Height = 720;

        buttonLoadImage = new Button {Left = 400, Top = 10, Width = 180, Height = 30, Text = "Загрузить изображение"};
        buttonGetPixel = new Button {Left = 10, Top = 575, Width = 180, Height = 30, Text = "Проверить пиксель"};
        buttonBrighten = new Button {Left = 500, Top = 575, Width = 200, Height = 30, Text = "Добавить яркость"};

        buttonLoadImage.Click += ButtonLoadImage_Click;
        buttonGetPixel.Click += ButtonGetPixel_Click;
        buttonBrighten.Click += ButtonChangeBrighten_Click;

        this.Controls.Add(buttonLoadImage);
        this.Controls.Add(buttonGetPixel);
        this.Controls.Add(buttonBrighten);

        pictureBoxOriginal = new PictureBox {Top = 50, Left = 10, Width = 460, Height = 500, BorderStyle = BorderStyle.Fixed3D, SizeMode = PictureBoxSizeMode.Zoom};
        pictureBoxModified = new PictureBox {Top = 50, Left = 500, Width = 460, Height = 500, BorderStyle = BorderStyle.Fixed3D, SizeMode = PictureBoxSizeMode.Zoom};
        
        this.Controls.Add(pictureBoxOriginal);
        this.Controls.Add(pictureBoxModified);

        labelX = new Label {Top = 580, Left = 210, Width = 20, Text = "X:"};
        labelY = new Label {Top = 580, Left = 290, Width = 20, Text = "Y:"};
        textBoxX = new TextBox {Top = 580, Left = 230, Width = 50};
        textBoxY = new TextBox {Top = 580, Left = 310, Width = 50};

        labelBright = new Label {Top = 580, Left = 730, Width = 130, Text = "Прибавка яркости:"};
        textBoxBrightness = new TextBox {Top = 580, Left = 860, Width = 40};

        labelResult = new Label {Top = 620, Left = 10, Width = 800, Height = 30};

        this.Controls.Add(labelX);
        this.Controls.Add(textBoxX);
        this.Controls.Add(labelY);
        this.Controls.Add(textBoxY);
        this.Controls.Add(labelBright);
        this.Controls.Add(textBoxBrightness);
        this.Controls.Add(labelResult);

        openFileDialog = new OpenFileDialog {Filter = "Изображения|*.bmp;*.jpg;*.jpeg;*.png"};
    }

    private void ButtonLoadImage_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            loadedImage = new Bitmap(openFileDialog.FileName);
            pictureBoxOriginal.Image = loadedImage;
            pictureBoxModified.Image = null;
        }
    }

    private void ButtonGetPixel_Click(object sender, EventArgs e)
    {
        if (loadedImage == null)
        {
            MessageBox.Show("Загрузите изображение!!!!");
            return;
        }

        if (!int.TryParse(textBoxX.Text, out int x) || !int.TryParse(textBoxY.Text, out int y))
        {
            MessageBox.Show("Введите корректные числа!!");
            return;
        }

        if (x < 0 || y < 0 || x >= loadedImage.Width || y >= loadedImage.Height)
        {
            MessageBox.Show("Координаты вне изображения");
            return;
        }

        var pixel = loadedImage.GetPixel(x, y);
        var brightness = (pixel.R + pixel.G + pixel.B) / 3;
        labelResult.Text = $"Цвет: R={pixel.R}, G={pixel.G}, B={pixel.B}, Яркость: {brightness}";
    }

    private void ButtonChangeBrighten_Click(object sender, EventArgs e)
    {
        if (loadedImage == null)
        {
            MessageBox.Show("Загрузите изображение!!!!");
            return;
        }

        if (!int.TryParse(textBoxBrightness.Text, out int brightness))
        {
            MessageBox.Show("Введите корректные числа!!");
            return;
        }

        modifiedImage = new Bitmap(loadedImage.Width, loadedImage.Height);

        for (int y = 0; y < loadedImage.Height; y++)
        {
            for (int x = 0; x < loadedImage.Width; x++)
            {
                var original = loadedImage.GetPixel(x, y);
                var r = Math.Max(Math.Min(original.R + brightness, 255), 0);
                var g = Math.Max(Math.Min(original.G + brightness, 255), 0);
                var b = Math.Max(Math.Min(original.B + brightness, 255), 0);
                
                modifiedImage.SetPixel(x, y, Color.FromArgb(original.A, r, g, b));
            }
        }

        pictureBoxModified.Image = modifiedImage;
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new MainForm());
    }
}
