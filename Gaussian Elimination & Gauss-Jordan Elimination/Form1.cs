using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private TextBox[,] matrixInputs = null!;
        private int rows;
        private int cols;

        public Form1()
        {
            InitializeComponent();
            // إنشاء زر الاستيراد برمجياً وتحديد مكانه تلقائياً بجانب زر الحل
            Button btnImport = new Button();
            btnImport.Name = "btnImport";
            btnImport.Text = "استيراد من ملف / PDF";
            btnImport.Width = btnSolve.Width;
            btnImport.Height = 35;
            // وضعه تحت زر الحل مباشرة بمسافة 10 بكسل
            btnImport.Left = btnSolve.Left;
            btnImport.Top = btnSolve.Top + btnSolve.Height + 10;

            // ربط حدث الضغط بالدالة
            btnImport.Click += BtnImport_Click;

            // إضافة الزر للنافذة
            this.Controls.Add(btnImport);

            // تكبير ارتفاع النافذة قليلاً لتسع الزر الجديد
            this.Height += 50;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateMatrixFields();
        }

        private void btnGenerateMatrix_Click(object sender, EventArgs e)
        {
            GenerateMatrixFields();
        }

        private void GenerateMatrixFields()
        {
            panelMatrix.Controls.Clear();
            rows = (int)numRows.Value;
            cols = (int)numCols.Value;

            matrixInputs = new TextBox[rows, cols];

            int boxWidth = 50;
            int boxHeight = 30;
            int margin = 4;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int currentRow = i;
                    int currentCol = j;
                    TextBox txt = new TextBox
                    {
                        Width = boxWidth,
                        Height = boxHeight,
                        Left = j * (boxWidth + margin) + 10,
                        Top = i * (boxHeight + margin) + 10,
                        Text = "",
                        TextAlign = HorizontalAlignment.Center
                    };
                    // === ميزة الانتقال للخلية التالية عند الضغط على Enter ===
                    txt.KeyDown += (s, args) =>
                    {
                        if (args.KeyCode == Keys.Enter)
                        {
                            args.SuppressKeyPress = true; // إلغاء صوت التنبيه المزعج في النظام (Beep)
                            MoveToNextTextBox(currentRow, currentCol);
                        }
                    };

                    // تمييز عمود الثوابت 
                    if (j == cols - 1)
                    {
                        txt.BackColor = Color.Yellow;
                    }

                    matrixInputs[i, j] = txt;
                    panelMatrix.Controls.Add(txt);
                }
            }
        }
        // دالة الانتقال التلقائي للخلية التالية
        private void MoveToNextTextBox(int r, int c)
        {
            c++; // الانتقال للعمود التالي في نفس الصف

            // إذا تجاوزنا آخر عمود، ننتقل لبداية الصف التالي
            if (c >= cols)
            {
                c = 0;
                r++;
            }

            // التحقق من أن الصف الجديد ضمن حدود المصفوفة
            if (r < rows)
            {
                matrixInputs[r, c].Focus();
                matrixInputs[r, c].SelectAll(); // تحديد النص بالكامل لتسهيل الكتابة فوقه مباشرة
            }
            else
            {
                // إذا وصلنا لآخر خلية في المصفوفة بالكامل، نعود للخلية الأولى (0, 0)
                matrixInputs[0, 0].Focus();
                matrixInputs[0, 0].SelectAll();
            }
        }
        private void BtnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image & Document Files|*.png;*.jpg;*.jpeg;*.pdf;*.txt|All Files|*.*";
                openFileDialog.Title = "اختر صورة أو ملف PDF لاستخراج الأرقام";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string extension = Path.GetExtension(filePath).ToLower();

                    try
                    {
                        string rawText = "";

                        if (extension == ".txt")
                        {
                            rawText = File.ReadAllText(filePath);
                        }
                        else if (extension == ".pdf" || extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                        {
                            MessageBox.Show("لقراءة الصور أو الـ PDF تلقائياً، تأكد من توفر النصوص المستخرجة.", "تنبيه الاستيراد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (File.Exists(filePath))
                            {
                                rawText = File.ReadAllText(filePath);
                            }
                        }

                        PopulateMatrixFromText(rawText);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"حدث خطأ أثناء قراءة الملف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void PopulateMatrixFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("الملف الفارغ لا يحتوي على بيانات!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // تقسيم النص بناءً على المسافات، الفواصل، أو أسطر جديدة مع تجاهل الفراغات الزائدة
            char[] separators = new char[] { ' ', '\r', '\n', '\t', ',' };
            string[] tokens = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            int tokenIndex = 0;
            bool outOfRange = false;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (tokenIndex < tokens.Length)
                    {
                        matrixInputs[i, j].Text = tokens[tokenIndex];
                        tokenIndex++;
                    }
                    else
                    {
                        matrixInputs[i, j].Text = "0"; // وضع صفر افتراضي إذا نفدت الأرقام من الملف
                        outOfRange = true;
                    }
                }
            }

            if (outOfRange)
            {
                MessageBox.Show("تمت التعبئة، ولكن عدد الأرقام في الملف أقل من حجم المصفوفة المحدد!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("تم استيراد وتعبئة المصفوفة بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnSolve_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
            double[,] matrix = new double[rows, cols];

            // 1. قراءة قِيَم المصفوفة ودعم الكُسور مثل (1/2)
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!ParseFraction(matrixInputs[i, j].Text, out matrix[i, j]))
                    {
                        MessageBox.Show($"خطأ في الإدخال عند الصف {i + 1}، العمود {j + 1}.\nيرجى كتابة رقم صحيح، عشري، أو كسر مثل (1/2).", "خطأ إدخال", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }


            StringBuilder logger = new StringBuilder();
            logger.AppendLine("=== Initial augmented matrix ===\n");
            AppendMatrix(logger, matrix);

            logger.AppendLine("==============================================\n");
            logger.AppendLine("\t (Gaussian Elimination) \t");
            logger.AppendLine("==============================================");

            int currentPivotRow = 0;

            // 5) ننتقل إلى أول صف، ونعمل نفس الخطوات على الأعمدة التالية
            for (int col = 0; col < cols - 1 && currentPivotRow < rows; col++)
            {
                // 1) نعيّن أول عمود غير صفري على يسار المصفوفة
                int nonZeroRow = -1;
                for (int r = currentPivotRow; r < rows; r++)
                {
                    if (Math.Abs(matrix[r, col]) > 1e-9)
                    {
                        nonZeroRow = r;
                        break;
                    }
                }

                // إذا كان العمود بالكامل أصفاراً أسفل الصف الحالي، ننتقل للعمود التالي
                if (nonZeroRow == -1)
                {
                    continue;
                }

                logger.AppendLine($"\n--- [الخطوة 1] تعيين أول عمود غير صفري: العمود {col + 1} ---");

                //   نجعل اول عنصر في العمود قيمته لا تساوي صفر row operation(تبديل صف مكان صف) 
                if (nonZeroRow != currentPivotRow)
                {
                    SwapRows(matrix, currentPivotRow, nonZeroRow);
                    logger.AppendLine($"[عملیات الصف] تبديل الصف {currentPivotRow + 1} مع الصف {nonZeroRow + 1} تجنباً للصفر");
                    AppendMatrix(logger, matrix);
                }

                // 3) نجعل أول عنصر في العمود قيمته تساوي 1 ويسمى (Leading 1)
                // وذلك بضرب الصف في ثابت غير صفري (1 / pivot)
                double pivotVal = matrix[currentPivotRow, col];
                if (Math.Abs(pivotVal - 1.0) > 1e-9)
                {
                    double factor = 1.0 / pivotVal;
                    for (int j = col; j < cols; j++)
                    {
                        matrix[currentPivotRow, j] *= factor;
                    }
                    logger.AppendLine($"\n[الخطوة 3 - Row Operation] ضرب الصف {currentPivotRow + 1} في الثابت ({Math.Round(factor, 3)}) للحصول على Leading 1:");
                    AppendMatrix(logger, matrix);
                }
                else
                {
                    logger.AppendLine($"\nالعنصر القائد (Leading 1) جاهز في الصف {currentPivotRow + 1}، العمود {col + 1}.");
                }

                // 4) نجعل العناصر أسفل الـ Leading 1 تساوي صفر
                // وذلك بضرب الصف القائد في ثابت وجمعه على الصف المراد تصفيره
                bool subRowsEliminated = false;
                for (int r = currentPivotRow + 1; r < rows; r++)
                {
                    if (Math.Abs(matrix[r, col]) > 1e-9)
                    {
                        double multiplier = matrix[r, col];
                        for (int j = col; j < cols; j++)
                        {
                            matrix[r, j] -= multiplier * matrix[currentPivotRow, j];
                        }
                        logger.AppendLine($"\n[الخطوة 4 - Row Operation] ضرب الصف {currentPivotRow + 1} في ({Math.Round(-multiplier, 3)}) وجمعه على الصف {r + 1} لتصفير أسفل Leading 1");
                        subRowsEliminated = true;
                    }
                }

                if (subRowsEliminated)
                {
                    AppendMatrix(logger, matrix);
                }

                currentPivotRow++;
            }

            logger.AppendLine("\n--- تم الوصول إلى صيغة الدرج الصفية (Row Echelon Form) ---");

            // المرحلة الثانية: Gauss-Jordan Elimination (تصفير ما فوق الـ Leading 1)
            logger.AppendLine("\n==============================================");
            logger.AppendLine("     (Gauss-Jordan Elimination)     ");
            logger.AppendLine("==============================================");

            for (int r = rows - 1; r >= 0; r--)
            {
                // البحث عن موقع الـ Leading 1 في الصف الحالي
                int leadingCol = -1;
                for (int c = 0; c < cols - 1; c++)
                {
                    if (Math.Abs(matrix[r, c] - 1.0) < 1e-9)
                    {
                        leadingCol = c;
                        break;
                    }
                }

                if (leadingCol == -1) continue; // صف صفري بالكامل

                // جعل العناصر فوق الـ Leading 1 تساوي صفر
                for (int k = r - 1; k >= 0; k--)
                {
                    if (Math.Abs(matrix[k, leadingCol]) > 1e-9)
                    {
                        double multiplier = matrix[k, leadingCol];
                        for (int j = leadingCol; j < cols; j++)
                        {
                            matrix[k, j] -= multiplier * matrix[r, j];
                        }
                        logger.AppendLine($"\n[Row Operation] ضرب الصف {r + 1} في ({Math.Round(-multiplier, 3)}) وجمعه على الصف {k + 1} لتصفير أعلا الـ Leading 1:");
                        AppendMatrix(logger, matrix);
                    }
                }
            }

            logger.AppendLine("\n--- تم الوصول إلى صورة الدرج الصفية المختزلة (Reduced Row Echelon Form) ---");

            // تحليل الحل النهائي
            logger.AppendLine("\n================================================");
            logger.AppendLine("تحليل حل المعادلة الخطية");
            logger.AppendLine("================================================");
            AnalyzeSolution(matrix, logger);

            txtOutput.Text = logger.ToString();
        }

        // عملية صفية: تبديل صف مكان صف آخر
        private void SwapRows(double[,] matrix, int r1, int r2)
        {
            for (int j = 0; j < cols; j++)
            {
                double temp = matrix[r1, j];
                matrix[r1, j] = matrix[r2, j];
                matrix[r2, j] = temp;
            }
        }

        // طباعة طبقة المصفوفة بطريقة منسقة
        private void AppendMatrix(StringBuilder sb, double[,] matrix)
        {
            for (int i = 0; i < rows; i++)
            {
                sb.Append("| ");
                for (int j = 0; j < cols; j++)
                {
                    if (j == cols - 1) sb.Append(" | "); // فاصل العمود الممتد

                    double val = Math.Abs(matrix[i, j]) < 1e-9 ? 0.0 : matrix[i, j];
                    sb.Append($"{val,8:F2} ");
                }
                sb.AppendLine("|");
            }
        }

        // تحليل حل النظام (حل وحيد - عدد لا نهائي من الحلول - لا يوجد حل)
        private void AnalyzeSolution(double[,] matrix, StringBuilder logger)
        {
            int rankA = 0;

            for (int i = 0; i < rows; i++)
            {
                bool nonZeroA = false;
                bool nonZeroAug = Math.Abs(matrix[i, cols - 1]) > 1e-9;

                for (int j = 0; j < cols - 1; j++)
                {
                    if (Math.Abs(matrix[i, j]) > 1e-9) nonZeroA = true;
                }

                if (nonZeroA) rankA++;

                // تناقض: صف أصفار في المعاملات يُساوي قيماً غير صفرية في الناتج (0 = b)
                if (!nonZeroA && nonZeroAug)
                {
                    logger.AppendLine("النظام غير متسق: لا يوجد حل للمنظومة (No Solution).");
                    logger.AppendLine($"تناقض في الصف {i + 1}: 0 = {matrix[i, cols - 1]:F2}");
                    return;
                }
            }

            int variables = cols - 1;

            if (rankA == variables)
            {
                logger.AppendLine("يوجد حل وحيد للمنظومة (Unique Solution):\n");
                for (int i = 0; i < variables; i++)
                {
                    logger.AppendLine($"x{i + 1} = {matrix[i, cols - 1]:F4}");
                }
            }
            else if (rankA < variables)
            {
                logger.AppendLine("يوجد عدد لا نهائي من الحلول (Infinitely Many Solutions).");
                logger.AppendLine($"عدد المتغيرات الحرة (Free Variables) هو: {variables - rankA}");
            }
        }
        private bool ParseFraction(string input, out double result)
        {
            result = 0;
            if (string.IsNullOrWhiteSpace(input)) return false;

            input = input.Trim();

            // إذا كان الإدخال يحتوي على علامة الكسر '/'
            if (input.Contains("/"))
            {
                string[] parts = input.Split('/');
                if (parts.Length == 2)
                {
                    if (double.TryParse(parts[0], out double numerator) &&
                        double.TryParse(parts[1], out double denominator))
                    {
                        if (Math.Abs(denominator) < 1e-9) // تجنب القسمة على صفر
                        {
                            return false;
                        }
                        result = numerator / denominator;
                        return true;
                    }
                }
                return false;
            }

            // إذا كان إدخالاً رقمياً عادياً (عشري أو صحيح)
            return double.TryParse(input, out result);
        }
    }
}