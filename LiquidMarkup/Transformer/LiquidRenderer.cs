using System;
using System.Collections.Generic;
using System.Text;
using DotLiquid;
using System.IO;
using Newtonsoft.Json;

namespace LiquidMarkup.Transformer
{
    class LiquidRenderer
    {
        private Dictionary<SourceType, string> sourceInputTypeTemplatePathMap = new Dictionary<SourceType, string>();

        private Dictionary<SourceType, Template> sourceTypeTemplateMap = new Dictionary<SourceType, Template>();
        private static LiquidRenderer instance = new LiquidRenderer();
        private LiquidRenderer()
        {
            sourceInputTypeTemplatePathMap.Add(SourceType.PG, "\\LiquidTemplate\\ScoreCard_PG.liquid");
            sourceInputTypeTemplatePathMap.Add(SourceType.Olympiad, "\\LiquidTemplate\\ScoreCard_Olympiad.liquid");

            Initialize();
        }

        public static LiquidRenderer GetInstance()
        {
            return instance;
        }

        private void Initialize()
        {
            foreach (KeyValuePair<SourceType, string> item in sourceInputTypeTemplatePathMap)
            {
                string liquidTemplateFilePath = item.Value;
                string templateFullPath = System.IO.Directory.GetCurrentDirectory() + liquidTemplateFilePath;
                string templateText = File.ReadAllText(templateFullPath, Encoding.UTF8);

                Template template = Template.Parse(templateText);  // Parses and compiles the template
                sourceTypeTemplateMap.Add(item.Key, template);
            }
        }

        public string Render(SourceType sourceType, string inputJson)
        {
            string renderedText = null;

            Template compiledLiquidTemplate = null;
            bool compiledTemplateExists = sourceTypeTemplateMap.TryGetValue(sourceType, out compiledLiquidTemplate);
            if (compiledTemplateExists)
            {
                var deserializedObject =
                JsonConvert.DeserializeObject<Dictionary<string, object>>(inputJson, new DictionaryConverter());

                var hash = Hash.FromDictionary(deserializedObject);
                renderedText = compiledLiquidTemplate.Render(hash);
            }
            return renderedText;
        }

        public string Render2(SourceType sourceType, string inputJson)
        {
            string renderedText = null;

            Console.WriteLine("input json");
            Console.WriteLine(inputJson);

            string templateFullPath = System.IO.Directory.GetCurrentDirectory() + sourceInputTypeTemplatePathMap[sourceType];
            string templateText = File.ReadAllText(templateFullPath, Encoding.UTF8);

            Console.WriteLine("content of {0}", templateFullPath);
            Console.WriteLine(templateText);

            Template compiledLiquidTemplate = Template.Parse(templateText);  // Parses and compiles the template

            var deserializedObject =
                JsonConvert.DeserializeObject<Dictionary<string, object>>(inputJson, new DictionaryConverter());

            var hash = Hash.FromDictionary(deserializedObject);

            renderedText = compiledLiquidTemplate.Render(hash);
           
            return renderedText;
        }
    }
}

public enum SourceType { PG, Olympiad };
