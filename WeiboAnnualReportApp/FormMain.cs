using System.Diagnostics;
using System.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Python.Runtime;
using MyMapObjects;
using System.Globalization;
using System.Text;
using Mysqlx.Crud;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;


namespace WeiboAnnualReportApp
{
    public partial class FormMain : Form
    {
        #region �ֶ�

        private string connectionString = "server=127.0.0.1;user=root;password=lys150619; database=������΢������";
        private moMapcontrol moMap = new moMapcontrol();
        private bool booltest = false;
        
        #endregion

        #region ���캯��

        public FormMain()
        {
            InitializeComponent();
            // ��ʼ��һ��ͼ���ļ��Խ����ཻ�ж�
            String sFileName = "D:\\datasource\\WeiboAnnualReportApp\\WeiboAnnualReportApp\\bin\\Debug\\net6.0-windows7.0\\cities.shp";
            MyMapObjects.moMapLayer sLayer;
            try
            {
                sLayer = DataIOTools.LoadMapLayerFromShapeFile(sFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            moMap.Layers.Add(sLayer);  // ��control�ؼ������һ������ͼ��
        }

        #endregion

        #region �ؼ��¼�
        private void btnOpenReport_Click(object sender, EventArgs e)
        {

            if (booltest == false)
            {
                MessageBox.Show("���������û���ȱ���");
                return;
            }

            string basePath = Path.GetDirectoryName(Application.ExecutablePath);  // �ҵ��Ǹ�exe�ļ����ڵ�λ��
            string mainHtmlPath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage.html";
            OpenHtmlFile(mainHtmlPath);
            //webBrowserMap.Navigate(outputFilePath);
            //OpenHtmlFile(outputFilePath);
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {

            string id = textBox1.Text;
            string name = textBox1.Text;
            string visited_cities = "";

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("����������û�ID���ǳ�");
                return;
            }
            booltest = true;

            labelProgress.Text = "��ѯ�û�΢����Ϣ��...";
            this.Refresh();
            //�ǳƲ�ѯ��ȡID
            string query1 = "SELECT * FROM ������΢������.travel_poi_userinfo_suzhou WHERE ������΢������.travel_poi_userinfo_suzhou.screen_name = \"" + name + "\"";
            DataTable dt1 = new DataTable();
            using (MySqlConnection conn1 = new MySqlConnection(connectionString))
            {
                try
                {
                    conn1.Open();
                    MySqlCommand cmd1 = new MySqlCommand(query1, conn1);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd1);
                    adapter.Fill(dt1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (dt1.Rows.Count > 0)
            {
                id = dt1.Rows[0][0].ToString();
                string description = dt1.Rows[0][5].ToString();
                string gender = dt1.Rows[0][8].ToString();
                string followers = dt1.Rows[0][9].ToString();
                string friends = dt1.Rows[0][10].ToString();
                visited_cities = dt1.Rows[0][19].ToString();
                if (string.IsNullOrEmpty(description))
                    label2.Text = "����ǩ��������һ������û����ǩ��";
                else
                    label2.Text = "����ǩ����" + description;
                if (gender == "f")
                    label3.Text = "�Ա�С���";
                else if (gender == "m")
                    label3.Text = "�Ա�С���";
                label4.Text = "��˿������" + followers;
                label5.Text = "����������" + friends;
                label6.Text = "�û�ID��" + id;
                label7.Text = "�û��ǳƣ�" + name;
                
            }
            //ID��ȡ�ǳ�
            string query2 = "SELECT * FROM ������΢������.travel_poi_userinfo_suzhou WHERE ������΢������.travel_poi_userinfo_suzhou.id = " + id;
            DataTable dt2 = new DataTable();
            using (MySqlConnection conn2 = new MySqlConnection(connectionString))
            {
                try
                {
                    conn2.Open();
                    MySqlCommand cmd2 = new MySqlCommand(query2, conn2);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd2);
                    adapter.Fill(dt2);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (dt2.Rows.Count > 0)
            {
                name = dt2.Rows[0][1].ToString();
                string description = dt2.Rows[0][5].ToString();
                string gender = dt2.Rows[0][8].ToString();
                string followers = dt2.Rows[0][9].ToString();
                string friends = dt2.Rows[0][10].ToString();
                visited_cities = dt2.Rows[0][19].ToString();
                if (string.IsNullOrEmpty(description))
                {
                    label2.Text = "����ǩ��������һ������û����ǩ��";
                }
                else
                {
                    label2.Text = "����ǩ����" + description;
                }
                if (gender == "f")
                    label3.Text = "�Ա�С���";
                else if (gender == "m")
                    label3.Text = "�Ա�С���";
                label4.Text = "��˿������" + followers;
                label5.Text = "����������" + friends;
                label6.Text = "�û�ID��" + id;
                label7.Text = "�û��ǳƣ�" + name;
            }

            labelProgress.Text = "��ѯ΢���ռ���Ϣ��...";
            this.Refresh();
            string query = $"SELECT latitude,longitude,text,created_at FROM ������΢������.geotaggedweibo WHERE ������΢������.geotaggedweibo.userid = @id and ������΢������.geotaggedweibo.latitude < 90";
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    adapter.Fill(dt);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (dt.Rows.Count > 0)
            {
                //ShowMapWithPoints(dt);
                GenerateHtmlFile(dt, id, visited_cities);
            }
            else
            {
                MessageBox.Show("No data found for the provided ID �� name.");
            }
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region ˽�к���

        private void GenerateHtmlFile(DataTable dt, string id, string visited_cities)
        {
            string basePath = Path.GetDirectoryName(Application.ExecutablePath);  // �ҵ��Ǹ�exe�ļ����ڵ�λ��
            string templateFilePath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage_template.html";
            string templateFilePath1 = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage1_template.html";
            string outputFilePath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage.html";

            var points = dt.AsEnumerable().Select(row => new
            {
                latitude = row.Field<double>("latitude"),
                longitude = row.Field<double>("longitude"),
                text = row.Field<string>("text")
            }).ToList();

            labelProgress.Text = "���пռ���Ϣƥ��...";
            this.Refresh();
            Dictionary<String, int> freqMap;
            int numCity;
            getCity(dt, out freqMap, out numCity);  // ��ȡ���������ֵ��ȥ���ĳ�����������ֵ������������
            KeyValuePair<string, int> maxEntryCity = freqMap.OrderByDescending(entry => entry.Value).First();
            String topCity = maxEntryCity.Key;

            labelProgress.Text = "����΢�������Ϣ...";
            this.Refresh();
            Double avgSentiment;  // ����÷֣�1Ϊ������0Ϊ���
            Dictionary<string, int> wordFreqMap;  // ��Ƶ�ֲ�
            processPython(dt, out avgSentiment, out wordFreqMap);  // ��ȡƽ���ĸ���÷ֺʹ�Ƶ�ֲ�

            // ��������ԶԸ���÷ֽ���һЩ���ͺʹ���
            // �Ը�����з���
            String comment;
            if (avgSentiment > 0.8)
            {
                comment = "���������һ����÷ǳ����ģ�����������ͻ�����";
            }
            else if (avgSentiment > 0.6 && avgSentiment <= 0.8)
            {
                comment = "���һ�����кܶ�����ʱ�⣬�����˻�����������";
            }
            else if (avgSentiment > 0.4 && avgSentiment <= 0.6)
            {
                comment = "���һ���ﾭ���˸��ָ��������飬�и߳�Ҳ�е͹ȣ�������˵��һ��ƽ����һ�ꡣ";
            }
            else if (avgSentiment > 0.2 && avgSentiment <= 0.4)
            {
                comment = "һ���ȥ�ˣ����ƺ�������һЩ��ս�����ѣ������Ǽ����ǰ�С�";
            }
            else
            {
                comment = "���һ������ƺ���Щ��˳���������˲�����ս�����ѡ�";
            }

            // ��������ԶԴ�Ƶ�ֲ�����һЩ����
            // 1. ��ȡ˵�����Ĵ���
            KeyValuePair<string, int> maxEntry = wordFreqMap.OrderByDescending(entry => entry.Value).First();
            String topWord = maxEntry.Key;

            // ��ȡ���Ƶ��û������������û��������������
            labelProgress.Text = "ƥ�������û���Ϣ...";
            Stopwatch sw = Stopwatch.StartNew();
            this.Refresh();
            int userNum;
            DataTable dtSimilar = getSimilar(id, visited_cities, 6, out userNum);  // ���������������Զ������Ƚ϶��ٸ�һ��ȥ���ĳ��У���������Ϊ5
            sw.Stop();
            TimeSpan elapsedTime = sw.Elapsed;
            Debug.WriteLine(elapsedTime);
            // ������Զ������û������������û��ı���һЩ����
            string similarReport = "δ����";  // �����û����ֵ��ַ���
            if (userNum == 0)
            {
                similarReport = "����н��켣���ڲ�ͬ";
            }
            else if (userNum <= 4)  // ��������Ĭ��ֻ��ʾ4�������û������Ǻ��ڿ��Ը���
            {
                StringBuilder similarUsers = new StringBuilder();
                for (int i = 0; i < dtSimilar.Rows.Count; i++)
                {
                    string userName = dtSimilar.Rows[i]["nickname"].ToString();
                    similarUsers.Append(userName);
                    if (i < dtSimilar.Rows.Count - 1)
                    {
                        similarUsers.Append(", ");
                    }
                }
                similarReport =similarUsers.ToString();
            }
            else
            {
                StringBuilder similarUsers = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    string userName = dtSimilar.Rows[i]["nickname"].ToString();
                    similarUsers.Append(userName);
                    if (i < 3)
                    {
                        similarUsers.Append(", ");
                    }
                }
                similarReport = similarUsers.ToString();
            }

            string timesection;
            string latestdate;
            string latesttime;
            string latesttext;
            string posts;
            Analyzetime(dt, out timesection, out latestdate, out latesttime, out latesttext, out posts);

            string json = JsonConvert.SerializeObject(points);
            string phrase1 = numCity.ToString();//ȥ�����ٳ���
            string phrase2 = topCity;  //������õĳ���
            string phrase3 = timesection;  //ϲ����ʲôʱ�䷢΢��������/����/���ϣ�
            string phrase4 = topWord;  //��ϲ��˵�Ĵ���
            string phrase5 = latestdate;  //˯�������һ��
            string phrase6 = latesttime;  //������˯��
            string phrase7 = RemoveAfterSubstring(latesttext, "http");  //�Ǹ�ʱ��η���΢��
            string phrase8 = comment;  // ���û���е�һ������
            string phrase9 = posts;    //���ʱ��㷢��΢����
            string phrase10 = dt.Rows.Count.ToString();//һ�����˶���΢��
            string phrase11 = calculateTotalDistance(dt).ToString();//�ܹ��������
            string phrase12 = GetAfterSymbol(label7.Text,'��');//��ȡ�û��ǳ�
            string phrase13 = userNum.ToString();
            string phrase14 = similarReport;

            string template = "δ��ȡ";
            if (userNum > 0)
            {
                template = File.ReadAllText(templateFilePath);
            }
            else
            {
                template = File.ReadAllText(templateFilePath1);
            }
            string outputContent = template.Replace("{data_placeholder}", json)
            .Replace("{phrase1_placeholder}", phrase1)
            .Replace("{phrase2_placeholder}", phrase2)
            .Replace("{phrase3_placeholder}", phrase3)
            .Replace("{phrase4_placeholder}", phrase4)
            .Replace("{phrase5_placeholder}", phrase5)
            .Replace("{phrase6_placeholder}", phrase6)
            .Replace("{phrase7_placeholder}", phrase7)
            .Replace("{phrase8_placeholder}", phrase8)
            .Replace("{phrase9_placeholder}", phrase9)
            .Replace("{phrase10_placeholder}", phrase10)
            .Replace("{phrase11_placeholder}", phrase11)
            .Replace("{name_place}", phrase12)
            .Replace("{phrase13_placeholder}", phrase13)
            .Replace("{phrase14_placeholder}", phrase14);
            

            File.WriteAllText(outputFilePath, outputContent);
            labelProgress.Text = "���ɳɹ�����������ȱ���";
        }
        private void OpenHtmlFile(string filePath)
        {
            try
            {
                Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open the HTML file: " + ex.Message);
            }
        }

        /// <summary>
        /// ������Ҫ����python���������
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="avgSentiment"></param>
        /// <param name="wordFreqMap"></param>
        private void processPython(DataTable dt, out Double avgSentiment, out Dictionary<string, int> wordFreqMap)
        {
            if(Runtime.PythonDLL == null)
            {
                Runtime.PythonDLL = @"C:\Users\86151\AppData\Local\Programs\Python\Python38\python38.dll";
            }
            PythonEngine.Initialize();  // ��ʼ��python������
            var points = dt.AsEnumerable().Select(row => new
            {
                latitude = row.Field<double>("latitude"),
                longitude = row.Field<double>("longitude"),
                text = row.Field<string>("text")
            }).ToList();  // ��ȡdatatable����Ϣ
            wordFreqMap = new Dictionary<string, int>();  // ����Ƶ�α�
            List<Double> sentiments = new List<Double>();
            List<string> stopWords = new List<string>();

            // ��ȡͣ�ôʱ�
            string[] lines = File.ReadAllLines("wordDisable.txt");
            foreach (string line in lines)
            {
                stopWords.Add(line.Trim());
            }

            foreach (var point in points)
            {
                using (Py.GIL())
                {
                    var pythonScript = Py.Import("PythonProcessor");
                    var message = new PyString(point.text);
                    var sentiment = pythonScript.InvokeMethod("process_sentiment", new PyObject[] { message });
                    sentiments.Add(sentiment.ToDouble(CultureInfo.InvariantCulture));
                    var sWords = pythonScript.InvokeMethod("process_words", new PyObject[] { message });  // ��ô����б�
                    var pyList = sWords.As<PyList>();  // ��Python�б�ת��Ϊ��̬����
                    List<string> words = new List<string>();
                    foreach (var item in pyList)
                    {
                        words.Add(item.ToString(CultureInfo.InvariantCulture));
                    }
                    foreach (var word in words)
                    {   // ������ﲻ��ͣ�ôʱ���
                        if (!stopWords.Contains(word))
                        {
                            if (wordFreqMap.ContainsKey(word))
                            {
                                wordFreqMap[word]++;
                            }
                            else
                            {
                                wordFreqMap.Add(word, 1);
                            }
                        }
                    }
                }
            }
            avgSentiment = sentiments.Sum() / sentiments.Count;
        }

        /// <summary>
        /// ����û�ȥ���ĳ����Լ���������
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="freqMap"></param>
        /// <param name="numCity"></param>
        private void getCity(DataTable dt, out Dictionary<string, int> freqMap, out int numCity)
        {
            var points = dt.AsEnumerable().Select(row => new
            {
                latitude = row.Field<double>("latitude"),
                longitude = row.Field<double>("longitude"),
                text = row.Field<string>("text")
            }).ToList();
            List<String> sCities = new List<String>();  // ��¼����΢���ĳ���˳���
            foreach (var point in points)
            {
                moPoint sPoint = new moPoint(point.longitude, point.latitude);  // ��΢���ĵ�
                moFeatures sFeatures = moMap.Layers.GetItem(0).SearchFeaturesByPoint(sPoint, 0);  // ���Ұ������Ҫ��
                if (sFeatures.Count != 0)
                {
                    moAttributes sAttributes = sFeatures.GetItem(0).Attributes;
                    sCities.Add((String)sAttributes.GetItem(1));  // ����΢���ĳ�����ӵ�����
                }
            }
            freqMap = new Dictionary<string, int>();  // ����һ������Ƶ�ʱ�����ͳ���ĸ����з������
            foreach (string str in sCities)
            {
                if (freqMap.ContainsKey(str))
                {
                    freqMap[str]++;
                }
                else
                {
                    freqMap.Add(str, 1);
                }
            }

            numCity = freqMap.Count;
        }

        private DataTable getSimilar(string id, string visited_cities, int cityCompareLen, out int userNum)
        {
            string[] cities = Array.Empty<string>();
            if (string.IsNullOrEmpty(visited_cities) || visited_cities.Split(',').Length <= cityCompareLen)
            {
                cities = visited_cities.Split(',');
            }
            else
            {
                cities = visited_cities.Split(',').Take(cityCompareLen).ToArray();
            }
            StringBuilder cityParams = new StringBuilder();
            for (int i = 0; i < cities.Length; i++)
            {
                string paramName = "@city" + i;
                cityParams.Append(paramName);
                if (i < cities.Length - 1)
                    cityParams.Append(",");
            }

            string querySimilar =
                @"SELECT id, nickname
        FROM location_user_invert
        WHERE city_Name IN (" + cityParams.ToString() + @")
        GROUP BY id
        HAVING COUNT(DISTINCT city_name) = @cityCount AND id != @mainId;";

            DataTable dtUsers = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(querySimilar, conn);

                    // ��ӳ��в���
                    for (int i = 0; i < cities.Length; i++)
                    {
                        string paramName = "@city" + i;
                        cmd.Parameters.AddWithValue(paramName, cities[i]);
                    }

                    // ���ó�����������
                    cmd.Parameters.AddWithValue("@cityCount", cities.Length);
                    cmd.Parameters.AddWithValue("@mainId", id);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dtUsers);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            userNum = dtUsers.Rows.Count;
            return dtUsers;
        }

        private Dictionary<DateTime, string> postInfo = new Dictionary<DateTime, string>();
        private void Analyzetime(DataTable dt, out String timesection, out String latestdate, out String latesttime, out String latesttext, out String posts)
        {
            List<DateTime> postTimes = new List<DateTime>();

            foreach (DataRow row in dt.Rows)
            {
                string postTimeString = row["created_at"].ToString();
                string postText = row["text"].ToString();
                if (DateTime.TryParseExact(postTimeString, "ddd MMM dd HH:mm:ss K yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime postTime))
                {
                    postTimes.Add(postTime);
                    postInfo[postTime] = postText;
                }
            }

            //if (postTimes.Count == 0)
            //{
            //    MessageBox.Show("No valid post times found.");
            //    return;
            //}

            /*
            var latestDay = postTimes.GroupBy(t => t.Hour)
                                     .OrderByDescending(g => g.Key)
                                     .FirstOrDefault();
            */
            //var latestDay = postTimes.Max(t => t.TimeOfDay);
            //�ҵ�����ķ���ʱ��
            var latestPost = postTimes.GroupBy(p => p.Date)
                                        .Select(g => g.OrderByDescending(p => p.TimeOfDay).First())
                                        .OrderByDescending(p => p.TimeOfDay).FirstOrDefault();
            //������ʱ���Ӧ������
            var latestPostText = postInfo[latestPost];

            // ����Сʱͳ�Ʒ�������
            var mostActiveHour = postTimes.GroupBy(t => t.Hour)
                                          .OrderByDescending(g => g.Count())
                                          .FirstOrDefault();

            latesttext = latestPostText;
            latestdate = latestPost.Year.ToString() + "��" + latestPost.Month.ToString() + "��" + latestPost.Day.ToString() + "��";
            timesection = mostActiveHour.Key.ToString();
            posts = mostActiveHour.Count().ToString();
            latesttime = latestPost.TimeOfDay.ToString();

            // ������
            //StringBuilder resultMessage = new StringBuilder();

            //if (latestPost != null)
            //{
            //    resultMessage.AppendLine($"Latest post Time:  {latestPost}����{latestPostText}");

            //}
            //if (mostActiveHour != null)
            //{
            //    resultMessage.AppendLine($"Most active hour: {mostActiveHour.Key}:00 with {mostActiveHour.Count()} posts");
            //}

            //MessageBox.Show(resultMessage.ToString());
        }

        /// <summary>
        /// ���ڼ����û��ص���Ϣ��һ���Խű������ظ�����
        /// </summary>
        private void AddCityInfo()
        {
            string queryId = $"SELECT id FROM ������΢������.travel_poi_userinfo_suzhou";  // �Ȳ�ѯ�����е�΢���û�id
            DataTable dtId = new DataTable();  // �û���id��һ��
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(queryId, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dtId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                int numRow = 0;
                foreach (DataRow row in dtId.Rows)
                {
                    ++numRow;
                    string userId = row[0].ToString();  // ��ȡ�û�id��id�ڱ�ĵ�һ��
                    string queryCity = $"SELECT latitude, longitude, text FROM ������΢������.geotaggedweibo WHERE ������΢������.geotaggedweibo.userid = @id";
                    DataTable dt = new DataTable();
                    try
                    {
                        MySqlCommand cmdCity = new MySqlCommand(queryCity, conn);
                        cmdCity.Parameters.AddWithValue("@id", userId);
                        MySqlDataAdapter adapterCity = new MySqlDataAdapter(cmdCity);
                        adapterCity.Fill(dt);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error occurred. Error message: {ex.Message}");
                    }
                    getCity(dt, out Dictionary<String, int> freqMap, out int numCity);  // ���ݱ�񷵻��û�ȥ���ĳ����ֵ�
                                                                                        // ��freqMap�����еĳ��з���sql����в�����
                    string limitedCities;
                    if (freqMap.Count > 7)
                    {
                        // ��Ƶ���ֵ䰴ֵ����Ƶ�Σ��������򣬲�ѡ��ǰ7������
                        var sortedCities = freqMap.OrderByDescending(pair => pair.Value).Take(7).Select(pair => pair.Key);
                        limitedCities = string.Join(",", sortedCities);  // ѡȡǰ7�����в�ƴ�ӳ��ַ���
                    }
                    else
                    {
                        limitedCities = string.Join(",", freqMap.Keys);  // ����Ҫ����ֱ��ʹ�����г��У���ʡ������Դ
                    }
                    try
                    {
                        string query = $"UPDATE ������΢������.travel_poi_userinfo_suzhou SET visited_cities = CONCAT_WS(',', @cities) WHERE id = @userId;";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@cities", limitedCities);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine(numRow);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error occurred. limitedCities: {limitedCities}.  id: {userId}. Error message: {ex.Message}");
                    }
                }
            }

        }

        /// <summary>
        /// ���ڴ����û��ص㵹�ű��һ���Խű������ظ�����
        /// </summary>
        private void CreateInvert()
        {
            string queryId = @"SELECT id, screen_name, visited_cities FROM ������΢������.travel_poi_userinfo_suzhou;";
            DataTable dtId = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(queryId, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dtId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            var users = dtId.AsEnumerable().Select(row => new
            {
                id = row.Field<Int64>("id"),
                screen_name = row.Field<string>("screen_name"),
                visited_cities = row.Field<string>("visited_cities")
            }).ToList();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    int progress = 0;  // ���ƽ�����
                    foreach (var user in users)
                    {
                        ++progress;
                        Debug.WriteLine(progress);
                        string[] cities = user.visited_cities.Split(',');
                        foreach (string city in cities)
                        {
                            if (!string.IsNullOrEmpty(city.Trim()))
                            {
                                MySqlCommand cmd = new MySqlCommand("INSERT INTO ������΢������.location_user_invert (city_name, id, nickname) VALUES (@city, @id, @screen_name)", conn);
                                cmd.Parameters.AddWithValue("@city", city.Trim());
                                cmd.Parameters.AddWithValue("@id", user.id);
                                cmd.Parameters.AddWithValue("@screen_name", user.screen_name);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //�����������ϵ�������
        double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadius = 6371; // ����뾶����λΪǧ��

            // ����γ��ת��Ϊ����
            double lat1Rad = ToRadians(lat1);
            double lon1Rad = ToRadians(lon1);
            double lat2Rad = ToRadians(lat2);
            double lon2Rad = ToRadians(lon2);

            // ʹ�� Haversine ��ʽ��������֮��ľ���
            double dlon = lon2Rad - lon1Rad;
            double dlat = lat2Rad - lat1Rad;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Pow(Math.Sin(dlon / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = earthRadius * c;

            return distance;
        }
        // ת��Ϊ����
        double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
        // ���������ܳ���
        public double calculateTotalDistance(DataTable dt)
        {
            double totalDistance = 0.0;

            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                double lat1 = Convert.ToDouble(dt.Rows[i]["latitude"]);
                double lon1 = Convert.ToDouble(dt.Rows[i]["longitude"]);
                double lat2 = Convert.ToDouble(dt.Rows[i + 1]["latitude"]);
                double lon2 = Convert.ToDouble(dt.Rows[i + 1]["longitude"]);

                double distance = CalculateDistance(lat1, lon1, lat2, lon2);
                totalDistance += distance;
            }
            return totalDistance;
        }

        //�Ƴ��ַ������ض��ַ��������
        private string RemoveAfterSubstring(string input, string substring)
        {
            int index = input.IndexOf(substring);
            if (index != -1)
            {
                return input.Substring(0, index);
            }
            else
            {
                return input;
            }
        }

        //��ȡ�ַ����ض��ַ��������
        private string GetAfterSymbol(string original, char symbol)
        {
            // �ҵ��ض����ŵ�λ��
            int index = original.IndexOf(symbol);

            // ����ҵ��˷��ţ����ط��ź���Ĳ��֣����򷵻�ԭʼ�ַ���
            if (index != -1)
            {
                return original.Substring(index + 1);
            }
            else
            {
                return original;
            }
        }
        #endregion


    }
}

    # region ע��/���ڴ���
/*private void ShowMapWithPoints0(DataTable dt)
       {
           StringBuilder htmlBuilder = new StringBuilder();
           htmlBuilder.AppendLine("<!doctype html>");
           htmlBuilder.AppendLine("<html lang=\"en\">");
           htmlBuilder.AppendLine("<head>");
           htmlBuilder.AppendLine("    <meta charset=\"utf-8\">");
           htmlBuilder.AppendLine("    <meta http-equiv=\"X-UA-Compatible\" content=\"chrome=1\">");
           htmlBuilder.AppendLine("    <meta name=\"viewport\" content=\"initial-scale=1.0, user-scalable=yes, width=device-width\">");
           htmlBuilder.AppendLine("    <style type=\"text/css\">");
           htmlBuilder.AppendLine("        body, html, #container { height: 100%; margin: 0px; font: 12px Arial; }");
           htmlBuilder.AppendLine("        .taiwan { border: solid 1px red; color: red; float: left; width: 50px; background-color: rgba(255, 0, 0, 0.1); }");
           htmlBuilder.AppendLine("    </style>");
           htmlBuilder.AppendLine("    <title>Marker example</title>");
           htmlBuilder.AppendLine("</head>");
           htmlBuilder.AppendLine("<body>");
           htmlBuilder.AppendLine("    <div id=\"container\" tabindex=\"0\"></div>");
           htmlBuilder.AppendLine("    <script src=\"https://webapi.amap.com/js/marker.js\"></script>");
           htmlBuilder.AppendLine("    <script src=\"https://webapi.amap.com/maps?v=1.4.15&key=cd6579ead366b5d7c3b6802c2a5af67e\"></script>");
           htmlBuilder.AppendLine("    <script src=\"https://cdn.jsdelivr.net/npm/echarts-extension-amap@1.10.2/dist/echarts-extension-amap.min.js\"></script>");
           htmlBuilder.AppendLine("    <script type=\"text/javascript\">");

           htmlBuilder.AppendLine("        var map = new AMap.Map('container', { resizeEnable: true, zoom: 5 });");
           htmlBuilder.AppendLine("        var markers = [];");
           htmlBuilder.AppendLine("        var path = [];");

           foreach (DataRow row in dt.Rows)
           {
               string lat = row["latitude"].ToString();
               string lng = row["longitude"].ToString();
               string info = row["info"].ToString();

               htmlBuilder.AppendLine($"        var marker = new AMap.Marker({{");
               htmlBuilder.AppendLine($"            position: new AMap.LngLat({lng}, {lat}),");
               htmlBuilder.AppendLine($"            title: '{info}',");
               htmlBuilder.AppendLine("            map: map");
               htmlBuilder.AppendLine("        });");

               htmlBuilder.AppendLine($"        markers.push(marker);");
               htmlBuilder.AppendLine($"        path.push(new AMap.LngLat({lng}, {lat}));");

               htmlBuilder.AppendLine($"        var infoWindow = new AMap.InfoWindow({{");
               htmlBuilder.AppendLine($"            content: '{info}'");
               htmlBuilder.AppendLine("        });");

               htmlBuilder.AppendLine("        AMap.event.addListener(marker, 'click', (function(marker, infoWindow) {");
               htmlBuilder.AppendLine("            return function() {");
               htmlBuilder.AppendLine("                infoWindow.open(map, marker.getPosition());");
               htmlBuilder.AppendLine("            }");
               htmlBuilder.AppendLine("        })(marker, infoWindow));");
           }

           htmlBuilder.AppendLine("        var bezierCurve = new AMap.BezierCurve({");
           htmlBuilder.AppendLine("            path: path,");
           htmlBuilder.AppendLine("            strokeColor: '#FF33FF',");
           htmlBuilder.AppendLine("            strokeWeight: 6,");
           htmlBuilder.AppendLine("            strokeOpacity: 0.5,");
           htmlBuilder.AppendLine("            isOutline: true,");
           htmlBuilder.AppendLine("            outlineColor: '#000000'");

           htmlBuilder.AppendLine("        });");
           htmlBuilder.AppendLine("        bezierCurve.setMap(map);");
           htmlBuilder.AppendLine("        map.setFitView();");
           htmlBuilder.AppendLine("    </script>");
           htmlBuilder.AppendLine("    <script type=\"text/javascript\" src=\"https://webapi.amap.com/demos/js/liteToolbar.js\"></script>");
           htmlBuilder.AppendLine("</body>");
           htmlBuilder.AppendLine("</html>");

           webBrowserMap.DocumentText = htmlBuilder.ToString();
       }*/
/*private void ShowMapWithPoints(DataTable dt)
{
    string html = GenerateHtmlFile(dt);
    File.WriteAllText("map.html", html);
    webBrowserMap.Navigate(new Uri(Path.Combine(Application.StartupPath, "map.html")));
}

private string GenerateHtmlFile(DataTable dt)
{

    string htmlTemplate = $@"
    <!DOCTYPE html>
<html lang=""zh-CN"">
<head>
<meta charset=""utf-8"">
<title>MapVGL</title>
<meta http-equiv=""X-UA-Compatible"" content=""IE=Edge"">
<meta name=""viewport"" content=""initial-scale=1.0, user-scalable=no"">
<style>
html,
body {{
width: 100%;
height: 100%;
margin: 0;
padding: 0;
}}
#map_container {{
width: 100%;
height: 100%;
margin: 0;
}}
</style>
<script src=""https://api.map.baidu.com/api?v=1.0&type=webgl&ak=fpMKa6sGENtDsvgT3iA3MErslqRNK5Di""></script>
<script src=""https://unpkg.com/mapvgl/dist/mapvgl.min.js""></script>
<script src=""https://unpkg.com/mapvgl/dist/mapvgl.threelayers.min.js""></script>
</head>
<body>
<div id=""map_container""></div>
<script>
// 1. ������ͼʵ��
var bmapgl = new BMapGL.Map('map_container');
var point = new BMapGL.Point(116.403748, 39.915055);
bmapgl.centerAndZoom(point, 17);

// 2. ����MapVGLͼ�������
var view = new mapvgl.View({{
map: bmapgl
}});

// 3. �������ӻ�ͼ�㣬����ӵ�ͼ���������
var layer = new mapvgl.PointLayer({{
color: 'rgba(50, 50, 200, 1)',
blend: 'lighter',
size: 15
}});
view.addLayer(layer);

// 4. ׼���ù淶����������
var data = [{{
geometry: {{
type: 'Point',
coordinates: [116.403748, 39.915055]
}}
}}];

// 5. ����ͼ�������ݣ������𺳵Ŀ��ӻ�Ч��
layer.setData(data);
</script>
</body>
</html>
";

    return htmlTemplate;
}
*/


//string basePath = Path.GetDirectoryName(Application.ExecutablePath);
//string mainHtmlPath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage.html";
//string html = "<!DOCTYPE html>\r\n" +//ҳ������
//    "<html lang=\"en\">\r\n" +
//    "<head>\r\n" +
//    "<meta charset=\"UTF-8\">\r\n" +
//    "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n" +
//    "<title>�����л�ҳ��</title>\r\n" +

//    "<style>\r\n" +//css�ļ��༭����ʽ����
//    "body, html {\r\n" +
//    "margin: 0;\r\n" +
//    "padding: 0;\r\n" +
//    "overflow: hidden;\r\n" +
//    "height: 100%;\r\n" +
//    "font-family: Arial, sans-serif;\r\n" +
//    "}\r\n" +

//    ".page {\r\n" +
//    "position: absolute;\r\n" +
//    "width: 100%;\r\n" +
//    "height: 100%;\r\n" +
//    "display: flex;\r\n" +
//    "justify-content: center;\r\n" +
//    "align-items: center;\r\n" +
//    "transition: transform 0.5s ease-in-out;\r\n" +
//    "}\r\n" +

//    ".s-fcRed{\r\n" +
//    "color: red; /* ����������ɫΪ��ɫ */\r\n" +
//    "font-style: italic; /* ��������Ϊб�� */\r\n" +
//    "}\r\n" +
//    ".text-wrapper {\r\n" +
//    "position: absolute;\r\n" +
//    "top: 50%; /* �����ְ�����������ҳ��Ĵ�ֱ�м� */\r\n" +
//    "transform: translateY(-50%);\r\n" +
//    "width: 100%;\r\n" +
//    "text-align: center; /* ���־��� */\r\n" +
//    "}\r\n" +

//    "#page1 {\r\n" +
//    "background-color: #f1c40f;\r\n" +
//    "transform: translateY(0);\r\n" +
//    "}\r\n" +

//    "#page2 {\r\n" +
//    "background-color: #e74c3c;\r\n" +
//    "transform: translateY(100%);\r\n" +
//    "}\r\n" +

//    "#page3 {\r\n" +
//    "background-color: #3498db;\r\n" +
//    "transform: translateY(200%);\r\n" +
//    "}\r\n" +

//    "#page4 {\r\n" +
//    "background-color: #3498db;\r\n" +
//    "transform: translateY(300%);\r\n" +
//    "}\r\n" +

//    "#page5 {\r\n" +
//    "background-color: #3498db;\r\n" +
//    "transform: translateY(400%);\r\n" +
//    "}\r\n" +

//    "#page6 {\r\n" +
//    "background-color: #3498db;\r\n" +
//    "transform: translateY(500%);\r\n" +
//    "}\r\n" +
//    "</style>\r\n" +
//    "</head>\r\n" +

//    "<body>\r\n" +//ҳ������༭

//    "<div class=\"container\" id=\"container\">\r\n" +

//    "<div class=\"page\" id=\"page1\">\r\n" +//��һ��ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<h1 style=\"letter-spacing:.2em;font-size:29px;\">�ҵ������ռ�</h1>\r\n" +
//    "<p style=\"letter-spacing:.3em;font-size:14px;\">����һ�����΢��ʹ�ñ���</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page2\">\r\n" +//�ڶ���ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<h1>��һ����</h1>\r\n" +
//    "<p>��һ��ȥ��<em class=\"s-fcRed\">13</em>������</p>\r\n" +
//    "<p>������õ���<em class=\"s-fcRed\">�Ͼ�</em></p>\r\n" +
//    "<p>&nbsp;</p>\r\n" +
//    "<p>�����еķ�����</p>\r\n" +
//    "<p>ϲ����<em class=\"s-fcRed\">����</em>����΢��</p>\r\n" +
//    "<p>&nbsp;</p>\r\n" +
//    "<p>����ϲ��˵�Ĵ�����<em class=\"s-fcRed\">���Ȱ�</em></p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page3\">\r\n" +//������ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p><em class=\"s-fcRed\">8��19��</em></p>\r\n" +
//    "<p>��һ����˯�ú���</p>\r\n" +
//    "<p><em class=\"s-fcRed\">4��11</em>������΢�����</p>\r\n" +
//    "<p>&nbsp;</p>\r\n" +
//    "<p>��һ���㷢�˸�΢��</p>\r\n" +
//    "<p><em class=\"s-fcRed\">������賿�ĵ��������</em></p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page4\">\r\n" +//������ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p>���ǵ���ҳ</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page5\">\r\n" +//������ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p>���ǵ���ҳ</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page6\">\r\n" +//������ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p>���ǵ���ҳ</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "</div>\r\n" +

//    "<script>\r\n" +//Javascript�༭����������
//    "document.addEventListener('DOMContentLoaded', () => {\r\n" +
//    "let currentPage = 0;\r\n" +
//    "const pages = document.querySelectorAll('.page');\r\n" +
//    "const switchPage = (direction) => {\r\n" +
//    "if (direction === 'up' && currentPage > 0) {\r\n" +
//    "currentPage--;\r\n" +
//    "} else if (direction === 'down' && currentPage < pages.length - 1) {\r\n" +
//    "currentPage++;\r\n" +
//    "}\r\n" +
//    "pages.forEach((page, index) => {\r\n" +
//    "page.style.transform = `translateY(${(index - currentPage) * 100}%)`;\r\n" +
//    "});\r\n" +
//    "};\r\n" +
//    "function handleScroll(event) {\r\n" +
//    "if (event.deltaY > 0) {\r\n" +
//    "switchPage('down');\r\n" +//ҳ���»�
//    "} else {\r\n" +
//    "switchPage('up');\r\n" +//ҳ���ϻ�
//    "}\r\n" +
//    "showPage(currentPage);\r\n" +
//    "}\r\n" +
//    "window.addEventListener('wheel', handleScroll);\r\n" +
//    "});\r\n" +
//    "</script>\r\n" +
//    "</body>\r\n" +
//    "</html>";


//File.WriteAllText(mainHtmlPath, html);
# endregion
