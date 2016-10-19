using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucene.Net.Analysis;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using YK.FrameworkTools.SensitiveFilterService;

namespace YK.FrameworkAuth.LuceneTestNew
{
    [TestClass]
    public class LuceneUnitTest
    {
        [TestMethod]
        public void FullTextSearchTest()
        {
            Analyzer analyzer = new YKChineseAnalyzer();
            Directory directory = new RAMDirectory();

            IndexWriter iwriter = new IndexWriter(directory, analyzer, new IndexWriter.MaxFieldLength(100));

            Document[] doc = new Document[6];
            for (int i = 0; i < 6; i++)
            {
                doc[i] = new Document();
            }
            string[] text = { "穷 人,中华人民共和国中央人民政府", "中国是个伟大的国家", "我出生在美丽的中国，我爱中国，中国",
                "中华美丽的中国爱你", "美国跟中国式的国家", "文革,卧槽，你是中国的,xinsheng" };

            doc[0].Add(new Field("fieldname0", new System.IO.StringReader(text[0]), Field.TermVector.YES));
            doc[0].Add(new Field("fieldname1", new System.IO.StringReader(text[1]), Field.TermVector.YES));
            doc[0].Add(new Field("fieldname2", new System.IO.StringReader(text[2]), Field.TermVector.YES));
            doc[0].Add(new Field("fieldname3", new System.IO.StringReader(text[3]), Field.TermVector.YES));
            doc[0].Add(new Field("fieldname4", new System.IO.StringReader(text[4]), Field.TermVector.YES));
            doc[0].Add(new Field("fieldname5", new System.IO.StringReader(text[5]), Field.TermVector.YES));
            iwriter.AddDocument(doc[0]);
            iwriter.Dispose();

            IndexSearcher isearcher = new IndexSearcher(directory, true);

            string[] multiFields = { "fieldname0", "fieldname1", "fieldname5" };
            MultiFieldQueryParser parser = new MultiFieldQueryParser(
                    Lucene.Net.Util.Version.LUCENE_29, multiFields, analyzer);
            var token = parser.GetNextToken();

            // 设定具体的搜索词
            Query query = parser.Parse("文革");

            TopScoreDocCollector collector = TopScoreDocCollector.Create(1000, true);
            isearcher.Search(query, null, collector);//根据query查询条件进行查询，查询结果放入collector容器
            var docs2 = collector.TopDocs();

            System.Console.WriteLine(docs2);
        }

        [TestMethod]
        public void AnalysisVocabularyTest()
        {
            List<SensitiveWordEntity> sensitiveWordInfos = new List<SensitiveWordEntity>();
            sensitiveWordInfos.Add(new SensitiveWordEntity()
                {
                    Field = "Content1",
                    Text = "文革,卧槽，你是中国的,xinsheng,蒋介石",
                    FieldType = EFieldType.Text
                });
            sensitiveWordInfos.Add(new SensitiveWordEntity()
            {
                Field = "Content2",
                Text = "穷 人,中华人民共和国中央人民政府，蒋介石",
                FieldType = EFieldType.Text
            });
            sensitiveWordInfos.Add(new SensitiveWordEntity()
            {
                Field = "Content3",
                Text = System.IO.File.OpenText(@"E:\test.html").ReadToEnd(),
                FieldType = EFieldType.HtmlText
            });

            //var result = SensitiveWordHelper.GetSensitiveWordResult(sensitiveWordInfos, ESensitiveWordResultType.OnlyOne);
            //Console.WriteLine(result.IsSensitiveWord);
            //Console.WriteLine(result.OnlyOneDictErrorWord.Value);

            //var result = SensitiveWordHelper.GetSensitiveWordResult(sensitiveWordInfos, ESensitiveWordResultType.PerOne);
            //Console.WriteLine(result.IsSensitiveWord);
            //foreach (var item in result.PerOneDictErrorWord)
            //{
            //    Console.WriteLine(item.Key + "_" + item.Value);
            //}

            var result = SensitiveWordHelper.GetSensitiveWordResult(sensitiveWordInfos, ESensitiveWordResultType.All);
            Console.WriteLine(result.IsSensitiveWord);
            foreach (var item in result.AllDictErrorWord)
            {
                Console.WriteLine(item.Key + "_" + item.Value.Aggregate((r,t)=>r + "." + t));
            }
        }

        [TestMethod]
        public void HtmlLuceneTest()
        {
            Parse();
        }

        [TestMethod]
        public void QuestionTest()
        {
            Person p1 = new Person();
            Console.WriteLine(p1.B);
            Console.WriteLine(Person.A);

            Person p2 = new Person();
            Console.WriteLine(p2.B);
            Console.WriteLine(Person.A);

            Console.WriteLine(F(7));
        }

        public static void Parse()
        {
            HtmlDocument doc = new HtmlDocument();
            System.IO.StreamReader sr = System.IO.File.OpenText(@"E:\test.html");
            doc.Load(sr);
            Console.WriteLine(doc.DocumentNode.InnerText.Replace(" ", ""));
            int i = 0;
        }

        static int F(int n)
        {
            if (n == 1)
            {
                return 1;
            }
            if (n == 2)
            {
                return 1;
            }
            return F(n - 2) + F(n - 1);
        }
    }

    class Person
    {
        public static int A = 30;
        static Person()
        {
            A++;
        }

        public int B = A++;
        public Person()
        {
            B++;
        }
    }
}
