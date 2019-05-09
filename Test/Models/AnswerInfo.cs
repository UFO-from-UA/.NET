using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class AnswerInfo
    {
        private double _IQ;
        private double _Percent;
        private int Age;
        private List<Answer> answerList = new List<Answer>();
        private List<bool> res = new List<bool>();
        private Dictionary<string,int> resCateg = new Dictionary<string, int>();
        private Dictionary<string,int> resDiff = new Dictionary<string, int>();
        private List<int[]> normalCateg = new List<int[]>();

        public int[] _row;
        public double IQ { get => _IQ; }
        public double Percent { get => _Percent; }
        public Dictionary<string, int> ResDiff { get => resDiff; }
        public Dictionary<string, int> ResCateg { get => resCateg; }
        public List<bool> Res { get => res;}

        public void Check(List<Answer> l)
        {
            try
            {
                for (int i = 0; i < answerList.Count; i++)
                {
                    if (answerList[i].AnswerNumber == l[i].AnswerNumber)
                    {
                        Res.Add(true);
                        if (ResCateg.ContainsKey(answerList[i].TypeOf))
                        {
                            ResCateg[answerList[i].TypeOf] += 1;
                        }
                        else
                        {
                            ResCateg.Add(answerList[i].TypeOf, 1);
                        }
                    }
                    else
                    {
                        Res.Add(false);
                        if (!ResCateg.ContainsKey(answerList[i].TypeOf)) { ResCateg.Add(answerList[i].TypeOf, 0); }
                    }
                }
                int totalScore = sum();
                if (totalScore>14)
                {
                    _IQ = Difference(normalCateg.Where(x => x[0] == totalScore).First());//fill resDiff
                    _IQ /= age(Age);
                }
                else//Cлишкм  глубоко , не нашел инструкций.
                {
                    _IQ = Difference(normalCateg.Where(x => x[0] == 15).First());
                    _Percent = totalScore * (100.0 / 60.0);
                }
            }
            catch (Exception ex)
            {
                _IQ = _Percent= 0;
                string mes = ex.Message;
            }
        }

        private double age(double a)
        {
            if (a>13&& a<31)
            {
                return 1;
            }
            else if (a > 30 && a < 40)
            {
                return 0.97;
            }
            else if (a > 39 && a < 45)
            {
                return 0.93;
            }
            else if (a > 44 && a < 50)
            {
                return 0.88;
            }
            else if (a > 49 && a < 55)
            {
                return 0.82;
            }
            else if (a > 54 && a < 60)
            {
                return 0.76;
            }
            else if (a > 60)
            {
                return 0.70;
            }
            else
            {
                return 1;
            }
        }

        private int Difference(int[] row)
        {
            int i = 1;
            foreach (var it in ResCateg)
            {
                int tmp = Math.Abs(row[i] - it.Value);
                if (tmp < 5)
                {
                    ResDiff[it.Key] =  it.Value- row[i];
                }
                else if (tmp > 4)
                {
                    ResDiff[it.Key] = it.Value - row[i];
                }
                else { }
                i++;
            }
            _Percent = row[0] * (100.0 / 60.0);
            _row = row;
            return row[6];
        }

        private int sum()
        {
            int tmpPont = 0;
            // foreach (var it in fake().Values)
             foreach (var it in ResCateg.Values)
            {
                tmpPont += it;
            }
            return tmpPont;
        }

        private Dictionary<string, int> fake()
        {
            Dictionary<string, int> tmp = new Dictionary<string, int>();
            Random r = new Random();
            foreach (var it in ResCateg.Keys)
            {
                tmp.Add(it, r.Next(3, 12));
            }
            return tmp;
        }

        public AnswerInfo(int a)
        {
            Age = a;

            #region AnswerList

            answerList.Add(new Answer(answerList.Count + 1, "A", 4));
            answerList.Add(new Answer(answerList.Count + 1, "A", 5));
            answerList.Add(new Answer(answerList.Count + 1, "A", 1));
            answerList.Add(new Answer(answerList.Count + 1, "A", 2));
            answerList.Add(new Answer(answerList.Count + 1, "A", 6));
            answerList.Add(new Answer(answerList.Count + 1, "A", 3));
            answerList.Add(new Answer(answerList.Count + 1, "A", 6));
            answerList.Add(new Answer(answerList.Count + 1, "A", 2));
            answerList.Add(new Answer(answerList.Count + 1, "A", 1));
            answerList.Add(new Answer(answerList.Count + 1, "A", 3));
            answerList.Add(new Answer(answerList.Count + 1, "A", 4));
            answerList.Add(new Answer(answerList.Count + 1, "A", 5));

            answerList.Add(new Answer(answerList.Count + 1, "B", 2));
            answerList.Add(new Answer(answerList.Count + 1, "B", 6));
            answerList.Add(new Answer(answerList.Count + 1, "B", 1));
            answerList.Add(new Answer(answerList.Count + 1, "B", 2));
            answerList.Add(new Answer(answerList.Count + 1, "B", 1));
            answerList.Add(new Answer(answerList.Count + 1, "B", 3));
            answerList.Add(new Answer(answerList.Count + 1, "B", 5));
            answerList.Add(new Answer(answerList.Count + 1, "B", 6));
            answerList.Add(new Answer(answerList.Count + 1, "B", 4));
            answerList.Add(new Answer(answerList.Count + 1, "B", 3));
            answerList.Add(new Answer(answerList.Count + 1, "B", 4));
            answerList.Add(new Answer(answerList.Count + 1, "B", 5));

            answerList.Add(new Answer(answerList.Count + 1, "C", 8));
            answerList.Add(new Answer(answerList.Count + 1, "C", 2));
            answerList.Add(new Answer(answerList.Count + 1, "C", 3));
            answerList.Add(new Answer(answerList.Count + 1, "C", 8));
            answerList.Add(new Answer(answerList.Count + 1, "C", 7));
            answerList.Add(new Answer(answerList.Count + 1, "C", 4));
            answerList.Add(new Answer(answerList.Count + 1, "C", 5));
            answerList.Add(new Answer(answerList.Count + 1, "C", 1));
            answerList.Add(new Answer(answerList.Count + 1, "C", 7));
            answerList.Add(new Answer(answerList.Count + 1, "C", 6));
            answerList.Add(new Answer(answerList.Count + 1, "C", 1));
            answerList.Add(new Answer(answerList.Count + 1, "C", 2));

            answerList.Add(new Answer(answerList.Count + 1, "D", 3));
            answerList.Add(new Answer(answerList.Count + 1, "D", 4));
            answerList.Add(new Answer(answerList.Count + 1, "D", 3));
            answerList.Add(new Answer(answerList.Count + 1, "D", 7));
            answerList.Add(new Answer(answerList.Count + 1, "D", 8));
            answerList.Add(new Answer(answerList.Count + 1, "D", 6));
            answerList.Add(new Answer(answerList.Count + 1, "D", 5));
            answerList.Add(new Answer(answerList.Count + 1, "D", 4));
            answerList.Add(new Answer(answerList.Count + 1, "D", 1));
            answerList.Add(new Answer(answerList.Count + 1, "D", 2));
            answerList.Add(new Answer(answerList.Count + 1, "D", 5));
            answerList.Add(new Answer(answerList.Count + 1, "D", 6));

            answerList.Add(new Answer(answerList.Count + 1, "E", 7));
            answerList.Add(new Answer(answerList.Count + 1, "E", 6));
            answerList.Add(new Answer(answerList.Count + 1, "E", 8));
            answerList.Add(new Answer(answerList.Count + 1, "E", 2));
            answerList.Add(new Answer(answerList.Count + 1, "E", 1));
            answerList.Add(new Answer(answerList.Count + 1, "E", 5));
            answerList.Add(new Answer(answerList.Count + 1, "E", 1));
            answerList.Add(new Answer(answerList.Count + 1, "E", 6));
            answerList.Add(new Answer(answerList.Count + 1, "E", 3));
            answerList.Add(new Answer(answerList.Count + 1, "E", 2));
            answerList.Add(new Answer(answerList.Count + 1, "E", 4));
            answerList.Add(new Answer(answerList.Count + 1, "E", 5));

            #endregion

            #region Categs
            int g = 14;
            normalCateg.Add(new int[] { ++g, 8, 4, 2, 1, 0, 62 });
            normalCateg.Add(new int[] { ++g, 8, 4, 3, 1, 0, normalCateg.Last()[6]+1 });
            normalCateg.Add(new int[] { ++g, 8, 5, 3, 1, 0, normalCateg.Last()[6] + 2 });
            normalCateg.Add(new int[] { ++g, 8, 5, 3, 2, 0, normalCateg.Last()[6] + 1 });
            normalCateg.Add(new int[] { ++g, 8, 6, 3, 2, 0, normalCateg.Last()[6] + 1 });
            normalCateg.Add(new int[] { ++g, 8, 6, 4, 2, 0, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 8, 6, 4, 2, 1, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 9, 6, 4, 2, 1, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 9, 7, 4, 2, 1, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 9, 7, 4, 3, 1, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 10, 7, 4, 3, 1, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 10, 7, 5, 3, 1, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 10, 7, 5, 4, 1, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 10, 7, 6, 4, 1, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 10, 7, 6, 5, 1, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 10, 7, 6, 5, 2, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 10, 7, 7, 5, 2, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 10, 8, 7, 5, 2, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 11, 8, 7, 5, 2, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 11, 8, 7, 6, 2, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 11, 8, 7, 7, 2, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 11, 8, 8, 7, 2, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 11, 9, 8, 7, 2, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 11, 9, 8, 8, 2, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 11, 10, 8, 8, 2, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 11, 10, 8, 8, 3, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 11, 10, 9, 8, 3, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 11, 10, 9, 9, 3, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 10, 9, 9, 3, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 12, 10, 9, 9, 4, normalCateg.Last()[6]+1  });
            normalCateg.Add(new int[] { ++g, 12, 10, 9, 9, 5, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 10, 10, 9, 5, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 10, 10, 9, 6, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 11, 10, 9, 6, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 11, 10, 10, 6, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 11, 10, 10, 7, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 11, 11, 10, 7, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 11, 11, 10, 8, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 11, 11, 11, 8, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 12, 11, 11, 8, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 12, 11, 11, 9, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 12, 12, 11, 9, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 12, 12, 11, 10, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 12, 12, 12, 10, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 12, 12, 12, 11, normalCateg.Last()[6]+2  });
            normalCateg.Add(new int[] { ++g, 12, 12, 12, 12, 12, normalCateg.Last()[6]+10  }); 
            #endregion
        }
    }
}
