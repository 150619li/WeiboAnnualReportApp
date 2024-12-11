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
        #region 字段

        private string connectionString = "server=127.0.0.1;user=root;password=lys150619; database=苏州市微博数据";
        private moMapcontrol moMap = new moMapcontrol();
        private bool booltest = false;
        
        #endregion

        #region 构造函数

        public FormMain()
        {
            InitializeComponent();
            // 初始化一个图层文件以进行相交判断
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
            moMap.Layers.Add(sLayer);  // 在control控件中添加一个城市图层
        }

        #endregion

        #region 控件事件
        private void btnOpenReport_Click(object sender, EventArgs e)
        {

            if (booltest == false)
            {
                MessageBox.Show("请先生成用户年度报告");
                return;
            }

            string basePath = Path.GetDirectoryName(Application.ExecutablePath);  // 找到那个exe文件所在的位置
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
                MessageBox.Show("请输入你的用户ID或昵称");
                return;
            }
            booltest = true;

            labelProgress.Text = "查询用户微博信息中...";
            this.Refresh();
            //昵称查询获取ID
            string query1 = "SELECT * FROM 苏州市微博数据.travel_poi_userinfo_suzhou WHERE 苏州市微博数据.travel_poi_userinfo_suzhou.screen_name = \"" + name + "\"";
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
                    label2.Text = "个性签名：这个家伙很懒，没留下签名";
                else
                    label2.Text = "个性签名：" + description;
                if (gender == "f")
                    label3.Text = "性别：小姐姐";
                else if (gender == "m")
                    label3.Text = "性别：小哥哥";
                label4.Text = "粉丝数量：" + followers;
                label5.Text = "好友数量：" + friends;
                label6.Text = "用户ID：" + id;
                label7.Text = "用户昵称：" + name;
                
            }
            //ID获取昵称
            string query2 = "SELECT * FROM 苏州市微博数据.travel_poi_userinfo_suzhou WHERE 苏州市微博数据.travel_poi_userinfo_suzhou.id = " + id;
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
                    label2.Text = "个性签名：这个家伙很懒，没留下签名";
                }
                else
                {
                    label2.Text = "个性签名：" + description;
                }
                if (gender == "f")
                    label3.Text = "性别：小姐姐";
                else if (gender == "m")
                    label3.Text = "性别：小哥哥";
                label4.Text = "粉丝数量：" + followers;
                label5.Text = "好友数量：" + friends;
                label6.Text = "用户ID：" + id;
                label7.Text = "用户昵称：" + name;
            }

            labelProgress.Text = "查询微博空间信息中...";
            this.Refresh();
            string query = $"SELECT latitude,longitude,text,created_at FROM 苏州市微博数据.geotaggedweibo WHERE 苏州市微博数据.geotaggedweibo.userid = @id and 苏州市微博数据.geotaggedweibo.latitude < 90";
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
                MessageBox.Show("No data found for the provided ID 或 name.");
            }
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region 私有函数

        private void GenerateHtmlFile(DataTable dt, string id, string visited_cities)
        {
            string basePath = Path.GetDirectoryName(Application.ExecutablePath);  // 找到那个exe文件所在的位置
            string templateFilePath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage_template.html";
            string templateFilePath1 = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage1_template.html";
            string outputFilePath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage.html";

            var points = dt.AsEnumerable().Select(row => new
            {
                latitude = row.Field<double>("latitude"),
                longitude = row.Field<double>("longitude"),
                text = row.Field<string>("text")
            }).ToList();

            labelProgress.Text = "进行空间信息匹配...";
            this.Refresh();
            Dictionary<String, int> freqMap;
            int numCity;
            getCity(dt, out freqMap, out numCity);  // 获取城市批次字典和去过的城市数量并赋值到两个变量上
            KeyValuePair<string, int> maxEntryCity = freqMap.OrderByDescending(entry => entry.Value).First();
            String topCity = maxEntryCity.Key;

            labelProgress.Text = "计算微博情感信息...";
            this.Refresh();
            Double avgSentiment;  // 感情得分，1为最正向，0为最负向
            Dictionary<string, int> wordFreqMap;  // 词频分布
            processPython(dt, out avgSentiment, out wordFreqMap);  // 获取平均的感情得分和词频分布

            // 在这里可以对感情得分进行一些解释和处理
            // 对感情进行分类
            String comment;
            if (avgSentiment > 0.8)
            {
                comment = "看起来你的一年过得非常开心，充满了阳光和活力！";
            }
            else if (avgSentiment > 0.6 && avgSentiment <= 0.8)
            {
                comment = "你的一年里有很多愉快的时光，充满了积极的能量！";
            }
            else if (avgSentiment > 0.4 && avgSentiment <= 0.6)
            {
                comment = "你的一年里经历了各种各样的事情，有高潮也有低谷，总体来说是一个平淡的一年。";
            }
            else if (avgSentiment > 0.2 && avgSentiment <= 0.4)
            {
                comment = "一年过去了，你似乎遇到了一些挑战和困难，但还是坚持着前行。";
            }
            else
            {
                comment = "你的一年过得似乎有些不顺利，遇到了不少挑战和困难。";
            }

            // 在这里可以对词频分布进行一些处理
            // 1. 获取说的最多的词语
            KeyValuePair<string, int> maxEntry = wordFreqMap.OrderByDescending(entry => entry.Value).First();
            String topWord = maxEntry.Key;

            // 获取相似的用户，并将相似用户的数量输出出来
            labelProgress.Text = "匹配相似用户信息...";
            Stopwatch sw = Stopwatch.StartNew();
            this.Refresh();
            int userNum;
            DataTable dtSimilar = getSimilar(id, visited_cities, 6, out userNum);  // 第三个参数可以自定义最多比较多少个一样去过的城市，这里先设为5
            sw.Stop();
            TimeSpan elapsedTime = sw.Elapsed;
            Debug.WriteLine(elapsedTime);
            // 这里可以对相似用户数量和相似用户的表做一些处理
            string similarReport = "未定义";  // 最后给用户呈现的字符串
            if (userNum == 0)
            {
                similarReport = "你的行进轨迹与众不同";
            }
            else if (userNum <= 4)  // 这里我们默认只显示4个相似用户，但是后期可以更改
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
            string phrase1 = numCity.ToString();//去过多少城市
            string phrase2 = topCity;  //待得最久的城市
            string phrase3 = timesection;  //喜欢在什么时间发微博（上午/下午/晚上）
            string phrase4 = topWord;  //最喜欢说的词语
            string phrase5 = latestdate;  //睡的最晚的一天
            string phrase6 = latesttime;  //几点钟睡的
            string phrase7 = RemoveAfterSubstring(latesttext, "http");  //那个时间段发的微博
            string phrase8 = comment;  // 对用户情感的一个评价
            string phrase9 = posts;    //最多时间点发表微博数
            string phrase10 = dt.Rows.Count.ToString();//一共发了多少微博
            string phrase11 = calculateTotalDistance(dt).ToString();//跑过的总里程
            string phrase12 = GetAfterSymbol(label7.Text,'：');//获取用户昵称
            string phrase13 = userNum.ToString();
            string phrase14 = similarReport;

            string template = "未读取";
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
            labelProgress.Text = "生成成功！请点击打开年度报告";
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
        /// 所有需要进行python处理的事项
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
            PythonEngine.Initialize();  // 初始化python启动器
            var points = dt.AsEnumerable().Select(row => new
            {
                latitude = row.Field<double>("latitude"),
                longitude = row.Field<double>("longitude"),
                text = row.Field<string>("text")
            }).ToList();  // 获取datatable的信息
            wordFreqMap = new Dictionary<string, int>();  // 词语频次表
            List<Double> sentiments = new List<Double>();
            List<string> stopWords = new List<string>();

            // 读取停用词表
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
                    var sWords = pythonScript.InvokeMethod("process_words", new PyObject[] { message });  // 获得词语列表
                    var pyList = sWords.As<PyList>();  // 将Python列表转换为动态对象
                    List<string> words = new List<string>();
                    foreach (var item in pyList)
                    {
                        words.Add(item.ToString(CultureInfo.InvariantCulture));
                    }
                    foreach (var word in words)
                    {   // 如果词语不在停用词表内
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
        /// 获得用户去过的城市以及城市数量
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
            List<String> sCities = new List<String>();  // 记录所有微博的城市顺序表
            foreach (var point in points)
            {
                moPoint sPoint = new moPoint(point.longitude, point.latitude);  // 发微博的点
                moFeatures sFeatures = moMap.Layers.GetItem(0).SearchFeaturesByPoint(sPoint, 0);  // 查找包含点的要素
                if (sFeatures.Count != 0)
                {
                    moAttributes sAttributes = sFeatures.GetItem(0).Attributes;
                    sCities.Add((String)sAttributes.GetItem(1));  // 将该微博的城市添加到表中
                }
            }
            freqMap = new Dictionary<string, int>();  // 创建一个城市频率表，用于统计哪个城市发博最多
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

                    // 添加城市参数
                    for (int i = 0; i < cities.Length; i++)
                    {
                        string paramName = "@city" + i;
                        cmd.Parameters.AddWithValue(paramName, cities[i]);
                    }

                    // 设置城市数量参数
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
            //找到最晚的发帖时间
            var latestPost = postTimes.GroupBy(p => p.Date)
                                        .Select(g => g.OrderByDescending(p => p.TimeOfDay).First())
                                        .OrderByDescending(p => p.TimeOfDay).FirstOrDefault();
            //最晚发帖时间对应的内容
            var latestPostText = postInfo[latestPost];

            // 按照小时统计发帖数量
            var mostActiveHour = postTimes.GroupBy(t => t.Hour)
                                          .OrderByDescending(g => g.Count())
                                          .FirstOrDefault();

            latesttext = latestPostText;
            latestdate = latestPost.Year.ToString() + "年" + latestPost.Month.ToString() + "月" + latestPost.Day.ToString() + "日";
            timesection = mostActiveHour.Key.ToString();
            posts = mostActiveHour.Count().ToString();
            latesttime = latestPost.TimeOfDay.ToString();

            // 输出结果
            //StringBuilder resultMessage = new StringBuilder();

            //if (latestPost != null)
            //{
            //    resultMessage.AppendLine($"Latest post Time:  {latestPost}内容{latestPostText}");

            //}
            //if (mostActiveHour != null)
            //{
            //    resultMessage.AppendLine($"Most active hour: {mostActiveHour.Key}:00 with {mostActiveHour.Count()} posts");
            //}

            //MessageBox.Show(resultMessage.ToString());
        }

        /// <summary>
        /// 用于加入用户地点信息的一次性脚本，勿重复运行
        /// </summary>
        private void AddCityInfo()
        {
            string queryId = $"SELECT id FROM 苏州市微博数据.travel_poi_userinfo_suzhou";  // 先查询到所有的微博用户id
            DataTable dtId = new DataTable();  // 用户的id表，一列
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
                    string userId = row[0].ToString();  // 获取用户id，id在表的第一列
                    string queryCity = $"SELECT latitude, longitude, text FROM 苏州市微博数据.geotaggedweibo WHERE 苏州市微博数据.geotaggedweibo.userid = @id";
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
                    getCity(dt, out Dictionary<String, int> freqMap, out int numCity);  // 根据表格返回用户去过的城市字典
                                                                                        // 将freqMap中所有的城市放入sql语句中并插入
                    string limitedCities;
                    if (freqMap.Count > 7)
                    {
                        // 将频次字典按值（即频次）降序排序，并选择前7个城市
                        var sortedCities = freqMap.OrderByDescending(pair => pair.Value).Take(7).Select(pair => pair.Key);
                        limitedCities = string.Join(",", sortedCities);  // 选取前7个城市并拼接成字符串
                    }
                    else
                    {
                        limitedCities = string.Join(",", freqMap.Keys);  // 不需要排序，直接使用所有城市，节省计算资源
                    }
                    try
                    {
                        string query = $"UPDATE 苏州市微博数据.travel_poi_userinfo_suzhou SET visited_cities = CONCAT_WS(',', @cities) WHERE id = @userId;";
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
        /// 用于创建用户地点倒排表的一次性脚本，勿重复运行
        /// </summary>
        private void CreateInvert()
        {
            string queryId = @"SELECT id, screen_name, visited_cities FROM 苏州市微博数据.travel_poi_userinfo_suzhou;";
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
                    int progress = 0;  // 自制进度条
                    foreach (var user in users)
                    {
                        ++progress;
                        Debug.WriteLine(progress);
                        string[] cities = user.visited_cities.Split(',');
                        foreach (string city in cities)
                        {
                            if (!string.IsNullOrEmpty(city.Trim()))
                            {
                                MySqlCommand cmd = new MySqlCommand("INSERT INTO 苏州市微博数据.location_user_invert (city_name, id, nickname) VALUES (@city, @id, @screen_name)", conn);
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

        //计算地理坐标系两点距离
        double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadius = 6371; // 地球半径，单位为千米

            // 将经纬度转换为弧度
            double lat1Rad = ToRadians(lat1);
            double lon1Rad = ToRadians(lon1);
            double lat2Rad = ToRadians(lat2);
            double lon2Rad = ToRadians(lon2);

            // 使用 Haversine 公式计算两点之间的距离
            double dlon = lon2Rad - lon1Rad;
            double dlat = lat2Rad - lat1Rad;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Pow(Math.Sin(dlon / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = earthRadius * c;

            return distance;
        }
        // 转换为弧度
        double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
        // 计算大地线总长度
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

        //移除字符串中特定字符后的内容
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

        //获取字符串特定字符后的内容
        private string GetAfterSymbol(string original, char symbol)
        {
            // 找到特定符号的位置
            int index = original.IndexOf(symbol);

            // 如果找到了符号，返回符号后面的部分，否则返回原始字符串
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

    # region 注释/过期代码
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
// 1. 创建地图实例
var bmapgl = new BMapGL.Map('map_container');
var point = new BMapGL.Point(116.403748, 39.915055);
bmapgl.centerAndZoom(point, 17);

// 2. 创建MapVGL图层管理器
var view = new mapvgl.View({{
map: bmapgl
}});

// 3. 创建可视化图层，并添加到图层管理器中
var layer = new mapvgl.PointLayer({{
color: 'rgba(50, 50, 200, 1)',
blend: 'lighter',
size: 15
}});
view.addLayer(layer);

// 4. 准备好规范化坐标数据
var data = [{{
geometry: {{
type: 'Point',
coordinates: [116.403748, 39.915055]
}}
}}];

// 5. 关联图层与数据，享受震撼的可视化效果
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
//string html = "<!DOCTYPE html>\r\n" +//页面声明
//    "<html lang=\"en\">\r\n" +
//    "<head>\r\n" +
//    "<meta charset=\"UTF-8\">\r\n" +
//    "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n" +
//    "<title>上拉切换页面</title>\r\n" +

//    "<style>\r\n" +//css文件编辑，样式创建
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
//    "color: red; /* 设置文字颜色为红色 */\r\n" +
//    "font-style: italic; /* 设置文字为斜体 */\r\n" +
//    "}\r\n" +
//    ".text-wrapper {\r\n" +
//    "position: absolute;\r\n" +
//    "top: 50%; /* 将文字包裹容器置于页面的垂直中间 */\r\n" +
//    "transform: translateY(-50%);\r\n" +
//    "width: 100%;\r\n" +
//    "text-align: center; /* 文字居中 */\r\n" +
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

//    "<body>\r\n" +//页面主体编辑

//    "<div class=\"container\" id=\"container\">\r\n" +

//    "<div class=\"page\" id=\"page1\">\r\n" +//第一张页面
//    "<div class=\"text-wrapper\">\r\n" +
//    "<h1 style=\"letter-spacing:.2em;font-size:29px;\">我的旅行日记</h1>\r\n" +
//    "<p style=\"letter-spacing:.3em;font-size:14px;\">二零一四年度微博使用报告</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page2\">\r\n" +//第二张页面
//    "<div class=\"text-wrapper\">\r\n" +
//    "<h1>这一年里</h1>\r\n" +
//    "<p>你一共去了<em class=\"s-fcRed\">13</em>个城市</p>\r\n" +
//    "<p>待得最久的是<em class=\"s-fcRed\">南京</em></p>\r\n" +
//    "<p>&nbsp;</p>\r\n" +
//    "<p>你旅行的风格成谜</p>\r\n" +
//    "<p>喜欢在<em class=\"s-fcRed\">晚上</em>发表微博</p>\r\n" +
//    "<p>&nbsp;</p>\r\n" +
//    "<p>你最喜欢说的词语是<em class=\"s-fcRed\">好热啊</em></p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page3\">\r\n" +//第三张页面
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p><em class=\"s-fcRed\">8月19日</em></p>\r\n" +
//    "<p>这一天你睡得很晚</p>\r\n" +
//    "<p><em class=\"s-fcRed\">4：11</em>还在于微博相伴</p>\r\n" +
//    "<p>&nbsp;</p>\r\n" +
//    "<p>那一刻你发了个微博</p>\r\n" +
//    "<p><em class=\"s-fcRed\">你见过凌晨四点的苏州吗</em></p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page4\">\r\n" +//第四张页面
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p>这是第四页</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page5\">\r\n" +//第五张页面
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p>这是第五页</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page6\">\r\n" +//第六张页面
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p>这是第六页</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "</div>\r\n" +

//    "<script>\r\n" +//Javascript编辑，滑动动画
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
//    "switchPage('down');\r\n" +//页面下滑
//    "} else {\r\n" +
//    "switchPage('up');\r\n" +//页面上滑
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
