using System.Diagnostics;
using QuikGraph;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;

namespace Lab3Apps;
public partial class MainForm : Form
{
    private readonly RedBlackTree _database = new();
    private const string DatabaseFile = "database.json";

    public MainForm()
    {
        InitializeComponent();
        
        _database.LoadFromFile(DatabaseFile);
        
        RefreshList();
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(txtKey.Text, out int key))
        {
            MessageBox.Show("Будь ласка, введіть дійсний цілий ключ.", "Помилка вводу", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string data = txtData.Text;

        try
        {
            _database.Insert(key, data);
            MessageBox.Show("Запис успішно додано.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            _database.SaveToFile(DatabaseFile);
            RefreshList();
            ClearFields();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(txtKey.Text, out int key))
        {
            MessageBox.Show("Будь ласка, введіть дійсний цілий ключ.", "Помилка вводу", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string newData = txtData.Text;

        var searchResult = _database.Search(key);
        var node = searchResult.Node;

        if (node == _database.NullNode)
        {
            MessageBox.Show("Запис для редагування не знайдено.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        node.Data = newData;
        _database.SaveToFile(DatabaseFile);
        RefreshList();
        MessageBox.Show("Запис успішно оновлено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        ClearFields();
    }


    private void BtnSearch_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(txtKey.Text, out int key))
        {
            MessageBox.Show("Будь ласка, введіть дійсний цілий ключ.", "Помилка вводу", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var searchResult = _database.Search(key);

        if (searchResult.Node != _database.NullNode)
        {
            txtData.Text = searchResult.Node.Data;
            MessageBox.Show($"Запис знайдено.\nКількість порівнянь: {searchResult.Comparisons}", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show($"Запис не знайдено.\nКількість порівнянь: {searchResult.Comparisons}", "Результат пошуку", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtData.Clear();
        }
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(txtKey.Text, out int key))
        {
            MessageBox.Show("Будь ласка, введіть дійсний цілий ключ.", "Помилка вводу", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            _database.Delete(key);
            _database.SaveToFile(DatabaseFile);
            RefreshList();
            MessageBox.Show("Запис видалено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFields();
        }
        catch (KeyNotFoundException)
        {
            MessageBox.Show("Запис не знайдено.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnClear_Click(object sender, EventArgs e)
    {
        var confirmation = MessageBox.Show(
            "Ви впевнені, що хочете очистити всю базу даних? Цю дію не можна скасувати.",
            "Підтвердження очищення бази даних",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmation == DialogResult.Yes)
        {
            _database.Clear();
            File.WriteAllText(DatabaseFile, "[]");
            RefreshList();
            MessageBox.Show("Базу даних очищено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        RefreshList();
    }
    
    private void BtnFill_Click(object sender, EventArgs e)
    {
        int numberOfRecords = 10000;
        int minKey = 1;
        int maxKey = 100000;
        bool isRandom = true;

        FillDatabase(numberOfRecords, minKey, maxKey, isRandom);
        
        _database.SaveToFile(DatabaseFile);
        RefreshList();
        MessageBox.Show($"{numberOfRecords} записів додано до бази даних.", "Заповнення завершене", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private void FillDatabase(int numberOfRecords, int minKey, int maxKey, bool isRandom)
    {
        if (isRandom)
        {
            List<int> allKeys = Enumerable.Range(minKey, maxKey - minKey + 1).ToList();
            if (numberOfRecords > allKeys.Count)
            {
                MessageBox.Show("Діапазон ключів занадто малий для вказаної кількості записів.", 
                    "Помилка", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            Random random = new Random();
            allKeys = allKeys.OrderBy(_ => random.Next()).ToList();

            for (int i = 0; i < numberOfRecords; i++)
            {
                int key = allKeys[i];
                string data = $"Data {key}";

                try
                {
                    _database.Insert(key, data);
                }
                catch (ArgumentException)
                {
                }
            }
        }
        else
        {
            // Якщо ключі не випадкові, використовуємо існуючу логіку.
            for (int i = 0; i < numberOfRecords; i++)
            {
                int key = minKey + i;
                if (key > maxKey)
                {
                    break;
                }

                string data = $"Data {key}";

                try
                {
                    _database.Insert(key, data);
                }
                catch (ArgumentException)
                {
                }
            }
        }
    }

    private void LstResults_DoubleClick(object sender, EventArgs e)
    {
        if (lstResults.SelectedItem == null) return;

        string? selectedRecord = lstResults.SelectedItem.ToString();
        var parts = selectedRecord?.Replace("Key: ", "").Replace("Data: ", "").Split(',');

        if (parts is { Length: 2 })
        {
            txtKey.Text = parts[0].Trim();
            txtData.Text = parts[1].Trim();
        }
    }

    private void RefreshList()
    {
        lstResults.Items.Clear();

        var allNodes = _database.GetAllRecords();

        if (allNodes.Count == 0)
        {
            MessageBox.Show("Немає записів для відображення. База даних може бути порожньою.",
                "Інформаційне", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        foreach (var node in allNodes)
        {
            lstResults.Items.Add($"Key: {node.Key}, Data: {node.Data}");
        }
    }

    private void ClearFields()
    {
        txtKey.Clear();
        txtData.Clear();
    }
    
    private void VisualizeWithQuikGraph()
{
    var graph = _database.ToAdjacencyGraph();
    var nodeColors = _database.GetNodeColors();

    var graphviz = new GraphvizAlgorithm<int, Edge<int>>(graph);
    
    graphviz.FormatVertex += (_, args) =>
    {
        args.VertexFormat.Label = args.Vertex.ToString();
        if (nodeColors.TryGetValue(args.Vertex, out GraphvizColor color))
        {
            args.VertexFormat.FillColor = color;
            args.VertexFormat.Style = GraphvizVertexStyle.Filled;
            args.VertexFormat.FontColor = GraphvizColor.White;
        }
    };
    
    graphviz.FormatEdge += (_, args) =>
    {
        args.EdgeFormat.FontColor = GraphvizColor.Black;
    };

    string output = graphviz.Generate();
    
    var dotExe = @"C:\Program Files\Graphviz\bin\dot.exe";

    var processStartInfo = new ProcessStartInfo
    {
        FileName = dotExe,
        Arguments = "-Tpng",
        UseShellExecute = false,
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true
    };

    using var process = new Process();
    process.StartInfo = processStartInfo;
    process.Start();

    using (StreamWriter writer = process.StandardInput)
    {
        writer.Write(output);
    }

    using (var memoryStream = new MemoryStream())
    {
        process.StandardOutput.BaseStream.CopyTo(memoryStream);
        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            memoryStream.Position = 0;
            Image image = Image.FromStream(memoryStream);

            if (pictureBoxGraph.Image != null)
            {
                pictureBoxGraph.Image.Dispose();
            }

            pictureBoxGraph.Image = image;
        }
        else
        {
            string error = process.StandardError.ReadToEnd();
            MessageBox.Show($"Graphviz error: {error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

private void BtnVisualize_Click(object sender, EventArgs e)
{
    VisualizeWithQuikGraph();
}

}