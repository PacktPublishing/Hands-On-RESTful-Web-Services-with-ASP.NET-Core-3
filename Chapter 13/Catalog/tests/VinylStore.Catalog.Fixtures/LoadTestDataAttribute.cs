using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Sdk;

namespace VinylStore.Catalog.Fixtures
{ 
    public class LoadTestDataAttribute : DataAttribute
    {
        private readonly string _path;
        private readonly string _section;

        public LoadTestDataAttribute(string path, string section)
        {
            _path = path;
            _section = section;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) throw new ArgumentNullException(nameof(testMethod));

            string path = Path.IsPathRooted(_path)
                ? _path
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), _path);

            if (!File.Exists(path)) throw new ArgumentException($"File not found: {path}");

            string fileData = File.ReadAllText(_path);

            if (string.IsNullOrEmpty(_section)) return JsonConvert.DeserializeObject<List<string[]>>(fileData);

            var allData = JObject.Parse(fileData);
            var data = allData[_section];
            return new List<object[]> {new[] {data.ToObject(testMethod.GetParameters().First().ParameterType)}};
        }
    }
}