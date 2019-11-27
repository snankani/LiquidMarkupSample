using System;
using DotLiquid;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LiquidMarkup.Drops;
using System.Collections.Generic;
using LiquidMarkup.Transformer;

namespace LiquidMarkup
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            LiquidAlertTest();

          //  LiquidLitmusTest3();

          //  LiquidLitmusTest6();

            Console.ReadLine();
        }

        static void LiquidAlertTest()
        {
            Dictionary<SourceType, string> sourceInputPathMap = new Dictionary<SourceType, string>();
            sourceInputPathMap.Add(SourceType.PG, "\\SourceInput\\Reyansh_PG.json");
            sourceInputPathMap.Add(SourceType.Olympiad, "\\SourceInput\\Reyansh_Olympiad.json");

            foreach (KeyValuePair<SourceType, string> item in sourceInputPathMap)
            {
                string sourceInputFilePath = item.Value;
                string sourceInputFullPath = System.IO.Directory.GetCurrentDirectory() + sourceInputFilePath;
                string sourceInputText = File.ReadAllText(sourceInputFullPath, Encoding.UTF8);

                string commonJson = LiquidRenderer.GetInstance().Render(item.Key, sourceInputText);
                Console.WriteLine(commonJson);
            }
        }

        static void LiquidLitmusTest()
        {
            Template template = Template.Parse("hi {{Model.name}}");  // Parses and compiles the template
            string inputJson = "{\"name\": \"reyansh\"}";

            //JsonConvert.DeserializeObject(inputJson);
            JObject json = JObject.Parse(inputJson);
            
            // string renderedTemplate = template.Render(Hash.FromAnonymousObject(new { name = "tobi", a = "1" })); // Renders the output => "hi tobi"
            string renderedTemplate = template.Render(Hash.FromAnonymousObject(new { Model = json }));

            Console.WriteLine(renderedTemplate);
        }

        static void LiquidLitmusTest2()
        {
            Template template = Template.Parse("SCORE CARD \n FULL NAME: {{firstname}} {{lastname}} \n INSTITUTION:{{school}}");  // Parses and compiles the template
            string inputJson = "{\"firstname\": \"reyansh\", \"lastname\": \"nankani\", \"school\": \"phoenix greens\", \"exam\": \"Annual\", \"subjects\": [{\"english\": \"90\"},{ \"math\": \"100\"}]}";
            string renderedTemplate = template.Render(Hash.FromAnonymousObject(new { name = inputJson })); // Renders the output => "hi tobi"
            Console.WriteLine(renderedTemplate);
        }

        static void LiquidLitmusTest3()
        {

            Template template = Template.Parse("hi {{Model.name}} LiquidLitmusTest3");  // Parses and compiles the template
            string inputJson = "{\"name\": \"reyansh\", \"school\": \"phoenix greens\"}";

            TestData td = JsonConvert.DeserializeObject<TestData>(inputJson);
            
            string renderedTemplate = template.Render(Hash.FromAnonymousObject(new { Model = td }));

            Console.WriteLine(renderedTemplate);
        }

        static void LiquidLitmusTest4()
        {

            Template template = Template.Parse("hi {{Model.name}} LiquidLitmusTest4");  // Parses and compiles the template
            string inputJson = "{\"name\": \"reyansh\", \"school\": \"phoenix greens\"}";

            Test td = JsonConvert.DeserializeObject<Test>(inputJson);

            string renderedTemplate = template.Render(Hash.FromAnonymousObject(new { Model = td }));

            Console.WriteLine(renderedTemplate);
        }

        static void LiquidLitmusTest5()
        {

            Template template = Template.Parse("hi {{Model.name}} LiquidLitmusTest5");  // Parses and compiles the template
            string inputJson = "{'name': 'reyansh', 'school': 'phoenix greens'}";

            dynamic stuff = JObject.Parse(inputJson);
            Console.WriteLine(stuff.name);
            Console.WriteLine(stuff.school);

            string renderedTemplate = template.Render(Hash.FromAnonymousObject(new { Model = stuff }));

            Console.WriteLine(renderedTemplate);
        }

        static void LiquidLitmusTest6()
        {

            Template template = Template.Parse("hi {{school}} LiquidLitmusTest6");  // Parses and compiles the template
            string inputJson = "{'name': 'reyansh', 'school': 'phoenix greens'}";

            var deserializedObject =
                JsonConvert.DeserializeObject<Dictionary<string, object>>(inputJson, new DictionaryConverter());

            var hash = Hash.FromDictionary(deserializedObject);

            string renderedTemplate = template.Render(hash);

            Console.WriteLine(renderedTemplate);
        }
    }
}