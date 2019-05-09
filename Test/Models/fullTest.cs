using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Management;
using System.Runtime.InteropServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Test.Models;
using System.Threading.Tasks;

namespace Test
{
    class FullTest
    {
        #region CONST
        private const string EXCEPTION_MAIL = "yuhim93@gmail.com";
        private const string SELF_MAIL = "anastasia.75.work@gmail.com";
        private const string SELF_MAIL_PASS = "Q1W2E3R4QWER";
        #endregion
        private Student student;
        private string _result_string;
        private string _result_string_diff;
        private int currentQuestionNum;
        private string dirPath; 
        private List<Answer> userAnswers;
        private List<bool> TrueAnswers;
        private Dictionary<int, string> _IQdegree;
        private Dictionary<string, int> resDiff = new Dictionary<string, int>();
        private Dictionary<string, int> resCateg = new Dictionary<string, int>();
        private int[] NormalCateg;

        public string _folder_PDF=null, _folder_EXCEL = null;
        public bool _PDF_save { get; set; }
        public bool _EXCEL_save { get; set; }
        public int CurrentQuestionNum { get => currentQuestionNum; set => currentQuestionNum = value; }
        public Student Student { get => student; set => student = value; }
        public string DirPath { get => dirPath; set => dirPath = value; }
        public List<Answer> Answers { get => userAnswers; set => userAnswers = value; }
        public string IQ { get => _IQdegree.ElementAt(0).Key.ToString();  }
        public string IQ_percent { get => _IQdegree.ElementAt(1).Key.ToString();  }
        
        public FullTest(Student st)
        {
            student = st;
            currentQuestionNum = 1;
            DirPath = Directory.GetCurrentDirectory() + "\\Questions\\";
            userAnswers = new List<Answer>();
            for (int i = 1; i < 61; i++)
            {
                userAnswers.Add(new Answer(userAnswers.Count + 1, Q_categ(i),0));
            }
        }

        private string Q_categ(int n)
        {
            if (n < 13)
            {
                return "A";
            }
            else if (n > 12 && n < 25)
            {
                return "B";
            }
            else if (n > 24 && n < 37)
            {
                return "C";
            }
            else if (n > 36 && n < 49)
            {
                return "D";
            }
            else if (n > 48 && n < 61)
            {
                return "E";
            }
            return "ERROR";
        }

        public void CheckResult()
        {
            _IQdegree = new Dictionary<int, string>();
            var tmp = new AnswerInfo(student.Age);
            tmp.Check(Answers);
            _IQdegree.Add(Convert.ToInt32(tmp.IQ), lvl(Convert.ToInt32(tmp.IQ)));
            _IQdegree.Add(Convert.ToInt32(tmp.Percent), deg(Convert.ToInt32(tmp.Percent)));
            resDiff = tmp.ResDiff;
            resCateg = tmp.ResCateg;
            NormalCateg = tmp._row;
            TrueAnswers = tmp.Res;
        }

        private void CreatePDF(string path)
        {
            // creation of a document-object
            Document pdf = new Document(PageSize.A4.Rotate());  //Document doc = new Document(PageSize.LETTER, 10, 10, 42, 35);
            try
            {
                var pathFont = Path.GetFullPath(@"Fonts/ArialRegular/ArialRegular.ttf");
                //Подключаем собсвенный  шрифт  чтобы видить русский
                BaseFont baseFont = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                #region Экземпляры шрифтов
                //Экземпляры шрифтов
                iTextSharp.text.Font f16bold = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font f16 = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font f14 = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.NORMAL);
                #endregion
                // Now create a writer that listens to this doucment and writes the document to desired Stream.
                PdfWriter.GetInstance(pdf, new FileStream(path + $"{student.Lname}_{student.Fname}_{student.Sname}.pdf", FileMode.Create));
                pdf.Open();
                PdfPTable table = null;

                table = new PdfPTable(13);
                table.TotalWidth = 340f;
                table.LockedWidth = false;
                table.SpacingBefore = 20f;
                table.HorizontalAlignment = Element.ALIGN_CENTER;

                //Now add some contents to the document
                pdf.Add(new Paragraph($"{ student.Lname} { student.Fname} { student.Sname}", new iTextSharp.text.Font(baseFont, 20, iTextSharp.text.Font.BOLD)) {Alignment = Element.ALIGN_CENTER });
                pdf.Add(new Paragraph($"{ student.Age} Год(а). { student.Group} класс", f14) {Alignment = Element.ALIGN_CENTER });
                pdf.Add(new Paragraph(new Chunk(new VerticalPositionMark())));//Cдвиг на стоку  вниз

                #region FILL TABLE
                int z = 1;
                int qNum = 0;
                int CategsIndex = 0;
                string[] CategsLetters = { "A", "B", "C", "D", "E" };
                for (int i = 0; i < 120; i++)
                {
                    if (i % 12 == 0 && (i / 12) % 2 == 1)
                    {
                        addCell(table, "Ответы", f16bold);
                    }
                    if (i % 12 == 0 && (i / 12) % 2 == 0)
                    {
                        addCell(table, "Категория " + CategsLetters[CategsIndex++], f16bold);
                    }
                    FillTable(ref z, ref qNum, ref table, f16bold, i);
                }
                float[] widths = new float[] { 60f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f };
                table.SetWidths(widths);
                #endregion
                GenerateResultStrings();
                pdf.Add(new Paragraph($"IQ = {_IQdegree.ElementAt(0).Key}", f16) { Alignment = Element.ALIGN_CENTER });
                pdf.Add(new Paragraph($"Процентная шкала степени развития интеллекта : {_IQdegree.ElementAt(1).Key}", f16) { Alignment = Element.ALIGN_CENTER });
                pdf.Add(new Paragraph(new Chunk(new VerticalPositionMark())));
                pdf.Add(table);
                pdf.Add(new Paragraph(new Chunk(new VerticalPositionMark())));
                pdf.Add(new Paragraph($"{_result_string.Replace("<br>", "")}", f14) { Alignment = Element.ALIGN_CENTER });
                pdf.Add(new Paragraph(new Chunk(new VerticalPositionMark())));
                pdf.Add(new Paragraph($"{_result_string_diff.Replace("<br>", "")}", f14) { Alignment = Element.ALIGN_CENTER });
                pdf.Add(new Paragraph(new Chunk(new VerticalPositionMark())));
            }
            catch (DocumentException de)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 175, de.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
            catch (IOException ioe)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 180, ioe.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
            // Remember to close the documnet
            pdf.Close();
        }

        #region PDF Table generation methods
        private void FillTable(ref int z, ref int qNum, ref PdfPTable table, iTextSharp.text.Font f16bold, int i)
        {
            if ((i / 12) % 2 == 1)
            {
                addCell(table, $"{new string(TrueAnswers[qNum] ? '+' : '-', 1)}", f16bold, TrueAnswers[qNum++] ? BaseColor.GREEN : BaseColor.RED);
            }
            else
            {
                addCell(table, z.ToString(), f16bold);
                z++;
            }
        }

        private void addCell(PdfPTable table, string text, iTextSharp.text.Font F = null, BaseColor color = null)
        {
            if (F == null)
            {
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                F = times;
            }

            PdfPCell cell = new PdfPCell(new Phrase(text, F));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            if (color != null)
            {
                cell.BackgroundColor = color;
            }
            table.AddCell(cell);
        }
        #endregion

        #region Excel FULL

        private void CreateExcel()
        {
            try
            {
                Application excel = new Application()
                {
                    //Visible = true,//Показать
                    SheetsInNewWorkbook = 1
                };
                //Добавить рабочую книгу
                Excel.Workbook workBook = excel.Workbooks.Add(Type.Missing);
                //Отключить отображение окон с сообщениями
                excel.DisplayAlerts = false;
                //Получаем первый лист документа (счет начинается с 1)
                Excel.Worksheet sheet = (Excel.Worksheet)excel.Worksheets.get_Item(1);
                //Название листа (вкладки снизу)
                sheet.Name = $"{student.Lname} {student.Fname} {student.Sname}";
                //расчет формул в ручном режиме
                excel.Calculation = XlCalculation.xlCalculationManual;
                excel.UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                //https://www.e-iceblue.com/
                #region BaseStyle
                // set each cell format to Text
                sheet.Cells.NumberFormat = "@";

                sheet.Cells[1, 2].Style.HorizontalAlignment = XlHAlign.xlHAlignCenter;//обяз оди  взов иначе  сатйл не пашет
                sheet.Cells[1, 2].Style.VerticalAlignment = XlHAlign.xlHAlignCenter;

                Excel.Style style_mid = workBook.Styles.Add("mid");
                style_mid.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                style_mid.VerticalAlignment = XlVAlign.xlVAlignCenter;
                style_mid.Font.Size = 14;
                sheet.Range["A1: M3"].Style = style_mid;

                Excel.Style FSize14 = workBook.Styles.Add("FSize14");
                FSize14.Font.Size = 14;
                sheet.Range["A1:S22"].Style = FSize14;

                sheet.Range["B:M"].ColumnWidth = 6;
                //sheet.get_Range("A1", "A1").RowHeight = 100;
                #endregion

                #region initCells

                Range range = (Range)sheet.get_Range("B1", "M1").Cells;
                range.Font.Bold = true;
                range.Merge(Type.Missing);//sheet.Range["B1:J1"].Merge();//

                sheet.Cells[1, 2] = $"{student.Lname} {student.Fname} {student.Sname}";
                sheet.Cells[1, 1] = $"ФИО";

                sheet.Range["B2:C2"].Merge();
                sheet.Range["B2"].Value2 = $"Класс";
                sheet.Range["D2:G2"].Merge();
                sheet.Range["D2"].Value2 = $"{student.Group}";
                sheet.Range["H2:I2"].Merge();
                sheet.Range["H2"].Value2 = $"Возраст";
                sheet.Range["J2:M2"].Merge();
                sheet.Range["J2"].Value2 = $"{student.Age}";

                sheet.Range["B3:C3"].Merge();
                sheet.Range["B3"].Value2 = $"Начало";
                sheet.Range["D3:G3"].Merge();
                sheet.Range["D3"].Value2 = $"{student.Start}";
                sheet.Range["H3:I3"].Merge();
                sheet.Range["H3"].Value2 = $"Конец";
                sheet.Range["J3:M3"].Merge();
                sheet.Range["J3"].Value2 = $"{student.End}";

                sheet.Range["A5"].Value2 = $"Категория А";
                sheet.Range["A5"].EntireColumn.AutoFit();
                sheet.Range["A6"].Value2 = $"Ответы";
                sheet.Range["A7"].Value2 = $"Категория B";
                sheet.Range["A8"].Value2 = $"Ответы";
                sheet.Range["A9"].Value2 = $"Категория C";
                sheet.Range["A10"].Value2 = $"Ответы";
                sheet.Range["A11"].Value2 = $"Категория D";
                sheet.Range["A12"].Value2 = $"Ответы";
                sheet.Range["A13"].Value2 = $"Категория E";
                sheet.Range["A14"].Value2 = $"Ответы";

                sheet.Range["N5"].Value2 = $"Итог";
                sheet.Range["O5"].Value2 = $"Норма";
                sheet.Range["P5"].Value2 = $"Отклонение";
                sheet.Range["P5"].EntireColumn.AutoFit();
                sheet.Range["P:P"].NumberFormat = "@";//в текстовый  формат

                sheet.Range["A19"].Value2 = $"Вывод ";
                sheet.Range["B19:M27"].Merge();
                sheet.Range["N19:S27"].Merge();
                sheet.Range["A16:A17"].Merge();
                sheet.Range["B16:B17"].Merge();
                sheet.Range["C16:F17"].Merge();
                sheet.Range["G16:G17"].Merge();

                sheet.Range["A16"].Value2 = $"IQ = ";
                sheet.Range["B16"].Value2 = $"{_IQdegree.ElementAt(0).Key}";
                sheet.Range["C16"].Value2 = $"Процентная шкала степени развития интеллекта";
                sheet.Range["C16"].WrapText = true;
                sheet.Range["C16:C17"].EntireRow.AutoFit();
                sheet.Range["G16"].Value2 = $"{_IQdegree.ElementAt(1).Key}";
                sheet.Range["B19"].WrapText = true;

                GenerateResultStrings();
                sheet.Range["B19"].Value2 = _result_string.Replace("<br>", "");
                sheet.Range["N19"].WrapText = true;
                sheet.Range["N19"].Value2 = _result_string_diff.Replace("<br>", "");

                #endregion

                #region ResultTable FILL

                int qNum = 1;
                for (int i = 1, k = 5; i < 6; i++, k += 2)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        sheet.Cells[k, j + 2] = $" {qNum++}";
                    }
                }
                qNum = 0;
                for (int i = 1, k = 6; i < 6; i++, k += 2)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        sheet.Cells[k, j + 2] = $"{new string(TrueAnswers[qNum] ? '+' : '-', 1)}";
                        sheet.Cells[k, j + 2].Interior.Color = ColorTranslator.ToOle(TrueAnswers[qNum++] ? Color.Green : Color.Red);
                    }
                }
                int x = 6;
                int totalScore = 0;
                foreach (var it in resCateg.Values)
                {
                    string s = "N" + x;
                    sheet.Range[s].Value2 = $" {it} / 12 ";
                    x += 2; totalScore += it;
                }
                sheet.Range["N15"].Value2 = $"{totalScore}";//total

                for (int i = 1, z = 6; i < NormalCateg.Length - 1; i++, z += 2)
                {
                    string s = "O" + z;
                    sheet.Range[s].Value2 = $"{NormalCateg[i]}";
                }
                sheet.Range["O15"].Value2 = $"{NormalCateg[0]}";//total answers
                sheet.Range["O16"].Value2 = $"{NormalCateg[6]}";//IQby table

                x = 6;
                foreach (var it in resDiff.Values)
                {

                    string s = "P" + x;
                    sheet.Range[s].Value2 = $"{new string(it > 0 ? '+' : ' ', 1)} {it}";
                    x += 2;
                }

                //sheet.Range["A2:J2"].Interior.Color = ColorTranslator.ToOle(Color.FromArgb(125, 00, 250));
                //sheet.Range["2:2"].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml("#B7DEE8"));

                //sheet.Cells[1, 2].EntireColumn.AutoFit();
                //range.EntireColumn.AutoFit();
                //range.EntireRow.AutoFit();
                #endregion

                #region borders

                // http://csharp.net-informations.com/excel/csharp-format-excel.htm 
                MyBorder("A5:P14", sheet);
                MyBorder("B1:M3", sheet);
                MyBorderAround("B1:M3", sheet);
                MyBorderAround("A5:P14", sheet);
                MyBorderAround("C16: G17", sheet);
                MyBorderAround("A16: B17", sheet);
                MyBorderAround("B19:M27", sheet);
                MyBorderAround("N19:S27", sheet);
                #endregion
                if (_folder_EXCEL != null)
                {
                    excel.ActiveWorkbook.SaveAs(_folder_EXCEL + $"\\{student.Lname}_{student.Fname}_{student.Sname}.xlsx");
                }
                else
                {
                    var path = Path.GetFullPath(@"Files/");
                    CreateIfMissing(path);
                    excel.ActiveWorkbook.SaveAs(path + $"{student.Lname}_{student.Fname}_{student.Sname}.xlsx");
                }

                #region beforCLOSING
                workBook.Close();
                Marshal.ReleaseComObject(sheet);
                sheet = null;
                Marshal.ReleaseComObject(workBook);
                workBook = null;
                excel.Workbooks.Close();
                Marshal.ReleaseComObject(excel.Workbooks);
                excel.Quit();//https://www.e-iceblue.com/Tutorials/Spire.XLS/Spire.XLS-Program-Guide/Convert-Excel-to-PDF-in-WPF.html
                Marshal.ReleaseComObject(excel);
                excel = null;
                GC.Collect();
                #endregion
            }
            catch (Exception ex)
            {
                GenerateResultStrings();
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 425, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        #region MY XLS METHODS

        private void MyBorderAround(string r, Excel.Worksheet s)
        {
            s.Range[r].BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThick, XlColorIndex.xlColorIndexNone, ColorTranslator.FromHtml("#00f"));
        }
        private void MyBorder(string r, Excel.Worksheet s, double b = 2d, XlLineStyle ss = XlLineStyle.xlContinuous)
        {
            s.Range[r].Borders.LineStyle = ss;
            s.Range[r].Borders.Weight = b;
        }
        #endregion
        private void GenerateResultStrings()
        {
            _result_string = $"Уровень развития испытуемого на основании процентной шкалы: {_IQdegree.ElementAt(1).Key} что означает {_IQdegree.ElementAt(1).Value}.<br> Общее количество баллов, полученных в результате диагностики по методике Равена, равно  {NormalCateg[0]} " +
                $"и это соответствует значению коэффициента интеллекта IQ равному {NormalCateg[6]}. <br>Делаем поправку на возраст и получаем фактическое значение IQ с учетом возраста испытуемого {_IQdegree.ElementAt(0).Key} - {_IQdegree.ElementAt(0).Value} ";
            string tmp_diff_key = null;
            int tmp_diff_value = 0;
            int t_diff = 0;
            foreach (var it in resDiff)
            {
                if (Math.Abs(it.Value) > t_diff)
                {
                    tmp_diff_key = it.Key;
                    tmp_diff_value = it.Value;
                    t_diff = Math.Abs(it.Value);
                }
            }
            if (t_diff < 5)
            {
                _result_string_diff =
                 $"Отклонение результатов тестирования по каждой серии не превышает {t_diff}.<br>" +
                 "Следовательно, числовой показатель по каждой серии можно считать надежным. Общий показатель также надежен.<br>" +
                 "Тем самым, можно говорить о достоверности результата исследования.<br>";
            }
            else
            {
                _result_string_diff =
                $"Отклонение результатов превышает 4 на серии {tmp_diff_key} и представляет собой {tmp_diff_value}.<br>" +
                "Следовательно, числовой показатель по данной серии ненадежен.";// Общий показатель также надежен.<br>" +
                                                                                // "Тем самым, можно говорить о достоверности результата исследования.<br>";
            }
        }
        
        #endregion

        #region BackStringFunction
#pragma warning disable IDE1006 // Naming Styles
        private string lvl(int iq)
        {
            if (iq < 21)
            {
                return "тяжёлая степень слабоумия";
            }
            else if (iq < 51)
            {
                return "средняя степень слабоумия";
            }
            else if (iq < 71)
            {
                return "лёгкая степень слабоумия";
            }
            else if (iq < 81)
            {
                return "низкий уровень интеллекта";
            }
            else if (iq < 91)
            {
                return "интеллект ниже среднего";
            }
            else if (iq < 111)
            {
                return "средний уровень интеллекта";
            }
            else if (iq < 121)
            {
                return "интеллект выше среднего";
            }
            else if (iq < 141)
            {
                return "высокий уровень интеллекта";
            }
            else if (iq > 140)
            {
                return "незаурядный, выдающийся интеллект";
            }
            return "ERROR private string lvl(int iq)";
        }

        private string deg(int percent)
        {
            if (percent < 5)
            {
                return "5 степень: дефектная интеллектуальная способность";
            }
            else if (percent < 25)
            {
                return "4 степень: интеллект ниже среднего";
            }
            else if (percent < 75)
            {
                return "3 степень: средний интеллект";
            }
            else if (percent < 95)
            {
                return "2 степень; незаурядный интеллект";
            }
            else if (percent > 94)
            {
                return "I степень: особо высокоразвитый интеллект испытуемого";
            }
            return "ERROR";
        }
#pragma warning restore IDE1006 // Naming Styles
        #endregion

        #region MAILs
        public void MailTo()
        {
            DateTime start = DateTime.Now;
            var EXCEL_Task = Task.Factory.StartNew(() => 
            {
                CreateExcel();
            });
            var PDF_Task = Task.Factory.StartNew(() => 
            {
                CreateIfMissing(Path.GetFullPath(@"Files/"));
                CreatePDF(Path.GetFullPath(@"Files/"));
                if (_folder_PDF != null)
                {
                    CreatePDF(_folder_PDF + "\\");
                }
            });
            EXCEL_Task.Wait();
            PDF_Task.Wait();

            var Mails = Task.Factory.StartNew(() =>     
            {
                var MailT1 = Task.Factory.StartNew(() => // вложенная задача
                {
                    Mail(SELF_MAIL);
                });
                var MailT2 = Task.Factory.StartNew(() => 
                {
                    Mail(student.Mail);
                });
                var MailEXCEPTION = Task.Factory.StartNew(() => 
                {
                    Mail(EXCEPTION_MAIL);
                });
                MailT1.Wait();
                MailT2.Wait();
                MailEXCEPTION.Wait();
            });
            Mails.Wait(); // ожидаем выполнения внешней задачи

            var t3 = Task.Factory.StartNew(() =>
            {
                try
                {
                    var path = Path.GetFullPath(@"Files/");
                    if (!_PDF_save)
                    {
                        File.Delete(path + $"{student.Lname}_{student.Fname}_{student.Sname}.pdf");
                    }
                    if (!_EXCEL_save)
                    {
                        File.Delete(path + $"{student.Lname}_{student.Fname}_{student.Sname}.xlsx");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 607, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
                }
            });
            t3.Wait(); 
            DateTime end = DateTime.Now;
            var DT = end - start;
            ExceptionForMail.ExceptionList.Add(new myException($"{DT } ", 607, $"Start {start}", $"end  {end}"));
        }

        private void Mail(string mail)
        {
            try
            {
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress(SELF_MAIL, "Psyholog");
                // кому отправляем
                MailAddress to = new MailAddress(mail);// "sashabloodoff@ukr.net" vsevlada_lubosh@ukr.net//zaderey_step@i.ua
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Тест";
                // текст письма
                if (EXCEPTION_MAIL.ToUpper() == mail.ToUpper())
                {
                    string t = PKInfo();
                    string z = "";
                    foreach (var i in ExceptionForMail.ExceptionList)
                    {                        z += i+"\n<br>";                    }
                    m.Body = $"<h1>Имя компа {System.Security.Principal.WindowsIdentity.GetCurrent().Name}</h1><h3><br>{t}</h3><br><h1>{student.Lname} {student.Fname} {student.Sname}</h1><h2>{student.Group} класс</h2> <br>{_result_string} <br> ------------------------------------<br> {_result_string_diff}" +
                        $"<br><h2>EXCEPTION ({ExceptionForMail.ExceptionList.Count}) </h2><br>" + z;
                }
                else
                {
                    m.Body = $"<h1>{student.Lname} {student.Fname} {student.Sname}</h1><h2>{student.Group} класс</h2> <br><h2>{_result_string}</h2> <br> ------------------------------------<br> {_result_string_diff}";
                }
                // письмо представляет код html
                m.IsBodyHtml = true;
                // Вложения
                AttachEXCEL(ref m);
                AttachPDF(ref m);

                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {                    // логин и пароль
                    Credentials = new NetworkCredential(SELF_MAIL, SELF_MAIL_PASS),
                    EnableSsl = true
                };
                smtp.Send(m);
                m.Dispose();
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 660, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void AttachEXCEL(ref MailMessage m)
        {
            var path = Path.GetFullPath(@"Files/");
            try
            {
                if (File.Exists(path + $"{student.Lname}_{student.Fname}_{student.Sname}.xlsx"))
                {
                    m.Attachments.Add(new Attachment(path + $"{student.Lname}_{student.Fname}_{student.Sname}.xlsx"));
                }
                else if (File.Exists(_folder_EXCEL + $"\\{student.Lname}_{student.Fname}_{student.Sname}.xlsx"))
                {
                    m.Attachments.Add(new Attachment(_folder_EXCEL + $"\\{student.Lname}_{student.Fname}_{student.Sname}.xlsx"));
                }
                else
                {
                    ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 678, "Files not found", System.Reflection.MethodInfo.GetCurrentMethod().Name));
                    m.Body += "xlsx file ERROR";
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 684, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void AttachPDF(ref MailMessage m)
        {
            var path = Path.GetFullPath(@"Files/");
            try
            {
                if (File.Exists(path + $"{student.Lname}_{student.Fname}_{student.Sname}.pdf"))
                {
                    m.Attachments.Add(new Attachment(path + $"{student.Lname}_{student.Fname}_{student.Sname}.pdf"));
                }
                else if (File.Exists(_folder_PDF + $"\\{student.Lname}_{student.Fname}_{student.Sname}.xlsx"))
                {
                    m.Attachments.Add(new Attachment(_folder_PDF + $"\\{student.Lname}_{student.Fname}_{student.Sname}.xlsx"));
                }
                else
                {
                    ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 703, "Files not found", System.Reflection.MethodInfo.GetCurrentMethod().Name));
                    m.Body += "\npdf file ERROR";
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 710, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }
        #endregion

        #region StealInfo
        private string PKInfo()
        {
            ManagementClass myManagementClass = new ManagementClass("Win32_Processor");
            ManagementObjectCollection myManagementCollection = myManagementClass.GetInstances();
            PropertyDataCollection myProperties = myManagementClass.Properties;
            Dictionary<string, object> myPropertyResults = new Dictionary<string, object>();

            foreach (var obj in myManagementCollection)
            {
                foreach (var myProperty in myProperties)
                {
                    myPropertyResults.Add(myProperty.Name,
                        obj.Properties[myProperty.Name].Value);
                }
            }
            string t = "";
            foreach (var Result in myPropertyResults)
            {
                t += $"{Result.Key}: {Result.Value}<br>";
            }
            return t;
        }
        #endregion
    }
}
